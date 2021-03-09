using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public readonly struct BatchGetRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public BatchGetRequestBuilder(DynamoDbContext context)
        {
            _context = context;
            _node = null;
        }

        private BatchGetRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public BatchGetRequestBuilder FromTables(params IBatchGetTableBuilder[] tables) =>
            new BatchGetRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public BatchGetRequestBuilder FromTables(IEnumerable<IBatchGetTableBuilder> tables) =>
            new BatchGetRequestBuilder(_context, new BatchItemsNode<IBatchGetTableBuilder>(tables, null));

        public BatchGetRequestBuilder WithItems(params IBatchGetItemBuilder[] items) =>
            new BatchGetRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));

        public BatchGetRequestBuilder WithItems(IEnumerable<IBatchGetItemBuilder> items)=>
            new BatchGetRequestBuilder(_context, new BatchItemsNode<IBatchGetItemBuilder>(items, null));

        public async Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            return await _context.BatchGetItemAsync<TEntity>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BatchGetItemAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
}