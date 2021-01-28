using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    internal sealed class PutItemRequestBuilder : IPutItemRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public PutItemRequestBuilder(DynamoDbContext context) => _context = context;

        private PutItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IPutItemRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class =>
            new PutItemRequestBuilder<TEntity>(_context, new ItemNode(item, _node));

        public IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues) =>
            new PutItemRequestBuilder(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemRequestBuilder WithUpdateCondition(FilterBase condition) =>
            new PutItemRequestBuilder(_context, new UpdateConditionNode(condition, _node));
    }

    internal sealed class PutItemRequestBuilder<TEntity> : IPutItemRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public PutItemRequestBuilder(DynamoDbContext context) => _context = context;

        internal PutItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<PutItemEntityResponse<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default) =>
            await _context.PutItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);

        public IPutItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemRequestBuilder<TEntity> WithUpdateCondition(FilterBase condition) =>
            new PutItemRequestBuilder<TEntity>(_context, new UpdateConditionNode(condition, _node));
    }
}