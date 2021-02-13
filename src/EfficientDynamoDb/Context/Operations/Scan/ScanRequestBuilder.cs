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
    public class ScanRequestBuilder<TEntity> : IScanRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public ScanRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private ScanRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToAsyncEnumerable(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ScanAsyncEnumerable<TEntity>(tableName, _node, cancellationToken);
        }

        public IAsyncEnumerable<IReadOnlyList<Document>> ToDocumentAsyncEnumerable(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ScanAsyncEnumerable<Document>(tableName, _node, cancellationToken);
        }
        
        public IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelAsyncEnumerable(int totalSegments, CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ParallelScanAsyncEnumerable<TEntity>(tableName, _node, totalSegments, cancellationToken);
        }

        public IAsyncEnumerable<IReadOnlyList<Document>> ToParallelDocumentAsyncEnumerable(int totalSegments, CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return _context.ParallelScanAsyncEnumerable<Document>(tableName, _node, totalSegments, cancellationToken);
        }

        public async Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanPageAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<Document>> ToDocumentPageAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanPageAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ScanEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default)
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.ScanAsync<Document>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public IScanRequestBuilder<TEntity> FromIndex(string indexName) =>
            new ScanRequestBuilder<TEntity>(_context, new IndexNameNode(indexName, _node));

        public IScanRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead) =>
            new ScanRequestBuilder<TEntity>(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IScanRequestBuilder<TEntity> WithLimit(int limit) => new ScanRequestBuilder<TEntity>(_context, new LimitNode(limit, _node));

        public IScanRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new ScanRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(_context.Config.Metadata.GetOrAddClassInfo(typeof(TProjection)), null, _node));

        public IScanRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new ScanRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(_context.Config.Metadata.GetOrAddClassInfo(typeof(TProjection)), properties, _node));
        
        public IScanRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new ScanRequestBuilder<TEntity>(_context, new ProjectedAttributesNode(_context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), properties, _node));

        public IScanRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new ScanRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IScanRequestBuilder<TEntity> WithSelectMode(Select selectMode) =>
            new ScanRequestBuilder<TEntity>(_context, new SelectNode(selectMode, _node));

        public IScanRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch) =>
            new ScanRequestBuilder<TEntity>(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IScanRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new ScanRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterExpressionBuilder, _node));

        public IScanRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new ScanRequestBuilder<TEntity>(_context, new FilterExpressionNode(filterSetup(Filter.ForEntity<TEntity>()), _node));

        public IScanRequestBuilder<TEntity> WithPaginationToken(string? paginationToken) =>
            new ScanRequestBuilder<TEntity>(_context, new PaginationTokenNode(paginationToken, _node));
    }
}