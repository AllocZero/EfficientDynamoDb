using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public readonly struct QueryRequestBuilder<TEntity> where TEntity : class 
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public QueryRequestBuilder(DynamoDbContext context)
        {
            _context = context;
            _node = null;
        }

        private QueryRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<IReadOnlyList<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryListAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<IReadOnlyList<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryListAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.QueryAsyncEnumerable<TEntity>(tableName, GetNode());
        }

        public IAsyncEnumerable<IReadOnlyList<Document>> ToDocumentAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.QueryAsyncEnumerable<Document>(tableName, GetNode());
        }

        public async Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryPageAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<Document>> ToDocumentPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryPageAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<QueryEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public QueryRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryRequestBuilder<TEntity>(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public QueryRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup) =>
            new QueryRequestBuilder<TEntity>(_context, new KeyExpressionNode(keySetup(Condition.ForEntity<TEntity>()), _node));

        public QueryRequestBuilder<TEntity> FromIndex(string indexName) =>
            new QueryRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public QueryRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new QueryRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public QueryRequestBuilder<TEntity> WithLimit(int limit) => new QueryRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));
        
        public QueryRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new QueryRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public QueryRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new QueryRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public QueryRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties)=>
            new QueryRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public QueryRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public QueryRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new QueryRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public QueryRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new QueryRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public QueryRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public QueryRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) => 
            new QueryRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));
        public QueryRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new QueryRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));
        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }
}