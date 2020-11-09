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

namespace Amazon.Runtime.CredentialManagement
{
#if !NETSTANDARD13
    /// <summary>
    /// Represents the different types of authentication available for SAML endpoints.
    /// </summary>
    public enum SAMLAuthenticationType
    {
        NTLM,
        Digest,
        Kerberos,
        Negotiate
    }

    /// <summary>
    /// Representation of a SAML Endpoint.
    /// </summary>
    public class SAMLEndpoint
    {
        private SAMLAuthenticationType DefaultAuthenticationType = SAMLAuthenticationType.Kerberos;

        /// <summary>
        /// The name given to this SAMLEndpoint.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The URI of the SAML endnpoint.
        /// </summary>
        public Uri EndpointUri { get; private set; }

        /// <summary>
        /// The authentication type associated with the SAML endpoint.
        /// </summary>
        public SAMLAuthenticationType AuthenticationType { get; private set; }

        /// <summary>
        /// Internal constructor.  Used by SAMLEndpointManager when reading endpoints from the encrypted store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="endpointUri"></param>
        /// <param name="authenticationType"></param>
        internal SAMLEndpoint(string name, string endpointUri, string authenticationType)
        {
            var parsedEndpointUri = new Uri(endpointUri, UriKind.RelativeOrAbsolute);
            var parsedAuthenticationType = DefaultAuthenticationType;

            if (!string.IsNullOrEmpty(authenticationType))
            {
                parsedAuthenticationType = (SAMLAuthenticationType)Enum.Parse(typeof(SAMLAuthenticationType), authenticationType);
            }

            SetProperties(name, parsedEndpointUri, parsedAuthenticationType);
        }

        /// <summary>
        /// Construct a SAMLEndpoint using the default SAMLAuthenticationType - Kerberos.
        /// </summary>
        /// <param name="name">The name of the endpoint.</param>
        /// <param name="endpointUri">The URI of the endpoint.</param>
        public SAMLEndpoint(string name, Uri endpointUri)
        {
            SetProperties(name, endpointUri, DefaultAuthenticationType);
        }

        /// <summary>
        /// Construct a SAMLEndpoint.
        /// </summary>
        /// <param name="name">The name of the endpoint.</param>
        /// <param name="endpointUri">The URI of the endpoint.</param>
        /// <param name="authenticationType">The authentication type of the endpoint.</param>
        public SAMLEndpoint(string name, Uri endpointUri, SAMLAuthenticationType authenticationType)
        {
            SetProperties(name, endpointUri, authenticationType);
        }

        private void SetProperties(string name, Uri endpointUri, SAMLAuthenticationType authenticationType)
        {
            if (!string.Equals(endpointUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("EndpointUri is not Https protocol.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is null or empty.");
            }

            Name = name;
            EndpointUri = endpointUri;
            AuthenticationType = authenticationType;
        }
    }
#endif
}
