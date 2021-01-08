using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryRequestBuilder : IQueryRequestBuilder, IBasicQueryRequestBuilder
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
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryListAsync<TEntity>(Build(tableName!), cancellationToken).ConfigureAwait(false);
        }

        public async Task<QueryEntityResponse<TEntity>> ToResponseAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var tableName = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)).TableName;
            return await _context.QueryAsync<TEntity>(Build(tableName!), cancellationToken).ConfigureAwait(false);
        }

        public IQueryRequestBuilder WithKeyExpression(FilterBase keyExpressionBuilder) =>
            new QueryRequestBuilder(_context, new KeyExpressionNode(keyExpressionBuilder, _node));

        public IQueryRequestBuilder FromIndex(string indexName) => new QueryRequestBuilder(_context, new IndexNameNode(indexName, _node));

        public IQueryRequestBuilder WithConsistentRead(bool useConsistentRead) =>
            new QueryRequestBuilder(_context, new ConsistentReadNode(useConsistentRead, _node));

        public IQueryRequestBuilder WithLimit(int limit) => new QueryRequestBuilder(_context, new LimitNode(limit, _node));

        public IQueryRequestBuilder WithProjectedAttributes(IReadOnlyList<string> projectedAttributes) =>
            new QueryRequestBuilder(_context, new ProjectedAttributesNode(projectedAttributes, _node));

        public IQueryRequestBuilder ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode) =>
            new QueryRequestBuilder(_context, new ReturnConsumedCapacityNode(consumedCapacityMode, _node));

        public IQueryRequestBuilder WithSelectMode(Select selectMode) => new QueryRequestBuilder(_context, new SelectNode(selectMode, _node));

        public IQueryRequestBuilder BackwardSearch(bool useBackwardSearch) =>
            new QueryRequestBuilder(_context, new BackwardSearchNode(useBackwardSearch, _node));

        public IQueryRequestBuilder WithFilterExpression(FilterBase filterExpressionBuilder) =>
            new QueryRequestBuilder(_context, new FilterExpressionNode(filterExpressionBuilder, _node));
        
        private QueryHighLevelRequest Build(string tableName)
        {
            var request = new QueryHighLevelRequest {TableName = tableName};
            _node?.SetValues(request);

            return request;
        }
    }
    
}