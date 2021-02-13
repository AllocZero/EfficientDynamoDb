using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetTableRequestBuilder<TTableEntity> where TTableEntity : class
    {
        IBatchGetTableRequestBuilder<TEntity> FromTable<TEntity>() where TEntity : class;
        
        IBatchGetItemRequestBuilder<TTableEntity, TEntity> GetItem<TEntity>() where TEntity : class;
        
        IBatchGetItemRequestBuilder<TTableEntity, TTableEntity> GetItem();
        
        IBatchGetTableRequestBuilder<TTableEntity> GetItem<TPk>(TPk pk);
        
        IBatchGetTableRequestBuilder<TTableEntity> GetItem<TPk, TSk>(TPk pk, TSk sk);
        
        IBatchGetTableRequestBuilder<TTableEntity> WithConsistentRead(bool useConsistentRead);
        
        IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes(params Expression<Func<TTableEntity, object>>[] properties);
        
        Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;

        Task<List<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default);
    }
}