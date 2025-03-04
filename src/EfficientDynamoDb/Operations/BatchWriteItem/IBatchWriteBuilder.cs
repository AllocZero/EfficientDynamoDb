using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public interface IBatchWriteBuilder
    {
        internal BuilderNodeType NodeType => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchWriteBuilder)} must implement the {nameof(NodeType)} property.");
        
        internal string? TableName => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchWriteBuilder)} must implement the {nameof(TableName)} property.");

        internal Type GetEntityType() => throw new NotImplementedException(
            $"All internal implementations of {nameof(IBatchWriteBuilder)} must implement the {nameof(GetEntityType)} method.");
    }
}