using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.GetItem
{
    public abstract class GetItemRequestBase : GetRequest
    {
        /// <summary>
        /// Determines the read consistency model: If set to true, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads. <br/><br/>
        /// </summary>
        public bool ConsistentRead { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
    }
}