using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemRequestBuilder : IBatchWriteItemRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public BatchWriteItemRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }
        
        private BatchWriteItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IBatchWriteItemRequestBuilder WithItems(params IBatchWriteBuilder[] items) =>
            new BatchWriteItemRequestBuilder(_context, new BatchItemsNode<IBatchWriteBuilder>(items, _node));
        
        public IBatchWriteItemRequestBuilder WithItems(IEnumerable<IBatchWriteBuilder> items) =>
            new BatchWriteItemRequestBuilder(_context, new BatchItemsNode<IBatchWriteBuilder>(items, _node));

        public IBatchWriteItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new BatchWriteItemRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => 
            _context.BatchWriteItemAsync(_node ?? throw new DdbException("Can't execute empty batch write item request."), cancellationToken);

        public Task<BatchWriteItemResponse> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.BatchWriteItemResponseAsync(_node ?? throw new DdbException("Can't execute empty batch write item request."), cancellationToken);
    }
}