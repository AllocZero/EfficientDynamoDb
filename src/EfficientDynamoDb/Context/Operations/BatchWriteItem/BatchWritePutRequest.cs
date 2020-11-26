using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public class BatchWritePutRequest
    {
        /// <summary>
        /// A map of attributes and their values. Each entry in this map consists of an attribute name and an attribute value. 
        /// </summary>
        public Document Item { get; }

        public BatchWritePutRequest(Document item) => Item = item;
    }
}