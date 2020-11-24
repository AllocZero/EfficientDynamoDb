using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

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
        public static void WriteExpressionAttributeValues(this Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> attributeValues)
        {
            writer.WritePropertyName("ExpressionAttributeValues");
            writer.WriteAttributesDictionary(attributeValues);
        }
    }
}