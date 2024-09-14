using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Scan
{
    internal sealed class ScanEntityRequestBuilder<TEntity> : IScanEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IScanEntityRequestBuilder<TEntity>>.Node => _node;

        IScanEntityRequestBuilder<TEntity> ITableBuilder<IScanEntityRequestBuilder<TEntity>>.Create(BuilderNode newNode) =>
            new ScanEntityRequestBuilder<TEntity>(_context, newNode);

        public ScanEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private ScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
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
            return _context.ScanAsyncEnumerable<TEntity>(tableName, _node);
        }
        
        public async IAsyncEnumerable<TEntity> ToParallelAsyncEnumerable(int totalSegments)
        {
            await foreach (var page in ToParallelPagedAsyncEnumerable(totalSegments).ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }
        
        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelPagedAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.ParallelScanAsyncEnumerable<TEntity>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<TEntity>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<TEntity>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IScanEntityRequestBuilder<TEntity> FromIndex(string indexName) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public IScanEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IScanEntityRequestBuilder<TEntity> WithLimit(int limit) => new ScanEntityRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));
        
        public IScanEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IScanEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public IScanEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IScanEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IScanEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public IScanEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));

        public IScanDocumentRequestBuilder<TEntity> AsDocuments() => new ScanDocumentRequestBuilder<TEntity>(_context, _node);
        
        public IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));
        
        public IScanEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));
        
        public ISuppressedScanEntityRequestBuilder<TEntity> SuppressThrowing() => new SuppressedScanEntityRequestBuilder<TEntity>(_context, _node);
    }

    internal sealed class SuppressedScanEntityRequestBuilder<TEntity> : ISuppressedScanEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        internal SuppressedScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<OpResult<PagedResult<TEntity>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<ScanEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }
    }

    public class ScanEntityRequestBuilder<TEntity, TProjection> : IScanEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IScanEntityRequestBuilder<TEntity, TProjection>>.Node => _node;

        IScanEntityRequestBuilder<TEntity, TProjection> ITableBuilder<IScanEntityRequestBuilder<TEntity, TProjection>>.Create(BuilderNode newNode) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, newNode);

        public ScanEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal ScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
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
            return _context.ScanAsyncEnumerable<TProjection>(tableName, _node);
        }
        
        public async IAsyncEnumerable<TProjection> ToParallelAsyncEnumerable(int totalSegments)
        {
            await foreach (var page in ToParallelPagedAsyncEnumerable(totalSegments).ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }

        public IAsyncEnumerable<IReadOnlyList<TProjection>> ToParallelPagedAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.ParallelScanAsyncEnumerable<TProjection>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<TProjection>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<TProjection>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IScanEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new IndexNameNode(indexName, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit) => new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new LimitNode(limit, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new SelectNode(selectMode, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken) =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new PaginationTokenNode(paginationToken, _node));

        public IScanDocumentRequestBuilder<TEntity> AsDocuments() => new ScanDocumentRequestBuilder<TEntity>(_context, _node);

        public ISuppressedScanEntityRequestBuilder<TEntity, TProjection> SuppressThrowing() =>
            new SuppressedScanEntityRequestBuilder<TEntity, TProjection>(_context, _node);
    }
    
    internal sealed class SuppressedScanEntityRequestBuilder<TEntity, TProjection> : ISuppressedScanEntityRequestBuilder<TEntity, TProjection>
        where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        internal SuppressedScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<OpResult<PagedResult<TProjection>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<TProjection>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<ScanEntityResponse<TProjection>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<TProjection>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }
    }

    public class ScanDocumentRequestBuilder<TEntity> : IScanDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IScanDocumentRequestBuilder<TEntity>>.Node => _node;

        IScanDocumentRequestBuilder<TEntity> ITableBuilder<IScanDocumentRequestBuilder<TEntity>>.Create(BuilderNode newNode) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, newNode);

        public ScanDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal ScanDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
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
            return _context.ScanAsyncEnumerable<Document>(tableName, _node);
        }
        
        public async IAsyncEnumerable<Document> ToParallelAsyncEnumerable(int totalSegments)
        {
            await foreach (var page in ToParallelPagedAsyncEnumerable(totalSegments).ConfigureAwait(false))
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < page.Count; i++)
                {
                    yield return page[i];
                }
            }
        }
        
        public IAsyncEnumerable<IReadOnlyList<Document>> ToParallelPagedAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return _context.ParallelScanAsyncEnumerable<Document>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<Document>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }
        
        public async Task<ScanEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<Document>(tableName, _node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        }

        public IScanDocumentRequestBuilder<TEntity> FromIndex(string indexName) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public IScanDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IScanDocumentRequestBuilder<TEntity> WithLimit(int limit) => new ScanDocumentRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));

        public IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));
        
        public IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public IScanDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IScanDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public IScanDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IScanDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IScanDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public IScanDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new ScanDocumentRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));
        
        public ISuppressedScanDocumentRequestBuilder<TEntity> SuppressThrowing() => new SuppressedScanDocumentRequestBuilder<TEntity>(_context, _node);
    }
    
    internal sealed class SuppressedScanDocumentRequestBuilder<TEntity> : ISuppressedScanDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        internal SuppressedScanDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<OpResult<PagedResult<Document>>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanPageAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<OpResult<ScanEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.ScanAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }
    }
}