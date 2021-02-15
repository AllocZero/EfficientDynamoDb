using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemRequestBuilder : IBatchWriteItemRequestBuilder
    {
        internal readonly DynamoDbContext Context;
        internal readonly BuilderNode? Node;

        public BatchWriteItemRequestBuilder(DynamoDbContext context)
        {
            Context = context;
        }
        
        internal BatchWriteItemRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            Context = context;
            Node = node;
        }
        
        public IBatchWriteItemRequestBuilder PutItem<TEntity>(TEntity entity) where TEntity : class =>
            new BatchWriteItemRequestBuilder(Context, new ItemNode(entity, Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), Node));

        public IBatchDeleteItemRequestBuilder DeleteItem<TEntity>() where TEntity : class =>
            new BatchDeleteItemRequestBuilder<TEntity>(this);

        public Task ExecuteAsync() => Context.BatchWriteItemAsync(Node ?? throw new DdbException("Can't execute empty batch write item request."));
    }
}