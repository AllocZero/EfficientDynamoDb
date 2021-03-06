using System;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetItemBuilder
    {
        internal PrimaryKeyNodeBase GetPrimaryKeyNode();

        internal Type GetEntityType();
        
        IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk);
        
        IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
    }
}