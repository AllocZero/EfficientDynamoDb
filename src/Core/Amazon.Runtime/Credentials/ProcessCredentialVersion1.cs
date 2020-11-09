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
using ThirdParty.Json.LitJson;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class to which the Json stdout from credential process is 
    /// de-serialized into.
    /// </summary>
    public class ProcessCredentialVersion1
    {
        /// <summary>
        /// Version of the Json payload
        /// </summary>
        [JsonProperty(Required = true)]
        public int Version { get; set; }
        /// <summary>
        /// AWS Access key
        /// </summary>
        [JsonProperty(Required = true)]
        public string AccessKeyId { get; set; }
        /// <summary>
        /// AWS Secret key
        /// </summary>
        [JsonProperty(Required = true)]
        public string SecretAccessKey { get; set; }
        /// <summary>
        /// AWS Session token for session credentials
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// ISO8601 formatted timestamp till when the credential is valid.
        /// </summary>
        public DateTime Expiration { get; set; } = DateTime.MaxValue;
    }
}