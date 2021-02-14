using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel;
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
        IPutItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IPutItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        IPutItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemRequestBuilder<TEntity> WithUpdateCondition(FilterBase condition);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default);
        
        Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default);
        
        Task<PutItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default);
        
        Task<PutItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}