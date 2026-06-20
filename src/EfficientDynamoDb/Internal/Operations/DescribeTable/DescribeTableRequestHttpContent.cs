using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DescribeTable
{
    internal class DescribeTableRequestHttpContent : DynamoDbHttpContent
    {
        private readonly string _tableName;
        private readonly ITableNameFormatter? _tableNameFormatter;

        public DescribeTableRequestHttpContent(ITableNameFormatter? tableNameFormatter, string tableName) : base("DynamoDB_20120810.DescribeTable")
        {
            _tableNameFormatter = tableNameFormatter;
            _tableName = tableName;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            writer.WriteTableName(_tableNameFormatter, _tableName);
            writer.WriteEndObject();

            return default;
        }
    }
}