using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsEntityRequestBuilder : ITransactGetItemsEntityRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public TransactGetItemsEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private TransactGetItemsEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public ITransactGetItemsEntityRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactGetItemsEntityRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public ITransactGetItemsEntityRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items) =>
            new TransactGetItemsEntityRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));
        
        public ITransactGetItemsEntityRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items) =>
            new TransactGetItemsEntityRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));

        public ITransactGetItemsDocumentRequestBuilder AsDocuments() => new TransactGetItemsDocumentRequestBuilder(_context, _node);

        public async Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _context.TransactGetItemsAsync<TResultEntity>(GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<TransactGetItemsEntityResponse<TResultEntity>> ToResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _context.TransactGetItemsResponseAsync<TResultEntity>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty transact get items request.");
    }
    
     internal sealed class TransactGetItemsDocumentRequestBuilder : ITransactGetItemsDocumentRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public TransactGetItemsDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        internal TransactGetItemsDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public ITransactGetItemsDocumentRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactGetItemsDocumentRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public ITransactGetItemsDocumentRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items) =>
            new TransactGetItemsDocumentRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));
        
        public ITransactGetItemsDocumentRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items) =>
            new TransactGetItemsDocumentRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));
        
        
        public async Task<List<Document?>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TransactGetItemsAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<TransactGetItemsEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default) 
        {
            return await _context.TransactGetItemsResponseAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty transact get items request.");
    }
}