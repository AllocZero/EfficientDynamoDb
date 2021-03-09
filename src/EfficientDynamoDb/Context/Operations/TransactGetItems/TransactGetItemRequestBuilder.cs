using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    public readonly struct TransactGetItemRequestBuilder<TEntity> : ITransactGetItemRequestBuilder where TEntity : class
    {
        private readonly BuilderNode? _node;
        
        private TransactGetItemRequestBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public TransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public TransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public TransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public TransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new TransactGetItemRequestBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public TransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactGetItemRequestBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, _node));

        BuilderNode ITransactGetItemRequestBuilder.GetNode() => _node ?? throw new DdbException("Can't execute empty transact get item request.");

        Type ITransactGetItemRequestBuilder.GetEntityType() => typeof(TEntity);
    }
}