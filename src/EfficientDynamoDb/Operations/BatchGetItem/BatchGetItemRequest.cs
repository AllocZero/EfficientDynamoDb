using System.Collections.Generic;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    public class BatchGetItemRequest
    {
        /// <summary>
        /// <para>A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table.</para>
        /// Each table name can be used only once per BatchGetItem request.
        /// </summary>
        public IReadOnlyDictionary<string, TableBatchGetItemRequest>? RequestItems { get; set; }
        
        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
    }
}