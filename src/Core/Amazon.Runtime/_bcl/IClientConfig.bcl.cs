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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Amazon.Util;

using Amazon.Runtime.Internal.Auth;

namespace Amazon.Runtime
{
    public partial interface IClientConfig
    {
        /// <summary>
        /// Gets the ProxyHost property.
        /// </summary>
        string ProxyHost { get; }

        /// <summary>
        /// Gets the ProxyPort property.
        /// </summary>
        int ProxyPort { get; }

        /// <summary>
        /// Gets the max idle time set on the ServicePoint for the WebRequest.
        /// Default value is 50 seconds (50,000 ms) unless ServicePointManager.MaxServicePointIdleTime is set,
        /// in which case ServicePointManager.MaxServicePointIdleTime will be used as the default.
        /// </summary>
        int MaxIdleTime { get; }

        /// <summary>
        /// Gets the default read-write timeout value.
        /// </summary>
        /// <seealso cref="P:System.Net.HttpWebRequest.ReadWriteTimeout"/>
        /// <seealso cref="P:System.Net.Http.WebRequestHandler.ReadWriteTimeout"/>
        TimeSpan? ReadWriteTimeout { get; }

        /// <summary>
        /// Gets the connection limit set on the ServicePoint for the WebRequest.
        /// Default value is 50 connections unless ServicePointManager.DefaultConnectionLimit is set in 
        /// which case ServicePointManager.DefaultConnectionLimit will be used as the default.
        /// </summary>
        int ConnectionLimit { get; }

        /// <summary>
        /// Gets whether the Nagle algorithm is used on connections managed by the ServicePoint object used
        /// for requests to AWS. This is defaulted to false for lower latency with responses that return small amount of data. This is the opposite
        /// default than ServicePoint.UseNagleAlgorithm which is optimized for large responses like web pages or images.
        /// </summary>
        bool UseNagleAlgorithm { get; }


        /// <summary>
        /// Returns a WebProxy instance configured to match the proxy settings
        /// in the client configuration.
        /// </summary>
        WebProxy GetWebProxy();

    }
}