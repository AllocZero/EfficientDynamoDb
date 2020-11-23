namespace EfficientDynamoDb.Context.Responses
{
    public class ConsumedCapacity
    {
        public float CapacityUnits { get; set; }
        
        public float ReadCapacityUnits { get; set; }
        
        public float WriteCapacityUnits { get; set; }
    }
}