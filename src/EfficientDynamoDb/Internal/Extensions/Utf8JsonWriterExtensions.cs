using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class Utf8JsonWriterExtensions
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
        public static async ValueTask WriteAttributesDictionaryAsync(this Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter, IReadOnlyDictionary<string, AttributeValue> attributesDictionary)
        {
            writer.WriteStartObject();

            foreach (var pair in attributesDictionary)
            {
                writer.WritePropertyName(pair.Key);

                pair.Value.Write(writer);

                if (bufferWriter.ShouldWrite(writer))
                    await bufferWriter.WriteToStreamAsync().ConfigureAwait(false);
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExpressionAttributeNames(this Utf8JsonWriter writer, HashSet<string> attributeNames)
        {
            writer.WritePropertyName("ExpressionAttributeNames");
                
            writer.WriteStartObject();

            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);

            try
            {
                foreach (var str in attributeNames)
                {
                    builder.Append('#');
                    builder.Append(str);
                    writer.WriteString(builder.GetBuffer(), str);

                    builder.Clear();
                }
            }
            finally
            {
                builder.Dispose();
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
        public static void WriteExpressionAttributeValues(this Utf8JsonWriter writer, DynamoDbContextMetadata metadata, FilterBase? filter1,
            FilterBase? filter2 = null)
        {
            if (filter1 == null && filter2 == null)
                return;

            writer.WritePropertyName("ExpressionAttributeValues");
            writer.WriteStartObject();

            var counter = 0;
            filter1?.WriteAttributeValues(writer, metadata, ref counter);
            filter2?.WriteAttributeValues(writer, metadata, ref counter);

            writer.WriteEndObject();
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
        public static void WriteReturnValuesOnConditionCheckFailure(this Utf8JsonWriter writer, ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure)
        {
            writer.WriteString("ReturnValuesOnConditionCheckFailure", returnValuesOnConditionCheckFailure switch
            {
                ReturnValuesOnConditionCheckFailure.None => "NONE",
                ReturnValuesOnConditionCheckFailure.AllOld => "ALL_OLD",
                _ => "NONE"
            });
        }
    }
}