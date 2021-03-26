using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemBuilder
    {
        internal BuilderNodeType NodeType { get; }
        
        internal BuilderNode GetNode();

        internal Type GetEntityType();
    }
}