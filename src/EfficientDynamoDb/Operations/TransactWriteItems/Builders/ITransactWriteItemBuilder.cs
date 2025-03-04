using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemBuilder
    {
        internal BuilderNodeType NodeType => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITransactWriteItemBuilder)} must implement the {nameof(GetNode)} method.");
        
        internal BuilderNode GetNode() => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITransactWriteItemBuilder)} must implement the {nameof(GetNode)} method.");

        internal Type GetEntityType() => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITransactWriteItemBuilder)} must implement the {nameof(GetEntityType)} method.");
    }
}