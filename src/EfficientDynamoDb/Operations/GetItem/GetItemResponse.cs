using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.GetItem
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
    
    public class GetItemEntityResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// The capacity units consumed by the <c>GetItem</c> operation.
        /// <c>ConsumedCapacity</c> is only returned if the <see cref="GetItemRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<TableConsumedCapacity>))]
        public TableConsumedCapacity? ConsumedCapacity { get; set; }
        
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/>> objects, as specified by <see cref="GetRequest.ProjectionExpression"/>>.
        /// </summary>
        [DynamoDbProperty("Item")]
        public TEntity? Item { get; set; }
    }
    
    public class GetItemEntityProjection<TEntity> where TEntity : class
    {
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/>> objects, as specified by <see cref="GetRequest.ProjectionExpression"/>>.
        /// </summary>
        [DynamoDbProperty("Item")]
        public TEntity? Item { get; set; }
    }
}