using EfficientDynamoDb.Context.Operations.TransactGetItems;
using EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb.Context
{
    public static class Transact
    {
        public static TransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class => new TransactConditionCheckBuilder<TEntity>();

        public static TransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new TransactDeleteItemBuilder<TEntity>();

        public static TransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class => new TransactPutItemBuilder<TEntity>();

        public static TransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class => new TransactUpdateItemBuilder<TEntity>();

        public static TransactGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new TransactGetItemRequestBuilder<TEntity>();
    }
}