using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactGetItems
{
    public interface ITransactGetItemRequestBuilder
    {
        internal BuilderNode GetNode();

        internal Type GetEntityType();
    }
    
    public interface ITransactGetItemRequestBuilder<TEntity> : ITransactGetItemRequestBuilder, ITableBuilder<ITransactGetItemRequestBuilder<TEntity>> where TEntity : class
    {
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
    }
}