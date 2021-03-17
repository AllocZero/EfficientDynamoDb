
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetEntityRequestBuilder
    {
        Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;
        
        IBatchGetEntityRequestBuilder FromTables(params IBatchGetTableBuilder[] tables);
        
        IBatchGetEntityRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables);
        
        IBatchGetEntityRequestBuilder WithItems(params IBatchGetItemBuilder[] items);
        
        IBatchGetEntityRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items);

        IBatchGetDocumentRequestBuilder AsDocuments();
    }
    
    public interface IBatchGetDocumentRequestBuilder
    {
        Task<List<Document>> ToListAsync(CancellationToken cancellationToken = default);

        IBatchGetDocumentRequestBuilder FromTables(params IBatchGetTableBuilder[] tables);
        
        IBatchGetDocumentRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables);
        
        IBatchGetDocumentRequestBuilder WithItems(params IBatchGetItemBuilder[] items);
        
        IBatchGetDocumentRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items);
    }
}