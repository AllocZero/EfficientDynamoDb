using System.Collections.Generic;

namespace EfficientDynamoDb.DocumentModel.Capacity
{
    public class FullConsumedCapacity : ConsumedCapacity
    {
        public IReadOnlyDictionary<string, ConsumedCapacity>? GlobalSecondaryIndexes { get; set; }
        
        public IReadOnlyDictionary<string, ConsumedCapacity>? LocalSecondaryIndexes { get; set; }

        public string? TableName { get; set; }
    }
}