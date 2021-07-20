using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static partial class Utf8JsonWriterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteReturnConsumedCapacity(this Utf8JsonWriter writer, ReturnConsumedCapacity value)
        {
            writer.WriteString("ReturnConsumedCapacity", value switch
            {
                ReturnConsumedCapacity.Indexes => "INDEXES",
                ReturnConsumedCapacity.Total => "TOTAL",
                ReturnConsumedCapacity.None => "NONE",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteAttributesDictionary(this Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> attributesDictionary)
        {
            writer.WriteStartObject();

            foreach (var pair in attributesDictionary)
            {
                writer.WritePropertyName(pair.Key);

                pair.Value.Write(writer);
            }

            writer.WriteEndObject();
        }

        public static void WriteTableName(this Utf8JsonWriter writer, string? prefix, string tableName)
        {
            const string tableNameKey = "TableName";
            if (prefix == null)
            {
                writer.WriteString(tableNameKey, tableName);
                return;
            }

            var fullLength = prefix.Length + tableName.Length;

            char[]? pooledArray = null;
            var arr = fullLength < NoAllocStringBuilder.MaxStackAllocSize
                ? stackalloc char[fullLength]
                : pooledArray = ArrayPool<char>.Shared.Rent(fullLength);

            try
            {
                prefix.AsSpan().CopyTo(arr);
                tableName.AsSpan().CopyTo(arr.Slice(prefix.Length));
                writer.WriteString(tableNameKey, arr);
            }
            finally
            {
                if (pooledArray != null)
                {
                    pooledArray.AsSpan(0, fullLength).Clear();
                    ArrayPool<char>.Shared.Return(pooledArray);
                }
            }
        }

        /// <summary>
        /// Only call async implementation when you are writing big document that in sum can exceed default JSON buffer size (16KB).
        /// </summary>
        public static async ValueTask WriteAttributesDictionaryAsync(this Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter,
            IReadOnlyDictionary<string, AttributeValue> attributesDictionary)
        {
            writer.WriteStartObject();

            foreach (var pair in attributesDictionary)
            {
                writer.WritePropertyName(pair.Key);

                pair.Value.Write(writer);

                if (bufferWriter.ShouldFlush(writer))
                    await bufferWriter.FlushAsync(writer).ConfigureAwait(false);
            }

            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExpressionAttributeNames(this Utf8JsonWriter writer, IReadOnlyDictionary<string, string> attributeNames)
        {
            writer.WritePropertyName("ExpressionAttributeNames");

            writer.WriteStartObject();

            foreach (var pair in attributeNames)
                writer.WriteString(pair.Key, pair.Value);

            writer.WriteEndObject();
        }
        
        public static void WriteExpressionAttributeNames(this Utf8JsonWriter writer, ref NoAllocStringBuilder builder, IReadOnlyList<string> attributeNames)
        {
            writer.WritePropertyName("ExpressionAttributeNames");

            writer.WriteStartObject();

            for (var i = 0; i < attributeNames.Count; i++)
            {
                builder.Append("#f");
                builder.Append(i);

                writer.WriteString(builder.GetBuffer(), attributeNames[i]);

                builder.Clear();
            }

            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExpressionAttributeValues(this Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> attributeValues)
        {
            writer.WritePropertyName("ExpressionAttributeValues");
            writer.WriteAttributesDictionary(attributeValues);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExpressionAttributeValues(this in DdbWriter writer, DynamoDbContextMetadata metadata, DdbExpressionVisitor visitor,
            FilterBase? filter1, FilterBase? filter2 = null)
        {
            if (filter1 == null && filter2 == null)
                return;

            writer.JsonWriter.WritePropertyName("ExpressionAttributeValues");
            writer.JsonWriter.WriteStartObject();

            var counter = 0;
            filter1?.WriteAttributeValues(in writer, metadata, ref counter, visitor);
            filter2?.WriteAttributeValues(in writer, metadata, ref counter, visitor);

            writer.JsonWriter.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePrimaryKey(this Utf8JsonWriter writer, PrimaryKey primaryKey)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();

            writer.WritePropertyName(primaryKey.PartitionKeyName!);
            primaryKey.PartitionKeyValue.Write(writer);

            if (primaryKey.SortKeyValue != null)
            {
                writer.WritePropertyName(primaryKey.SortKeyName!);
                primaryKey.SortKeyValue.Value.Write(writer);
            }

            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteReturnItemCollectionMetrics(this Utf8JsonWriter writer, ReturnItemCollectionMetrics returnItemCollectionMetrics)
        {
            writer.WriteString("ReturnItemCollectionMetrics", returnItemCollectionMetrics switch
            {
                ReturnItemCollectionMetrics.None => "NONE",
                ReturnItemCollectionMetrics.Size => "SIZE",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteReturnValues(this Utf8JsonWriter writer, ReturnValues returnValues)
        {
            writer.WriteString("ReturnValues", returnValues switch
            {
                ReturnValues.None => "NONE",
                ReturnValues.AllOld => "ALL_OLD",
                ReturnValues.UpdatedOld => "UPDATED_OLD",
                ReturnValues.AllNew => "ALL_NEW",
                ReturnValues.UpdatedNew => "UPDATED_NEW",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteReturnValuesOnConditionCheckFailure(this Utf8JsonWriter writer,
            ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure)
        {
            writer.WriteString("ReturnValuesOnConditionCheckFailure", returnValuesOnConditionCheckFailure switch
            {
                ReturnValuesOnConditionCheckFailure.None => "NONE",
                ReturnValuesOnConditionCheckFailure.AllOld => "ALL_OLD",
                _ => "NONE"
            });
        }
        
        public static void WriteConditionExpression(this in DdbWriter writer, FilterBase condition, DynamoDbContextMetadata metadata)
        {
            var visitor = new DdbExpressionVisitor(metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                WriteConditionExpression(in writer, ref builder, visitor, condition, metadata);
            }
            finally
            {
                builder.Dispose();
            }
        }
        
        public static void WriteConditionExpression(this in DdbWriter writer, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, FilterBase condition, DynamoDbContextMetadata metadata)
        {
            var expressionValuesCount = 0;

            condition.WriteExpressionStatement(ref builder, ref expressionValuesCount, visitor);
            writer.JsonWriter.WriteString("ConditionExpression", builder.GetBuffer());
                
            builder.Clear();
                
            if (visitor.CachedAttributeNames.Count > 0)
                writer.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

            if (expressionValuesCount > 0)
                writer.WriteExpressionAttributeValues(metadata, visitor, condition);
            
            builder.Clear();
            visitor.Clear();
        }

        public static void WriteProjectionExpression(this Utf8JsonWriter writer, ref DdbExpressionVisitor? visitor, BuilderNode projectedAttributeStart, DynamoDbContextMetadata metadata)
        {
            if (visitor == null)
                visitor = new DdbExpressionVisitor(metadata);
            else
                visitor.Clear();
            
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
           
            try
            {
                writer.WriteProjectedAttributes(projectedAttributeStart, ref builder, visitor, metadata);

                if (visitor.CachedAttributeNames.Count > 0)
                    writer.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);
            }
            finally
            {
                builder.Dispose();
            }
        }
        
        public static void WriteProjectedAttributes(this Utf8JsonWriter writer, BuilderNode projectedAttributeStart, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, DynamoDbContextMetadata metadata)
        {
            var isFirst = true;

            foreach (var node in projectedAttributeStart)
            {
                if (node.Type != BuilderNodeType.ProjectedAttributes)
                    continue;

                var projectedAttributeNode = (ProjectedAttributesNode) node;
                var classInfo = metadata.GetOrAddClassInfo(projectedAttributeNode.ProjectionType);
                
                if (projectedAttributeNode.Expressions == null)
                {
                    foreach (var attributeName in classInfo.AttributesMap.Keys)
                    {
                        if (!isFirst)
                            builder.Append(',');

                        builder.Append("#f");
                        builder.Append(visitor.CachedAttributeNames.Count);
                        
                        // TODO: Consider optimizing this
                        visitor.VisitAttribute(attributeName);

                        isFirst = false;
                    }
                }
                else
                {
                    foreach (var expression in projectedAttributeNode.Expressions)
                        visitor.Visit(classInfo, expression);
                    
                    if (!isFirst)
                        builder.Append(',');

                    builder.Append(visitor.Builder);

                    isFirst = false;
                }
            }
           
            if (!isFirst)
                writer.WriteString("ProjectionExpression", builder.GetBuffer());

            builder.Clear();
        }
    }
}