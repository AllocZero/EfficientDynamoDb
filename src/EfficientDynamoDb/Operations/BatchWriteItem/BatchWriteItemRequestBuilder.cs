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

        public IBatchWriteItemRequestBuilder WithReturnItemCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new BatchWriteItemRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => 
            _context.BatchWriteItemAsync(GetNode(), cancellationToken).EnsureSuccess();

        public Task<BatchWriteItemResponse> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.BatchWriteItemResponseAsync(GetNode(), cancellationToken).EnsureSuccess();
        
        public ISuppressedBatchWriteItemRequestBuilder SuppressThrowing() => new SuppressedBatchWriteItemRequestBuilder(_context, _node);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch write item request.");
    }
    
    internal sealed class SuppressedBatchWriteItemRequestBuilder : ISuppressedBatchWriteItemRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;
        
        public SuppressedBatchWriteItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public Task<OpResult> ExecuteAsync(CancellationToken cancellationToken = default) => 
            _context.BatchWriteItemAsync(GetNode(), cancellationToken);

        public Task<OpResult<BatchWriteItemResponse>> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.BatchWriteItemResponseAsync(GetNode(), cancellationToken);
        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch write item request.");
    }
}