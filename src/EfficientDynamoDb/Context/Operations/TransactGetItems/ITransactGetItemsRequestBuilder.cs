using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public interface ITransactGetItemsRequestBuilder
    {
        ITransactGetItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        ITransactGetItemsRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items);
        
        ITransactGetItemsRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items);
        
        Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        Task<List<Document?>> ToDocumentListAsync(CancellationToken cancellationToken = default);
        
        Task<TransactGetItemsEntityResponse<TResultEntity>> ToEntityResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        Task<TransactGetItemsEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}