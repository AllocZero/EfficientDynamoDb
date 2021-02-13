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

namespace EfficientDynamoDb.Context.Operations.Query
{
    public interface IQueryRequestBuilder<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> ToListAsync(CancellationToken cancellationToken = default);
        
        Task<IReadOnlyList<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default);
        
        Task<QueryEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);

        Task<QueryEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);

        IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable(CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToDocumentAsyncEnumerable(CancellationToken cancellationToken = default);

        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<PagedResult<Document>> ToDocumentPageAsync(CancellationToken cancellationToken = default);
        
        public IQueryRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder);
        
        public IQueryRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);
        
        public IQueryRequestBuilder<TEntity> FromIndex(string indexName);

        public IQueryRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        public IQueryRequestBuilder<TEntity> WithLimit(int limit);
        
        public IQueryRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        public IQueryRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        public IQueryRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        public IQueryRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        public IQueryRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        public IQueryRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        public IQueryRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        public IQueryRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        public IQueryRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
    }
}