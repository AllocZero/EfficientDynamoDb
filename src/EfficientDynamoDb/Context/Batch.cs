using EfficientDynamoDb.Context.Operations.BatchWriteItem;

namespace EfficientDynamoDb.Context
{
    public static class Batch
    {
        public static IBatchWriteBuilder PutItem<TEntity>(TEntity entity) where TEntity : class => new BatchPutItemBuilder(typeof(TEntity), entity);
        
        public static IBatchDeleteItemBuilder DeleteItem<TEntity>() where TEntity : class => new BatchDeleteItemBuilder(typeof(TEntity));
    }
}