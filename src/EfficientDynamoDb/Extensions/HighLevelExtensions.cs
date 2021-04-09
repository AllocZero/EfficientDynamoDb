using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Extensions
{
    public static class HighLevelExtensions
    {
        public static TBuilder WithTableName<TBuilder>(this TBuilder builder, string tableName) where TBuilder : ITableBuilder<TBuilder>
        {
            return builder.Create(new TableNameNode(tableName, builder.Node));
        }
    }
}