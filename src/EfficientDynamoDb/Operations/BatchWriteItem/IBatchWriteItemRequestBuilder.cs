using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    /// <summary>
    /// Represents a builder for the BatchWrite operation.
    /// </summary>
    public interface IBatchWriteItemRequestBuilder
    {
        /// <summary>
        /// Executes the BatchWrite operation.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the BatchWrite operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<BatchWriteItemResponse> ToResponseAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Specify the write operations to perform in batch.
        /// </summary>
        /// <param name="items">BatchWrite item builders.</param>
        /// <returns>BatchWrite operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Batch"/> static class to construct item builders. <br/><br/>
        /// Due to DynamoDB's restrictions, a single batch is limited to 25 operations.
        /// It is the responsibility of the caller to ensure the operation does not exceed this limit.
        /// A batch request that surpasses this limit will fail, resulting in an exception.
        /// </remarks>
        IBatchWriteItemRequestBuilder WithItems(params IBatchWriteBuilder[] items);
        
        /// <summary>
        /// Specify the write operations to perform in batch.
        /// </summary>
        /// <param name="items">BatchWrite item builders.</param>
        /// <returns>BatchWrite operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Batch"/> static class to construct item builders.
        /// </remarks>
        IBatchWriteItemRequestBuilder WithItems(IEnumerable<IBatchWriteBuilder> items);
        
        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="returnConsumedCapacity">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>BatchWrite operation builder.</returns>
        IBatchWriteItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        /// <summary>
        /// Specifies whether item collection metrics are returned.
        /// </summary>
        /// <param name="returnItemCollectionMetrics">The <see cref="ReturnItemCollectionMetrics"/> option.</param>
        /// <returns>BatchWrite operation builder.</returns>
        IBatchWriteItemRequestBuilder WithReturnItemCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
    }
}