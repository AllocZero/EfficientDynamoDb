using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsRequestBuilder : ITransactGetItemsRequestBuilder
    {
        internal readonly DynamoDbContext Context;
        internal readonly BuilderNode? Node;

        public TransactGetItemsRequestBuilder(DynamoDbContext context)
        {
            Context = context;
        }

        internal TransactGetItemsRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            Context = context;
            Node = node;
        }

        public ITransactGetItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactGetItemsRequestBuilder(Context, new ReturnConsumedCapacityNode(returnConsumedCapacity, Node));

        public ITransactGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new TransactGetItemRequestBuilder<TEntity>(this);

        internal BuilderNode GetNode() => Node ?? throw new DdbException("Can't execute empty transact get items request.");
    }
}