using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryRequestBuilder : IQueryRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public QueryRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private QueryRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<IReadOnlyList<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryListAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TEntity>> ToResponseAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryAsync<TEntity>(tableName, _node, cancellationToken).ConfigureAwait(false);
        }

        public IQueryRequestBuilder WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryRequestBuilder(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public IQueryRequestBuilder FromIndex(string indexName) =>
            new QueryRequestBuilder(_context, new IndexNameNode(indexName, _node));

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead) =>
            new QueryRequestBuilder(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IQueryRequestBuilder WithLimit(int limit) => new QueryRequestBuilder(_context, new LimitNode(limit, _node));
        
        public IQueryRequestBuilder WithProjectedAttributes<TProjection>() where TProjection : class =>
            new QueryRequestBuilder(_context, new ProjectedAttributesNode(_context.Config.Metadata.GetOrAddClassInfo(typeof(TProjection)), _node));

        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryRequestBuilder(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IQueryRequestBuilder WithSelectMode(Select selectMode) =>
            new QueryRequestBuilder(_context, new SelectNode(selectMode, _node));

        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch) =>
            new QueryRequestBuilder(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IQueryRequestBuilder WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryRequestBuilder(_context, new FilterExpressionNode(filterExpressionBuilder, _node));
        
        public IQueryRequestBuilder WithPaginationToken(string? paginationToken) =>
            new QueryRequestBuilder(_context, new PaginationTokenNode(paginationToken, _node));
    }
}