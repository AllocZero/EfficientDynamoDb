using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal abstract class TransactWriteItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        protected readonly TransactWriteItemsRequestBuilder RequestBuilder;
        protected readonly BuilderNode? Node;
        
        protected abstract BuilderNodeType NodeType { get; }

        protected TransactWriteItemBuilder(TransactWriteItemsRequestBuilder requestBuilder)
        {
            RequestBuilder = requestBuilder;
        }

        protected TransactWriteItemBuilder(TransactWriteItemsRequestBuilder requestBuilder, BuilderNode? node)
        {
            RequestBuilder = requestBuilder;
            Node = node;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => RequestBuilder.Context.TransactWriteItemsAsync(Unwrap().GetNode(), cancellationToken);

        public Task<TransactWriteItemsEntityResponse> ToResponseAsync(CancellationToken cancellationToken = default) =>
            RequestBuilder.Context.TransactWriteItemsResponseAsync(Unwrap().GetNode(), cancellationToken);

        public ITransactConditionCheckBuilder<TOtherEntity> ConditionCheck<TOtherEntity>() where TOtherEntity : class =>
            new TransactConditionCheckBuilder<TOtherEntity>(new TransactWriteItemsRequestBuilder(RequestBuilder.Context, Unwrap().Node));

        public ITransactDeleteItemBuilder<TOtherEntity> DeleteItem<TOtherEntity>() where TOtherEntity : class =>
            new TransactDeleteItemBuilder<TOtherEntity>(new TransactWriteItemsRequestBuilder(RequestBuilder.Context, Unwrap().Node));

        public ITransactPutItemBuilder<TOtherEntity> PutItem<TOtherEntity>(TOtherEntity entity) where TOtherEntity : class
        {
            var classInfo = RequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TOtherEntity));
            return new TransactPutItemBuilder<TOtherEntity>(new TransactWriteItemsRequestBuilder(RequestBuilder.Context, Unwrap().Node), new ItemNode(entity, classInfo, null));
        }

        public ITransactUpdateItemBuilder<TOtherEntity> UpdateItem<TOtherEntity>() where TOtherEntity : class =>
            new TransactUpdateItemBuilder<TOtherEntity>(new TransactWriteItemsRequestBuilder(RequestBuilder.Context, Unwrap().Node));

        private TransactWriteItemsRequestBuilder Unwrap()
        {
            if (Node == null)
                return RequestBuilder;

            var classInfo = RequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return new TransactWriteItemsRequestBuilder(RequestBuilder.Context, new TransactWriteItemNode(NodeType, classInfo, Node, RequestBuilder.Node));
        }
    }
}