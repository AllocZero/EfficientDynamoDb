namespace EfficientDynamoDb.Context.Responses.Misc.Capacity
{
    public class TableConsumedCapacity : ConsumedCapacity
    {
        public string? TableName { get; set; }
    }
}