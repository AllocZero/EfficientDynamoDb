using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Query
{
    internal sealed class QueryEntityRequestBuilder<TEntity> : IQueryEntityRequestBuilder<TEntity> where TEntity : class 
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IQueryEntityRequestBuilder<TEntity>>.Node => _node;

        IQueryEntityRequestBuilder<TEntity> ITableBuilder<IQueryEntityRequestBuilder<TEntity>>.Create(BuilderNode newNode) =>
            new QueryEntityRequestBuilder<TEntity>(_context, newNode);

        public QueryEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private QueryEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<IReadOnlyList<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<TEntity>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async IAsyncEnumerable<TEntity> ToAsyncEnumerable()
        {
            await foreach (var page in ToPagedAsyncEnumerable().ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }

        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToPagedAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.QueryAsyncEnumerable<TEntity>(tableName, GetNode());
        }
        
        public async Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<TEntity>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<TEntity>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IQueryDocumentRequestBuilder<TEntity> AsDocuments() => new QueryDocumentRequestBuilder<TEntity>(_context, _node);

        public IQueryEntityRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public IQueryEntityRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new KeyExpressionNode(keySetup(Condition.ForEntity<TEntity>()), _node));

        public IQueryEntityRequestBuilder<TEntity> FromIndex(string indexName) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public IQueryEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        // TODO: Consider restricting calling ToListAsync() when limit is set because it might be a source of errors
        public IQueryEntityRequestBuilder<TEntity> WithLimit(int limit) => new QueryEntityRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));
        
        public IQueryEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IQueryEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public IQueryEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IQueryEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IQueryEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) => 
            new QueryEntityRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));
        public IQueryEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));
        
        public IQueryEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public IQueryEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new QueryEntityRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));
        
        public ISuppressedQueryEntityRequestBuilder<TEntity> SuppressThrowing() => new SuppressedQueryEntityRequestBuilder<TEntity>(_context, _node);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }
    
    internal sealed class SuppressedQueryEntityRequestBuilder<TEntity> : ISuppressedQueryEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        
        internal SuppressedQueryEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<OpResult<IReadOnlyList<TEntity>>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<QueryEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<PagedResult<TEntity>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<TEntity>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }
    
    internal sealed class QueryEntityRequestBuilder<TEntity, TProjection> : IQueryEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IQueryEntityRequestBuilder<TEntity, TProjection>>.Node => _node;

        IQueryEntityRequestBuilder<TEntity, TProjection> ITableBuilder<IQueryEntityRequestBuilder<TEntity, TProjection>>.Create(BuilderNode newNode) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, newNode);

        public QueryEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal QueryEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<IReadOnlyList<TProjection>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<TProjection>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }
        
        public async IAsyncEnumerable<TProjection> ToAsyncEnumerable()
        {
            await foreach (var page in ToPagedAsyncEnumerable().ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }

        public IAsyncEnumerable<IReadOnlyList<TProjection>> ToPagedAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.QueryAsyncEnumerable<TProjection>(tableName, GetNode());
        }
        
        public async Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<TProjection>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<TProjection>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IQueryDocumentRequestBuilder<TEntity> AsDocuments() => new QueryDocumentRequestBuilder<TEntity>(_context, _node);

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new KeyExpressionNode(keySetup(Condition.ForEntity<TEntity>()), _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new IndexNameNode(indexName, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit) => new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new LimitNode(limit, _node));
        
        public IQueryEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new SelectNode(selectMode, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) => 
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));
        
        public IQueryEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken) =>
            new QueryEntityRequestBuilder<TEntity, TProjection>(_context, new PaginationTokenNode(paginationToken, _node));
        
        public ISuppressedQueryEntityRequestBuilder<TEntity, TProjection> SuppressThrowing() => 
            new SuppressedQueryEntityRequestBuilder<TEntity, TProjection>(_context, _node);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }

    internal sealed class SuppressedQueryEntityRequestBuilder<TEntity, TProjection> : ISuppressedQueryEntityRequestBuilder<TEntity, TProjection>
        where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        
        internal SuppressedQueryEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<OpResult<IReadOnlyList<TProjection>>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<TProjection>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<QueryEntityResponse<TProjection>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<TProjection>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<PagedResult<TProjection>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<TProjection>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }

    internal sealed class QueryDocumentRequestBuilder<TEntity> : IQueryDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IQueryDocumentRequestBuilder<TEntity>>.Node => _node;

        IQueryDocumentRequestBuilder<TEntity> ITableBuilder<IQueryDocumentRequestBuilder<TEntity>>.Create(BuilderNode newNode) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, newNode);

        public QueryDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal QueryDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<IReadOnlyList<Document>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<Document>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async IAsyncEnumerable<Document> ToAsyncEnumerable()
        {
            await foreach (var page in ToPagedAsyncEnumerable().ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }
        
        public IAsyncEnumerable<IReadOnlyList<Document>> ToPagedAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.QueryAsyncEnumerable<Document>(tableName, GetNode());
        }

        public async Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<Document>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<Document>(tableName, GetNode(), cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new KeyExpressionNode(keySetup(Condition.ForEntity<TEntity>()), _node));

        public IQueryDocumentRequestBuilder<TEntity> FromIndex(string indexName) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithLimit(int limit) => new QueryDocumentRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));

        public IQueryDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));
        public IQueryDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public IQueryDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) => 
            new QueryDocumentRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));
        public IQueryDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new QueryDocumentRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));
        
        public ISuppressedQueryDocumentRequestBuilder<TEntity> SuppressThrowing() => new SuppressedQueryDocumentRequestBuilder<TEntity>(_context, _node);
        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");

    }
    
    internal sealed class SuppressedQueryDocumentRequestBuilder<TEntity> : ISuppressedQueryDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        
        internal SuppressedQueryDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<OpResult<IReadOnlyList<Document>>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<QueryEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<PagedResult<Document>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryPageAsync<Document>(tableName, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty query request.");
    }
}