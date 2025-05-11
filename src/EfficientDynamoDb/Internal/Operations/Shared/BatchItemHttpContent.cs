using System;
using System.Buffers;
using System.Text.Json;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal abstract class BatchItemHttpContent : DynamoDbHttpContent
    {
        protected BatchItemHttpContent(string amzTarget) : base(amzTarget)
        {
        }
        
        protected static void WriteTableNameAsKey(Utf8JsonWriter writer, ITableNameFormatter? tableNameFormatter, string tableName)
        {
            if (tableNameFormatter == null)
            {
                writer.WritePropertyName(tableName);
                return;
            }



            var tableNameContext = new TableNameFormatterContext(tableName);
            var length = tableNameFormatter.CalculateLength(ref tableNameContext);

            char[]? pooledArray = null;
            var arr = length < NoAllocStringBuilder.MaxStackAllocSize
                ? stackalloc char[length]
                : pooledArray = ArrayPool<char>.Shared.Rent(length);

            try
            {
                if( !tableNameFormatter.TryFormat(arr, ref tableNameContext, out length) ) {
                    throw new DdbException($"Couldn't format table name '{tableName}' using the provided formatter");
                }

                writer.WritePropertyName(arr[..length]);
            }
            finally
            {
                if (pooledArray != null)
                {
                    pooledArray.AsSpan(0, length).Clear();
                    ArrayPool<char>.Shared.Return(pooledArray);
                }
            }
        }
    }
}