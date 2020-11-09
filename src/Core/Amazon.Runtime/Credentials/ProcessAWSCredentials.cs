/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using Amazon.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using ThirdParty.Json.LitJson;
using System.Diagnostics;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
#if NETSTANDARD
using System.Runtime.InteropServices;
#endif

namespace Amazon.Runtime
{
    /// <summary>
    /// Process Credentials  can retrieve credentials by running a process and reading its stdout. 
    /// A new config option, "credential_process" is added to the shared config file that allows customers 
    /// to specify which process to run. The credentials retrieved by running this process could be either
    /// Basic or Session credentials.
    /// </summary>
    public class ProcessAWSCredentials : RefreshingAWSCredentials
    {
        #region Private members
        private const string _versionString = "Version";
        private Logger _logger = Logger.GetLogger(typeof(ProcessAWSCredentials));
        private readonly ProcessStartInfo _processStartInfo;
        #endregion

        #region Public constructors

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public ProcessAWSCredentials(string processCredentialInfo)
        {
            processCredentialInfo = processCredentialInfo.Trim();
            
            //Default to cmd on Windows since that is the only thing BCL runs on.
            var fileName = "cmd.exe";
            var arguments = $@"/c {processCredentialInfo}";
#if NETSTANDARD
            //If it is netstandard and not running on Windows use sh.
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))            
            {
                fileName = "sh";
                var escapedArgs = processCredentialInfo.Replace("\\", "\\\\").Replace("\"", "\\\"");                
                arguments = $"-c \"{escapedArgs}\"";
            }
#endif
            _processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };            

            // Make sure to fetch new credentials well before the current credentials expire to avoid
            // any request being made with expired credentials.
            PreemptExpiryTime = TimeSpan.FromMinutes(5);
        }

        #endregion

        #region Protected overridden methods

        protected override CredentialsRefreshState GenerateNewCredentials()
        {
            return DetermineProcessCredential();
        }

#if AWS_ASYNC_API
        protected override Task<CredentialsRefreshState> GenerateNewCredentialsAsync()
        {
            return DetermineProcessCredentialAsync();
        }
#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Generates new credentials by running the "credential_process" process.
        /// </summary>
        public CredentialsRefreshState DetermineProcessCredential()
        {            
            try
            {
                var processInfo = AWSSDKUtils.RunProcess(_processStartInfo);
                return SetCredentialsRefreshState(processInfo);
            }
            catch (ProcessAWSCredentialException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.DebugFormat("Process recorded exception - {0}", e);
                throw new ProcessAWSCredentialException(string.Format(CultureInfo.CurrentCulture,"AWS credential process terminated with {0}", e.GetType()), e);
            }
        }

#if AWS_ASYNC_API
        public async Task<CredentialsRefreshState> DetermineProcessCredentialAsync()
        {            
            try
            {
                var processInfo = await AWSSDKUtils.RunProcessAsync(_processStartInfo).ConfigureAwait(false);
                return SetCredentialsRefreshState(processInfo);
            }
            catch (ProcessAWSCredentialException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.DebugFormat("Process recorded exception - {0}", e);
                throw new ProcessAWSCredentialException(string.Format(CultureInfo.CurrentCulture,"AWS credential process terminated with {0}", e.GetType()), e);
            }
        }
#endif
        #endregion

        #region Private methods
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private CredentialsRefreshState SetCredentialsRefreshState(ProcessExecutionResult processInfo)
        {            
            _logger.InfoFormat("Process ends with exitcode - {0}", processInfo.ExitCode);
            // Setting useShellExecute to false enables the user to just specify the executable name.
            // The system will attempt to find the executable within folders specified by the PATH environment variable.
            if (processInfo.ExitCode == 0)
            {
                JsonData data = null;
                try
                {
                    data = JsonMapper.ToObject(processInfo.StandardOutput);
                }
                catch(JsonException je)
                {
                    throw new ProcessAWSCredentialException("The response back from the process credential provider returned back a malformed JSON document.", je);
                }
                
                if (!data.PropertyNames.Contains(_versionString)|| string.IsNullOrEmpty(data[_versionString].ToString()))
                {
                    throw new ProcessAWSCredentialException("Missing required parameter - Version in JSON Payload");
                }
                var version = (int)data[_versionString];
                switch (version)
                {
                    case 1:
                        ProcessCredentialVersion1 processCredentialDataV1 = null;                        
                        try
                        {
                            processCredentialDataV1 = JsonMapper.ToObject<ProcessCredentialVersion1>(processInfo.StandardOutput);
                        }
                        catch (Exception e)
                        {
                            throw new ProcessAWSCredentialException("The response back from the process credential provider returned back a malformed JSON document.", e);
                        }

                        return new CredentialsRefreshState(
                            new ImmutableCredentials(processCredentialDataV1.AccessKeyId,
                                processCredentialDataV1.SecretAccessKey, processCredentialDataV1.SessionToken),processCredentialDataV1.Expiration);
                    default:
                        throw new ProcessAWSCredentialException(string.Format(CultureInfo.CurrentCulture,"Unsupported credential version: {0}" + version));
                }
            }
            var processException = new ProcessAWSCredentialException(string.Format(CultureInfo.CurrentCulture, "Command returned non-zero exit value {0} with the error - {1}", processInfo.ExitCode, processInfo.StandardError));
            _logger.DebugFormat("Process {0} recorded exception - {1}", _processStartInfo.FileName, processException);
            throw processException;
        }
        #endregion
    }

    /// <summary>
    /// Exception class to capture all exceptions encountered when starting or
    /// executing the "credential_process" process. If the user has specified an executable
    /// and the SDK is unable to execute it, the exception should be surfaces to the user 
    /// instead of moving on to the next credential provider. 
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class ProcessAWSCredentialException : Exception
    {
        public ProcessAWSCredentialException(string message) : base(message)
        {
        }

        public ProcessAWSCredentialException(string message, Exception inner)
            : base(message, inner)
        {
        }
        
#if !NETSTANDARD
        protected ProcessAWSCredentialException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}