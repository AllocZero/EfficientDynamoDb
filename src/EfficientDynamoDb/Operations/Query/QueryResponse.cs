using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Query
{
    public class QueryResponse : IterableResponse
    {

    }
    
    public class QueryEntityResponse<TEntity> : IterableEntityResponse<TEntity> where TEntity : class
    {

    }

    internal class QueryEntityResponseProjection<TEntity> where TEntity : class
    {
        [DynamoDbProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? PaginationToken { get; set; }
         
        [DynamoDbProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        [DynamoDbProperty("Items", typeof(JsonListHintDdbConverter<>))]
        public List<TEntity> Items { get; set; } = null!;
    }
}