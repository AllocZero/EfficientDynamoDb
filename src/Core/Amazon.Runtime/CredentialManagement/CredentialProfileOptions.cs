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

namespace Amazon.Runtime.CredentialManagement
{
#if !NETSTANDARD13
    /// <summary>
    /// The options that are available for creating AWSCredentials with the AWSCredentialsFactory.
    /// The type of AWSCredentials that are created depends on which Options are set.
    ///
    /// Below are the different types of credentials returned, along with the options that are used to obtain them.
    /// ([] denotes options that are not required)
    ///
    /// BasicAWSCredentials         AccessKey SecretKey
    /// SessionAWSCredentials       AccessKey SecretKey Token
    /// AssmeRoleAWSCredentials     SourceProfile RoleArn [ExternalID] [MfaSerial]
    /// FederatedAWSCredentials     EndpointName RoleArn [UserIdentity]
    /// </summary>
#else
    /// <summary>
    /// The options that are available for creating AWSCredentials with the AWSCredentialsFactory.
    /// The type of AWSCredentials that are created depends on which Options are set.
    ///
    /// Below are the different types of credentials returned, along with the options that are used to obtain them.
    /// ([] denotes options that are not required)
    ///
    /// BasicAWSCredentials         AccessKey SecretKey
    /// SessionAWSCredentials       AccessKey SecretKey Token
    /// AssmeRoleAWSCredentials     SourceProfile RoleArn [ExternalID] [MfaSerial]
    /// </summary>
#endif
    public class CredentialProfileOptions
    {
        /// <summary>
        /// The access key to be used in the AWSCredentials.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// The source of credentials to be used to obtain AWSCredentials.
        /// </summary>
        public string CredentialSource { get; set; }
#if !NETSTANDARD13
        /// <summary>
        /// The endpoint name to be used for federated AWSCredentials.
        /// </summary>
        public string EndpointName { get; set; }
#endif
        /// <summary>
        /// The external id to use in assume role AWSCredentials.
        /// </summary>
        public string ExternalID { get; set; }
        /// <summary>
        /// The serial number of the MFA to use in assume role AWSCredentials.
        /// </summary>
        public string MfaSerial { get; set; }
#if !NETSTANDARD13
        /// <summary>The role ARN to use when creating assume role or federated AWSCredentials.</summary>
#else
        /// <summary>The role ARN to use when creating assume role AWSCredentials.</summary>
#endif
        public string RoleArn { get; set; }

        /// <summary>
        /// An identifier for the assumed role session.
        /// </summary>
        public string RoleSessionName { get; set; }

        /// <summary>
        /// The secret key to use when creating AWSCredentials.
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// When this CredentialProfileOptions object references another CredentialProfile,
        /// the name of the referenced CredentialProfile.
        /// </summary>
        public string SourceProfile { get; set; }
        /// <summary>
        /// The session token to be used to create AWSCredentials.
        /// </summary>
        public string Token { get; set; }
#if !NETSTANDARD13
        /// <summary>
        /// The user identity to use when creating federated AWSCredentials.
        /// If not set, the user identity that the code is running under will be used.
        /// </summary>
        public string UserIdentity { get; set; }
#endif
        /// <summary>
        /// Contains the executable information to be used by the process credential retriever
        /// to either fetch Basic or Session credentials
        /// </summary>
        public string CredentialProcess { get; set; }

        /// <summary>
        /// Absolute path to the file on disk containing an OIDC token.
        /// </summary>
        public string WebIdentityTokenFile { get; set; }

        /// <summary>
        /// Return true the properties are all null or empty, false otherwise.
        /// </summary>
        internal bool IsEmpty
        {
            get
            {
                return
#if !NETSTANDARD13
                    string.IsNullOrEmpty(EndpointName) &&
                    string.IsNullOrEmpty(UserIdentity) &&
#endif
                    string.IsNullOrEmpty(AccessKey) &&
                    string.IsNullOrEmpty(ExternalID) &&
                    string.IsNullOrEmpty(MfaSerial) &&
                    string.IsNullOrEmpty(RoleArn) &&
                    string.IsNullOrEmpty(RoleSessionName) &&
                    string.IsNullOrEmpty(SecretKey) &&
                    string.IsNullOrEmpty(SourceProfile) &&
                    string.IsNullOrEmpty(Token) &&
                    string.IsNullOrEmpty(CredentialProcess) &&
                    string.IsNullOrEmpty(WebIdentityTokenFile);
            }
        }
        public override string ToString()
        {
            return
                "[AccessKey=" + AccessKey + ", " +
#if !NETSTANDARD13
                "EndpointName=" + EndpointName + ", " +
#endif
                "ExternalID=" + ExternalID + ", " +
                "MfaSerial=" + MfaSerial + ", " +
                "RoleArn=" + RoleArn + ", " +
                "RoleSessionName=" + RoleSessionName + ", " +
                "SecretKey=XXXXX, " +
                "SourceProfile=" + SourceProfile + ", " +
                "Token=" + Token +
#if !NETSTANDARD13
                ", " + "UserIdentity=" + UserIdentity +
#endif
                ", " + "CredentialProcess=" + CredentialProcess +
                ", " + "WebIdentityTokenFile=" + WebIdentityTokenFile +
                "]";
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var po = obj as CredentialProfileOptions;
            if (po == null)
                return false;

#if !NETSTANDARD13
            return AWSSDKUtils.AreEqual(
                new object[] { AccessKey, EndpointName, ExternalID, MfaSerial, RoleArn, RoleSessionName, SecretKey, SourceProfile, Token, UserIdentity, CredentialProcess, WebIdentityTokenFile },
                new object[] { po.AccessKey, po.EndpointName, po.ExternalID, po.MfaSerial, po.RoleArn, po.RoleSessionName, po.SecretKey, po.SourceProfile, po.Token, po.UserIdentity, po.CredentialProcess, po.WebIdentityTokenFile });
#else
            return AWSSDKUtils.AreEqual(
                new object[] { AccessKey, ExternalID, MfaSerial, RoleArn, RoleSessionName, SecretKey, SourceProfile, Token, CredentialProcess, WebIdentityTokenFile},
                new object[] { po.AccessKey, po.ExternalID, po.MfaSerial, po.RoleArn, po.RoleSessionName, po.SecretKey, po.SourceProfile, po.Token, po.CredentialProcess, po.WebIdentityTokenFile });
#endif
        }

        public override int GetHashCode()
        {
#if !NETSTANDARD13
            return Hashing.Hash(AccessKey, EndpointName, ExternalID, MfaSerial, RoleArn, RoleSessionName, SecretKey, SourceProfile, Token, UserIdentity, CredentialProcess, WebIdentityTokenFile);
#else
            return Hashing.Hash(AccessKey, ExternalID, MfaSerial, RoleArn, RoleSessionName, SecretKey, SourceProfile, Token, CredentialProcess, WebIdentityTokenFile);
#endif
        }
    }
}
