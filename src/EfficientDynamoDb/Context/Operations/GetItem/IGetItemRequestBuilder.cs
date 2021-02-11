using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public interface IGetItemRequestBuilder<TEntity> where TEntity : class
    {
        public IGetItemRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);
        
        public IGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        public IGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        public IGetItemRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        public IGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        public IGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        Task<TEntity?> ToEntityAsync(CancellationToken cancellationToken = default);

        Task<Document?> ToDocumentAsync(CancellationToken cancellationToken = default);
        
        Task<GetItemEntityResponse<TEntity>> ToEntityResponseAsync(CancellationToken cancellationToken = default);
        
        Task<GetItemEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}