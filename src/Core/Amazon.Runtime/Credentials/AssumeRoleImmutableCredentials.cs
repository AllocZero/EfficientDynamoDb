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
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;

namespace Amazon.Runtime
{
    /// <summary>
    /// Immutable representation of AWS credentials obtained as a result of
    /// authenticating with the AssumeRole method of the Security Token Service.
    /// </summary>
    public class AssumeRoleImmutableCredentials : ImmutableCredentials
    {
        /// <summary>
        /// The expiry time of the credentials, obtained from the AssumeRoleResponse.
        /// </summary>
        public DateTime Expiration { get; private set; }

        /// <summary>
        /// Constructs an instance with supplied keys, token, and expiration.
        /// </summary>
        /// <param name="awsAccessKeyId">The AccessKey for the credentials.</param>
        /// <param name="awsSecretAccessKey">The SecretKey for the credentials.</param>
        /// <param name="token">The security token for the credentials.</param>
        /// <param name="expiration">The expiration time for the credentials.</param>
        public AssumeRoleImmutableCredentials(string awsAccessKeyId, string awsSecretAccessKey, string token, DateTime expiration)
            : base(awsAccessKeyId, awsSecretAccessKey, token)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("token");
            Expiration = expiration;
        }

        /// <summary>
        /// Get a copy of this AssumeRoleImmutableCredentials object.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        new public AssumeRoleImmutableCredentials Copy()
        {
            return new AssumeRoleImmutableCredentials(AccessKey, SecretKey, Token, Expiration);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(AccessKey, SecretKey, Token, Expiration);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            AssumeRoleImmutableCredentials aric = obj as AssumeRoleImmutableCredentials;
            if (aric == null)
                return false;

            return AWSSDKUtils.AreEqual(
                new object[] { AccessKey, SecretKey, Token, Expiration },
                new object[] { aric.AccessKey, aric.SecretKey, aric.Token, Expiration });
        }
    }
}
