namespace EfficientDynamoDb.DocumentModel.Capacity
{
    public class TableConsumedCapacity : ConsumedCapacity
    {
        public string? TableName { get; set; }
    }
}