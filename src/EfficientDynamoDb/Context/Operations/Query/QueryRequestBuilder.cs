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
        private readonly BuilderNode<QueryHighLevelRequest>? _node;

        public QueryRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private QueryRequestBuilder(DynamoDbContext context, BuilderNode<QueryHighLevelRequest>? node)
        {
            _context = context;
            _node = node;
        }

        // TODO: Add ExclusiveStartKey support

        public async Task<IReadOnlyList<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryListAsync<TEntity>(Build(tableName), cancellationToken).ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TEntity>> ToResponseAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).GetTableName();
            return await _context.QueryAsync<TEntity>(Build(tableName), cancellationToken).ConfigureAwait(false);
        }

        public IQueryRequestBuilder WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryRequestBuilder(_context, new KeyExpressionNode<QueryHighLevelRequest>(keyExpressionBuilder, _node));

        public IQueryRequestBuilder FromIndex(string indexName) =>
            new QueryRequestBuilder(_context, new IndexNameNode<QueryHighLevelRequest>(indexName, _node));

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead) =>
            new QueryRequestBuilder(_context, new ConsistentReadNode<QueryHighLevelRequest>(useConsistentRead, _node));

        public IQueryRequestBuilder WithLimit(int limit) => new QueryRequestBuilder(_context, new LimitNode<QueryHighLevelRequest>(limit, _node));

        public IQueryRequestBuilder WithProjectedAttributes(IReadOnlyList<string> projectedAttributes) =>
            new QueryRequestBuilder(_context, new ProjectedAttributesNode<QueryHighLevelRequest>(projectedAttributes, _node));

        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryRequestBuilder(_context, new ReturnConsumedCapacityNode<QueryHighLevelRequest>(consumedCapacityMode, _node));

        public IQueryRequestBuilder WithSelectMode(Select selectMode) =>
            new QueryRequestBuilder(_context, new SelectNode<QueryHighLevelRequest>(selectMode, _node));

        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch) =>
            new QueryRequestBuilder(_context, new BackwardSearchNode<QueryHighLevelRequest>(useBackwardSearch, _node));

        public IQueryRequestBuilder WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryRequestBuilder(_context, new FilterExpressionNode<QueryHighLevelRequest>(filterExpressionBuilder, _node));

        private QueryHighLevelRequest Build(string tableName)
        {
            var request = new QueryHighLevelRequest {TableName = tableName};
            _node?.SetValues(request);

            return request;
        }
    }
}