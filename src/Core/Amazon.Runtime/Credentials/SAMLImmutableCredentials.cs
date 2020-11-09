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
using System.Collections.Generic;
using System.Globalization;
using ThirdParty.Json.LitJson;

namespace Amazon.Runtime
{
    /// <summary>
    /// Immutable representation of AWS credentials obtained as a result of
    /// authenticating against a SAML-supporting federated identity provider.
    /// </summary>
    public class SAMLImmutableCredentials : ImmutableCredentials
    {
        #region Properties

        /// <summary>
        /// The expiry time of the credentials, obtained from the AssumeRoleWithSAML response.
        /// </summary>
        public DateTime Expires { get; private set; }

        /// <summary>
        /// The value of the NameID element in the Subject element of the SAML assertion, as returned in the
        /// AssumeRoleWithSAML response from the Security Token Service.
        /// </summary>
        public string Subject { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance with supplied keys and SAML assertion data.
        /// </summary>
        /// <param name="awsAccessKeyId"></param>
        /// <param name="awsSecretAccessKey"></param>
        /// <param name="token"></param>
        /// <param name="expires"></param>
        /// <param name="subject"></param>
        public SAMLImmutableCredentials(string awsAccessKeyId,
                                        string awsSecretAccessKey,
                                        string token,
                                        DateTime expires,
                                        string subject)
            : base(awsAccessKeyId, awsSecretAccessKey, token)
        {
            Expires = expires;
            Subject = subject;
        }

        /// <summary>
        /// Constructs an instance with supplied keys and SAML assertion data.
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="subject"></param>
        /// <param name="expires"></param>
        public SAMLImmutableCredentials(ImmutableCredentials credentials,
                                        DateTime expires,
                                        string subject)
            : base(credentials.AccessKey, credentials.SecretKey, credentials.Token)
        {
            Expires = expires;
            Subject = subject;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return Hashing.Hash(AccessKey, SecretKey, Token, Subject, Expires);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var ic = obj as SAMLImmutableCredentials;
            if (ic == null)
                return false;

            if (base.Equals(obj))
                return string.Equals(Subject, ic.Subject, StringComparison.Ordinal) &&
                    DateTime.Equals(Expires, ic.Expires);

            return false;
        }

        /// <summary>
        /// Returns a copy of the current credentials.
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials Copy()
        {
            return new SAMLImmutableCredentials(AccessKey, SecretKey, Token, Expires, Subject);
        }
        #endregion

        #region Serialization

        const string AccessKeyProperty = "AccessKey";
        const string SecretKeyProperty = "SecretKey";
        const string TokenProperty = "Token";
        const string ExpiresProperty = "Expires";
        const string SubjectProperty = "Subject";

        /// <summary>
        /// Serializes the instance to a json-format string for external storage
        /// </summary>
        /// <returns>The serialized object in json</returns>
        internal string ToJson()
        {
            // don't need all data, and we want to be explicit on the datetime format
            var props = new Dictionary<string, string>();
            props.Add(AccessKeyProperty, AccessKey);
            props.Add(SecretKeyProperty, SecretKey);
            props.Add(TokenProperty, Token);

            props.Add(ExpiresProperty, Expires.ToString("u", CultureInfo.InvariantCulture));

            props.Add(SubjectProperty, Subject);

            return JsonMapper.ToJson(props);
        }

        /// <summary>
        /// Instantiates an instance from persisted json data
        /// </summary>
        /// <param name="json">The serialized data</param>
        /// <returns>Deserialized instance corresponding to the json data</returns>
        internal static SAMLImmutableCredentials FromJson(string json)
        {
            try
            {
                var o = JsonMapper.ToObject(json);

                // get the expiry first - if the credentials have expired we can then
                // ignore the data
                var expires = DateTime.Parse((string)o[ExpiresProperty], CultureInfo.InvariantCulture).ToUniversalTime();
#pragma warning disable CS0612 // Type or member is obsolete
                if (expires <= AWSSDKUtils.CorrectedUtcNow)
#pragma warning restore CS0612 // Type or member is obsolete
                {
                    Logger.GetLogger(typeof(SAMLImmutableCredentials)).InfoFormat("Skipping serialized credentials due to expiry.");
                    return null;
                }

                var accessKey = (string)o[AccessKeyProperty];
                var secretKey = (string)o[SecretKeyProperty];
                var token = (string)o[TokenProperty];
                var subject = (string)o[SubjectProperty];

                return new SAMLImmutableCredentials(accessKey, secretKey, token, expires, subject);
            }
            catch (Exception e)
            {
                Logger.GetLogger(typeof(SAMLImmutableCredentials)).Error(e, "Error during deserialization");
            }

            return null;
        }

        #endregion
    }
}
