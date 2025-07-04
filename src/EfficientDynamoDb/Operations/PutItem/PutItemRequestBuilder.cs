using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.PutItem
{
    internal sealed class PutItemRequestBuilder : IPutItemRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IPutItemRequestBuilder>.Node => _node;

        IPutItemRequestBuilder ITableBuilder<IPutItemRequestBuilder>.Create(BuilderNode newNode)
            => new PutItemRequestBuilder(_context, newNode);

        public PutItemRequestBuilder(DynamoDbContext context) => _context = context;

        private PutItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IPutItemEntityRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ItemNode(item, _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), _node));

        public IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues) =>
            new PutItemRequestBuilder(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemRequestBuilder WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option) =>
            new PutItemRequestBuilder(_context, new ReturnValuesOnConditionCheckFailureNode(option, _node));

        public IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemRequestBuilder WithCondition(FilterBase condition) =>
            new PutItemRequestBuilder(_context, new ConditionNode(condition, _node));
    }

    internal sealed class PutItemEntityRequestBuilder<TEntity> : IPutItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IPutItemEntityRequestBuilder<TEntity>>.Node => _node;

        IPutItemEntityRequestBuilder<TEntity> ITableBuilder<IPutItemEntityRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new PutItemEntityRequestBuilder<TEntity>(_context, newNode);

        public PutItemEntityRequestBuilder(DynamoDbContext context) => _context = context;

        internal PutItemEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task ExecuteAsync(CancellationToken cancellationToken = default) =>
            await _context.PutItemAsync<TEntity>(_node, cancellationToken).EnsureSuccess().ConfigureAwait(false);

        public Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemAsync<TEntity>(_node, cancellationToken).EnsureSuccess();
        
        public Task<PutItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemResponseAsync<TEntity>(_node, cancellationToken).EnsureSuccess();
        
        public IPutItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));
        
        public IPutItemEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ReturnValuesOnConditionCheckFailureNode(option, _node));

        public IPutItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IPutItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new PutItemEntityRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public IPutItemDocumentRequestBuilder<TEntity> AsDocument() => new PutItemDocumentRequestBuilder<TEntity>(_context, _node);
        
        public ISuppressedPutItemEntityRequestBuilder<TEntity> SuppressThrowing() => new SuppressedPutItemEntityRequestBuilder<TEntity>(_context, _node);
    }

    internal sealed class SuppressedPutItemEntityRequestBuilder<TEntity> : ISuppressedPutItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        
        internal SuppressedPutItemEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }
        
        public async Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.PutItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);
            return result.DiscardValue();
        }

        public Task<OpResult<TEntity?>> ToItemAsync(CancellationToken cancellationToken = default) => 
            _context.PutItemAsync<TEntity>(_node, cancellationToken);

        public Task<OpResult<PutItemEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default) => 
            _context.PutItemResponseAsync<TEntity>(_node, cancellationToken);
    }

    internal sealed class PutItemDocumentRequestBuilder<TEntity> : IPutItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IPutItemDocumentRequestBuilder<TEntity>>.Node => _node;

        IPutItemDocumentRequestBuilder<TEntity> ITableBuilder<IPutItemDocumentRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new PutItemDocumentRequestBuilder<TEntity>(_context, newNode);

        public PutItemDocumentRequestBuilder(DynamoDbContext context) => _context = context;

        internal PutItemDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default) =>
            await _context.PutItemAsync<TEntity>(_node, cancellationToken).EnsureSuccess().ConfigureAwait(false);
        
        public Task<Document?> ToItemAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemAsync<Document>(_node, cancellationToken).EnsureSuccess();
        
        public Task<PutItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemResponseAsync<Document>(_node, cancellationToken).EnsureSuccess();

        public IPutItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new PutItemDocumentRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IPutItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new PutItemDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IPutItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new PutItemDocumentRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IPutItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new PutItemDocumentRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IPutItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new PutItemDocumentRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));
        
        public ISuppressedPutItemDocumentRequestBuilder<TEntity> SuppressThrowing() => new SuppressedPutItemDocumentRequestBuilder<TEntity>(_context, _node);
    }

    internal sealed class SuppressedPutItemDocumentRequestBuilder<TEntity> : ISuppressedPutItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        internal SuppressedPutItemDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.PutItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);
            return result.DiscardValue();
        }

        public Task<OpResult<Document?>> ToItemAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemAsync<Document>(_node, cancellationToken);

        public Task<OpResult<PutItemEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.PutItemResponseAsync<Document>(_node, cancellationToken);
    }
}