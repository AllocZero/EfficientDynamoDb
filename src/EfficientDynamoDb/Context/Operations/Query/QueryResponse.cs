using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
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
        [DynamoDBProperty("LastEvaluatedKey", typeof(JsonIReadOnlyDictionaryDdbConverter<string, AttributeValue>))]
        public IReadOnlyDictionary<string, AttributeValue>? LastEvaluatedKey { get; set; }
         
        [DynamoDBProperty("Count", typeof(JsonIntSizeHintDdbConverter))]
        public int Count { get; set; }
        
        [DynamoDBProperty("Items", typeof(JsonIReadOnlyListDdbConverter<>))]
        public IReadOnlyList<TEntity> Items { get; set; } = null!;
    }
}