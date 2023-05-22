using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    /// <summary>
    /// Represents a builder for the TransactWrite operation.
    /// </summary>
    public interface ITransactWriteItemsRequestBuilder
    {
        /// <summary>
        /// Specifies a token that makes the call idempotent, meaning that multiple identical calls have the same effect as one single call
        /// </summary>
        /// <param name="token">Token used to ensure idempotency.</param>
        /// <returns>TransactWrite operation builder.</returns>
        ITransactWriteItemsRequestBuilder WithClientRequestToken(string token);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>TransactWrite operation builder.</returns>
        ITransactWriteItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // ITransactWriteItemsRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        /// <summary>
        /// Specifies the operations to perform in transaction.
        /// </summary>
        /// <param name="items">TransactWrite item builders.</param>
        /// <returns>TransactWrite operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactWriteItemsRequestBuilder WithItems(params ITransactWriteItemBuilder[] items);
        
        /// <summary>
        /// Specifies the operations to perform in transaction.
        /// </summary>
        /// <param name="items">TransactWrite item builders.</param>
        /// <returns>TransactWrite operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Transact"/> static class to construct item builders.
        /// </remarks>
        ITransactWriteItemsRequestBuilder WithItems(IEnumerable<ITransactWriteItemBuilder> items);
        
        /// <summary>
        /// Executes the TransactWrite operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the TransactWrite operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<TransactWriteItemsEntityResponse> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}