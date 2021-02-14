using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.DeleteItem
{
    internal sealed class DeleteItemRequestBuilder<TEntity> : IDeleteItemRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public DeleteItemRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private DeleteItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => ToDocumentAsync(cancellationToken);
        
        public async Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<DeleteItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemResponseAsync<TEntity>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<DeleteItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.DeleteItemResponseAsync<Document>(classInfo, GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public IDeleteItemRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IDeleteItemRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup)=>
            new DeleteItemRequestBuilder<TEntity>(_context, new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), _node));

        public IDeleteItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IDeleteItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        public IDeleteItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IDeleteItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IDeleteItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new DeleteItemRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));
        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty delete item request.");
    }
}