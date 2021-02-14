using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public interface ITransactGetItemRequestBuilder<TEntity> where TEntity : class
    {
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        ITransactGetItemRequestBuilder<TOtherEntity> GetItem<TOtherEntity>() where TOtherEntity : class;
        
        Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        Task<List<Document?>> ToDocumentListAsync(CancellationToken cancellationToken = default);
        
        Task<TransactGetItemsEntityResponse<TResultEntity>> ToEntityResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class;
        
        Task<TransactGetItemsEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default);
    }
}