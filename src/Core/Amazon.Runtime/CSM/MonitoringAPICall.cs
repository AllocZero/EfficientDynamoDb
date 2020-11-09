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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// CSM event base class
    /// </summary>
    public class MonitoringAPICall
    {
        public MonitoringAPICall(IRequestContext requestContext)
           : this()
        {
            Service = requestContext.ServiceMetaData.ServiceId;
            Api = CSMUtilities.GetApiNameFromRequest(requestContext.RequestName, requestContext.ServiceMetaData.OperationNameMapping, Service);
        }
        public MonitoringAPICall()
        {
            Timestamp = AWSSDKUtils.ConvertDateTimetoMilliseconds(DateTime.UtcNow);
            ClientId = DeterminedCSMConfiguration.Instance.CSMConfiguration.ClientId;
        }

        /// <summary>
        /// Contains the operation name for the api call being made
        /// </summary>
        public string Api { get; internal set; }

        /// <summary>
        /// Contains the service id ServiceId Sep of the service 
        /// against which the call is being made
        /// </summary>
        public string Service { get; internal set; }

        /// <summary>
        /// Contains the "ClientId" configuration value
        /// computed from the CSMConfigChain class
        /// </summary>
        public string ClientId { get; internal set; }

        /// <summary>
        /// Contains the elapsed time, in milliseconds, 
        /// since January 1st, 1970, for the time point 
        /// at which the event occurred
        /// </summary>
        public long Timestamp { get; internal set; }

        /// <summary>
        /// Contains "ApiCall" or "ApiCallAttempt" 
        /// based on what the monitoring event is describing
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Contains the enum of the above CSM type property
        /// </summary>
        protected enum CSMType
        {
            ApiCall,
            ApiCallAttempt
        }

        /// <summary>
        /// Contains the "Version" configuration value
        /// computed from the CSMConfigChain class
        /// Defaults to 1.
        /// </summary>
        public int Version { get; internal set; } = 1;

        /// <summary>
        /// Contains the signing region used by the 
        /// service client that made the request attempt.
        /// </summary>
        public string Region { get; internal set; }

        /// <summary>
        /// Contains the full value of the SDK's default 
        /// user agent header for http requests.
        /// </summary>
        public string UserAgent { get; internal set; }
    }
}
