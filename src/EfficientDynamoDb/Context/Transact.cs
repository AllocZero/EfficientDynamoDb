using EfficientDynamoDb.Context.Operations.TransactGetItems;
using EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb.Context
{
    public static class Transact
    {
        public static ITransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class => new TransactConditionCheckBuilder<TEntity>();

        public static ITransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new TransactDeleteItemBuilder<TEntity>();

        public static ITransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class => new TransactPutItemBuilder<TEntity>();

        public static ITransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class => new TransactUpdateItemBuilder<TEntity>();

        public static ITransactGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new TransactGetItemRequestBuilder<TEntity>();
    }
}