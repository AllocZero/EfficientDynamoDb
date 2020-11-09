#if BCL || (NETSTANDARD && !NETSTANDARD13)
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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Logger wrapper for System.Diagnostics.TraceSource logger.
    /// </summary>
    internal class InternalSystemDiagnosticsLogger : InternalLogger
    {
        volatile int eventId = 0;
        TraceSource trace;

        public InternalSystemDiagnosticsLogger(Type declaringType)
            : base(declaringType)
        {
            this.trace = TraceSourceUtil.GetTraceSource(declaringType);
        }

        #region Overrides

        public override void Flush()
        {
            if (trace != null)
                this.trace.Flush();
        }

        public override void Error(Exception exception, string messageFormat, params object[] args)
        {
            trace.TraceData(TraceEventType.Error, eventId++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args), exception);
        }

        public override void Debug(Exception exception, string messageFormat, params object[] args)
        {
            trace.TraceData(TraceEventType.Verbose, eventId++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args), exception);
        }

        public override void DebugFormat(string messageFormat, params object[] args)
        {
            trace.TraceData(TraceEventType.Verbose, eventId++, new LogMessage(CultureInfo.InvariantCulture, messageFormat, args));
        }

        public override void InfoFormat(string message, params object[] arguments)
        {
            trace.TraceData(TraceEventType.Information, eventId++, new LogMessage(CultureInfo.InvariantCulture, message, arguments));
        }

        public override bool IsDebugEnabled { get { return (trace != null); } }

        public override bool IsErrorEnabled { get { return (trace != null); } }

        public override bool IsInfoEnabled { get { return (trace != null); } }

        #endregion
    }

    /// <summary>
    /// Creates TraceRoute for a given Type or the closest "parent" that has a listener configured.
    /// Example: if type is Amazon.DynamoDB.AmazonDynamoDBClient, listeners can be configured for:
    /// -Amazon.DynamoDB.AmazonDynamoDBClient
    /// -Amazon.DynamoDB
    /// -Amazon
    /// The first matching TraceSource with listeners will be used.
    /// If no listeners are configured for type or one of its "parents", will return null.
    /// </summary>
    internal static class TraceSourceUtil
    {
        #region Public methods

        /// <summary>
        /// Gets a TraceSource for given Type with SourceLevels.All.
        /// If there are no listeners configured for targetType or one of its "parents", returns null.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static TraceSource GetTraceSource(Type targetType)
        {
            return GetTraceSource(targetType, SourceLevels.All);
        }

        /// <summary>
        /// Gets a TraceSource for given Type and SourceLevels.
        /// If there are no listeners configured for targetType or one of its "parents", returns null.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="sourceLevels"></param>
        /// <returns></returns>
        public static TraceSource GetTraceSource(Type targetType, SourceLevels sourceLevels)
        {
            TraceSource traceSource = GetTraceSourceWithListeners(targetType.FullName, sourceLevels);
            return traceSource;
        }

        #endregion

        #region Private methods

        // Gets the name of the closest "parent" TraceRoute that has listeners, or null otherwise.
        private static TraceSource GetTraceSourceWithListeners(string name, SourceLevels sourceLevels)
        {
            string[] parts = name.Split(new char[] { '.' }, StringSplitOptions.None);

            List<string> namesToTest = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var part in parts)
            {
                if (sb.Length > 0)
                    sb.Append(".");
                sb.Append(part);

                string partialName = sb.ToString();
                namesToTest.Add(partialName);
            }

            namesToTest.Reverse();
            foreach (var testName in namesToTest)
            {
                TraceSource ts = new TraceSource(testName, sourceLevels);

                ts.Listeners.AddRange(AWSConfigs.TraceListeners(testName));

                // no listeners? skip
                if (ts.Listeners == null || ts.Listeners.Count == 0)
                {
                    ts.Close();
                    continue;
                }
                // more than one listener? use this TraceSource
                if (ts.Listeners.Count > 1)
                    return ts;
                TraceListener listener = ts.Listeners[0];
                // single listener isn't DefaultTraceListener? use this TraceRoute
                if (!(listener is DefaultTraceListener))
                    return ts;
                // single listener is DefaultTraceListener but isn't named Default? use this TraceRoute
                if (!string.Equals(listener.Name, "Default", StringComparison.Ordinal))
                    return ts;

                // not the TraceSource we're looking for, close it
                ts.Close();
            }

            // nothing found? no listeners are configured for any of the names, even the original,
            // so return null to signify failure
            return null;
        }

        #endregion
    }
}
#endif