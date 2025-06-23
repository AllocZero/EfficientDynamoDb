using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.PutItem
{
    /// <summary>
    /// Represents a builder for the PutItem operation. Allows configuring parameters and options for saving an item in a table.
    /// </summary>
    public interface IPutItemRequestBuilder : ITableBuilder<IPutItemRequestBuilder>
    {
        /// <summary>
        /// Specifies the item to save.
        /// </summary>
        /// <param name="item">The item to save.</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>PutItem operation builder.</returns>
        IPutItemEntityRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class;
        
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// The only supported values for <c>PutItem</c> operation are <see cref="ReturnValues.None"/> and <see cref="ReturnValues.AllOld"/>.
        /// </remarks>
        IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="option">Option for handling return values on condition check failure.</param>
        /// <returns>PutItem operation builder.</returns>
        IPutItemRequestBuilder WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);
        
        // IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies condition for the PutItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail.
        /// </remarks>
        IPutItemRequestBuilder WithCondition(FilterBase condition);
    }

    /// <summary>
    /// Represents a builder for the PutItem operation with an entity type constraint.
    /// Provides methods for configuring options and executing the operation with typed response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IPutItemEntityRequestBuilder<TEntity> : ITableBuilder<IPutItemEntityRequestBuilder<TEntity>> where TEntity: class
    {
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// The only supported values for <c>PutItem</c> operation are <see cref="ReturnValues.None"/> and <see cref="ReturnValues.AllOld"/>.
        /// </remarks>
        IPutItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);

        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="option">Option for handling return values on condition check failure.</param>
        /// <returns>PutItem operation builder.</returns>
        IPutItemEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        IPutItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IPutItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies condition for the PutItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail.
        /// </remarks>
        IPutItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);
        
        /// <summary>
        /// Specifies the condition function for the PutItem operation.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>The PutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail.
        /// </remarks>
        IPutItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        /// <summary>
        /// Represents the returned item as <see cref="Document"/>.
        /// </summary>
        /// <returns>PutItem operation builder suitable for document response.</returns>
        IPutItemDocumentRequestBuilder<TEntity> AsDocument();

        /// <summary>
        /// Suppresses the throwing of the exceptions related to the PutItem operation.
        /// </summary>
        ISuppressedPutItemEntityRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the PutItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the PutItem operation and returns the item before the update.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The item is returned as it appeared before the PutItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the PutItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PutItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);

    }
    
    public interface ISuppressedPutItemEntityRequestBuilder<TEntity> : ITableBuilder<ISuppressedPutItemEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the PutItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the PutItem operation and returns the operation result with item before the update.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The item is returned as it appeared before the PutItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<OpResult<TEntity?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the PutItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<PutItemEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Represents a builder for the PutItem operation with an entity type constraint and a document response.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IPutItemDocumentRequestBuilder<TEntity> : ITableBuilder<IPutItemDocumentRequestBuilder<TEntity>> where TEntity: class
    {
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// The only supported values for <c>PutItem</c> operation are <see cref="ReturnValues.None"/> and <see cref="ReturnValues.AllOld"/>.
        /// </remarks>
        IPutItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>PutItem operation builder.</returns>
        IPutItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IPutItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies condition for the PutItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>PutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail.
        /// </remarks>
        IPutItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);
        
        /// <summary>
        /// Specifies the condition function for the PutItem operation.
        /// </summary>
        /// <param name="conditionSetup">The condition function to set.</param>
        /// <returns>The PutItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the PutItem operation should succeed or fail.
        /// </remarks>
        IPutItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        /// <summary>
        /// Suppresses the throwing of the exceptions related to the PutItem operation.
        /// </summary>
        ISuppressedPutItemDocumentRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the PutItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the PutItem operation and returns the item's attributes before the update.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Item attributes are returned as they appeared before the PutItem operation, but only if <see cref="WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the PutItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PutItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface ISuppressedPutItemDocumentRequestBuilder<TEntity> : ITableBuilder<ISuppressedPutItemDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the PutItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the PutItem operation and returns the operation result with item's attributes before the update.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The item is returned as it appeared before the PutItem operation, but only if <see cref="IPutItemRequestBuilder.WithReturnValues"/> with <see cref="ReturnValues.AllOld"/> was specified in the request chain.
        /// Otherwise, <c>null</c> is returned.
        /// </remarks>
        Task<OpResult<Document?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the PutItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<PutItemEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}