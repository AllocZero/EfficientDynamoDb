using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Lambda
{
    public class DynamoDbEvent
    {
        [DynamoDbProperty("Records", typeof(JsonIReadOnlyListDdbConverter<>))]
        public IReadOnlyList<Record> Records { get; set; } = null!;
    }
}