using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public interface ITransactGetItemsEntityRequestBuilder
    {
        ITransactGetItemsEntityRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        ITransactGetItemsEntityRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items);
        
        ITransactGetItemsEntityRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items);

        ITransactGetItemsDocumentRequestBuilder AsDocuments();
        
        Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        Task<TransactGetItemsEntityResponse<TResultEntity>> ToResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
    }
    
    public interface ITransactGetItemsDocumentRequestBuilder
    {
        ITransactGetItemsDocumentRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        ITransactGetItemsDocumentRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items);
        
        ITransactGetItemsDocumentRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items);
        
        Task<List<Document?>> ToListAsync(CancellationToken cancellationToken = default);
        
        Task<TransactGetItemsEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}