using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.DeleteItem
{
    /// <summary>
    /// Represents a builder for the DeleteItem operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface IDeleteItemEntityRequestBuilder<TEntity> : ITableBuilder<IDeleteItemEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies condition for the DeleteItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail.
        /// </remarks>
        IDeleteItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the DeleteItem operation.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>The DeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail.
        /// </remarks>
        IDeleteItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        /// <summary>
        /// Specifies partition and sort keys of the item to delete.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to delete.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);        
        
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// The only supported values for <c>DeleteItem</c> operation are <see cref="ReturnValues.None"/> and <see cref="ReturnValues.AllOld"/>.
        /// </remarks>
        IDeleteItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);

        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="option">Option for handling return values on condition check failure.</param>
        /// <returns>DeleteItem operation builder.</returns>
        IDeleteItemEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>DeleteItem operation builder.</returns>
        IDeleteItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IDeleteItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);

        /// <summary>
        /// Represents the returned item as <see cref="Document"/>.
        /// </summary>
        /// <returns>DeleteItem operation builder suitable for document response.</returns>
        IDeleteItemDocumentRequestBuilder<TEntity> AsDocument();
        
        /// <summary>
        /// Suppresses the throwing of the exceptions related to the DeleteItem operation.
        /// </summary>
        ISuppressedDeleteItemEntityRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the DeleteItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the DeleteItem operation and returns the item's attributes before the delete.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Item attributes are returned as they appeared before the DeleteItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the DeleteItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<DeleteItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a builder for the DeleteItem operation with an entity type constraint.
    /// Provides methods for configuring options and executing the operation with typed response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface ISuppressedDeleteItemEntityRequestBuilder<TEntity> : ITableBuilder<ISuppressedDeleteItemEntityRequestBuilder<TEntity>>
        where TEntity : class
    {
        /// <summary>
        /// Executes the DeleteItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the DeleteItem operation and returns the operation result with item's attributes before the delete.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The item is returned as it appeared before the DeleteItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<OpResult<TEntity?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the DeleteItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<DeleteItemEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a builder for the DeleteItem operation with an entity type constraint and a document response.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDeleteItemDocumentRequestBuilder<TEntity> : ITableBuilder<IDeleteItemDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies condition for the DeleteItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail.
        /// </remarks>
        IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the DeleteItem operation.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>The DeleteItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the DeleteItem operation should succeed or fail.
        /// </remarks>
        IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        /// <summary>
        /// Specifies partition and sort keys of the item to delete.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to delete.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);        
        
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>DeleteItem operation builder.</returns>
        /// <remarks>
        /// The only supported values for <c>DeleteItem</c> operation are <see cref="ReturnValues.None"/> and <see cref="ReturnValues.AllOld"/>.
        /// </remarks>
        IDeleteItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>DeleteItem operation builder.</returns>
        IDeleteItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IDeleteItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Suppresses the throwing of the exceptions related to the DeleteItem operation.
        /// </summary>
        /// <returns></returns>
        ISuppressedDeleteItemDocumentRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the DeleteItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the DeleteItem operation and returns the item's attributes before the delete.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Item attributes are returned as they appeared before the DeleteItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the DeleteItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<DeleteItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface ISuppressedDeleteItemDocumentRequestBuilder<TEntity> : ITableBuilder<ISuppressedDeleteItemDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the DeleteItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the DeleteItem operation and returns the operation result with item's attributes before the delete.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The item is returned as it appeared before the DeleteItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<OpResult<Document?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the DeleteItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<DeleteItemEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}