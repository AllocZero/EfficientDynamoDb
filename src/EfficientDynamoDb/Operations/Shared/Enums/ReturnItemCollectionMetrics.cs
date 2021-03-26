namespace EfficientDynamoDb.Operations.Shared
{
    public enum ReturnItemCollectionMetrics : byte
    {
        /// <summary>
        /// No item collection metrics are included in the response.
        /// </summary>
        None = 0,
        /// <summary>
        /// The response includes statistics about item collections, if any, that were modified during the operation are returned in the response
        /// </summary>
        Size = 1
    }
}