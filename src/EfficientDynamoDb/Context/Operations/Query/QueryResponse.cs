namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryResponse : IterableResponse
    {

    }
    
    public class QueryEntityResponse<TEntity> : IterableEntityResponse<TEntity> where TEntity : class
    {

    }
}