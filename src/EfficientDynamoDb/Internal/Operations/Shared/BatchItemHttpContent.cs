using System.Text.Json;
using EfficientDynamoDb.Configs;

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


            tableNameFormatter.WriteTableName(tableName, writer, (arr, w) => w.WritePropertyName(arr));
        }
    }
}