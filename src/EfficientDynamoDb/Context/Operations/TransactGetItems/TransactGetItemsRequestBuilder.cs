using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsRequestBuilder : ITransactGetItemsRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public TransactGetItemsRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private TransactGetItemsRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public ITransactGetItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactGetItemsRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public ITransactGetItemsRequestBuilder WithItems(params ITransactGetItemRequestBuilder[] items) =>
            new TransactGetItemsRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));
        
        public ITransactGetItemsRequestBuilder WithItems(IEnumerable<ITransactGetItemRequestBuilder> items) =>
            new TransactGetItemsRequestBuilder(_context, new BatchItemsNode<ITransactGetItemRequestBuilder>(items, _node));
        
        public async Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _context.TransactGetItemsAsync<TResultEntity>(GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<List<Document?>> ToDocumentListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TransactGetItemsAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<TransactGetItemsEntityResponse<TResultEntity>> ToEntityResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _context.TransactGetItemsResponseAsync<TResultEntity>(GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<TransactGetItemsEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default) 
        {
            return await _context.TransactGetItemsResponseAsync<Document>(GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty transact get items request.");
    }
}