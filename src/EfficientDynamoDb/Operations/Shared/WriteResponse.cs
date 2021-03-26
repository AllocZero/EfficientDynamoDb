using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.Shared
{
    public class WriteResponse
    {
        /// <summary>
        /// The capacity units consumed by the operation.
        /// The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation.
        /// <c>ConsumedCapacity</c> is only returned if the <c>ReturnConsumedCapacity</c> parameter was specified.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }

        /// <summary>
        /// Information about item collections, if any, that were affected by the operation.
        /// <c>ItemCollectionMetrics</c> is only returned if the <c>ReturnItemCollectionMetrics</c> parameter was specified.
        /// If the table does not have any local secondary indexes, this information is not returned in the response.
        /// </summary>
        public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }
    
    public class WriteEntityResponse
    {
        /// <summary>
        /// The capacity units consumed by the operation.
        /// The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation.
        /// <c>ConsumedCapacity</c> is only returned if the <c>ReturnConsumedCapacity</c> parameter was specified.
        /// </summary>
        [DynamoDbProperty("ConsumedCapacity", typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }

        // /// <summary>
        // /// Information about item collections, if any, that were affected by the operation.
        // /// <c>ItemCollectionMetrics</c> is only returned if the <c>ReturnItemCollectionMetrics</c> parameter was specified.
        // /// If the table does not have any local secondary indexes, this information is not returned in the response.
        // /// </summary>
        // public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }
}