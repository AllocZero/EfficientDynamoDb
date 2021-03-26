using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Operations.DeleteItem
{
    public class DeleteItemResponse
    {
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/> objects, representing the item as it appeared before the <c>DeleteItem</c> operation. This map appears in the response only if <see cref="DeleteItemRequest.ReturnValues"/> was specified as ALL_OLD in the request.
        /// </summary>
        public Document? Attributes { get; set; }
        
        /// <summary>
        /// The capacity units consumed by the <c>DeleteItem</c> operation. The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation. <see cref="ConsumedCapacity"/> is only returned if the <see cref="DeleteItemRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        /// <summary>
        /// Information about item collections, if any, that were affected by the DeleteItem operation. <see cref="ItemCollectionMetrics"/> is only returned if the <see cref="DeleteItemRequest.ReturnItemCollectionMetrics"/> parameter was specified. If the table does not have any local secondary indexes, this information is not returned in the response.<br/><br/>
        /// <list type="bullet">
        /// <listheader>
        /// <description>Each ItemCollectionMetrics element consists of:</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Shared.ItemCollectionMetrics.ItemCollectionKey"/> - The partition key value of the item collection. This is the same as the partition key value of the item itself.</term>
        /// </item>
        /// <item>
        /// <term><see cref="Shared.ItemCollectionMetrics.SizeEstimateRangeGb"/> - An estimate of item collection size, in gigabytes. This value is a two-element array containing a lower bound and an upper bound for the estimate. The estimate includes the size of all the items in the table, plus the size of all attributes projected into all of the local secondary indexes on that table. Use this estimate to measure whether a local secondary index is approaching its size limit.<br/>
        ///The estimate is subject to change over time; therefore, do not rely on the precision or accuracy of the estimate.</term>
        /// </item>
        /// </list>
        /// </summary>
        public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }
    
    public class DeleteItemEntityResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/> objects, representing the item as it appeared before the <c>DeleteItem</c> operation. This map appears in the response only if <see cref="DeleteItemRequest.ReturnValues"/> was specified as ALL_OLD in the request.
        /// </summary>
        [DynamoDbProperty("Attributes")]
        public TEntity? Attributes { get; set; }
        
        /// <summary>
        /// The capacity units consumed by the <c>DeleteItem</c> operation. The data returned includes the total provisioned throughput consumed, along with statistics for the table and any indexes involved in the operation. <see cref="ConsumedCapacity"/> is only returned if the <see cref="DeleteItemRequest.ReturnConsumedCapacity"/> parameter was specified.
        /// </summary>
        [DynamoDbProperty("Attributes")]
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        // /// <summary>
        // /// Information about item collections, if any, that were affected by the DeleteItem operation. <see cref="ItemCollectionMetrics"/> is only returned if the <see cref="DeleteItemRequest.ReturnItemCollectionMetrics"/> parameter was specified. If the table does not have any local secondary indexes, this information is not returned in the response.<br/><br/>
        // /// <list type="bullet">
        // /// <listheader>
        // /// <description>Each ItemCollectionMetrics element consists of:</description>
        // /// </listheader>
        // /// <item>
        // /// <term><see cref="EfficientDynamoDb.DocumentModel.ItemCollectionMetrics.ItemCollectionKey"/> - The partition key value of the item collection. This is the same as the partition key value of the item itself.</term>
        // /// </item>
        // /// <item>
        // /// <term><see cref="EfficientDynamoDb.DocumentModel.ItemCollectionMetrics.SizeEstimateRangeGb"/> - An estimate of item collection size, in gigabytes. This value is a two-element array containing a lower bound and an upper bound for the estimate. The estimate includes the size of all the items in the table, plus the size of all attributes projected into all of the local secondary indexes on that table. Use this estimate to measure whether a local secondary index is approaching its size limit.<br/>
        // ///The estimate is subject to change over time; therefore, do not rely on the precision or accuracy of the estimate.</term>
        // /// </item>
        // /// </list>
        // /// </summary>
        // public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }

    internal sealed class DeleteItemEntityProjection<TEntity> where TEntity : class
    {
        /// <summary>
        /// A map of attribute names to <see cref="AttributeValue"/> objects, representing the item as it appeared before the <c>DeleteItem</c> operation. This map appears in the response only if <see cref="DeleteItemRequest.ReturnValues"/> was specified as ALL_OLD in the request.
        /// </summary>
        [DynamoDbProperty("Attributes")]
        public TEntity? Attributes { get; set; }
    }
}