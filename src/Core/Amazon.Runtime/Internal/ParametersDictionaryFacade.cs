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
using System.Collections;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// IDictionary{String, String} access to ParameterCollection.
    /// TODO: remove this class in version 3.4 of the SDK.
    /// </summary>
    public class ParametersDictionaryFacade : IDictionary<String, String>
    {
        private readonly ParameterCollection _parameterCollection;

        /// <summary>
        /// Constructs ParametersDictionaryFacade for a ParameterCollection
        /// </summary>
        /// <param name="collection"></param>
        public ParametersDictionaryFacade(ParameterCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            _parameterCollection = collection;
        }

        // Converts ParameterValue to a string representation
        private static string ParameterValueToString(ParameterValue pv)
        {
            if (pv == null)
                throw new ArgumentNullException("pv");

            var spv = pv as StringParameterValue;
            var slpv = pv as StringListParameterValue;

            if (spv != null)
                return spv.Value;
            else if (slpv != null)
            {
                var json = ThirdParty.Json.LitJson.JsonMapper.ToJson(slpv.Value);
                return json;
            }
            else
                throw new AmazonClientException("Unexpected parameter value type " + pv.GetType().FullName);
        }
        // Update a ParameterValue with a string representation of its value
        private static void UpdateParameterValue(ParameterValue pv, string newValue)
        {
            if (pv == null)
                throw new ArgumentNullException("pv");

            var spv = pv as StringParameterValue;
            var slpv = pv as StringListParameterValue;

            if (spv != null)
            {
                spv.Value = newValue;
            }
            else if (slpv != null)
            {
                var stringList = ThirdParty.Json.LitJson.JsonMapper.ToObject<List<string>>(newValue);
                slpv.Value = stringList;
            }
            else
            {
                throw new AmazonClientException("Unexpected parameter value type " + pv.GetType().FullName);
            }
        }

        #region IDictionary<String, String> methods

        public void Add(string key, string value)
        {
            _parameterCollection.Add(key, value);
        }

        public int Count
        {
            get { return _parameterCollection.Count; }
        }

        public bool ContainsKey(string key)
        {
            return _parameterCollection.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _parameterCollection.Remove(key);
        }

        public string this[string key]
        {
            get
            {
                var pv = _parameterCollection[key];
                var s = ParameterValueToString(pv);
                return s;
            }
            set
            {
                ParameterValue pv;
                if (_parameterCollection.TryGetValue(key, out pv))
                {
                    UpdateParameterValue(pv, value);
                }
                else
                {
                    // if not updating existing ParameterValue, we only
                    // support creating StringParameterValue
                    pv = new StringParameterValue(value);
                }
                _parameterCollection[key] = pv;
            }
        }

        public ICollection<string> Keys
        {
            get { return _parameterCollection.Keys; }
        }

        public bool TryGetValue(string key, out string value)
        {
            ParameterValue pv;
            if (_parameterCollection.TryGetValue(key, out pv))
            {
                value = ParameterValueToString(pv);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            if (this.Contains(item))
                return _parameterCollection.Remove(item.Key);
            else
                return false;
        }

        public ICollection<string> Values
        {
            get
            {
                var values = new List<string>();
                foreach(var kvp in _parameterCollection)
                {
                    var stringValue = ParameterValueToString(kvp.Value);
                    values.Add(stringValue);
                }
                return values;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            // when not updating existing ParameterValue, we only
            // support creating StringParameterValue
            var pv = new StringParameterValue(item.Value);
            _parameterCollection.Add(item.Key, pv);
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            var key = item.Key;
            var value = item.Value;

            ParameterValue pv;
            if (_parameterCollection.TryGetValue(key, out pv))
            {
                var expectedValue = ParameterValueToString(pv);
                return string.Equals(expectedValue, value, StringComparison.Ordinal);
            }
            else
                return false;
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if (array.Length - arrayIndex < _parameterCollection.Count)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "Not enough space in target array");
            }
            
            foreach(var kvp in this)
            {
                array[arrayIndex++] = kvp;
            }
        }

        public bool IsReadOnly
        {
            get { return (_parameterCollection as IDictionary).IsReadOnly; }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var kvp in _parameterCollection)
            {
                var key = kvp.Key;
                var stringValue = ParameterValueToString(kvp.Value);
                var result = new KeyValuePair<string, string>(key, stringValue);
                yield return result;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _parameterCollection.Clear();
        }

        #endregion
    }
}
