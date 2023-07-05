using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.DeleteItem
{
    internal sealed class DeleteItemEntityRequestBuilder<TEntity> : IDeleteItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IDeleteItemEntityRequestBuilder<TEntity>>.Node => _node;

        IDeleteItemEntityRequestBuilder<TEntity> ITableBuilder<IDeleteItemEntityRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new DeleteItemEntityRequestBuilder<TEntity>(_context, newNode);

        public DeleteItemEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private DeleteItemEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            await _context.DeleteItemAsync(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<DeleteItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemResponseAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public IDeleteItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup)=>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure option) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ReturnValuesOnConditionCheckFailureNode(option, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IDeleteItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new DeleteItemEntityRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> AsDocument() => new DeleteItemDocumentRequestBuilder<TEntity>(_context, _node);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty delete item request.");
    }
    
    internal sealed class DeleteItemDocumentRequestBuilder<TEntity> : IDeleteItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IDeleteItemDocumentRequestBuilder<TEntity>>.Node => _node;

        IDeleteItemDocumentRequestBuilder<TEntity> ITableBuilder<IDeleteItemDocumentRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new DeleteItemDocumentRequestBuilder<TEntity>(_context, newNode);

        public DeleteItemDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal DeleteItemDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            await _context.DeleteItemAsync(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<Document?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<DeleteItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemResponseAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup)=>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IDeleteItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new DeleteItemDocumentRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));
        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty delete item request.");
    }
}