using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.TransactGetItems
{
    internal sealed class TransactGetItemRequestBuilder<TEntity> : ITransactGetItemRequestBuilder<TEntity> where TEntity : class
    {
        private readonly TransactGetItemsRequestBuilder _transactGetItemsRequestBuilder;
        private readonly BuilderNode? _node;

        public TransactGetItemRequestBuilder(TransactGetItemsRequestBuilder transactGetItemsRequestBuilder)
        {
            _transactGetItemsRequestBuilder = transactGetItemsRequestBuilder;
        }

        private TransactGetItemRequestBuilder(TransactGetItemsRequestBuilder transactGetItemsRequestBuilder, BuilderNode? node)
        {
            _transactGetItemsRequestBuilder = transactGetItemsRequestBuilder;
            _node = node;
        }

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(_transactGetItemsRequestBuilder,
                new ProjectedAttributesNode(_transactGetItemsRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TProjection)), null, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new TransactGetItemRequestBuilder<TEntity>(_transactGetItemsRequestBuilder,
                new ProjectedAttributesNode(_transactGetItemsRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TProjection)), properties, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties) =>
            new TransactGetItemRequestBuilder<TEntity>(_transactGetItemsRequestBuilder,
                new ProjectedAttributesNode(_transactGetItemsRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), properties, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new TransactGetItemRequestBuilder<TEntity>(_transactGetItemsRequestBuilder, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactGetItemRequestBuilder<TEntity>(_transactGetItemsRequestBuilder, new PartitionKeyNode<TPk>(pk, _node));

        public ITransactGetItemRequestBuilder<TOtherEntity> GetItem<TOtherEntity>() where TOtherEntity : class => Unwrap().GetItem<TOtherEntity>();

        public async Task<List<TResultEntity?>> ToListAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _transactGetItemsRequestBuilder.Context.TransactGetItemsAsync<TResultEntity>(Unwrap().GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<List<Document?>> ToDocumentListAsync(CancellationToken cancellationToken = default)
        {
            return await _transactGetItemsRequestBuilder.Context.TransactGetItemsAsync<Document>(Unwrap().GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<TransactGetItemsEntityResponse<TResultEntity>> ToEntityResponseAsync<TResultEntity>(CancellationToken cancellationToken = default) where TResultEntity : class
        {
            return await _transactGetItemsRequestBuilder.Context.TransactGetItemsResponseAsync<TResultEntity>(Unwrap().GetNode(), cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<TransactGetItemsEntityResponse<Document>> ToDocumentResponseAsync(CancellationToken cancellationToken = default) 
        {
            return await _transactGetItemsRequestBuilder.Context.TransactGetItemsResponseAsync<Document>(Unwrap().GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private TransactGetItemsRequestBuilder Unwrap()
        {
            if (_node == null)
                return _transactGetItemsRequestBuilder;
            
            var classInfo = _transactGetItemsRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var itemNode = new GetItemNode(classInfo, _node, _transactGetItemsRequestBuilder.Node);
            return new TransactGetItemsRequestBuilder(_transactGetItemsRequestBuilder.Context, itemNode);
        }
    }
}