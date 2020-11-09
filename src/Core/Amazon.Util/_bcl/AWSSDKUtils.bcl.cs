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

namespace Amazon.Util
{
    public static partial class AWSSDKUtils
    {
        public static void ForceCanonicalPathAndQuery(Uri uri)
        {
            try
            {
                string paq = uri.PathAndQuery; // need to access PathAndQuery
                FieldInfo flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
                ulong flags = (ulong)flagsFieldInfo.GetValue(uri);
                flags &= ~((ulong)0x30); // Flags.PathNotCanonical|Flags.QueryNotCanonical
                flagsFieldInfo.SetValue(uri, flags);
            }
            catch { }
        }

        static readonly object _preserveStackTraceLookupLock = new object();
        static bool _preserveStackTraceLookup = false;
        static MethodInfo _preserveStackTrace = null;
        /// <summary>
        /// This method is used preserve the stacktrace used from clients that support async calls.  This 
        /// make sure that exceptions thrown during EndXXX methods has the orignal stacktrace that happen 
        /// in the background thread.
        /// </summary>
        /// <param name="exception"></param>
        public static void PreserveStackTrace(Exception exception)
        {
            if (!_preserveStackTraceLookup)
            {
                lock (_preserveStackTraceLookupLock)
                {
                    _preserveStackTraceLookup = true;
                    try
                    {
                        _preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace",
                            BindingFlags.Instance | BindingFlags.NonPublic);
                    }
                    catch { }
                }
            }

            if (_preserveStackTrace != null)
            {
                _preserveStackTrace.Invoke(exception, null);
            }
        }

        private const int _defaultDefaultConnectionLimit = 2;
        internal static int GetConnectionLimit(int? clientConfigValue)
        {
            // Connection limit has been explicitly set on the client.
            if (clientConfigValue.HasValue)
                return clientConfigValue.Value;

            // If default has been left at the system default return the SDK default.
            if (ServicePointManager.DefaultConnectionLimit == _defaultDefaultConnectionLimit)
                return AWSSDKUtils.DefaultConnectionLimit;

            // The system default has been explicitly changed so we will honor that value.
            return ServicePointManager.DefaultConnectionLimit;
        }

        private const int _defaultMaxIdleTime = 100 * 1000;
        internal static int GetMaxIdleTime(int? clientConfigValue)
        {
            // MaxIdleTime has been explicitly set on the client.
            if (clientConfigValue.HasValue)
                return clientConfigValue.Value;

            // If default has been left at the system default return the SDK default.
            if (ServicePointManager.MaxServicePointIdleTime == _defaultMaxIdleTime)
                return AWSSDKUtils.DefaultMaxIdleTime;

            // The system default has been explicitly changed so we will honor that value.
            return ServicePointManager.MaxServicePointIdleTime;
        }

        public static void Sleep(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }
    }
}
