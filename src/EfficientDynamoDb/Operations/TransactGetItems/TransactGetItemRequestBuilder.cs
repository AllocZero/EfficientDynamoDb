using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactGetItems
{
    internal sealed class TransactGetItemRequestBuilder<TEntity> : ITransactGetItemRequestBuilder<TEntity> where TEntity : class
    {
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<ITransactGetItemRequestBuilder<TEntity>>.Node => _node;

        ITransactGetItemRequestBuilder<TEntity> ITableBuilder<ITransactGetItemRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new TransactGetItemRequestBuilder<TEntity>(newNode);

        public TransactGetItemRequestBuilder()
        {
        }

        private TransactGetItemRequestBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TProjection), properties, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new TransactGetItemRequestBuilder<TEntity>(new ProjectedAttributesNode(typeof(TEntity), properties, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new TransactGetItemRequestBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactGetItemRequestBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, _node));

        BuilderNode ITransactGetItemRequestBuilder.GetNode() => _node ?? throw new DdbException("Can't execute empty transact get item request.");

        Type ITransactGetItemRequestBuilder.GetEntityType() => typeof(TEntity);
    }
}