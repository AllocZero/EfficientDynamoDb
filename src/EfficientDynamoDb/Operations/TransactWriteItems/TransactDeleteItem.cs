using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems
{
    public class TransactDeleteItem : DeleteRequest
    {
        /// <summary>
        /// Use <see cref="ReturnValuesOnConditionCheckFailure"/> to get the item attributes if the <c>Delete</c> condition fails. For <see cref="ReturnValuesOnConditionCheckFailure"/>, the valid values are: NONE and ALL_OLD.
        /// </summary>
        public ReturnValuesOnConditionCheckFailure ReturnValuesOnConditionCheckFailure { get; set; }
    }
}