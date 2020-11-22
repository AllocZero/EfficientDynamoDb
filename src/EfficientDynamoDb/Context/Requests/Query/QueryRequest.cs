namespace EfficientDynamoDb.Context.Requests.Query
{
    public class QueryRequest
    {
        public string? TableName { get; set; }
        
        // public string? IndexName { get; set; }
        
        public Expression? KeyExpression { get; set; }
        
        // public Expression? FilterExpression { get; set; }

    }
}