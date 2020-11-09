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
    /// Interface for a single logged message.
    /// </summary>
    public interface ILogMessage
    {
        /// <summary>
        /// Log message format.
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Log message arguments.
        /// </summary>
        object[] Args { get; }

        /// <summary>
        /// Culture-specific formatting provider.
        /// </summary>
        IFormatProvider Provider { get; }
    }
}
