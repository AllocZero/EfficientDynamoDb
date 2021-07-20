using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DescribeTable
{
    internal class DescribeTableRequestHttpContent : DynamoDbHttpContent
    {
        private readonly string _tableName;
        private readonly string? _tablePrefix;

        public DescribeTableRequestHttpContent(string? tablePrefix, string tableName) : base("DynamoDB_20120810.DescribeTable")
        {
            _tablePrefix = tablePrefix;
            _tableName = tableName;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            writer.WriteTableName(_tablePrefix, _tableName);
            writer.WriteEndObject();

            return default;
        }
    }
}