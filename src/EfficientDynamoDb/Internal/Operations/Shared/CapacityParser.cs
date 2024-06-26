using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal static class CapacityParser
    {
        internal static FullConsumedCapacity? ParseFullConsumedCapacity(Document response, string capacityFieldName = "ConsumedCapacity")
        {
            if (!response.TryGetValue(capacityFieldName, out var consumedCapacityAttribute))
                return null;

            return ParseFullConsumedCapacityInternal(consumedCapacityAttribute);
        }

        internal static List<FullConsumedCapacity>? ParseFullConsumedCapacities(Document response, string capacityFieldName = "ConsumedCapacity")
        {
            if (!response.TryGetValue(capacityFieldName, out var consumedCapacityAttribute))
                return null;

            var capacities = consumedCapacityAttribute.AsListAttribute().Items;
            if (capacities.Count == 0)
                return null;

            var result = new List<FullConsumedCapacity>(capacities.Count);
            foreach (var capacity in capacities)
            {
                result.Add(ParseFullConsumedCapacityInternal(capacity));
            }

            return result;
        }

        internal static TableConsumedCapacity? ParseTableConsumedCapacity(Document response, string capacityFieldName = "ConsumedCapacity")
        {
            if (!response.TryGetValue(capacityFieldName, out var consumedCapacityAttribute))
                return null;

            return ParseTableConsumedCapacityInternal(consumedCapacityAttribute);
        }

        internal static IReadOnlyList<TableConsumedCapacity>? ParseTableConsumedCapacities(Document response, string capacityFieldName = "ConsumedCapacity")
        {
            if (!response.TryGetValue(capacityFieldName, out var consumedCapacityAttribute))
                return null;

            var capacities = consumedCapacityAttribute.AsListAttribute().Items;
            if (capacities.Count == 0)
                return null;

            var result = new TableConsumedCapacity[capacities.Count];
            for (var i = 0; i < capacities.Count; i++)
            {
                result[i] = ParseTableConsumedCapacityInternal(capacities[i]);
            }

            return result;
        }

        private static FullConsumedCapacity ParseFullConsumedCapacityInternal(AttributeValue consumedCapacityAttribute)
        {
            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            return new FullConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
                GlobalSecondaryIndexes = ParseConsumedCapacities(consumedCapacityDocument, "GlobalSecondaryIndexes"),
                LocalSecondaryIndexes = ParseConsumedCapacities(consumedCapacityDocument, "LocalSecondaryIndexes"),
                Table = consumedCapacityDocument.TryGetValue("Table", out var table)
                    ? new ConsumedCapacity { CapacityUnits = table.AsDocument().GetOptionalFloat("CapacityUnits") }
                    : null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TableConsumedCapacity ParseTableConsumedCapacityInternal(AttributeValue consumedCapacityAttribute)
        {
            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            return new TableConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
            };
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