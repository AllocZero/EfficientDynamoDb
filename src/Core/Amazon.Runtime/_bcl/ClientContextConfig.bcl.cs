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

using System.Collections;
using System;

using Amazon.Util;
using Amazon.Util.Internal;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Provides information for Client Context header. 
    /// Client Context header needs information like App title, version code, version name, package name etc.
    /// </summary>
    public class ClientContextConfig
    {         
        /// <summary>
        /// The title of your app. For example, "My App".
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string AppTitle { get; set; }

        /// <summary>
        /// The version for your app. For example, V3.0.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string AppVersionName { get; set; }

        /// <summary>
        /// The version code of your app. For example, 3.0.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string AppVersionCode { get; set; }

        /// <summary>
        /// The name of your app package. For example, com.your_company.your_app.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string AppPackageName { get; set; }

        /// <summary>
        /// The operating system of the device. For example, iPhoneOS.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// The version of the operating system of the device. For example, 8.1.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string PlatformVersion { get; set; }

        /// <summary>
        /// The locale of the device. For example, en_US.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The manufacturer of the device. For example, Samsung.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// The model of the device. For example, Nexus.
        /// If this property is not null, the value would be used in Client Context header.
        /// </summary>
        public string Model { get; set; }
    }
}
