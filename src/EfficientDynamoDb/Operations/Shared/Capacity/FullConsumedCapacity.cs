using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Converters.Primitives;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    public class FullConsumedCapacity : ConsumedCapacity
    {
        [DynamoDBProperty("GlobalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? GlobalSecondaryIndexes { get; set; }

        [DynamoDBProperty("LocalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? LocalSecondaryIndexes { get; set; }

        [DynamoDBProperty("TableName", typeof(StringDdbConverter))]
        public string? TableName { get; set; }
    }
}