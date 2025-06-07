using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.UpdateItem
{
    /// <summary>
    /// Represents a builder for the UpdateItem operation. Allows configuring parameters and options for saving an item in a table.
    /// </summary>
    public interface IUpdateEntityRequestBuilder<TEntity> : IUpdateItemBuilder<IUpdateEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// All values of <see cref="ReturnValues"/> are supported for <c>UpdateItem</c> operation.
        /// </remarks>
        IUpdateEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        /// <summary>
        /// Specifies how to handle return values if the operation fails.
        /// </summary>
        /// <param name="option">Option for handling return values on condition check failure.</param>
        /// <returns>UpdateItem operation builder.</returns>
        IUpdateEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>UpdateItem operation builder.</returns>
        IUpdateEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        // IUpdateEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies condition for the UpdateItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail.
        /// </remarks>
        IUpdateEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the UpdateItem operation.
        /// </summary>
        /// <param name="filterSetup">The condition function to set.</param>
        /// <returns>The UpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail.
        /// </remarks>
        IUpdateEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        /// <summary>
        /// Specifies partition and sort keys of the item to update.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to update.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        /// <summary>
        /// Specifies the attribute to be updated in the DynamoDB item.
        /// </summary>
        /// <param name="expression">An expression identifying the attribute to be updated.</param>
        /// <typeparam name="TProperty">The type of the attribute to be updated.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// To update multiple attributes, call this method multiple times.
        /// For a detailed walkthrough and examples, refer to the developer guide: https://alloczero.github.io/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/update-expression
        /// </remarks>
        IAttributeUpdate<IUpdateEntityRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        /// <summary>
        /// Represents the returned item as <see cref="Document"/>.
        /// </summary>
        /// <returns>UpdateItem operation builder suitable for document response.</returns>
        IUpdateDocumentRequestBuilder<TEntity> AsDocument();
        
        /// <summary>
        /// Suppresses the throwing of the exceptions related to the UpdateItem operation.
        /// </summary>
        ISuppressedUpdateItemEntityRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the UpdateItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the UpdateItem operation and returns the item according to the <see cref="ReturnValues"/> option set in <see cref="WithReturnValues"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// If <see cref="ReturnValues"/> is set to <see cref="ReturnValues.None"/>, this method will always return null.
        /// </remarks>
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the UpdateItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<UpdateItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface ISuppressedUpdateItemEntityRequestBuilder<TEntity> : IUpdateItemBuilder<ISuppressedUpdateItemEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the UpdateItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the UpdateItem operation and returns the operation result with item according to the <see cref="ReturnValues"/> option set in <see cref="IUpdateEntityRequestBuilder{TEntity}.WithReturnValues"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// If <see cref="ReturnValues"/> is set to <see cref="ReturnValues.None"/>, this method will always return null.
        /// </remarks>
        Task<OpResult<TEntity?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the UpdateItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<UpdateItemEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Represents a builder for the UpdateItem operation with an entity type constraint and a document response.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUpdateDocumentRequestBuilder<TEntity> : IUpdateItemBuilder<IUpdateDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the attributes to include in the response.
        /// </summary>
        /// <param name="returnValues">The <see cref="ReturnValues"/> option.</param>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// All values of <see cref="ReturnValues"/> are supported for <c>UpdateItem</c> operation.
        /// </remarks>
        IUpdateDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>UpdateItem operation builder.</returns>
        IUpdateDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        // IUpdateDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies condition for the UpdateItem operation.
        /// </summary>
        /// <param name="condition">Condition to set.</param>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail.
        /// </remarks>
        IUpdateDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);

        /// <summary>
        /// Specifies the condition function for the UpdateItem operation.
        /// </summary>
        /// <param name="filterSetup">The condition function to set.</param>
        /// <returns>The UpdateItem operation builder.</returns>
        /// <remarks>
        /// This condition is used to determine whether the UpdateItem operation should succeed or fail.
        /// </remarks>
        IUpdateDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        /// <summary>
        /// Specifies partition and sort keys of the item to update.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to update.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        /// <summary>
        /// Specifies the attribute to be updated in the DynamoDB item.
        /// </summary>
        /// <param name="expression">An expression identifying the attribute to be updated.</param>
        /// <typeparam name="TProperty">The type of the attribute to be updated.</typeparam>
        /// <returns>UpdateItem operation builder.</returns>
        /// <remarks>
        /// To update multiple attributes, call this method multiple times.
        /// For a detailed walkthrough and examples, refer to the developer guide: https://alloczero.github.io/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/update-expression
        /// </remarks>
        IAttributeUpdate<IUpdateDocumentRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
        
        /// <summary>
        /// Suppresses the throwing of the exceptions related to the UpdateItem operation.
        /// </summary>
        /// <returns></returns>
        ISuppressedUpdateItemDocumentRequestBuilder<TEntity> SuppressThrowing();
        
        /// <summary>
        /// Executes the UpdateItem operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the UpdateItem operation and returns the item according to the <see cref="ReturnValues"/> option set in <see cref="WithReturnValues"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// If <see cref="ReturnValues"/> is set to <see cref="ReturnValues.None"/>, this method will always return null.
        /// </remarks>
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the UpdateItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<UpdateItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }

    public interface ISuppressedUpdateItemDocumentRequestBuilder<TEntity> : IUpdateItemBuilder<ISuppressedUpdateItemDocumentRequestBuilder<TEntity>>
        where TEntity : class
    {
        /// <summary>
        /// Executes the UpdateItem operation and returns the operation result.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the UpdateItem operation and returns the operation result with item according to the <see cref="ReturnValues"/> option set in <see cref="IUpdateDocumentRequestBuilder{TEntity}.WithReturnValues"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// If <see cref="ReturnValues"/> is set to <see cref="ReturnValues.None"/>, this method will always return null.
        /// </remarks>
        Task<OpResult<Document?>> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the UpdateItem operation and returns the operation results with deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<UpdateItemEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}