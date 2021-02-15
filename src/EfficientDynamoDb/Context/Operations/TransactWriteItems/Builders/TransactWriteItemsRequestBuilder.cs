using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactWriteItemsRequestBuilder : ITransactWriteItemsRequestBuilder
    {
        internal readonly DynamoDbContext Context;
        internal readonly BuilderNode? Node;

        public TransactWriteItemsRequestBuilder(DynamoDbContext context)
        {
            Context = context;
        }

        internal TransactWriteItemsRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            Context = context;
            Node = node;
        }

        public ITransactWriteItemsRequestBuilder WithClientRequestToken(string token) =>
            new TransactWriteItemsRequestBuilder(Context, new ClientRequestTokenNode(token, Node));

        public ITransactWriteItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new TransactWriteItemsRequestBuilder(Context, new ReturnConsumedCapacityNode(returnConsumedCapacity, Node));

        public ITransactWriteItemsRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new TransactWriteItemsRequestBuilder(Context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, Node));

        public ITransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class => new TransactConditionCheckBuilder<TEntity>(this);

        public ITransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new TransactDeleteItemBuilder<TEntity>(this);

        public ITransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class
        {
            var classInfo = Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return new TransactPutItemBuilder<TEntity>(this, new ItemNode(entity, classInfo, null));
        }

        public ITransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class => new TransactUpdateItemBuilder<TEntity>(this);
        
        internal BuilderNode GetNode() => Node ?? throw new DdbException("Can't execute empty transact write items request.");
    }
}