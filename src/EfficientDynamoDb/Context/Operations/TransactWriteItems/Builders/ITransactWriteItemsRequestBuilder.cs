using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactWriteItemsRequestBuilder
    {
        ITransactWriteItemsRequestBuilder WithClientRequestToken(string token);
        
        ITransactWriteItemsRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        ITransactWriteItemsRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        ITransactConditionCheckBuilder<TEntity> ConditionCheck<TEntity>() where TEntity : class;
        
        ITransactDeleteItemBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class;
        
        ITransactPutItemBuilder<TEntity> PutItem<TEntity>(TEntity entity) where TEntity : class;
        
        ITransactUpdateItemBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class;
    }
}