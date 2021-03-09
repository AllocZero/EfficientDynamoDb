using System;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemBuilder
    {
        internal BuilderNodeType NodeType { get; }
        
        internal BuilderNode GetNode();

        internal Type GetEntityType();
    }
}