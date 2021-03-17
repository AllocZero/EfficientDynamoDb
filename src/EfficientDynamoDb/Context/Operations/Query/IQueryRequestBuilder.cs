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

    public interface IQueryEntityRequestBuilder<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

        Task<QueryEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);

        IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable();

        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        IQueryDocumentRequestBuilder<TEntity> AsDocuments();
        
        IQueryEntityRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder);

        IQueryEntityRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        IQueryEntityRequestBuilder<TEntity> FromIndex(string indexName);

        IQueryEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IQueryEntityRequestBuilder<TEntity> WithLimit(int limit);

        IQueryEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IQueryEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        IQueryEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        IQueryEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        IQueryEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        IQueryEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
        
        IQueryEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>() where TProjection : class;
        
        IQueryEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IQueryEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
    }
    
     public interface IQueryEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        Task<IReadOnlyList<TProjection>> ToListAsync(CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToAsyncEnumerable();
        
        Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default);
        
        Task<QueryEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);

        IQueryDocumentRequestBuilder<TEntity> AsDocuments();
        
        IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(FilterBase keyExpressionBuilder);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        IQueryEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit);

        IQueryEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode);

        IQueryEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        IQueryEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken);
    }
    
    public interface IQueryDocumentRequestBuilder<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<Document>> ToListAsync(CancellationToken cancellationToken = default);
        
        Task<QueryEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
        
        IAsyncEnumerable<IReadOnlyList<Document>> ToAsyncEnumerable();
        
        Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default);
        
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;
        
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder);

        IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        IQueryDocumentRequestBuilder<TEntity> FromIndex(string indexName);

        IQueryDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IQueryDocumentRequestBuilder<TEntity> WithLimit(int limit);

        IQueryDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        IQueryDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        IQueryDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        IQueryDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
    }
}