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
using System.Globalization;
using System.Net;

namespace Amazon.Runtime
{
    /// <summary>
    /// When running in an ECS container and AWS_CONTAINER_CREDENTIALS_RELATIVE_URI is set,
    /// use the given end point to retrieve the credentials.
    /// </summary>
    public class ECSTaskCredentials : URIBasedRefreshingCredentialHelper
    {
        /// <summary>
        /// These constants should not be consumed by client code.  They are only relevant
        /// in the context of ECS container and, especially, AWS_CONTAINER_CREDENTIALS_RELATIVE_URI
        /// environment variable should not be overriden by the client code.
        /// </summary>
        public const string ContainerCredentialsURIEnvVariable = "AWS_CONTAINER_CREDENTIALS_RELATIVE_URI";        
        public const string EndpointAddress = "http://169.254.170.2";

        private string Uri = null;
        private string Server = null;
        private static int MaxRetries = 5;

        private IWebProxy Proxy;

        public ECSTaskCredentials() : this(null) { }

        public ECSTaskCredentials(IWebProxy proxy)
        {
            Uri = System.Environment.GetEnvironmentVariable(ECSTaskCredentials.ContainerCredentialsURIEnvVariable);
            Server = EndpointAddress;
            Proxy = proxy;
        }

        protected override CredentialsRefreshState GenerateNewCredentials()
        {
            SecurityCredentials credentials = null;
            Uri ecsEndpointUri = new Uri(Server + Uri);
            JitteredDelay retry = new JitteredDelay(new TimeSpan(0, 0, 0, 0, 200), new TimeSpan(0, 0, 0, 0, 50));
            // Attempt to get the credentials 4 times ignoring null return/exceptions and on the 5th try, escalate the exception if there is one.
            for (int i = 1; ; i++)
            {
                try
                {
                    credentials = GetObjectFromResponse<SecurityCredentials>(ecsEndpointUri, Proxy);
                    if (credentials != null)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    if (i == MaxRetries)
                    {
                        throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture,
                        "Unable to retrieve credentials. Message = \"{0}\".",
                        e.Message));
                    }
                };
                Util.AWSSDKUtils.Sleep(retry.Next());
            }

            return new CredentialsRefreshState(new ImmutableCredentials(credentials.AccessKeyId, credentials.SecretAccessKey, credentials.Token), credentials.Expiration);
        }
    }
}
