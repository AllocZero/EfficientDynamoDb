using EfficientDynamoDb.Operations.TransactWriteItems;

namespace EfficientDynamoDb.Operations.Shared
{
    /// <summary>
    /// Use <see cref="ReturnValuesOnConditionCheckFailure"/> to get the item attributes if the <see cref="ConditionCheck"/> condition fails. For <see cref="ReturnValuesOnConditionCheckFailure"/>, the valid values are: NONE and ALL_OLD.
    /// </summary>
    public enum ReturnValuesOnConditionCheckFailure
    {
        None = 0,
        AllOld = 1
    }
}