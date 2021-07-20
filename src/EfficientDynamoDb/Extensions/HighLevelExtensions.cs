using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Extensions
{
    public static class TableNameExtensions
    {
        /// <summary>
        /// Overrides the table name defined in <see cref="DynamoDbTableAttribute"/>.
        /// </summary>
        /// <param name="tableName">The table name to use instead of <see cref="DynamoDbTableAttribute"/>.</param>
        public static TBuilder WithTableName<TBuilder>(this TBuilder builder, string tableName) where TBuilder : ITableBuilder<TBuilder> =>
            builder.Create(new TableNameNode(tableName, builder.Node));

        /// <summary>
        /// Overrides the table name defined in <see cref="DynamoDbTableAttribute"/>.
        /// </summary>
        /// <param name="tableName">The table name to use instead of <see cref="DynamoDbTableAttribute"/>.</param>
        public static IBatchDeleteItemBuilder WithTableName(this IBatchDeleteItemBuilder builder, string tableName) => builder.WithTableName(tableName);

        /// <summary>
        /// Overrides the table name defined in <see cref="DynamoDbTableAttribute"/>.
        /// </summary>
        /// <param name="tableName">The table name to use instead of <see cref="DynamoDbTableAttribute"/>.</param>
        public static IBatchPutItemBuilder WithTableName(this IBatchPutItemBuilder builder, string tableName) => builder.WithTableName(tableName);

        /// <summary>
        /// Overrides the table name defined in <see cref="DynamoDbTableAttribute"/>.
        /// </summary>
        /// <param name="tableName">The table name to use instead of <see cref="DynamoDbTableAttribute"/>.</param>
        public static IBatchGetItemBuilder WithTableName(this IBatchGetItemBuilder builder, string tableName) => builder.WithTableName(tableName);
    }
}