using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.UpdateItem
{
    public class UpdateItemRequest : UpdateRequest
    {
        /// <summary>
        /// Use <c>ReturnValues</c> if you want to get the item attributes as they appeared before they were updated with the <c>UpdateItem</c> request.
        /// </summary>
        /// <remarks>
        /// There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed.
        /// <br/><br/>
        /// The values returned are strongly consistent.
        /// </remarks>
        public ReturnValues ReturnValues { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response. <br/><br/>
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }

        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }
    }
}