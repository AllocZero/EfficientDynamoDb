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
using System.Globalization;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// A single logged message
    /// </summary>
    public class LogMessage : ILogMessage
    {
        /// <summary>
        /// Log message arguments.
        /// </summary>
        public object[] Args { get; private set; }

        /// <summary>
        /// Culture-specific formatting provider.
        /// </summary>
        public IFormatProvider Provider { get; private set; }

        /// <summary>
        /// Log message format.
        /// </summary>
        public string Format { get; private set; }

        public LogMessage(string message) : this(CultureInfo.InvariantCulture, message) { }
        public LogMessage(string format, params object[] args) : this(CultureInfo.InvariantCulture, format, args) { }
        public LogMessage(IFormatProvider provider, string format, params object[] args)
        {
            Args = args;
            Format = format;
            Provider = provider;
        }

        public override string ToString()
        {
            return string.Format(Provider, Format, Args);
        }
    }
}
