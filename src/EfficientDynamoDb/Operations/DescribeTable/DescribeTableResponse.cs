using EfficientDynamoDb.Operations.DescribeTable.Models;

namespace EfficientDynamoDb.Operations.DescribeTable
{
    public class DescribeTableResponse
    {
        public TableDescription Table { get; }

        public DescribeTableResponse(TableDescription table) => Table = table;
    }
}