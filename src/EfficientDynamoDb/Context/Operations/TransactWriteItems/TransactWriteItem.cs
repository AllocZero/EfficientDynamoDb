using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems
{
    public class TransactWriteItem
    {
        /// <summary>
        /// A request to perform a check item operation.
        /// </summary>
        public ConditionCheck? ConditionCheck { get; }
        
        /// <summary>
        /// A request to perform a DeleteItem operation.
        /// </summary>
        public TransactDeleteItem? Delete { get; }
        
        /// <summary>
        /// A request to perform a PutItem operation.
        /// </summary>
        public TransactPutItem? Put { get; }
        
        /// <summary>
        /// A request to perform an UpdateItem operation.
        /// </summary>
        public TransactUpdateItem? Update { get; }

        public TransactWriteItem(TransactDeleteItem? delete, ConditionCheck? conditionCheck)
        {
            ConditionCheck = conditionCheck;
            Delete = delete;
        }

        public TransactWriteItem(TransactDeleteItem? delete)
        {
            Delete = delete;
        }

        public TransactWriteItem(TransactPutItem? put)
        {
            Put = put;
        }

        public TransactWriteItem(TransactPutItem? put, ConditionCheck? conditionCheck)
        {
            ConditionCheck = conditionCheck;
            Put = put;
        }

        public TransactWriteItem(TransactUpdateItem? update)
        {
            Update = update;
        }

        public TransactWriteItem(TransactUpdateItem? update, ConditionCheck? conditionCheck)
        {
            ConditionCheck = conditionCheck;
            Update = update;
        }
    }
}