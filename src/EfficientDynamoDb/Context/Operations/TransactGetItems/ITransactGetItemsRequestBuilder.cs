using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public interface ITransactGetItemsRequestBuilder
    {
        ITransactGetItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);

        ITransactGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class;
    }
}