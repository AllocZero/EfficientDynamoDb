using System;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public readonly struct BatchPutItemBuilder : IBatchWriteBuilder
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