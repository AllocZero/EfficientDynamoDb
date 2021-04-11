using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.BatchWriteItem;

namespace EfficientDynamoDb
{
    public static class Batch
    {
        public static IBatchPutItemBuilder PutItem<TEntity>(TEntity entity) where TEntity : class => new BatchPutItemBuilder(typeof(TEntity), entity);
        
        public static IBatchDeleteItemBuilder DeleteItem<TEntity>() where TEntity : class => new BatchDeleteItemBuilder(typeof(TEntity));

        public static IBatchGetTableBuilder<TTableEntity> FromTable<TTableEntity>() where TTableEntity : class => new BatchGetTableBuilder<TTableEntity>();

        public static IBatchGetItemBuilder GetItem<TEntity>() where TEntity : class => new BatchGetItemBuilder<TEntity>();
    }
}