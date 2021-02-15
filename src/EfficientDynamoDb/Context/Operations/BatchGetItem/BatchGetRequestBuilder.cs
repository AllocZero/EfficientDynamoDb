using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    internal sealed class BatchGetRequestBuilder : IBatchGetRequestBuilder
    {
        internal readonly DynamoDbContext Context;
        internal readonly BuilderNode? Node;

        public BatchGetRequestBuilder(DynamoDbContext context)
        {
            Context = context;
        }

        internal BatchGetRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            Context = context;
            Node = node;
        }

        public IBatchGetTableRequestBuilder<TTableEntity> FromTable<TTableEntity>() where TTableEntity : class => new BatchGetTableRequestBuilder<TTableEntity>(this);

        internal BuilderNode GetNode() => Node ?? throw new DdbException("Can't execute empty batch get item request.");
    }
}