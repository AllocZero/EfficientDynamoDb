using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.BatchGetItem
{
    public class BatchGetItemResponse
    {
        /// <summary>
        /// <para>The read capacity units consumed by the entire <c>BatchGetItem</c> operation.</para>
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
        public IReadOnlyList<TableConsumedCapacity>? ConsumedCapacity { get; }
        
        /// <summary>
        /// A map of table name to a list of items. Each object in Responses consists of a table name, along with a list of <see cref="Document"/>.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<Document>>? Responses { get; }
        
        /// <summary>
        /// A map of tables and their respective keys that were not processed with the current response.
        /// The <c>UnprocessedKeys</c> value is in the same form as <see cref="BatchGetItemRequest.RequestItems"/>, so the value can be provided directly to a subsequent BatchGetItem operation.
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<TableBatchGetItemRequest>>? UnprocessedKeys { get; }

        public BatchGetItemResponse(IReadOnlyList<TableConsumedCapacity>? consumedCapacity, IReadOnlyDictionary<string, IReadOnlyList<Document>>? responses, IReadOnlyDictionary<string, IReadOnlyList<TableBatchGetItemRequest>>? unprocessedKeys)
        {
            ConsumedCapacity = consumedCapacity;
            Responses = responses;
            UnprocessedKeys = unprocessedKeys;
        }
    }
}