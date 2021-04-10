using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public interface IBatchWriteBuilder
    {
        internal BuilderNodeType NodeType { get; }
        
        internal string? TableName { get; set; }

        internal Type GetEntityType();
    }
}