using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    internal sealed class BatchGetEntityRequestBuilder : IBatchGetEntityRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public BatchGetEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private BatchGetEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IBatchGetEntityRequestBuilder FromTables(params IBatchGetTableBuilder[] tables) =>
            new BatchGetEntityRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public IBatchGetEntityRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables) =>
            new BatchGetEntityRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public IBatchGetEntityRequestBuilder WithItems(params IBatchGetItemBuilder[] items) =>
            new BatchGetEntityRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));

        public IBatchGetEntityRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items)=>
            new BatchGetEntityRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));
        
        public IBatchGetEntityRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new BatchGetEntityRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IBatchGetDocumentRequestBuilder AsDocuments() => new BatchGetDocumentRequestBuilder(_context, _node);

        public async Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class => 
            await _context.BatchGetItemListAsync<TEntity>(GetNode(), cancellationToken).ConfigureAwait(false);

        public async Task<BatchGetItemResponse<TEntity>> ToResponseAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class =>
            await _context.BatchGetItemResponseAsync<TEntity>(GetNode(), cancellationToken).ConfigureAwait(false);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
    
    internal sealed class BatchGetDocumentRequestBuilder : IBatchGetDocumentRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public BatchGetDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal BatchGetDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public IBatchGetDocumentRequestBuilder FromTables(params IBatchGetTableBuilder[] tables) =>
            new BatchGetDocumentRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public IBatchGetDocumentRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables) =>
            new BatchGetDocumentRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public IBatchGetDocumentRequestBuilder WithItems(params IBatchGetItemBuilder[] items) =>
            new BatchGetDocumentRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));

        public IBatchGetDocumentRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items)=>
            new BatchGetDocumentRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));

        public IBatchGetDocumentRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new BatchGetDocumentRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));
        
        public async Task<List<Document>> ToListAsync(CancellationToken cancellationToken = default) => 
            await _context.BatchGetItemListAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);

        public Task<BatchGetItemResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default) => 
            _context.BatchGetItemResponseAsync<Document>(GetNode(), cancellationToken);

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
}