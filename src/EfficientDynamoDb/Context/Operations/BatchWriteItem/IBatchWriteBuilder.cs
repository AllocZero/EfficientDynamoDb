using System;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public interface IBatchWriteBuilder
    {
        internal BuilderNodeType NodeType { get; }

        internal Type GetEntityType();
    }
}