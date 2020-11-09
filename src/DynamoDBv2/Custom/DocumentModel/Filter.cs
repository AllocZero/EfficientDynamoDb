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
using System.Linq;

using Amazon.DynamoDBv2.Model;
using Amazon.Util;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Filter for use with scan and query operations
    /// </summary>
    public class Filter
    {
        #region Private members

        /// <summary>
        /// Filter conditions
        /// </summary>
        protected class FilterCondition
        {
            /// <summary>
            /// Gets the AttributeValues property.
            /// </summary>
            public List<AttributeValue> AttributeValues { get; private set; }

            /// <summary>
            /// Gets the DynamoDBEntries property.
            /// </summary>
            public List<DynamoDBEntry> DynamoDBEntries { get; private set; }

            /// <summary>
            /// Gets the ComparisonOperator property.
            /// </summary>
            public ComparisonOperator ComparisonOperator { get; private set; }

            /// <summary>
            /// Construct instance of FilterCondition
            /// </summary>
            /// <param name="comparisonOperator"></param>
            /// <param name="attributeValues"></param>
            public FilterCondition(ComparisonOperator comparisonOperator, List<AttributeValue> attributeValues)
            {
                ComparisonOperator = comparisonOperator;
                AttributeValues = attributeValues;
            }

            /// <summary>
            /// Construct instance of FilterCondition
            /// </summary>
            /// <param name="comparisonOperator"></param>
            /// <param name="dynamoDBEntries"></param>
            public FilterCondition(ComparisonOperator comparisonOperator, List<DynamoDBEntry> dynamoDBEntries)
            {
                ComparisonOperator = comparisonOperator;
                DynamoDBEntries = dynamoDBEntries;
            }

            /// <summary>
            /// Construct instance of FilterCondition
            /// </summary>
            /// <param name="condition"></param>
            public FilterCondition(Condition condition)
                : this(condition.ComparisonOperator, condition.AttributeValueList)
            { }

            /// <summary>
            /// Converts the FilterCondition to the Amazon.DynamoDBv2.Model.Condition object.
            /// </summary>
            /// <param name="conversion"></param>
            /// <returns></returns>
            public Condition ToCondition(DynamoDBEntryConversion conversion)
            {
                return ToCondition(conversion, shouldConvertToEpochSeconds: false, attributeName: null);
            }

            /// <summary>
            /// Converts the FilterCondition to the Amazon.DynamoDBv2.Model.Condition object.
            /// </summary>
            /// <param name="conversion"></param>
            /// <param name="shouldConvertToEpochSeconds"></param>
            /// <returns></returns>
            public Condition ToCondition(DynamoDBEntryConversion conversion, bool shouldConvertToEpochSeconds)
            {
                return ToCondition(conversion, shouldConvertToEpochSeconds: false, attributeName: null, isEmptyStringValueEnabled: shouldConvertToEpochSeconds);
            }

            /// <summary>
            /// Converts the FilterCondition to the Amazon.DynamoDBv2.Model.Condition object.
            /// </summary>
            /// <param name="conversion"></param>
            /// <param name="shouldConvertToEpochSeconds"></param>
            /// <param name="attributeName"></param>
            /// <returns></returns>
            public Condition ToCondition(DynamoDBEntryConversion conversion, bool shouldConvertToEpochSeconds, string attributeName)
            {
                return ToCondition(conversion, shouldConvertToEpochSeconds, attributeName, false);
            }

            /// <summary>
            /// Converts the FilterCondition to the Amazon.DynamoDBv2.Model.Condition object.
            /// </summary>
            /// <param name="conversion"></param>
            /// <param name="shouldConvertToEpochSeconds"></param>
            /// <param name="attributeName"></param>
            /// <param name="isEmptyStringValueEnabled"></param>
            /// <returns></returns>
            public Condition ToCondition(DynamoDBEntryConversion conversion, bool shouldConvertToEpochSeconds,
                string attributeName, bool isEmptyStringValueEnabled)
            {
                var attributeValues = AttributeValues;
                if (attributeValues == null)
                {
                    attributeValues = new List<AttributeValue>();
                    for (int i = 0; i < DynamoDBEntries.Count; i++)
                    {
                        var entry = DynamoDBEntries[i];
                        if (shouldConvertToEpochSeconds)
                            entry = Document.DateTimeToEpochSeconds(entry, attributeName);
                        
                        var attributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(conversion, isEmptyStringValueEnabled);
                        var attributeValue = entry.ConvertToAttributeValue(attributeConversionConfig);
                        attributeValues.Add(attributeValue);
                    }
                }

                var condition = new Condition
                {
                    ComparisonOperator = ComparisonOperator,
                    AttributeValueList = attributeValues
                };
                return condition;
            }
        }

        /// <summary>
        /// Gets the Conditions for the filter.
        /// </summary>
        protected Dictionary<string, FilterCondition> Conditions { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Filter()
        {
            Conditions = new Dictionary<string, FilterCondition>(StringComparer.Ordinal);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts filter to a map of conditions
        /// This call will use the conversion specified by AWSConfigs.DynamoDBConfig.ConversionSchema
        /// </summary>
        /// <returns>Map from attribute name to condition</returns>
        public Dictionary<string, Condition> ToConditions()
        {
            return ToConditions(DynamoDBEntryConversion.CurrentConversion);
        }

        /// <summary>
        /// Converts filter to a map of conditions
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <returns>Map from attribute name to condition</returns>
        public Dictionary<string, Condition> ToConditions(DynamoDBEntryConversion conversion)
        {
            return ToConditions(conversion, epochAttributes: null, isEmptyStringValueEnabled: false);
        }

        /// <summary>
        /// Converts filter to a map of conditions
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled"></param>
        /// <returns>Map from attribute name to condition</returns>
        public Dictionary<string, Condition> ToConditions(DynamoDBEntryConversion conversion, bool isEmptyStringValueEnabled)
        {
            return ToConditions(conversion, epochAttributes: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);
        }

        /// <summary>
        /// Converts filter to a map of conditions
        /// </summary>
        /// <param name="table">Table to use for converting .NET values to DynamoDB values.</param>
        /// <returns>Map from attribute name to condition</returns>
        public Dictionary<string, Condition> ToConditions(Table table)
        {
            return ToConditions(table.Conversion, table.StoreAsEpoch, table.IsEmptyStringValueEnabled);
        }

        private Dictionary<string, Condition> ToConditions(DynamoDBEntryConversion conversion,
            IEnumerable<string> epochAttributes, bool isEmptyStringValueEnabled)
        {
            var dic = new Dictionary<string, Condition>();
            foreach (var kvp in Conditions)
            {
                string name = kvp.Key;
                FilterCondition fc = kvp.Value;
                bool convertToEpochSeconds = epochAttributes != null && epochAttributes.Contains(name);
                Condition condition = fc.ToCondition(conversion, convertToEpochSeconds, name, isEmptyStringValueEnabled);

                dic[name] = condition;
            }

            return dic;
        }

        /// <summary>
        /// Creates a Filter from a conditions map
        /// </summary>
        /// <param name="conditions">Map from attribute name to condition</param>
        /// <returns>Equivalent Filter</returns>
        public static Filter FromConditions(Dictionary<string, Condition> conditions)
        {
            Filter ret = new Filter();
            foreach(var kvp in conditions)
            {
                string name = kvp.Key;
                Condition condition = kvp.Value;
                ret.Conditions.Add(name, new FilterCondition(condition));
            }
            return ret;
        }

        /// <summary>
        /// Creates a list of AttributeValues from a list of DynamoDBEntry items
        /// </summary>
        /// <param name="conversion"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static List<AttributeValue> ConvertToAttributeValues(DynamoDBEntryConversion conversion, params DynamoDBEntry[] values)
        {
            return ConvertToAttributeValues(conversion, false, values);
        }

        /// <summary>
        /// Creates a list of AttributeValues from a list of DynamoDBEntry items
        /// </summary>
        /// <param name="conversion"></param>
        /// <param name="isEmptyStringValueEnabled"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static List<AttributeValue> ConvertToAttributeValues(DynamoDBEntryConversion conversion,
            bool isEmptyStringValueEnabled, params DynamoDBEntry[] values)
        {
            List<AttributeValue> attributes = new List<AttributeValue>();
            foreach (DynamoDBEntry value in values)
            {
                var attributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(conversion, isEmptyStringValueEnabled);
                AttributeValue nativeValue = value.ConvertToAttributeValue(attributeConversionConfig);
                if (nativeValue != null)
                {
                    attributes.Add(nativeValue);
                }
            }
            return attributes;
        }


        #endregion

        #region Add methods

        /// <summary>
        /// Adds a condition for a specified attribute.
        /// 
        /// If a condition for the attribute already exists,
        /// it will be replaced with the new condition.
        /// </summary>
        /// <param name="attributeName">Target attribute name</param>
        /// <param name="condition">Condition to be added</param>
        public void AddCondition(string attributeName, Condition condition)
        {
            Conditions[attributeName] = new FilterCondition(condition);
        }

        /// <summary>
        /// Adds a condition 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="comparisonOperator"></param>
        /// <param name="values"></param>
        protected void AddCondition(string attributeName, ComparisonOperator comparisonOperator, List<DynamoDBEntry> values)
        {
            Conditions[attributeName] = new FilterCondition(comparisonOperator, values);
        }

        /// <summary>
        /// Adds a condition
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="filterCondition"></param>
        protected void AddCondition(string attributeName, FilterCondition filterCondition)
        {
            Conditions[attributeName] = filterCondition;
        }

        /// <summary>
        /// Removes a condition for a specific attribute name.
        /// </summary>
        /// <param name="attributeName">Target attribute</param>
        public void RemoveCondition(string attributeName)
        {
            Conditions.Remove(attributeName);
        }

        #endregion
    }

    /// <summary>
    /// Scan filter.
    /// </summary>
    public class ScanFilter : Filter
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ScanFilter()
            : base()
        { }

        /// <summary>
        /// Adds a condition for a specified attribute that consists
        /// of an operator and any number of AttributeValues.
        /// </summary>
        /// <param name="attributeName">Target attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">AttributeValues to compare to</param>
        public void AddCondition(string attributeName, ScanOperator op, List<AttributeValue> values)
        {
            AddCondition(attributeName, new Condition
            {
                ComparisonOperator = EnumMapper.Convert(op),
                AttributeValueList = values
            });
        }

        /// <summary>
        /// Adds a condition for a specified attribute that consists
        /// of an operator and any number of values
        /// </summary>
        /// <param name="attributeName">Target attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">Values to compare to</param>
        public void AddCondition(string attributeName, ScanOperator op, params DynamoDBEntry[] values)
        {
            ComparisonOperator comparisonOperator = EnumMapper.Convert(op);
            AddCondition(attributeName, comparisonOperator, values.ToList());
        }
    }

    /// <summary>
    /// Query filter.
    /// </summary>
    public class QueryFilter : Filter
    {
        /// <summary>
        /// Constructs an empty QueryFilter instance
        /// </summary>
        public QueryFilter()
            : base()
        { }

        /// <summary>
        /// Constructs an instance of QueryFilter with a single condition.
        /// More conditions can be added after initialization.
        /// </summary>
        /// <param name="attributeName">Target attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">Attributes to compare</param>
        public QueryFilter(string attributeName, QueryOperator op, List<AttributeValue> values)
        {
            AddCondition(attributeName, op, values);
        }

        /// <summary>
        /// Constructs an instance of QueryFilter with a single condition.
        /// More conditions can be added after initialization.
        /// </summary>
        /// <param name="attributeName">Target attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">Attributes to compare</param>
        public QueryFilter(string attributeName, QueryOperator op, params DynamoDBEntry[] values)
        {
            AddCondition(attributeName, op, values);
        }

        // Creates new filter that has the same conditions as base
        internal QueryFilter(QueryFilter baseFilter)
        {
            foreach (var kvp in baseFilter.Conditions)
            {
                string key = kvp.Key;
                FilterCondition condition = kvp.Value;

                AddCondition(key, condition);
            }
        }

        /// <summary>
        /// Adds a condition for a specified key attribute that consists
        /// of an operator and any number of AttributeValues.
        /// </summary>
        /// <param name="keyAttributeName">Target key attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">AttributeValues to compare to</param>
        public void AddCondition(string keyAttributeName, QueryOperator op, List<AttributeValue> values)
        {
            AddCondition(keyAttributeName, new Condition
            {
                ComparisonOperator = EnumMapper.Convert(op),
                AttributeValueList = values
            });
        }

        /// <summary>
        /// Adds a condition for a specified key attribute that consists
        /// of an operator and any number of values
        /// </summary>
        /// <param name="keyAttributeName">Target key attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">Values to compare to</param>
        public void AddCondition(string keyAttributeName, QueryOperator op, params DynamoDBEntry[] values)
        {
            ComparisonOperator comparisonOperator = EnumMapper.Convert(op);
            AddCondition(keyAttributeName, comparisonOperator, values.ToList());
        }

        /// <summary>
        /// Adds a condition for a specified non-key attribute that consists
        /// of an operator and any number of AttributeValues.
        /// </summary>
        /// <param name="nonKeyAttributeName">Target non-key attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">AttributeValues to compare to</param>
        public void AddCondition(string nonKeyAttributeName, ScanOperator op, List<AttributeValue> values)
        {
            AddCondition(nonKeyAttributeName, new Condition
            {
                ComparisonOperator = EnumMapper.Convert(op),
                AttributeValueList = values
            });
        }

        /// <summary>
        /// Adds a condition for a specified non-key attribute that consists
        /// of an operator and any number of values
        /// </summary>
        /// <param name="nonKeyAttributeName">Target non-key attribute name</param>
        /// <param name="op">Comparison operator</param>
        /// <param name="values">Values to compare to</param>
        public void AddCondition(string nonKeyAttributeName, ScanOperator op, params DynamoDBEntry[] values)
        {
            ComparisonOperator comparisonOperator= EnumMapper.Convert(op);
            AddCondition(nonKeyAttributeName, comparisonOperator, values.ToList());
        }
    }
}
