using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems
{
    public class TransactWriteItem
    {
        /// <summary>
        /// A request to perform a check item operation.
        /// </summary>
        public ConditionCheck? ConditionCheck { get; set; }
        
        /// <summary>
        /// A request to perform a DeleteItem operation.
        /// </summary>
        public TransactDeleteItem? Delete { get; set; }
        
        /// <summary>
        /// A request to perform a PutItem operation.
        /// </summary>
        public TransactPutItem? Put { get; set; }
        
        /// <summary>
        /// A request to perform an UpdateItem operation.
        /// </summary>
        public TransactUpdateItem? Update { get; set; }
    }
}