using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    internal abstract class TransactWriteItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        protected readonly BuilderNode? Node;

        protected abstract BuilderNodeType NodeType { get; }

        BuilderNode ITransactWriteItemBuilder.GetNode() => Node ?? throw new DdbException("Transact write can't contain an empty operation.");

        Type ITransactWriteItemBuilder.GetEntityType() => typeof(TEntity);

        BuilderNodeType ITransactWriteItemBuilder.NodeType => NodeType;

        protected TransactWriteItemBuilder()
        {
        }

        protected TransactWriteItemBuilder(BuilderNode? node)
        {
            Node = node;
        }
    }
}