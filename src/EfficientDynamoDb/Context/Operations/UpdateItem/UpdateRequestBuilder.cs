using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    internal sealed class UpdateRequestBuilder<TEntity> : IUpdateRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public UpdateRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }
        
        private UpdateRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IAttributeUpdate<TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) => new AttributeUpdate<TEntity, TProperty>(this, expression);

        public IUpdateRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IUpdateRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IUpdateRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new UpdateRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IUpdateRequestBuilder<TEntity> WithUpdateCondition(FilterBase condition) =>
            new UpdateRequestBuilder<TEntity>(_context, new UpdateConditionNode(condition, _node));
        
        public async Task<UpdateItemEntityResponse<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default) =>
            await _context.UpdateItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UpdateRequestBuilder<TEntity> Create(UpdateBase update) => new UpdateRequestBuilder<TEntity>(_context, new UpdateAttributeNode(update, _node));
    }
}