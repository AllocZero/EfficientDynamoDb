/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *
 */

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.Util
{

    #region Basic sections

    /// <summary>
    /// Settings for configuring a proxy for the SDK to use.
    /// </summary>
    public partial class ProxyConfig
    {
        /// <summary>
        /// The host name or IP address of the proxy server.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port number of the proxy.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// The username to authenticate with the proxy server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to authenticate with the proxy server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Collection of one or more regular expressions denoting addresses
        /// for which the proxy will not be used.
        /// </summary>
        /// <remarks>
        ///  For more information on bypass lists 
        ///  see https://msdn.microsoft.com/en-us/library/system.net.webproxy.bypasslist%28v=vs.110%29.aspx.
        /// </remarks>
        public List<string> BypassList { get; set; }

        /// <summary>
        /// If true requests to local addresses bypass the configured
        /// proxy.
        /// </summary>
        public bool BypassOnLocal { get; set; }

        internal ProxyConfig()
        {
        }
    }

    /// <summary>
    /// Settings for logging in the SDK.
    /// </summary>
    public partial class LoggingConfig
    {
        // Default limit for response logging is 1 KB.
        public static readonly int DefaultLogResponsesSizeLimit = 1024; 

        private LoggingOptions _logTo;

        /// <summary>
        /// Logging destination.
        /// </summary>
        public LoggingOptions LogTo
        {
            get { return _logTo; }
            set
            {
                _logTo = value;
                AWSConfigs.OnPropertyChanged(AWSConfigs.LoggingDestinationProperty);
            }
        }
        /// <summary>
        /// When to log responses.
        /// </summary>
        public ResponseLoggingOption LogResponses { get; set; }

        /// <summary>        
        /// Gets or sets the size limit in bytes for logged responses.
        /// If logging for response body is enabled, logged response
        /// body is limited to this size. The default limit is 1KB.
        /// </summary>
        public int LogResponsesSizeLimit { get; set; }

        /// <summary>
        /// Whether or not to log SDK metrics.
        /// </summary>
        public bool LogMetrics { get; set; }

        public LogMetricsFormatOption LogMetricsFormat { get; set; }

        /// <summary>
        /// A custom formatter added through Configuration
        /// </summary>
        public Amazon.Runtime.IMetricsFormatter LogMetricsCustomFormatter { get; set; }

        internal LoggingConfig()
        {
            LogTo = AWSConfigs._logging;
            LogResponses = AWSConfigs._responseLogging;
            LogResponsesSizeLimit = DefaultLogResponsesSizeLimit;
            LogMetrics = AWSConfigs._logMetrics;
        }
    }

    public partial class CSMConfig
    {
        internal const string DEFAULT_HOST = "127.0.0.1";
        internal const int DEFAULT_PORT = 31000;

        public string CSMHost { get; set; } = DEFAULT_HOST;

        public int CSMPort { get; set; } = DEFAULT_PORT;

        public string CSMClientId { get; set; }

        public bool? CSMEnabled { get; set; }
    }
    #endregion

}
