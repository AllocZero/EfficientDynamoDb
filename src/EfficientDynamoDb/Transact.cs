using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb
{
    /// <summary>
    /// A utility class that provides builder methods to initialize various transaction operations in DynamoDB.
    /// </summary>
    /// <remarks>
    /// The Transact class provides a static interface for initiating builders for ConditionCheck, DeleteItem, PutItem, UpdateItem, and GetItem operations within a transaction context.
    /// The operations constructed with these builders can be used in <see cref="DynamoDbContext.TransactGet"/> or <see cref="DynamoDbContext.TransactWrite"/>.
    /// </remarks>
    public static class Transact
    {
        /// <summary>
        /// Initializes the ConditionCheck builder for a transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>A ConditionCheck builder for a transaction operation.</returns>
        public static ITransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class => new TransactConditionCheckBuilder<TEntity>();

        /// <summary>
        /// Initializes the DeleteItem operation builder for a transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>A DeleteItem builder for a transaction operation.</returns>
        public static ITransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new TransactDeleteItemBuilder<TEntity>();

        /// <summary>
        /// Initializes the PutItem operation builder for a transaction with a given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <param name="entity">The entity to put in the DynamoDB table.</param>
        /// <returns>A PutItem operation builder for a transaction.</returns>
        public static ITransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class =>
            new TransactPutItemBuilder<TEntity>(new ItemTypeNode(entity, typeof(TEntity), null));

        /// <summary>
        /// Initializes the UpdateItem operation builder for a transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>An UpdateItem operation builder for a transaction.</returns>
        public static ITransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class => new TransactUpdateItemBuilder<TEntity>();

        /// <summary>
        /// Initializes the GetItem operation builder for a transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>A GetItem operation builder for a transaction operation.</returns>
        public static ITransactGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new TransactGetItemRequestBuilder<TEntity>();
    }
}