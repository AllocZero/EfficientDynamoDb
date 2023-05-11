using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactGetItems
{
    /// <summary>
    /// Represents a builder for the TransactGet operation.
    /// </summary>
    public interface ITransactGetItemsEntityRequestBuilder
    {
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>TransactGet operation builder.</returns>
        ITransactGetItemsEntityRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        /// <summary>
        /// Specifies the items to retrieve in transaction.
        /// </summary>
        /// <param name="items">TransactGet item builders.</param>
        /// <returns>TransactGet operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactGetItemsEntityRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items);
        
        /// <summary>
        /// Specifies the items to retrieve in transaction.
        /// </summary>
        /// <param name="items">TransactGet item builders.</param>
        /// <returns>TransactGet operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactGetItemsEntityRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items);

        /// <summary>
        /// Represents the returned items as <see cref="Document"/>.
        /// </summary>
        /// <returns>TransactGet operation builder suitable for document response.</returns>
        ITransactGetItemsDocumentRequestBuilder AsDocuments();
        
        /// <summary>
        /// Executes the TransactGet operation and returns the list of entities.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TResultEntity">Type of the DB entity.</typeparam>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        /// <summary>
        /// Executes the TransactGet operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<TransactGetItemsEntityResponse<TResultEntity>> ToResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
    }
    
    /// <summary>
    /// Represents a builder for the TransactGet operation.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    public interface ITransactGetItemsDocumentRequestBuilder
    {
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>TransactGet operation builder.</returns>
        ITransactGetItemsDocumentRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        /// <summary>
        /// Specifies the items to retrieve in transaction.
        /// </summary>
        /// <param name="items">TransactGet item builders.</param>
        /// <returns>TransactGet operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactGetItemsDocumentRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items);
        
        /// <summary>
        /// Specifies the items to retrieve in transaction.
        /// </summary>
        /// <param name="items">TransactGet item builders.</param>
        /// <returns>TransactGet operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactGetItemsDocumentRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items);
        
        /// <summary>
        /// Executes the TransactGet operation and returns the list of entities.
        /// Every entity is represented as <see cref="Document"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<Document?>> ToListAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the TransactGet operation and returns the deserialized response.
        /// Every entity in the response is represented as <see cref="Document"/>.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<TransactGetItemsEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}