using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    internal sealed class BatchGetTableBuilder<TTableEntity> : IBatchGetTableBuilder<TTableEntity> where TTableEntity : class
    {
        private readonly BuilderNode? _node;

        public BatchGetTableBuilder()
        {
        }
        
        private BatchGetTableBuilder(BuilderNode node)
        {
            _node = node;
        }

        BuilderNode IBatchGetTableBuilder.GetNode() => _node ?? throw new DdbException("Can't execute empty batch get item request.");

        Type IBatchGetTableBuilder.GetTableType() => typeof(TTableEntity);

        public IBatchGetTableBuilder<TTableEntity> WithConsistentRead(bool useConsistentRead) =>
            new BatchGetTableBuilder<TTableEntity>(new ConsistentReadNode(useConsistentRead, _node));

        public IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>() where TProjection : class =>
            new BatchGetTableBuilder<TTableEntity>(new ProjectedAttributesNode(typeof(TProjection), null, _node));

        public IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class =>
            new BatchGetTableBuilder<TTableEntity>(new ProjectedAttributesNode(typeof(TProjection), properties, _node));
        
        public IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes(params Expression<Func<TTableEntity, object>>[] properties) =>
            new BatchGetTableBuilder<TTableEntity>(new ProjectedAttributesNode(typeof(TTableEntity), properties, _node));

        public IBatchGetTableBuilder<TTableEntity> WithItems(params IBatchGetItemBuilder[] items) =>
            new BatchGetTableBuilder<TTableEntity>(new BatchItemsNode<IBatchGetItemBuilder>(items, _node));

        public IBatchGetTableBuilder<TTableEntity> WithItems(IEnumerable<IBatchGetItemBuilder> items) =>
            new BatchGetTableBuilder<TTableEntity>(new BatchItemsNode<IBatchGetItemBuilder>(items, _node));
    }
}