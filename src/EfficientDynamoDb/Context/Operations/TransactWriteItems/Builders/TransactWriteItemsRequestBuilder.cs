using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactWriteItemsRequestBuilder : ITransactWriteItemsRequestBuilder
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        public TransactWriteItemsRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }

        private TransactWriteItemsRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public ITransactWriteItemsRequestBuilder WithClientRequestToken(string token) =>
            new TransactWriteItemsRequestBuilder(_context, new ClientRequestTokenNode(token, _node));

        public ITransactWriteItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactWriteItemsRequestBuilder(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public ITransactWriteItemsRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new TransactWriteItemsRequestBuilder(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public ITransactWriteItemsRequestBuilder WithItems(params ITransactWriteItemBuilder[] items) =>
            new TransactWriteItemsRequestBuilder(_context, new BatchItemsNode<ITransactWriteItemBuilder>(items, _node));

        public ITransactWriteItemsRequestBuilder WithItems(IEnumerable<ITransactWriteItemBuilder> items) =>
            new TransactWriteItemsRequestBuilder(_context, new BatchItemsNode<ITransactWriteItemBuilder>(items, _node));
        
        public Task ExecuteAsync(CancellationToken cancellationToken = default) => _context.TransactWriteItemsAsync(GetNode(), cancellationToken);

        public Task<TransactWriteItemsEntityResponse> ToResponseAsync(CancellationToken cancellationToken = default) =>
            _context.TransactWriteItemsResponseAsync(GetNode(), cancellationToken);

        
        private BuilderNode GetNode() => _node ?? throw new DdbException("Can't execute empty transact write items request.");
    }
}