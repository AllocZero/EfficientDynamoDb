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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

using Microsoft.Win32;
using System.Globalization;

namespace Amazon.Util.Internal
{
    public static partial class InternalSDKUtils
    {
        private const string MobileServicesFolderName = "AWS Mobile Services";

#if BCL45
        static string _userAgentBaseName = "aws-sdk-dotnet-45";
#else
        static string _userAgentBaseName = "aws-sdk-dotnet-35";
#endif
        public static string DetermineFramework()
        {
            try
            {
                if (Environment.Version.Major >= 4 && Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Net Framework Setup\\NDP\\v4") != null)
                    return "4.0";
                if (Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Net Framework Setup\\NDP\\v3.5") != null)
                    return "3.5";
                if (Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Net Framework Setup\\NDP\\v3.0") != null)
                    return "3.0";
                if (Registry.LocalMachine.OpenSubKey(@"Software\\Microsoft\\Net Framework Setup\\NDP\\v2.0.50727") != null)
                    return "2.0";
            }
            catch
            {
            }

            return "Unknown";
        }

        public static string DetermineAppLocalStoragePath()
        {
            if (string.IsNullOrEmpty(AWSConfigs.ApplicationName))
                throw new InvalidOperationException("AWSConfigs.ApplicationName does not have a valid value.");

            var basePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), MobileServicesFolderName);
            var basePathForApp = System.IO.Path.Combine(basePath, AWSConfigs.ApplicationName);
            return basePathForApp;
        }

        public static string DetermineAppLocalStoragePath(string fileName)
        {
            return System.IO.Path.Combine(DetermineAppLocalStoragePath(), fileName);
        }
    }
}
