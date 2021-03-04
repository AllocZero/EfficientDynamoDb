using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel;

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