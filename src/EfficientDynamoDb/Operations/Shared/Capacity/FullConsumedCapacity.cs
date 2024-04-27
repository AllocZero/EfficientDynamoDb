using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Converters.Primitives;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<FullConsumedCapacity>))]
    public class FullConsumedCapacity : ConsumedCapacity
    {
        [DynamoDbProperty("GlobalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? GlobalSecondaryIndexes { get; set; }

        [DynamoDbProperty("LocalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? LocalSecondaryIndexes { get; set; }
        
        [DynamoDbProperty("Table", typeof(JsonObjectDdbConverter<ConsumedCapacity>))]
        public ConsumedCapacity? Table { get; set; }

        [DynamoDbProperty("TableName", typeof(StringDdbConverter))]
        public string? TableName { get; set; }
    }
}