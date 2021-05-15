using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.Shared
{
    public interface ITableBuilder<out TBuilder> where TBuilder : ITableBuilder<TBuilder>
    {
        internal BuilderNode? Node => throw new NotImplementedException();

        internal TBuilder Create(BuilderNode newNode) => throw new NotImplementedException();
    }
}