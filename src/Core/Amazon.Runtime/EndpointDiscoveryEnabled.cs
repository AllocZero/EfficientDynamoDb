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
using System;
using System.Globalization;

using Amazon.Runtime.Internal.Util;
using System.Collections.Generic;
using Amazon.Util;
using Amazon.Runtime.CredentialManagement;

namespace Amazon.Runtime
{
    /// <summary>
    /// Determines the endpoint discovery enabled value based on an environment variable. If no value is found in the
    /// environment then an InvalidOperationException is thrown.
    /// </summary>
    [Obsolete("This class has been deprecated in favor of EnvironmentVariableAWSConfiguration.")]
    public class EnvironmentVariableAWSEndpointDiscoveryEnabled
    {
        public const string ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY = "AWS_ENABLE_ENDPOINT_DISCOVERY";
        public bool Enabled { get; private set; }

        /// <summary>
        /// Attempts to construct an instance of EnvironmentVariable AWS_ENABLED_ENDPOINT_DISCOVERY. If no value is found in the
        /// environment then an InvalidOperationException is thrown.
        /// </summary>
        public EnvironmentVariableAWSEndpointDiscoveryEnabled()
        {
            string enabledValue = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY);
            if (string.IsNullOrEmpty(enabledValue))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "The environment variable {0} was not set with a boolean value.", ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY));
            }

            bool enabled;
            if(!bool.TryParse(enabledValue, out enabled))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "The environment variable {0} was set with value {1}, but it could not be parsed as a valid boolean value.", ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY, enabledValue));
            }

            this.Enabled = enabled;

            var logger = Logger.GetLogger(typeof(EnvironmentVariableAWSEndpointDiscoveryEnabled));
            logger.InfoFormat("Endpoint discovery enabled found using environment variable.");
        }
    }

    /// <summary>
    /// Determines endpoint discovery enabled based on a <see cref="CredentialProfile"/> stored in an <see cref="ICredentialProfileSource"/>.
    /// If the profile doesn't exist or there is no endpoint discovery enabled information an InvalidOperationException is thrown.
    /// </summary>
    [Obsolete("This class has been deprecated in favor of ProfileAWSConfiguration.")]
    public class ProfileAWSEndpointDiscoveryEnabled
    {
        public bool Enabled { get; private set; }

        /// <summary>
        /// Attempts to construct an instance of <see cref="ProfileAWSEndpointDiscoveryEnabled"/>.
        /// If the AWS_PROFILE environment variable is set the instance will be constructed using that profile,
        /// otherwise it will use the default profile.
        ///
        /// If the profile doesn't exist or there is no endpoint discovery enabled information an InvalidOperationException is thrown.
        /// </summary>
        /// <param name="source">The ICredentialProfileSource to read the profile from.</param>
        public ProfileAWSEndpointDiscoveryEnabled(ICredentialProfileSource source)
        {
            var profileName = Environment.GetEnvironmentVariable(FallbackCredentialsFactory.AWS_PROFILE_ENVIRONMENT_VARIABLE);
            if (profileName == null)
                profileName = FallbackCredentialsFactory.DefaultProfileName;
            Setup(source, profileName);
        }

        /// <summary>
        /// Attempts to construct an instance of <see cref="ProfileAWSEndpointDiscoveryEnabled"/>.
        /// If the profile doesn't exist or there is no endpoint discovery enabled information an InvalidOperationException is thrown.
        /// </summary>
        /// <param name="source">The ICredentialProfileSource to read the profile from.</param>
        /// <param name="profileName">The name of the profile.</param>
        public ProfileAWSEndpointDiscoveryEnabled(ICredentialProfileSource source, string profileName)
        {
            Setup(source, profileName);
        }

        private void Setup(ICredentialProfileSource source, string profileName)
        {
            bool? enabled = null;
            CredentialProfile profile;
            if (source.TryGetProfile(profileName, out profile))
            {
                enabled = profile.EndpointDiscoveryEnabled;
            }
            else
                throw new InvalidOperationException("Unable to find a profile named '" + profileName + "' in store " + source.GetType());

            if (enabled == null)
                throw new InvalidOperationException("There is no endpoint_discovery_enabled set in the profile named '" + profileName + "' in store " + source.GetType());
            else
            {
                this.Enabled = enabled.Value;
                var logger = Logger.GetLogger(typeof(ProfileAWSEndpointDiscoveryEnabled));
                logger.InfoFormat("endpoint_discovery_enabled found in profile '" + profileName + "' in store " + source.GetType());
            }
        }
    }

    /// <summary>
    /// Probing mechanism to determine the endpoint discovery enabled value from various sources.
    /// </summary>
    [Obsolete("This class has been deprecated in favor of FallbackConfigurationFactory.")]
    public static class FallbackEndpointDiscoveryEnabledFactory
    {
        private static CredentialProfileStoreChain credentialProfileChain = new CredentialProfileStoreChain();

        private static object _lock = new object();

        static FallbackEndpointDiscoveryEnabledFactory()
        {
            Reset();
        }

        private delegate bool ConfigGenerator();

        private static List<ConfigGenerator> EnabledGenerators { get; set; }
        
        public static void Reset()
        {
            endpointDiscoveryEnabled = null;
            EnabledGenerators = new List<ConfigGenerator>
            {                
                () => (new EnvironmentVariableAWSEndpointDiscoveryEnabled()).Enabled,
                () => (new ProfileAWSEndpointDiscoveryEnabled(credentialProfileChain)).Enabled,
            };                        
        }

        private static bool? endpointDiscoveryEnabled;
                
        public static bool? GetEnabled()
        {
            lock (_lock)
            {
                if (endpointDiscoveryEnabled != null)
                    return endpointDiscoveryEnabled;
                
                List<Exception> errors = new List<Exception>();

                //Determine the Enabled flag                
                foreach (var generator in EnabledGenerators)
                {
                    try
                    {
                        endpointDiscoveryEnabled = generator();
                    }
                    catch (Exception e)
                    {
                        errors.Add(e);
                        continue;
                    }

                    if (endpointDiscoveryEnabled.HasValue)
                        break;
                }

                return endpointDiscoveryEnabled;
            }
        }
    }
}
