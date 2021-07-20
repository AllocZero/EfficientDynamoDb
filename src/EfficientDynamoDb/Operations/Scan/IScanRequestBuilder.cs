using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Scan
{
    public interface IScanEntityRequestBuilder<TEntity> : ITableBuilder<IScanEntityRequestBuilder<TEntity>> where TEntity : class
    {
        IScanEntityRequestBuilder<TEntity> FromIndex(string indexName);

        IScanEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IScanEntityRequestBuilder<TEntity> WithLimit(int limit);

        IScanEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IScanEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        IScanEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        IScanEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        IScanEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        IScanEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
        
        IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class;

        IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IScanEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        IScanDocumentRequestBuilder<TEntity> AsDocuments();
       
        IAsyncEnumerable<TEntity> ToAsyncEnumerable();
        
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToPagedAsyncEnumerable();
        
        IAsyncEnumerable<TEntity> ToParallelAsyncEnumerable(int totalSegments);
        
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelPagedAsyncEnumerable(int totalSegments);
        
        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IScanEntityRequestBuilder<TEntity, TProjection> : ITableBuilder<IScanEntityRequestBuilder<TEntity, TProjection>> where TEntity : class where TProjection : class
    {
        IScanEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName);

        IScanEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        IScanEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit);

        IScanEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IScanEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode);

        IScanEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch);

        IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder);

        IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        IScanEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken);
        
        IScanDocumentRequestBuilder<TEntity> AsDocuments();
       
        IAsyncEnumerable<TProjection> ToAsyncEnumerable();
        
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToPagedAsyncEnumerable();
        
        IAsyncEnumerable<TProjection> ToParallelAsyncEnumerable(int totalSegments);
        
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToParallelPagedAsyncEnumerable(int totalSegments);

        Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<ScanEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IScanDocumentRequestBuilder<TEntity> : ITableBuilder<IScanDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        IScanDocumentRequestBuilder<TEntity> FromIndex(string indexName);

        IScanDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IScanDocumentRequestBuilder<TEntity> WithLimit(int limit);
        
        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        IScanDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IScanDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        IScanDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        IScanDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        IScanDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        IScanDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
       
        IAsyncEnumerable<Document> ToAsyncEnumerable();
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToPagedAsyncEnumerable();
        
        IAsyncEnumerable<Document> ToParallelAsyncEnumerable(int totalSegments);
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToParallelPagedAsyncEnumerable(int totalSegments);
        
        Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<ScanEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}