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
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.SharedInterfaces;
using System;
using System.Globalization;
using System.Net;

namespace Amazon.Runtime
{
    public class AssumeRoleAWSCredentialsOptions
    {
        /// <summary>
        /// A unique identifier that is used by third parties when assuming roles in their customers' accounts.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        ///  An IAM policy in JSON format.
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// The length of time in seconds before the credentials will expire.
        /// </summary>
        public int? DurationSeconds { get; set; }

        /// <summary>
        /// The proxy settings to use when calling AssumeRole.
        /// </summary>
#if BCL
        public WebProxy ProxySettings { get; set; }
#elif NETSTANDARD
        public IWebProxy ProxySettings { get; set; }
#endif

        /// <summary>
        /// The identification number of the MFA device that is associated with the user who is making the assume-role call.
        /// </summary>
        public string MfaSerialNumber { get; set; }

        /// <summary>
        /// The value provided by the MFA device, if the trust policy of the role being assumed requires MFA.
        /// </summary>
        public string MfaTokenCode
        {
            get
            {
                if (String.IsNullOrEmpty(MfaSerialNumber))
                {
                    return null;
                }
                else if (MfaTokenCodeCallback == null)
                {
                    throw new InvalidOperationException("The MfaSerialNumber has been set but the MfaTokenCodeCallback hasn't.  " +
                        "MfaTokenCodeCallback is required in order to determine the MfaTokenCode when MfaSerialNumber is set.");
                }
                else
                {
                    return MfaTokenCodeCallback();
                }
            }
        }

        /// <summary>
        /// A callback that's used to obtain the MFA token code when the AssumeRoleAWSCredentials are refreshed.
        /// </summary>
        public Func<string> MfaTokenCodeCallback { get; set; }
    }
}
