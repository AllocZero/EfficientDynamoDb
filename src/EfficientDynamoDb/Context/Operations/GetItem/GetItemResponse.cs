using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public class GetItemResponse
    {
        /// <summary>
        /// The capacity units consumed by the <c>GetItem</c> operation.
        /// <c>ConsumedCapacity</c> is only returned if the <see cref="GetItemRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        public TableConsumedCapacity? ConsumedCapacity { get; }
        
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/>> objects, as specified by <see cref="GetRequest.ProjectionExpression"/>>.
        /// </summary>
        public Document? Item { get; }
        
        public GetItemResponse(Document? item, TableConsumedCapacity? consumedCapacity)
        {
            Item = item;
            ConsumedCapacity = consumedCapacity;
        }
    }
}