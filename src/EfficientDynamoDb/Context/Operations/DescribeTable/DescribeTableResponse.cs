using EfficientDynamoDb.Context.Operations.DescribeTable.Models;

namespace EfficientDynamoDb.Context.Operations.DescribeTable
{
    public class DescribeTableResponse
    {
        public TableDescription Table { get; }

        public DescribeTableResponse(TableDescription table) => Table = table;
    }
}