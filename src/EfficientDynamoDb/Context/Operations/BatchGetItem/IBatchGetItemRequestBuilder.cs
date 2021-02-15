using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public interface IBatchGetItemRequestBuilder<TTableEntity, TEntity> where TTableEntity : class where TEntity : class
    {
        IBatchGetTableRequestBuilder<TTableEntity> WithPrimaryKey<TPk>(TPk pk);
        
        IBatchGetTableRequestBuilder<TTableEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
    }
}