using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public class GetItemRequest : GetRequest
    {
        /// <summary>
        /// Determines the read consistency model: If set to true, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads. <br/><br/>
        /// Strongly consistent reads are not supported on global secondary indexes. If you query a global secondary index with <see cref="ConsistentRead"/> set to true, you will receive a validation exception.
        /// </summary>
        public bool ConsistentRead { get; set; }

        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
    }
}