using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public class UpdateRequestBuilder : IUpdateRequestBuilder
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
        
        public IUpdateRequestBuilder WithReturnValues(ReturnValues returnValues) =>
            new UpdateRequestBuilder(_context, new ReturnValuesNode(returnValues, _node));

        public UpdateRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new UpdateRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IUpdateRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new UpdateRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IUpdateRequestBuilder WithUpdateCondition(FilterBase condition) =>
            new UpdateRequestBuilder(_context, new UpdateConditionNode(condition, _node));
        
        public async Task<UpdateItemEntityResponse<TEntity>> ExecuteAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class =>
            await _context.UpdateItemAsync<TEntity>(_node, cancellationToken).ConfigureAwait(false);
    }
}