using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public abstract class PutItemRequestBase : WriteRequest
    {
        /// <summary>
        /// Use <c>ReturnValues</c> if you want to get the item attributes as they appeared before they were updated with the <c>PutItem</c> request.
        /// </summary>
        /// <remarks>
        /// The <c>ReturnValues</c> parameter is used by several DynamoDB operations; however, <c>PutItem</c> does not recognize any values other than <c>NONE</c> or <c>ALL_OLD</c>.
        /// <br/><br/>
        /// There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed.
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