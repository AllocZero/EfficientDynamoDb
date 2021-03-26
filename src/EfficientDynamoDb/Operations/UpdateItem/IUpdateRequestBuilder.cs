using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.UpdateItem
{
    public interface IUpdateEntityRequestBuilder<TEntity> : IUpdateItemBuilder<IUpdateEntityRequestBuilder<TEntity>> where TEntity : class
    {
        IUpdateEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        IUpdateEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        // IUpdateEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IUpdateEntityRequestBuilder<TEntity> WithCondition(FilterBase condition);

        IUpdateEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        IAttributeUpdate<IUpdateEntityRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        IUpdateDocumentRequestBuilder<TEntity> AsDocument();
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<UpdateItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IUpdateDocumentRequestBuilder<TEntity> : IUpdateItemBuilder<IUpdateDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        IUpdateDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        IUpdateDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        // IUpdateDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        
        IUpdateDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition);

        IUpdateDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        IAttributeUpdate<IUpdateDocumentRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
        
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<UpdateItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}