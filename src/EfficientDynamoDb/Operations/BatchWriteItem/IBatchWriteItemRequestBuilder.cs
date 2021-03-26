using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public interface IBatchWriteItemRequestBuilder
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        IBatchWriteItemRequestBuilder WithItems(params IBatchWriteBuilder[] items);
        
        IBatchWriteItemRequestBuilder WithItems(IEnumerable<IBatchWriteBuilder> items);
    }
}