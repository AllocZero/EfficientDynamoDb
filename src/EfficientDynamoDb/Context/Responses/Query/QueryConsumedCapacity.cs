using System.Collections.Generic;

namespace EfficientDynamoDb.Context.Responses.Query
{
    public class QueryConsumedCapacity : ConsumedCapacity
    {
        public IReadOnlyDictionary<string, ConsumedCapacity>? GlobalSecondaryIndexes { get; set; }
        
        public IReadOnlyDictionary<string, ConsumedCapacity>? LocalSecondaryIndexes { get; set; }

        public string? TableName { get; set; }
    }
}