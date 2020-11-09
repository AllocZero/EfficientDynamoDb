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

using Amazon.Util.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Internal class that has the CSM configurations for the Monitoring Listener
    /// after being derived from the CSMConfigChain class.
    /// </summary>
    public sealed class DeterminedCSMConfiguration
    {
        private static readonly DeterminedCSMConfiguration instance = new DeterminedCSMConfiguration();
        public CSMConfiguration CSMConfiguration { get; set; }
        private DeterminedCSMConfiguration()
        {
            CSMConfiguration = new CSMFallbackConfigChain().GetCSMConfig();
        }

        static DeterminedCSMConfiguration()
        {
        }

        public static DeterminedCSMConfiguration Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
