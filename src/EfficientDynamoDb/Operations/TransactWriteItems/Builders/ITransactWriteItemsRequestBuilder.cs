using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemsRequestBuilder
    {
        ITransactWriteItemsRequestBuilder WithClientRequestToken(string token);
        
        ITransactWriteItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // ITransactWriteItemsRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        ITransactWriteItemsRequestBuilder WithItems(params ITransactWriteItemBuilder[] items);
        
        ITransactWriteItemsRequestBuilder WithItems(IEnumerable<ITransactWriteItemBuilder> items);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TransactWriteItemsEntityResponse> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}