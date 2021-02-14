using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Context.Operations.Scan
{
    public class ScanResponse : IterableResponse
    {
        
    }
    
    public class ScanEntityResponse<TEntity> : IterableEntityResponse<TEntity> where TEntity : class
    {
        
    }
    
    internal sealed class ScanEntityResponseProjection<TEntity> where TEntity : class
    {
        [DynamoDBProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? PaginationToken { get; set; }
         
        [DynamoDBProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        [DynamoDBProperty("Items", typeof(JsonListHintDdbConverter<>))]
        public List<TEntity> Items { get; set; } = null!;
    }
}