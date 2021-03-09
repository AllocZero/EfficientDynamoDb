using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public readonly struct UpdateRequestBuilder<TEntity> : IUpdateItemBuilder<UpdateRequestBuilder<TEntity>> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public UpdateRequestBuilder(DynamoDbContext context)
        {
            _context = context;
            _node = null;
        }
        
        private UpdateRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => ToDocumentAsync(cancellationToken);

        public async Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemAsync<TEntity>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemAsync<Document>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UpdateItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemResponseAsync<TEntity>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UpdateItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemResponseAsync<Document>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public AttributeUpdate<UpdateRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<UpdateRequestBuilder<TEntity>, TEntity, TProperty>(this, expression);

        public UpdateRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public UpdateRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public UpdateRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public UpdateRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new UpdateRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public UpdateRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new UpdateRequestBuilder<TEntity>(_context, new ConditionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public UpdateRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new UpdateRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public UpdateRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new UpdateRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        UpdateRequestBuilder<TEntity> IUpdateItemBuilder<UpdateRequestBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new UpdateRequestBuilder<TEntity>(_context, new UpdateAttributeNode(update, nodeType, _node));
    }
}