using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.Scan
{
    public class ScanResponse : IterableResponse
    {
        
    }
    
    public class ScanEntityResponse<TEntity> : IterableEntityResponse<TEntity> where TEntity : class
    {
        
    }
    
    internal sealed class ScanEntityResponseProjection<TEntity> where TEntity : class
    {
        [DynamoDbProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? PaginationToken { get; set; }
         
        [DynamoDbProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        [DynamoDbProperty("Items", typeof(JsonListHintDdbConverter<>))]
        public List<TEntity> Items { get; set; } = null!;
    }
}