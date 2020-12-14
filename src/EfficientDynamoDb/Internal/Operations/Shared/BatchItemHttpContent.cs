using System;
using System.Buffers;
using System.Text.Json;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal abstract class BatchItemHttpContent : DynamoDbHttpContent
    {
        protected BatchItemHttpContent(string amzTarget) : base(amzTarget)
        {
        }
        
        protected static void WriteTableNameAsKey(Utf8JsonWriter writer, string? prefix, string tableName)
        {
            if (prefix == null)
            {
                writer.WritePropertyName(tableName);
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
                writer.WritePropertyName(arr);
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
    }
}