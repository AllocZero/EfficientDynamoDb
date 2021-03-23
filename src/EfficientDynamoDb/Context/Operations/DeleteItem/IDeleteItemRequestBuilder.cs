using System;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.DeleteItem
{
    public interface IDeleteItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        IDeleteItemEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);

        IDeleteItemEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IDeleteItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);        
        
        IDeleteItemEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IDeleteItemEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IDeleteItemEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);

        IDeleteItemDocumentRequestBuilder<TEntity> AsDocument();
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<DeleteItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IDeleteItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);

        IDeleteItemDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IDeleteItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);        
        
        IDeleteItemDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        
        IDeleteItemDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        
        // IDeleteItemDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<DeleteItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}