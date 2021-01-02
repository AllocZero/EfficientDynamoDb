using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.DocumentModel.Capacity
{
    public class FullConsumedCapacity : ConsumedCapacity
    {
        [DynamoDBProperty("GlobalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? GlobalSecondaryIndexes { get; set; }

        [DynamoDBProperty("LocalSecondaryIndexes", typeof(JsonIReadOnlyDictionaryDdbConverter<string, ConsumedCapacity>))]
        public IReadOnlyDictionary<string, ConsumedCapacity>? LocalSecondaryIndexes { get; set; }

        public string? TableName { get; set; }
    }
}