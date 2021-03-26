using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal sealed class BatchPutItemBuilder : IBatchWriteBuilder
    {
        private readonly Type _entityType;
        internal readonly object Entity;

        public BatchPutItemBuilder(Type entityType, object entity)
        {
            _entityType = entityType;
            Entity = entity;
        }

        BuilderNodeType IBatchWriteBuilder.NodeType => BuilderNodeType.Item;

        Type IBatchWriteBuilder.GetEntityType() => _entityType;
    }
}