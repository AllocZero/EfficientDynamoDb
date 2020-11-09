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
using System.IO;
using System.Text;
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace Amazon.Runtime
{
    /// <summary>
    /// Abstract parameter value.
    /// </summary>
    public abstract class ParameterValue
    {
    }

    /// <summary>
    /// String parameter value.
    /// </summary>
    public class StringParameterValue : ParameterValue
    {
        /// <summary>
        /// String value of the parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Constructs ParameterValue for a single string.
        /// </summary>
        /// <param name="value"></param>
        public StringParameterValue(string value)
        {
            Value = value;
        }

        internal StringParameterValue() { }
    }

    /// <summary>
    /// String list parameter value.
    /// </summary>
    public class StringListParameterValue : ParameterValue
    {
        /// <summary>
        /// List of strings value of the parameter.
        /// </summary>
        public List<string> Value { get; set; }

        /// <summary>
        /// Constructs ParameterValue for a list of strings.
        /// </summary>
        /// <param name="values"></param>
        public StringListParameterValue(List<string> values)
        {
            Value = values;
        }

        internal StringListParameterValue() { }
    }
}
