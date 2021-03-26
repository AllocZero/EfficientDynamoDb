using System.Collections.Generic;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public class BatchWriteItemRequest
    {
        /// <summary>
        /// A map of one or more table names and, for each table, a list of operations to be performed (<c>DeleteItem</c> or <c>PutItem</c>). 
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<BatchWriteOperation>>? RequestItems { get; set; }
        
        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }

        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }
    }
}