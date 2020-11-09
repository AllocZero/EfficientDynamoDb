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
using System.Linq;
using System.Text;
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Collection of parameters that an SDK client will send to a service.
    /// </summary>
    public class ParameterCollection : SortedDictionary<string, ParameterValue>
    {
        /// <summary>
        /// Constructs empty ParameterCollection.
        /// </summary>
        public ParameterCollection()
            : base(comparer: StringComparer.Ordinal) { }

        /// <summary>
        /// Adds a parameter with a string value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            Add(key, new StringParameterValue(value));
        }

        /// <summary>
        /// Adds a parameter with a list-of-strings value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void Add(string key, List<string> values)
        {
            Add(key, new StringListParameterValue(values));
        }

        /// <summary>
        /// Converts the current parameters into a list of key-value pairs.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string,string>> GetSortedParametersList()
        {
            return GetParametersEnumerable().ToList();
        }

        private IEnumerable<KeyValuePair<string, string>> GetParametersEnumerable()
        {
            foreach (var kvp in this)
            {
                var name = kvp.Key;
                var value = kvp.Value;

                var spv = value as StringParameterValue;
                var slpv = value as StringListParameterValue;

                if (spv != null)
                    yield return new KeyValuePair<string, string>(name, spv.Value);
                else if (slpv != null)
                {
                    var sortedList = slpv.Value;
                    sortedList.Sort(StringComparer.Ordinal);
                    foreach (var listValue in sortedList)
                        yield return new KeyValuePair<string, string>(name, listValue);
                }
                else
                {
                    throw new AmazonClientException("Unsupported parameter value type '" + value.GetType().FullName + "'");
                }
            }
        }
    }
}
