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

namespace Amazon.Runtime
{
    /// <summary>
    /// TcpKeepAlive class used to group all the different properties used for working with TCP keep-alives.
    /// </summary>
    public class TcpKeepAlive
    {
        private bool enabled = false;
        private TimeSpan? timeout = new TimeSpan(0, 5, 0);
        private TimeSpan? interval = new TimeSpan(0, 0, 15);

        /// <summary>
        /// Specifies if TCP keep-alive is enabled or disabled. The default value is false for all services except Lambda.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// The timeout before a TCP keep-alive packet will be sent. The timeout value must be greater 
        /// than 0 seconds and not null if Enabled is set to true. The default value is 5 minutes.
        /// </summary>
        public TimeSpan? Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// The interval before retrying a TCP keep-alive packet that did not receive an acknowledgement. The 
        /// interval must be greater than 0 seconds and not null if Enabled is set to true. The default value is 15 seconds.
        /// </summary>
        public TimeSpan? Interval
        {
            get { return interval; }
            set { interval = value; }
        }
    }
}
