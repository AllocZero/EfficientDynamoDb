using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.PutItem
{
    public interface IPutItemRequestBuilder
    {
        IPutItemEntityRequestBuilder<TEntity> WithItem<TEntity>(TEntity item) where TEntity : class;
        
        IPutItemRequestBuilder WithReturnValues(ReturnValues returnValues);
        
        IPutItemRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IPutItemRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemRequestBuilder WithCondition(FilterBase condition);
    }
    
    public interface IPutItemEntityRequestBuilder<TEntity> where TEntity: class
    {
        IPutItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IPutItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IPutItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);
        
        IPutItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        IPutItemDocumentRequestBuilder<TEntity> AsDocument();
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<PutItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IPutItemDocumentRequestBuilder<TEntity> where TEntity: class
    {
        IPutItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IPutItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IPutItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IPutItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);
        
        IPutItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);

        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);

        Task<PutItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}