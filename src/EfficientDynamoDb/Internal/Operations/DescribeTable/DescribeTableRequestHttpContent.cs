using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DescribeTable
{
    internal class DescribeTableRequestHttpContent : DynamoDbHttpContent
    {
        public string TableName { get; }

        public DescribeTableRequestHttpContent(string tableName) : base("DynamoDB_20120810.DescribeTable")
        {
            TableName = tableName;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            writer.WriteString("TableName", TableName);
            writer.WriteEndObject();

            return default;
        }
    }
}