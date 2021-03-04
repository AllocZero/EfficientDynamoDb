using EfficientDynamoDb.Context.Operations.BatchGetItem;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;

namespace EfficientDynamoDb.Context
{
    public static class Batch
    {
        public static IBatchWriteBuilder PutItem<TEntity>(TEntity entity) where TEntity : class => new BatchPutItemBuilder(typeof(TEntity), entity);
        
        public static IBatchDeleteItemBuilder DeleteItem<TEntity>() where TEntity : class => new BatchDeleteItemBuilder(typeof(TEntity));

        public static IBatchGetTableBuilder<TTableEntity> FromTable<TTableEntity>() where TTableEntity : class => new BatchGetTableBuilder<TTableEntity>();

        public static IBatchGetItemBuilder GetItem<TEntity>() where TEntity : class => new BatchGetItemBuilder<TEntity>();
    }
}