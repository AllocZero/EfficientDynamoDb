using System.Text.Json;
using EfficientDynamoDb.Internal.Builder;

namespace EfficientDynamoDb.Api.DescribeTable
{
    public class DescribeTableRequestHttpContent : DynamoDbHttpContent
    {
        public string TableName { get; }

        public DescribeTableRequestHttpContent(string tableName) : base("DynamoDB_20120810.DescribeTable")
        {
            TableName = tableName;
        }

        protected override void WriteData(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString("TableName", TableName);
            writer.WriteEndObject();
        }
    }
}