using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.BatchWriteItem;

namespace EfficientDynamoDb
{
    /// <summary>
    /// A utility class that provides builder methods to initialize various batch operations in DynamoDB.
    /// </summary>
    /// <remarks>
    /// The Batch class provides a static interface for initiating builders for PutItem, DeleteItem, GetItem operations, and specifying the table for the operations within a batch context.
    /// The operations constructed with these builders can be used in <see cref="DynamoDbContext.BatchGet"/> or <see cref="DynamoDbContext.BatchWrite"/>.
    /// </remarks>
    public static class Batch
    {
        /// <summary>
        /// Initializes the PutItem operation builder for a batch operation with a given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <param name="entity">The entity to put in the DynamoDB table.</param>
        /// <returns>A PutItem operation builder for a batch operation.</returns>
        public static IBatchPutItemBuilder PutItem<TEntity>(TEntity entity) where TEntity : class => new BatchPutItemBuilder(typeof(TEntity), entity);
        
        /// <summary>
        /// Initializes the DeleteItem operation builder for a batch operation.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>A DeleteItem builder for a batch operation.</returns>
        public static IBatchDeleteItemBuilder DeleteItem<TEntity>() where TEntity : class => new BatchDeleteItemBuilder(typeof(TEntity));

        /// <summary>
        /// Specifies the table from which to retrieve items in a batch operation.
        /// Provides more granular control over the batch operation than <see cref="FromTable{TTableEntity}"/>.
        /// </summary>
        /// <typeparam name="TTableEntity">The type of the entities in the DynamoDB table.</typeparam>
        /// <returns>A table builder for a batch operation.</returns>
        public static IBatchGetTableBuilder<TTableEntity> FromTable<TTableEntity>() where TTableEntity : class => new BatchGetTableBuilder<TTableEntity>();

        /// <summary>
        /// Initializes the GetItem operation builder for a batch operation.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity in the DynamoDB table.</typeparam>
        /// <returns>A GetItem operation builder for a batch operation.</returns>
        public static IBatchGetItemBuilder GetItem<TEntity>() where TEntity : class => new BatchGetItemBuilder<TEntity>();
    }

}