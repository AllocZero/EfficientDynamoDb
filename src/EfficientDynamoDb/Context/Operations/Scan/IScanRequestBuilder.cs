using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Scan
{
    public interface IScanRequestBuilder<TEntity> where TEntity : class
    {
        IScanRequestBuilder<TEntity> FromIndex(string indexName);

        IScanRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IScanRequestBuilder<TEntity> WithLimit(int limit);
        
        IScanRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        IScanRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IScanRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        IScanRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IScanRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        IScanRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        IScanRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        IScanRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        IScanRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
       
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable(CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToDocumentAsyncEnumerable(CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelAsyncEnumerable(int totalSegments, CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToParallelDocumentAsyncEnumerable(int totalSegments, CancellationToken cancellationToken = default);
        
        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<PagedResult<Document>> ToDocumentPageAsync(CancellationToken cancellationToken = default);
        
        Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
        
        Task<ScanEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}