using System.Collections.Generic;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public class BatchWriteItemResponse
    {
        /// <summary>
        /// <para>The read capacity units consumed by the entire <c>BatchWriteItem</c> operation.</para>
        /// <list type="bullet">
        /// <listheader>
        /// Each element consists of:
        /// </listheader>
        /// <item>
        /// <c>TableName</c> - The table that consumed the provisioned throughput.
        /// </item>
        /// <item>
        /// <c>CapacityUnits</c> - The total number of capacity units consumed.
        /// </item>
        /// </list>
        /// </summary>
        public IReadOnlyList<FullConsumedCapacity>? ConsumedCapacity { get; }
        
        public IReadOnlyDictionary<string, ItemCollectionMetrics>? ItemCollectionMetrics { get; }
        
        /// <summary>
        /// A map of tables and requests against those tables that were not processed. The <c>UnprocessedItems</c> value is in the same form as <see cref="BatchWriteItemRequest.RequestItems"/>, so you can provide this value directly to a subsequent <c>BatchWriteItem</c> operation.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<BatchWriteOperation>>? UnprocessedItems { get; }

        public BatchWriteItemResponse(IReadOnlyList<FullConsumedCapacity>? consumedCapacity, IReadOnlyDictionary<string, ItemCollectionMetrics>? itemCollectionMetrics, IReadOnlyDictionary<string, IReadOnlyList<BatchWriteOperation>>? unprocessedItems)
        {
            ConsumedCapacity = consumedCapacity;
            UnprocessedItems = unprocessedItems;
            ItemCollectionMetrics = itemCollectionMetrics;
        }
    }
}