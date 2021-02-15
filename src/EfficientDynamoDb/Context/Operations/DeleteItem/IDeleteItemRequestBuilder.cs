using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.DeleteItem
{
    public interface IDeleteItemRequestBuilder<TEntity> where TEntity : class
    {
        IDeleteItemRequestBuilder<TEntity> WithCondition(FilterBase condition);

        IDeleteItemRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        IDeleteItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IDeleteItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);        
        
        IDeleteItemRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IDeleteItemRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        IDeleteItemRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default);
        
        Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default);
        
        Task<DeleteItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default);
        
        Task<DeleteItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}