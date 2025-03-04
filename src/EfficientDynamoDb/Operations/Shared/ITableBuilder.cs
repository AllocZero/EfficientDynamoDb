using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.Shared
{
    public interface ITableBuilder<out TBuilder> where TBuilder : ITableBuilder<TBuilder>
    {
        internal BuilderNode? Node => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITableBuilder<TBuilder>)} must implement the {nameof(Node)} property.");

        internal TBuilder Create(BuilderNode newNode) => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITableBuilder<TBuilder>)} must implement the {nameof(Create)} method.");
    }
}