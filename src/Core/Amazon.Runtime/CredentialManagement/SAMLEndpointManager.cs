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
using Amazon.Runtime;
using Amazon.Runtime.Internal.Settings;
using Amazon.Runtime.Internal.Util;
using Amazon.Util.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Amazon.Runtime.CredentialManagement
{
#if !NETSTANDARD13
    /// <summary>
    /// Manages endpoints to be used in conjunction with SAML role profiles. The role profiles
    /// reference the endpoint to obtain the actual endpoint and any customization settings
    /// needed to perform authentication.
    /// </summary>
    public class SAMLEndpointManager
    {
        private NamedSettingsManager settingsManager;

        /// <summary>
        /// Construct a SAMLEndpointManager.
        /// </summary>
        public SAMLEndpointManager()
        {
            settingsManager = new NamedSettingsManager(SettingsConstants.RegisteredSAMLEndpoints);
        }

        /// <summary>
        /// True if the SAMLEndpointManager is available on the current platform, false otherwise.
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                return NamedSettingsManager.IsAvailable;
            }
        }

        /// <summary>
        /// Register an endpoint, or update an existing one.
        /// </summary>
        /// <param name="samlEndpoint">The endpoint to register.</param>
        public void RegisterEndpoint(SAMLEndpoint samlEndpoint)
        {
            var properties = new Dictionary<string, string>()
            {
                { SettingsConstants.EndpointField, samlEndpoint.EndpointUri.ToString() },
                { SettingsConstants.AuthenticationTypeField, samlEndpoint.AuthenticationType.ToString() }
            };
            settingsManager.RegisterObject(samlEndpoint.Name, properties);
        }

        /// <summary>
        /// Get an endpoint, if it exists and is valid.
        /// </summary>
        /// <param name="endpointName">The name of the endpoint to get.</param>
        /// <param name="samlEndpoint">The endpoint, or null if it's invalid or doesn't exist.</param>
        /// <returns>True if the endpoint exists and is valid, false otherwise.</returns>
        public bool TryGetEndpoint(string endpointName, out SAMLEndpoint samlEndpoint)
        {
            samlEndpoint = null;

            try
            {
                samlEndpoint = GetEndpoint(endpointName);
            }
            catch (AmazonClientException e)
            {
                Logger.GetLogger(typeof(SAMLEndpointManager)).Error(e, "Unable to load SAML Endpoint '{0}'.",  endpointName);
            }

            return samlEndpoint != null;
        }

        /// <summary>
        /// Get an endpoint from the store.
        /// Throws an exception if there's an error reading the endpoint, or if it doesn't exist.
        /// </summary>
        /// <param name="endpointName">The name of the endpoint to get.</param>
        /// <returns>The endpoint with the given name.</returns>
        public SAMLEndpoint GetEndpoint(string endpointName)
        {
            Dictionary<string, string> properties;
            if (settingsManager.TryGetObject(endpointName, out properties))
            {
                try
                {
                    string authenticationType;
                    if (properties.TryGetValue(SettingsConstants.AuthenticationTypeField, out authenticationType))
                    {
                        return new SAMLEndpoint(endpointName, properties[SettingsConstants.EndpointField], authenticationType);
                    }
                    else
                    {
                        return new SAMLEndpoint(endpointName, properties[SettingsConstants.EndpointField], null);
                    }
                }
                catch (Exception e)
                {
                    throw new AmazonClientException(string.Format(CultureInfo.InvariantCulture, "Error reading A SAML endpoint with name {0}.", endpointName), e);
                }
            }
            else
            {
                throw new AmazonClientException(string.Format(CultureInfo.InvariantCulture, "There is no SAML endpoint registered with name {0}.", endpointName));
            }
        }

        /// <summary>
        /// Delete an endpoint from the store, if it exists.
        /// </summary>
        /// <param name="endpointName">The name of the endpoint to delete.</param>
        public void UnregisterEndpoint(string endpointName)
        {
            settingsManager.UnregisterObject(endpointName);
        }

        /// <summary>
        /// List the names of valid endpoints in the store.
        /// </summary>
        /// <returns></returns>
        public List<string> ListEndpointNames()
        {
            return settingsManager.ListObjectNames();
        }

        /// <summary>
        /// List valid endopints that can be read from the store.
        /// </summary>
        /// <returns></returns>
        public List<SAMLEndpoint> ListEndpoints()
        {
            var endpoints = new List<SAMLEndpoint>();
            foreach (var endpointName in settingsManager.ListObjectNames())
            {
                SAMLEndpoint endpoint;
                if (TryGetEndpoint(endpointName, out endpoint))
                {
                    endpoints.Add(endpoint);
                }
            }
            return endpoints;
        }
    }
#endif
}
