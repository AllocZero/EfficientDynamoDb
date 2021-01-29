using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.UpdateItem
{
    public interface IUpdateRequestBuilder
    {
        IUpdateRequestBuilder WithReturnValues(ReturnValues returnValues);
        UpdateRequestBuilder WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity);
        IUpdateRequestBuilder WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics);
        IUpdateRequestBuilder WithUpdateCondition(FilterBase condition);
    }
}