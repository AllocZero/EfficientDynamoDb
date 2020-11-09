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
using System.Net;
using Amazon.Util;
using System.Globalization;
using System.Collections.Generic;

namespace Amazon.Runtime
{
    /// <summary>
    /// This class is the base class of all the configurations settings to connect
    /// to a service.
    /// </summary>    
    public abstract partial class ClientConfig
    {
        private string proxyHost;
        private int proxyPort = -1;
        private List<string> proxyBypassList;
        private int? connectionLimit;
        private int? maxIdleTime;
        private bool useNagleAlgorithm = false;
        
        private static RegionEndpoint GetDefaultRegionEndpoint()
        {
            return FallbackRegionFactory.GetRegionEndpoint();
        }

        /// <summary>
        /// Gets and sets of the ProxyHost property.
        /// </summary>
        public string ProxyHost
        {
            get 
            {
                if (string.IsNullOrEmpty(this.proxyHost))
                    return AWSConfigs.ProxyConfig.Host;

                return this.proxyHost; 
            }
            set { this.proxyHost = value; }
        }


        /// <summary>
        /// Gets and sets the ProxyPort property.
        /// </summary>
        public int ProxyPort
        {
            get 
            {
                if (this.proxyPort <= 0)
                    return AWSConfigs.ProxyConfig.Port.GetValueOrDefault();

                return this.proxyPort; 
            }
            set { this.proxyPort = value; }
        }

        /// <summary>
        /// Gets and sets the ProxyBypassList property; a collection
        /// of regular expressions denoting the set of endpoints for
        /// which the configured proxy host will be bypassed.
        /// </summary>
        /// <remarks>
        ///  For more information on bypass lists 
        ///  see https://msdn.microsoft.com/en-us/library/system.net.webproxy.bypasslist%28v=vs.110%29.aspx.
        /// </remarks>
        public List<string> ProxyBypassList
        {
            get
            {
                if (this.proxyBypassList == null)
                    return AWSConfigs.ProxyConfig.BypassList;

                return this.proxyBypassList;
            }
            set 
            {
                this.proxyBypassList = value != null ? new List<string>(value) : null;
            }
        }

        /// <summary>
        /// Gets and sets the ProxyBypassOnLocal property.
        /// If set true requests to local addresses bypass the configured
        /// proxy.
        /// </summary>
        public bool ProxyBypassOnLocal { get; set; }

        /// <summary>
        /// Returns a WebProxy instance configured to match the proxy settings
        /// in the client configuration.
        /// </summary>
        public WebProxy GetWebProxy()
        {
            const string httpPrefix = "http://";

            WebProxy proxy = null;
            if (!string.IsNullOrEmpty(ProxyHost) && ProxyPort > 0)
            {
                // WebProxy constructor adds the http:// prefix, but doesn't
                // account for cases where it's already present which leads to
                // malformed addresses
                var host = ProxyHost.StartsWith(httpPrefix, StringComparison.OrdinalIgnoreCase)
                               ? ProxyHost.Substring(httpPrefix.Length)
                               : ProxyHost;
                proxy = new WebProxy(host, ProxyPort);

                if (ProxyCredentials != null)
                {
                    proxy.Credentials = ProxyCredentials;
                }
                if (ProxyBypassList != null)
                {
                    proxy.BypassList =  ProxyBypassList.ToArray();
                }
                proxy.BypassProxyOnLocal = ProxyBypassOnLocal;
            }

            return proxy;
        }

        /// <summary>
        /// Unpacks the host, port and any credentials info into the instance's
        /// proxy-related fields.
        /// </summary>
        /// <param name="proxy">The proxy details</param>
        public void SetWebProxy(WebProxy proxy)
        {
            if (proxy == null)
                throw new ArgumentNullException("proxy");

            var address = proxy.Address;
            ProxyHost = address.Host;
            ProxyPort = address.Port;
            ProxyBypassList = new List<string>(proxy.BypassList);
            ProxyBypassOnLocal = proxy.BypassProxyOnLocal;
            ProxyCredentials = proxy.Credentials;
        }

        /// <summary>
        /// Gets and sets the max idle time set on the ServicePoint for the WebRequest.
        /// Default value is 50 seconds (50,000 ms) unless ServicePointManager.MaxServicePointIdleTime is set,
        /// in which case ServicePointManager.MaxServicePointIdleTime will be used as the default.
        /// </summary>
        public int MaxIdleTime
        {
            get { return AWSSDKUtils.GetMaxIdleTime(this.maxIdleTime); }
            set { this.maxIdleTime = value; }
        }

        /// <summary>
        /// Gets and sets the connection limit set on the ServicePoint for the WebRequest.
        /// Default value is 50 connections unless ServicePointManager.DefaultConnectionLimit is set in 
        /// which case ServicePointManager.DefaultConnectionLimit will be used as the default.
        /// </summary>
        public int ConnectionLimit
        {
            get { return AWSSDKUtils.GetConnectionLimit(this.connectionLimit); }
            set { this.connectionLimit = value; }
        }

        /// <summary>
        /// Gets or sets a Boolean value that determines whether the Nagle algorithm is used on connections managed by the ServicePoint object used
        /// for requests to AWS. This is defaulted to false for lower latency with responses that return small amount of data. This is the opposite
        /// default than ServicePoint.UseNagleAlgorithm which is optimized for large responses like web pages or images.
        /// </summary>
        public bool UseNagleAlgorithm
        {
            get { return this.useNagleAlgorithm; }
            set { this.useNagleAlgorithm = value; }
        }
    }
}
