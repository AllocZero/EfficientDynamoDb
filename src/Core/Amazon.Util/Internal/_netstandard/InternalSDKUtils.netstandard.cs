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
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Amazon.Util.Internal
{
    public static partial class InternalSDKUtils
    {
        private const string UnknownPlaceholder = "Unknown";
        private const string UnknownPlatform = "unknown_platform";

        private static string _userAgentBaseName = "aws-sdk-dotnet-netstandard";
        private static readonly string SpecialPlatformInformation;

#pragma warning disable CA1810 // Initialize reference type static fields inline
        static InternalSDKUtils()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            if (GetExecutionEnvironment() == null)
            {
                try
                {
                    SpecialPlatformInformation = GetXamarinInformation() ?? GetUnityInformation();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    SpecialPlatformInformation = null;
                }
            }
        }

        private static string GetXamarinInformation()
        {
            var xamarinDevice = Type.GetType("Xamarin.Forms.Device, Xamarin.Forms.Core");
            if (xamarinDevice == null)
            {
                return null;
            }

            var runtime = xamarinDevice.GetProperty("RuntimePlatform")?.GetValue(null)?.ToString() ?? "";
            var idiom = xamarinDevice.GetProperty("Idiom")?.GetValue(null)?.ToString() ?? "";

            var platform = runtime + idiom;

            if (string.IsNullOrEmpty(platform))
            {
                platform = UnknownPlatform;
            }

            return string.Format(CultureInfo.InvariantCulture, "Xamarin_{0}", "Xamarin");
        }

        private static string GetUnityInformation()
        {
            var unityApplication = Type.GetType("UnityEngine.Application, UnityEngine.CoreModule");
            if (unityApplication == null)
            {
                return null;
            }

            var platform = unityApplication.GetProperty("platform")?.GetValue(null)?.ToString() ?? UnknownPlatform;

            return string.Format(CultureInfo.InvariantCulture, "Unity_{0}", platform);
        }

        private static string GetValidSubstringOrUnknown(string str, int start, int end)
        {
            if ((start != -1) && (end != -1) &&
                (0 <= start) && (end <= str.Length))
            {
                string substr = str.Substring(start, end-start);
                if (!string.IsNullOrWhiteSpace(substr))
                {
                    return substr.Trim().Replace(' ', '_');
                }

            }
            return UnknownPlaceholder;
        }

        /// <summary>
        /// Returns the type of platform and version.!-- If on a special platform, a static "0" is used as the version since
        /// we have nothing more specific that actually means anything. Otherwise, asks InteropServices RuntimeInformation for
        /// the OSDescription and trims off the OS name.
        /// </summary>
        public static string DetermineFramework()
        {
            if (SpecialPlatformInformation != null)
            {
                if(SpecialPlatformInformation.StartsWith("Xamarin"))
                {
                    return "Xamarin/0";
                }
                if (SpecialPlatformInformation.StartsWith("Unity"))
                {
                    return "Unity/0";
                }
            }
            try
            {
                var desc = RuntimeInformation.FrameworkDescription.Trim();
                return string.Format(CultureInfo.InvariantCulture, ".NET_Core/{0}", GetValidSubstringOrUnknown(desc, desc.LastIndexOf(' ') + 1, desc.Length));
            }
            catch
            {
                return UnknownPlaceholder;
            }
        }


        /// <summary>
        /// Returns the special platform information (e.g. Unity_OSXEditor, Xamarin_AndroidTablet) if
        /// on those platforms, otherwise asks InteropServices RuntimeInformation for the OSDescription
        /// and trims off the version.
        /// </summary>
        public static string DetermineOS()
        {
            if (SpecialPlatformInformation != null)
            {
                return SpecialPlatformInformation;
            }
            try
            {
                var desc = RuntimeInformation.OSDescription.Trim();
                return GetValidSubstringOrUnknown(desc, 0, desc.LastIndexOf(' '));
            }
            catch
            {
                return UnknownPlaceholder;
            }
        }

        /// <summary>
        /// Returns the special platform information (e.g. Unity_OSXEditor, Xamarin_AndroidTablet) if
        /// on those platforms, otherwise asks InteropServices RuntimeInformation for the OSDescription,
        /// keeping the version tail.
        /// </summary>
        public static string PlatformUserAgent()
        {
            try
            {
                var desc = SpecialPlatformInformation ?? RuntimeInformation.OSDescription;
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    return desc.Trim().Replace(' ', '_');
                }
                return UnknownPlaceholder;
            }
            catch
            {
                return UnknownPlaceholder;
            }
        }
    }
}
