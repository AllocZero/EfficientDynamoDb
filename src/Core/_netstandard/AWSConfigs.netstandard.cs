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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon
{
    /// <summary>
    /// Configuration options that apply to the entire SDK.
    /// </summary>
    public static partial class AWSConfigs
    {
        public static string GetConfig(string name)
        {
            return null;
        }

        internal static bool XmlSectionExists(string sectionName)
        {
            return false;
        }

        public static HttpClientFactory HttpClientFactory { get; set; }

#region TraceListeners
#if !NETSTANDARD13
        private static Dictionary<string, List<TraceListener>> _traceListeners
            = new Dictionary<string, List<TraceListener>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Add a listener for SDK logging. 
        /// </summary>
        /// <remarks>If the listener does not have a name, you will not be able to remove it later.</remarks>
        /// <param name="source">The source to log for, e.g. "Amazon", or "Amazon.DynamoDB".</param>
        /// <param name="listener">The listener to add.</param>
        public static void AddTraceListener(string source, TraceListener listener)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source cannot be null or empty", "source");
            if (null == listener)
                throw new ArgumentException("Listener cannot be null", "listener");

            lock (_traceListeners)
            {
                if (!_traceListeners.ContainsKey(source))
                    _traceListeners.Add(source, new List<TraceListener>());
                _traceListeners[source].Add(listener);
            }

            Logger.ClearLoggerCache();
        }

        /// <summary>
        /// Remove a trace listener from SDK logging.
        /// </summary>
        /// <param name="source">The source the listener was added to.</param>
        /// <param name="name">The name of the listener.</param>
        public static void RemoveTraceListener(string source, string name)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source cannot be null or empty", "source");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", "name");

            lock (_traceListeners)
            {
                if (_traceListeners.ContainsKey(source))
                {
                    foreach (var l in _traceListeners[source])
                    {
                        if (l.Name.Equals(name, StringComparison.Ordinal))
                        {
                            _traceListeners[source].Remove(l);
                            break;
                        }
                    }
                }
            }

            Logger.ClearLoggerCache();
        }

        // Used by Logger.Diagnostic to add listeners to TraceSources when loggers 
        // are created.
        internal static TraceListener[] TraceListeners(string source)
        {
            lock (_traceListeners)
            {
                List<TraceListener> temp;

                if (_traceListeners.TryGetValue(source, out temp))
                {
                    return temp.ToArray();
                }

                return new TraceListener[0];
            }
        }
#endif
#endregion
    }
}
