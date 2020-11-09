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
using System.Globalization;
using System.IO;
using System.Text;

using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.Runtime.Internal.Transform;
using Amazon.Util.Internal;

#pragma warning disable 1591

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// An enumeration of all supported return value directives
    /// </summary>
    public enum ReturnValues
    {
        None,
        AllOldAttributes,
        AllNewAttributes,
        UpdatedOldAttributes,
        UpdatedNewAttributes
    }

    /// <summary>
    /// An enumeration of all supported scan operator directives
    /// </summary>
    public enum ScanOperator
    {
        Equal,
        NotEqual,
        LessThanOrEqual,
        LessThan,
        GreaterThanOrEqual,
        GreaterThan,
        IsNotNull,
        IsNull,
        Contains,
        NotContains,
        BeginsWith,
        In,
        Between
    }

    /// <summary>
    /// An enumeration of all supported query operator directives
    /// </summary>
    public enum QueryOperator
    {
        Equal,
        LessThanOrEqual,
        LessThan,
        GreaterThanOrEqual,
        GreaterThan,
        BeginsWith,
        Between
    }

    /// <summary>
    /// An enumeration of all supported Select values for Query and Scan.
    /// Value of Count will force service to return the number of items,
    /// not the items themselves.
    /// </summary>
    public enum SelectValues
    {
        Count,
        SpecificAttributes,
        AllAttributes,
        AllProjectedAttributes
    }

    /// <summary>
    /// An enumeration of all supported ConditionalOperator values.
    /// Value of And for an operation will require that all conditions match.
    /// Value of Or for an operation will require that at least one of the conditons match.
    /// </summary>
    public enum ConditionalOperatorValues
    {
        /// <summary>
        /// And condition
        /// </summary>
        And,
        /// <summary>
        /// Or Condition
        /// </summary>
        Or
    }

    internal enum ProjectionTypeValues
    {
        All,
        Include,
        KeysOnly
    }

    internal enum ReturnConsumedCapacityValues
    {
        None,
        Total
    }

    internal enum ReturnItemCollectionMetricsValues
    {
        None,
        All
    }

    internal enum SearchType
    {
        Scan,
        Query
    }

    public class KeyDescription
    {
        public DynamoDBEntryType Type { get; internal set; }
        public bool IsHash { get; internal set; }
    }

    internal class Key : Dictionary<string, AttributeValue>
    {
        public Key(IDictionary<string, AttributeValue> values)
            : base(values)
        {
        }

        public Key()
            : base()
        { }
    }

    internal static class EnumMapper
    {
        public static string Convert(ReturnConsumedCapacityValues value)
        {
            switch(value)
            {
                case ReturnConsumedCapacityValues.None:
                    return "NONE";
                case ReturnConsumedCapacityValues.Total:
                    return "TOTAL";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ReturnConsumedCapacityValues value");
            }
        }

        public static string Convert(ReturnItemCollectionMetricsValues value)
        {
            switch (value)
            {
                case ReturnItemCollectionMetricsValues.None:
                    return "NONE";
                case ReturnItemCollectionMetricsValues.All:
                    return "ALL";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ReturnItemCollectionMetricsValues value");
            }
        }

        public static string Convert(ProjectionTypeValues value)
        {
            switch (value)
            {
                case ProjectionTypeValues.All:
                    return "ALL";
                case ProjectionTypeValues.Include:
                    return "INCLUDE";
                case ProjectionTypeValues.KeysOnly:
                    return "KEYS_ONLY";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ProjectionTypeValues value");
            }
        }

        public static string Convert(SelectValues value)
        {
            switch (value)
            {
                case SelectValues.Count:
                    return "COUNT";
                case SelectValues.AllAttributes:
                    return "ALL_ATTRIBUTES";
                case SelectValues.AllProjectedAttributes:
                    return "ALL_PROJECTED_ATTRIBUTES";
                case SelectValues.SpecificAttributes:
                    return "SPECIFIC_ATTRIBUTES";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid SelectValues value");
            }
        }

        public static string Convert(ReturnValues value)
        {
            switch (value)
            {
                case ReturnValues.None:
                    return "NONE";
                case ReturnValues.AllOldAttributes:
                    return "ALL_OLD";
                case ReturnValues.AllNewAttributes:
                    return "ALL_NEW";
                case ReturnValues.UpdatedOldAttributes:
                    return "UPDATED_OLD";
                case ReturnValues.UpdatedNewAttributes:
                    return "UPDATED_NEW";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ReturnValues value");
            }
        }

        public static string Convert(ScanOperator value)
        {
            switch (value)
            {
                case ScanOperator.Equal:
                    return "EQ";
                case ScanOperator.NotEqual:
                    return "NE";
                case ScanOperator.LessThanOrEqual:
                    return "LE";
                case ScanOperator.LessThan:
                    return "LT";
                case ScanOperator.GreaterThanOrEqual:
                    return "GE";
                case ScanOperator.GreaterThan:
                    return "GT";
                case ScanOperator.IsNotNull:
                    return "NOT_NULL";
                case ScanOperator.IsNull:
                    return "NULL";
                case ScanOperator.Contains:
                    return "CONTAINS";
                case ScanOperator.NotContains:
                    return "NOT_CONTAINS";
                case ScanOperator.BeginsWith:
                    return "BEGINS_WITH";
                case ScanOperator.In:
                    return "IN";
                case ScanOperator.Between:
                    return "BETWEEN";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ScanOperator value");
            }
        }

        public static string Convert(QueryOperator value)
        {
            switch(value)
            {
                case QueryOperator.Equal:
                    return "EQ";
                case QueryOperator.LessThanOrEqual:
                    return "LE";
                case QueryOperator.LessThan:
                    return "LT";
                case QueryOperator.GreaterThanOrEqual:
                    return "GE";
                case QueryOperator.GreaterThan:
                    return "GT";
                case QueryOperator.BeginsWith:
                    return "BEGINS_WITH";
                case QueryOperator.Between:
                    return "BETWEEN";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid QueryOperator value");
            }
        }

        public static string Convert(ConditionalOperatorValues value)
        {
            switch (value)
            {
                case ConditionalOperatorValues.And:
                    return "AND";
                case ConditionalOperatorValues.Or:
                    return "OR";
                default:
                    throw new ArgumentOutOfRangeException("value", "Invalid ConditionalOperatorValues value");
            }
        }
    }

    internal static class Common
    {
        private const string AwsVariablePrefix = "awsavar";

        // Convert collection of AttributeValueUpdate to an update expression. This is needed when doing an update
        // with a conditional expression.
        public static void ConvertAttributeUpdatesToUpdateExpression(Dictionary<string, AttributeValueUpdate> attributesToUpdates,
            out string statement,
            out Dictionary<string, AttributeValue> expressionAttributeValues,
            out Dictionary<string, string> expressionAttributes)
        {
            expressionAttributeValues = new Dictionary<string, AttributeValue>(StringComparer.Ordinal);
            expressionAttributes = new Dictionary<string, string>(StringComparer.Ordinal);

            // Build an expression string with a SET clause for the added/modified attributes and 
            // REMOVE clause for the attributes set to null.
            int attributeCount = 0;
            StringBuilder sets = new StringBuilder();
            StringBuilder removes = new StringBuilder();
            foreach (var kvp in attributesToUpdates)
            {
                var attribute = kvp.Key;
                var update = kvp.Value;

                string variableName = GetVariableName(ref attributeCount);
                var attributeReference = GetAttributeReference(variableName);
                var attributeValueReference = GetAttributeValueReference(variableName);

                if (update.Action == AttributeAction.DELETE)
                {
                    if (removes.Length > 0)
                        removes.Append(", ");
                    removes.Append(attributeReference);
                }
                else
                {
                    if (sets.Length > 0)
                        sets.Append(", ");
                    sets.AppendFormat("{0} = {1}", attributeReference, attributeValueReference);

                    // Add the attribute value for the variable in the added in the expression
                    expressionAttributeValues.Add(attributeValueReference, update.Value);
                }

                // Add the attribute name for the variable in the added in the expression
                expressionAttributes.Add(attributeReference, attribute);
            }

            // Combine the SET and REMOVE clause
            StringBuilder statementBuilder = new StringBuilder();
            if (sets.Length > 0)
            {
                statementBuilder.AppendFormat(CultureInfo.InvariantCulture, "SET {0}", sets.ToString());
            }
            if (removes.Length > 0)
            {
                if (sets.Length > 0)
                    statementBuilder.Append(" ");

                statementBuilder.AppendFormat(CultureInfo.InvariantCulture, "REMOVE {0}", removes.ToString());
            }

            statement = statementBuilder.ToString();
        }

        public static void ConvertAttributesToGetToProjectionExpression(QueryRequest request)
        {
            if (request.IsSetAttributesToGet() &&
                (request.IsSetExpressionAttributeNames() || request.IsSetExpressionAttributeValues() || request.IsSetFilterExpression()))
            {
                var attributesToGet = request.AttributesToGet;
                var attributeNames = request.ExpressionAttributeNames;

                request.ProjectionExpression = AttributesToGetAsProjectionExpression(attributesToGet, attributeNames);
                request.AttributesToGet = null;
            }
        }
        public static void ConvertAttributesToGetToProjectionExpression(ScanRequest request)
        {
            if (request.IsSetAttributesToGet() &&
                (request.IsSetExpressionAttributeNames() || request.IsSetExpressionAttributeValues() || request.IsSetFilterExpression()))
            {
                var attributesToGet = request.AttributesToGet;
                var attributeNames = request.ExpressionAttributeNames;

                request.ProjectionExpression = AttributesToGetAsProjectionExpression(attributesToGet, attributeNames);
                request.AttributesToGet = null;
            }
        }

        private static string AttributesToGetAsProjectionExpression(List<string> attributesToGet, Dictionary<string, string> attributeNames)
        {
            int attributeCount = attributeNames.Count;
            var referencesToGet = new List<string>();
            foreach (var attribute in attributesToGet)
            {
                string attributeReference = GetAttributeReference(attribute, attributeNames, ref attributeCount);
                referencesToGet.Add(attributeReference);
            }
            var projectionExpression = string.Join(", ", referencesToGet.ToArray());
            return projectionExpression;
        }

        private static string GetAttributeReference(string attribute, Dictionary<string, string> attributeNames, ref int attributeCount)
        {
            string attributeReference;
            if (!InternalSDKUtils.TryFindByValue(attributeNames, attribute, StringComparer.Ordinal, out attributeReference))
            {
                var variableName = GetVariableName(ref attributeCount);
                attributeReference = GetAttributeReference(variableName);
                attributeNames.Add(attributeReference, attribute);
            }
            return attributeReference;
        }
        public static string GetAttributeReference(string attributeName)
        {
            return string.Format(CultureInfo.InvariantCulture, "#{0}", attributeName);
        }
        public static string GetAttributeValueReference(string attributeName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":{0}", attributeName);
        }
        public static string GetVariableName(ref int attributeCount)
        {
            attributeCount++;
            string variableName = AwsVariablePrefix + attributeCount;
            return variableName;
        }

        public static Dictionary<string, T> Combine<T>(
            IDictionary<string, T> items1, IDictionary<string, T> items2,
            IEqualityComparer<T> valueComparer)
        {
            if (valueComparer == null)
                valueComparer = EqualityComparer<T>.Default;
            var result = new Dictionary<string, T>(items1, StringComparer.Ordinal);

            foreach (var kvp in items2)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                // check if the key is already present
                T existingValue;
                if (result.TryGetValue(key, out existingValue))
                {
                    if (!valueComparer.Equals(value, existingValue))
                    {
                        // values are different, throw exception
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Key [{0}] exists in both collections, but has different values: [{1}] != [{2}]",
                            key, value, existingValue));
                    }
                    else
                    {
                        // key and identical value already present, no-op
                    }
                }
                else
                    result[key] = value;
            }

            return result;
        }

        public static string ToPaginationToken(Dictionary<string, AttributeValue> nextKey)
        {
            if (nextKey == null)
                return null;

            var clearToken = SerializeClearString(nextKey);
            return clearToken;
        }
        private static string SerializeClearString(Dictionary<string, AttributeValue> nextKey)
        {
            using (var writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                WriteNextKey(nextKey, writer);

                return writer.ToString();
            }
        }

        public static Dictionary<string, AttributeValue> FromPaginationToken(string token)
        {
            if (token == null)
                return null;

            return DeserializeClearString(token);
        }
        private static Dictionary<string, AttributeValue> DeserializeClearString(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            using (var stream = new MemoryStream(bytes))
            {
                return ReadNextKey(stream);
            }
        }

        private static Dictionary<string, AttributeValue> ReadNextKey(Stream stream)
        {
            JsonUnmarshallerContext context = new JsonUnmarshallerContext(stream, false, null);
            var unmarshaller = new DictionaryUnmarshaller<string, AttributeValue, StringUnmarshaller, AttributeValueUnmarshaller>(StringUnmarshaller.Instance, AttributeValueUnmarshaller.Instance);
            var nextKey = unmarshaller.Unmarshall(context);
            return nextKey;
        }
        private static void WriteNextKey(Dictionary<string, AttributeValue> nextKey, TextWriter writer)
        {
            var marshaller = Amazon.DynamoDBv2.Model.Internal.MarshallTransformations.AttributeValueMarshaller.Instance;
            var jsonWriter = new ThirdParty.Json.LitJson.JsonWriter(writer);
            var context = new Runtime.Internal.Transform.JsonMarshallerContext(null, jsonWriter);

            context.Writer.WriteObjectStart();
            foreach (var kvp in nextKey)
            {
                context.Writer.WritePropertyName(kvp.Key);
                context.Writer.WriteObjectStart();
                {
                    marshaller.Marshall(kvp.Value, context);
                }
                context.Writer.WriteObjectEnd();
            }
            context.Writer.WriteObjectEnd();
        }
    }
 }