using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public readonly struct BatchWriteItemRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public BatchWriteItemRequestBuilder(DynamoDbContext context)
        {
            _context = context;
            _node = null;
        }
        
        private BatchWriteItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public BatchWriteItemRequestBuilder WithItems(params IBatchWriteBuilder[] items) =>
            new BatchWriteItemRequestBuilder(_context, new BatchItemsNode<IBatchWriteBuilder>(items, _node));
        
        public BatchWriteItemRequestBuilder WithItems(IEnumerable<IBatchWriteBuilder> items) =>
            new BatchWriteItemRequestBuilder(_context, new BatchItemsNode<IBatchWriteBuilder>(items, _node));

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => _context.BatchWriteItemAsync(_node ?? throw new DdbException("Can't execute empty batch write item request."), cancellationToken);
    }
}