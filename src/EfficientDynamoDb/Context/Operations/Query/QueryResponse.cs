using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.Query
{
    public class QueryResponse
    {
        public int Count { get; set; }
        
        public int ScannedCount { get; set; }

        public Document[] Items { get; set; } = null!;
        
        public IReadOnlyDictionary<string, AttributeValue>? LastEvaluatedKey { get; set; }
        
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
    }
}