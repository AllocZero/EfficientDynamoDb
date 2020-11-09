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

namespace Amazon.Runtime
{
    /// <summary>
    /// Basic set of credentials consisting of an AccessKey and SecretKey
    /// </summary>
    public class BasicAWSCredentials : AWSCredentials
    {
        #region Private members

        private ImmutableCredentials _credentials;

        #endregion


        #region Constructors


        /// <summary>
        /// Constructs a BasicAWSCredentials object for the specified accessKey and secretKey.
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public BasicAWSCredentials(string accessKey, string secretKey)
        {
            if (!string.IsNullOrEmpty(accessKey))
            {
                _credentials = new ImmutableCredentials(accessKey, secretKey, null);
            }
        }

        #endregion


        #region Abstract class overrides

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            if (this._credentials == null)
                return null;

            return _credentials.Copy();
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var bac = obj as BasicAWSCredentials;
            if (bac == null)
                return false;

            return AWSSDKUtils.AreEqual(
                new object[] { _credentials },
                new object[] { bac._credentials });
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(_credentials);
        }
    }
}
