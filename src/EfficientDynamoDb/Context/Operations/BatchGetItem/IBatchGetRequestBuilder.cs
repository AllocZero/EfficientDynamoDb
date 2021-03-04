
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetRequestBuilder
    {
        Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;
        
        Task<List<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default);

        IBatchGetRequestBuilder FromTables(params IBatchGetTableBuilder[] tables);
        
        IBatchGetRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables);
        
        IBatchGetRequestBuilder WithItems(params IBatchGetItemBuilder[] items);
        
        IBatchGetRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items);
    }
}