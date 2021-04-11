using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal class BatchPutItemBuilder : IBatchPutItemBuilder
    {
        private readonly Type _entityType;
        internal readonly object Entity;

        string? IBatchWriteBuilder.TableName => GetTableName();
        
        BuilderNodeType IBatchWriteBuilder.NodeType => BuilderNodeType.Item;

        public BatchPutItemBuilder(Type entityType, object entity)
        {
            _entityType = entityType;
            Entity = entity;
        }
        
        Type IBatchWriteBuilder.GetEntityType() => _entityType;

        IBatchPutItemBuilder IBatchPutItemBuilder.WithTableName(string tableName) => new BatchPutItemWithTableNameBuilder(_entityType, Entity, tableName);
        
        protected virtual string? GetTableName() => null;
    }

    internal sealed class BatchPutItemWithTableNameBuilder : BatchPutItemBuilder
    {
        private readonly string _tableName;

        public BatchPutItemWithTableNameBuilder(Type entityType, object entity, string tableName) : base(entityType, entity)
        {
            _tableName = tableName;
        }

        protected override string? GetTableName() => _tableName;
    }
}