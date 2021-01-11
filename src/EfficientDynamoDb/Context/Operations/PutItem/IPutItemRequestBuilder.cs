using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public interface IPutItemRequestBuilder
    {
        IPutItemRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class;
        
        IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues);
        
        IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemRequestBuilder WithUpdateCondition(FilterBase condition);
    }
    
    public interface IPutItemRequestBuilder<TEntity> where TEntity: class
    {
        Task<PutItemEntityResponse<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default);
        
        IPutItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IPutItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        IPutItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemRequestBuilder<TEntity> WithUpdateCondition(FilterBase condition);
    }
}