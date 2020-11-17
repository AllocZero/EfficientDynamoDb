using EfficientDynamoDb.Api.DescribeTable.Models;

namespace EfficientDynamoDb.Api.DescribeTable
{
    public class DescribeTableResponse
    {
        public TableDescription Table { get; }

        public DescribeTableResponse(TableDescription table) => Table = table;
    }
}