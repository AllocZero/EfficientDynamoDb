using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Responses;
using EfficientDynamoDb.Context.Responses.Misc.Capacity;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Parsers
{
    internal static class CapacityParser
    {
        internal static FullConsumedCapacity? ParseConsumedCapacity(Document response, string capacityFieldName = "ConsumedCapacity")
        {
            if (!response.TryGetValue(capacityFieldName, out var consumedCapacityAttribute))
                return null;

            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            var consumedCapacity = new FullConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
                GlobalSecondaryIndexes = ParseConsumedCapacities(consumedCapacityDocument, "GlobalSecondaryIndexes"),
                LocalSecondaryIndexes = ParseConsumedCapacities(consumedCapacityDocument, "LocalSecondaryIndexes"),
            };

            return consumedCapacity;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyDictionary<string, ConsumedCapacity>? ParseConsumedCapacities(Document document, string key)
        {
            if (!document.TryGetValue(key, out var consumedCapacityAttribute))
                return null;

            var consumedCapacitiesDocument = consumedCapacityAttribute.AsDocument();
            var consumedCapacities = new Dictionary<string, ConsumedCapacity>(consumedCapacitiesDocument.Count);
            
            foreach (var pair in consumedCapacitiesDocument)
            {
                var consumedCapacityDocument = pair.Value.AsDocument();
                
                consumedCapacities.Add(pair.Key, new ConsumedCapacity
                {
                    CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
                });
            }

            return consumedCapacities;
        }
    }
}