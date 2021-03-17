using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Context.Operations.Scan
{
    public class ScanEntityRequestBuilder<TEntity> : IScanEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public ScanEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private ScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ScanAsyncEnumerable<TEntity>(tableName, _node);
        }
        
        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ParallelScanAsyncEnumerable<TEntity>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanPageAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
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
        
        public IScanEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IScanEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new ScanEntityRequestBuilder<TEntity, TProjection>(_context, new ProjectedAttributesNode(typeof(TProjection), properties, _node));
        
        public IScanEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new ScanEntityRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(typeof(TEntity), properties, _node));
    }

    public class ScanEntityRequestBuilder<TEntity, TProjection> : IScanEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public ScanEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal ScanEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IAsyncEnumerable<IReadOnlyList<TProjection>> ToAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ScanAsyncEnumerable<TProjection>(tableName, _node);
        }

        public IAsyncEnumerable<IReadOnlyList<TProjection>> ToParallelAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ParallelScanAsyncEnumerable<TProjection>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanPageAsync<TProjection>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanAsync<TProjection>(tableName, _node, cancellationToken).ConfigureAwait(false);
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
    }

    public class ScanDocumentRequestBuilder<TEntity> : IScanDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public ScanDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal ScanDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public IAsyncEnumerable<IReadOnlyList<Document>> ToAsyncEnumerable()
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ScanAsyncEnumerable<Document>(tableName, _node);
        }
        public IAsyncEnumerable<IReadOnlyList<Document>> ToParallelAsyncEnumerable(int totalSegments)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ParallelScanAsyncEnumerable<Document>(tableName, _node, totalSegments);
        }

        public async Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanPageAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<ScanEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
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
    }
}