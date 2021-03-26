using EfficientDynamoDb.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb
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