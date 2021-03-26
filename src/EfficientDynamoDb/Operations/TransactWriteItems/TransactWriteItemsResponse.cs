using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.TransactWriteItems
{
    public class TransactWriteItemsResponse
    {
        /// <summary>
        /// The capacity units consumed by the entire <c>TransactWriteItems</c> operation. The values of the list are ordered according to the ordering of the <see cref="TransactWriteItemsRequest.TransactItems"/> request parameter.
        /// </summary>
        public FullConsumedCapacity? ConsumedCapacity { get; }
        
        /// <summary>
        /// A list of tables that were processed by <c>TransactWriteItems</c> and, for each table, information about any item collections that were affected by individual <c>UpdateItem</c>, <c>PutItem</c>, or <c>DeleteItem</c> operations.
        /// </summary>
        public IReadOnlyDictionary<string, ItemCollectionMetrics>? ItemCollectionMetrics { get; }

        public TransactWriteItemsResponse(FullConsumedCapacity? consumedCapacity, IReadOnlyDictionary<string, ItemCollectionMetrics>? itemCollectionMetrics)
        {
            ConsumedCapacity = consumedCapacity;
            ItemCollectionMetrics = itemCollectionMetrics;
        }
    }
    
    public class TransactWriteItemsEntityResponse
    {
        /// <summary>
        /// The capacity units consumed by the entire <c>TransactWriteItems</c> operation. The values of the list are ordered according to the ordering of the <see cref="TransactWriteItemsRequest.TransactItems"/> request parameter.
        /// </summary>
        [DynamoDBProperty("ConsumedCapacity")]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        // /// <summary>
        // /// A list of tables that were processed by <c>TransactWriteItems</c> and, for each table, information about any item collections that were affected by individual <c>UpdateItem</c>, <c>PutItem</c>, or <c>DeleteItem</c> operations.
        // /// </summary>
        // public IReadOnlyDictionary<string, ItemCollectionMetrics>? ItemCollectionMetrics { get; set; }
    }
}