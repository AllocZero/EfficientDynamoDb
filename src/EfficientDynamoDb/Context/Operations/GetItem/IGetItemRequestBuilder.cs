using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public interface IGetItemEntityRequestBuilder<TEntity> where TEntity : class
    {
        IGetItemEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        IGetItemEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>() where TProjection : class;

        IGetItemEntityRequestBuilder<TEntity, TProjection> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IGetItemEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        IGetItemDocumentRequestBuilder<TEntity> AsDocument();

        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);

        Task<GetItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IGetItemEntityRequestBuilder<TEntity, TProjection> where TEntity : class where TProjection : class
    {
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        IGetItemEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk>(TPk pk);

        IGetItemDocumentRequestBuilder<TEntity> AsDocument();

        Task<TProjection?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<GetItemEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IGetItemDocumentRequestBuilder<TEntity> where TEntity : class
    {
        IGetItemDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);
        
        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        IGetItemDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        Task<GetItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}