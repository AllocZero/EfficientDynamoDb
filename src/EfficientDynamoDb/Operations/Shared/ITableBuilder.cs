using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.Shared
{
    public interface ITableBuilder<out TBuilder> where TBuilder : ITableBuilder<TBuilder>
    {
        internal BuilderNode? Node { get; }

        internal TBuilder Create(BuilderNode newNode);
    }
}