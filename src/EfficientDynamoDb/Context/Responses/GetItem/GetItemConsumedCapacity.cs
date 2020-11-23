namespace EfficientDynamoDb.Context.Responses.GetItem
{
    public class GetItemConsumedCapacity : ConsumedCapacity
    {
        public string? TableName { get; set; }
    }
}