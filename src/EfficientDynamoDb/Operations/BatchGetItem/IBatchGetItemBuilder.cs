using System;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    public interface IBatchGetItemBuilder
    {
        internal PrimaryKeyNodeBase GetPrimaryKeyNode();

        internal Type GetEntityType();
        
        internal string? TableName { get; }

        internal IBatchGetItemBuilder WithTableName(string tableName);
        
        IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk);
        
        IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
    }
}