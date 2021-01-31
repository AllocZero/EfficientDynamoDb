using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public interface IUpdateRequestBuilder<TEntity> where TEntity : class
    {
        IUpdateRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues);
        IUpdateRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        IUpdateRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        IUpdateRequestBuilder<TEntity> WithUpdateCondition(FilterBase condition);

        Task<UpdateItemEntityResponse<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default);

        IAttributeUpdate<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
    }
}