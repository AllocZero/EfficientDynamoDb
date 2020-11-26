using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.DeleteItem
{
    public class DeleteItemRequest : DeleteRequest
    {
        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
        
        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }
        
        /// <summary>
        /// Use ReturnValues if you want to get the item attributes as they appeared before they were deleted.
        /// <list type="bullet">
        /// <listheader>
        /// <description>For delete, the valid values are:</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="EfficientDynamoDb.DocumentModel.ReturnDataFlags.ReturnValues.None"/> - If <see cref="ReturnValues"/> is not specified, or if its value is <see cref="EfficientDynamoDb.DocumentModel.ReturnDataFlags.ReturnValues.None"/>, then nothing is returned. (This setting is the default for <see cref="ReturnValues"/>.)</term>
        /// </item>
        /// <item>
        /// <term><see cref="DocumentModel.ReturnDataFlags.ReturnValues.AllOld"/> - The content of the old item is returned.</term>
        /// </item>
        /// </list>
        /// </summary>
        public ReturnValues ReturnValues { get; set; }
    }
}