using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    internal sealed class BatchGetTableRequestBuilder<TTableEntity> : IBatchGetTableRequestBuilder<TTableEntity> where TTableEntity : class
    {
        internal readonly BatchGetRequestBuilder BatchGetRequestBuilder;
        internal readonly BuilderNode? Node;

        public BatchGetTableRequestBuilder(BatchGetRequestBuilder batchGetRequestBuilder)
        {
            BatchGetRequestBuilder = batchGetRequestBuilder;
        }
        
        internal BatchGetTableRequestBuilder(BatchGetRequestBuilder batchGetRequestBuilder, BuilderNode node)
        {
            BatchGetRequestBuilder = batchGetRequestBuilder;
            Node = node;
        }

        public IBatchGetTableRequestBuilder<TEntity> FromTable<TEntity>() where TEntity : class => Unwrap().FromTable<TEntity>();

        public IBatchGetItemRequestBuilder<TTableEntity, TEntity> GetItem<TEntity>() where TEntity : class => new BatchGetItemRequestBuilder<TTableEntity, TEntity>(this);

        public IBatchGetItemRequestBuilder<TTableEntity, TTableEntity> GetItem() => new BatchGetItemRequestBuilder<TTableEntity, TTableEntity>(this);

        public IBatchGetTableRequestBuilder<TTableEntity> GetItem<TPk>(TPk pk) =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder,
                new EntityPartitionKeyNode<TPk>(BatchGetRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TTableEntity)), pk, Node));

        public IBatchGetTableRequestBuilder<TTableEntity> GetItem<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder,
                new EntityPartitionAndSortKeyNode<TPk, TSk>(BatchGetRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TTableEntity)), pk, sk, Node));

        public IBatchGetTableRequestBuilder<TTableEntity> WithConsistentRead(bool useConsistentRead) =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder, new ConsistentReadNode(useConsistentRead, Node));

        public IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder,
                new ProjectedAttributesNode(typeof(TProjection), null, Node));

        public IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder,
                new ProjectedAttributesNode(typeof(TProjection), properties, Node));
        
        public IBatchGetTableRequestBuilder<TTableEntity> WithProjectedAttributes(params Expression<Func<TTableEntity, object>>[] properties) =>
            new BatchGetTableRequestBuilder<TTableEntity>(BatchGetRequestBuilder,
                new ProjectedAttributesNode(typeof(TTableEntity), properties, Node));

        public async Task<List<TEntity>> ToListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            var requestBuilder = Unwrap();

            return await requestBuilder.Context.BatchGetItemAsync<TEntity>(requestBuilder.GetNode(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Document>> ToDocumentListAsync(CancellationToken cancellationToken = default)
        {
            var requestBuilder = Unwrap();

            return await requestBuilder.Context.BatchGetItemAsync<Document>(requestBuilder.GetNode(), cancellationToken).ConfigureAwait(false);
        }

        private BatchGetRequestBuilder Unwrap()
        {
            if (Node == null)
                return BatchGetRequestBuilder;
            
            var tableClassInfo = BatchGetRequestBuilder.Context.Config.Metadata.GetOrAddClassInfo(typeof(TTableEntity));
            var tableNode = new BatchGetTableNode(tableClassInfo, Node, BatchGetRequestBuilder.Node);
            return new BatchGetRequestBuilder(BatchGetRequestBuilder.Context, tableNode);
        }
    }
}