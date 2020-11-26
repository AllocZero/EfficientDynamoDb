namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public class BatchWriteOperation
    {
        /// <summary>
        /// Perform a <c>DeleteItem</c> operation on the specified item. The item to be deleted is identified by a <see cref="BatchWriteDeleteRequest.Key"/> subelement.
        /// </summary>
        public BatchWriteDeleteRequest? DeleteRequest { get; }
        
        /// <summary>
        /// Perform a <c>PutItem</c> operation on the specified item. The item to be put is identified by an <see cref="BatchWritePutRequest.Item"/> subelement.
        /// </summary>
        public BatchWritePutRequest? PutRequest { get; }

        public BatchWriteOperation(BatchWriteDeleteRequest deleteRequest) => DeleteRequest = deleteRequest;

        public BatchWriteOperation(BatchWritePutRequest putRequest) => PutRequest = putRequest;
    }
}