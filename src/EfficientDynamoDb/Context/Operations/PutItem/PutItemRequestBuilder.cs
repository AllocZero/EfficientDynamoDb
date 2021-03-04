using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel;
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
            new PutItemRequestBuilder<TEntity>(_context, new ItemNode(item, _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), _node));

        public IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues) =>
            new PutItemRequestBuilder(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemRequestBuilder WithCondition(FilterBase condition) =>
            new PutItemRequestBuilder(_context, new ConditionNode(condition, _node));
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
        
        public async Task ExecuteAsync(CancellationToken cancellationToken = default) =>
            await _context.PutItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);

        public Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemAsync<TEntity>(_node, cancellationToken);
        
        public Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemAsync<Document>(_node, cancellationToken);
        
        public Task<PutItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemResponseAsync<TEntity>(_node, cancellationToken);
        
        public Task<PutItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemResponseAsync<Document>(_node, cancellationToken);

        public IPutItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new PutItemRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IPutItemRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new PutItemRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));
    }
}