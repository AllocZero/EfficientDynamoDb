using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryResponse : IterableResponse
    {

    }
    
    public class QueryEntityResponse<TEntity> : IterableEntityResponse<TEntity> where TEntity : class
    {

    }

    internal class QueryEntityResponseProjection<TEntity> where TEntity : class
    {
        [DynamoDBProperty("LastEvaluatedKey", typeof(PaginationDdbConverter))]
        public string? PaginationToken { get; set; }
         
        [DynamoDBProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        [DynamoDBProperty("Items", typeof(JsonListDdbConverter<>))]
        public List<TEntity> Items { get; set; } = null!;
    }
}