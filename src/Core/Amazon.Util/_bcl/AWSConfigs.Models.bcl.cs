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

using Amazon.Runtime;
using System;
using System.Collections.Generic;
using Amazon.Util.Internal;

namespace Amazon.Util
{

    #region Basic sections

    /// <summary>
    /// Settings for configuring a proxy for the SDK to use.
    /// </summary>
    public partial class ProxyConfig
    {
        internal void Configure(ProxySection section)
        {
            if (section.ElementInformation.IsPresent)
            {
                Host = section.Host;
                Port = section.Port;
                Username = section.Username;
                Password = section.Password;
                BypassList = new List<string>(section.BypassList);
                BypassOnLocal = section.BypassOnLocal.GetValueOrDefault();
            }
        }
    }

    /// <summary>
    /// Settings for logging in the SDK.
    /// </summary>
    public partial class LoggingConfig
    {
        internal void Configure(LoggingSection section)
        {
            if (section.ElementInformation.IsPresent)
            {
                LogTo = section.LogTo;
                LogResponses = section.LogResponses;
                LogResponsesSizeLimit = section.LogResponsesSizeLimit ?? LoggingConfig.DefaultLogResponsesSizeLimit;
                LogMetrics = section.LogMetrics.GetValueOrDefault(false);
                LogMetricsFormat = section.LogMetricsFormat;
                if (section.LogMetricsCustomFormatter != null
                    && typeof(IMetricsFormatter).IsAssignableFrom(section.LogMetricsCustomFormatter))
                {
                    LogMetricsCustomFormatter = Activator.CreateInstance(section.LogMetricsCustomFormatter) as IMetricsFormatter;
                }
            }
        }
    }
    /// <summary>
    /// Settings for configuring CSM in the SDK.
    /// </summary>
    public partial class CSMConfig
    {
        internal void Configure(CSMSection section)
        {
            if (section.ElementInformation.IsPresent)
            {
                CSMEnabled = section.CSMEnabled;
                CSMClientId = section.CSMClientId;
                if (section.CSMPort.HasValue)
                {
                    CSMPort = section.CSMPort.GetValueOrDefault();
                }
            }
        }
    }


    #endregion

}
