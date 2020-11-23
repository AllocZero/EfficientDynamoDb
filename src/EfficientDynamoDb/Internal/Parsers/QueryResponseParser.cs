using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Requests;
using EfficientDynamoDb.Context.Responses;
using EfficientDynamoDb.Context.Responses.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Parsers
{
    public static class QueryResponseParser
    {
        public static QueryResponse Parse(Document response)
        {
            return new QueryResponse
            {
                Count = response.GetOptionalInt("Count"),
                ScannedCount = response.GetOptionalInt("ScannedCount"),
                Items = response.TryGetValue("Items", out var items) ? items._documentListValue.Items : Array.Empty<Document>(),
                ConsumedCapacity = ParseConsumedCapacity(response),
                LastEvaluatedKey = ParseLastEvaluatedKey(response)
            };
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyDictionary<string, AttributeValue>? ParseLastEvaluatedKey(Document response)
        {
            if(!response.TryGetValue("LastEvaluatedKey", out var attribute))
                return null;

            var document = attribute.AsDocument();
            return document.Count > 0 ? document : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static QueryConsumedCapacity? ParseConsumedCapacity(Document response)
        {
            if (!response.TryGetValue("ConsumedCapacity", out var consumedCapacityAttribute))
                return null;

            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            var consumedCapacity = new QueryConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
                ReadCapacityUnits = consumedCapacityDocument.GetOptionalFloat("ReadCapacityUnits"),
                WriteCapacityUnits = consumedCapacityDocument.GetOptionalFloat("WriteCapacityUnits"),
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
                    ReadCapacityUnits = consumedCapacityDocument.GetOptionalFloat("ReadCapacityUnits"),
                    WriteCapacityUnits = consumedCapacityDocument.GetOptionalFloat("WriteCapacityUnits")
                });
            }

            return consumedCapacities;
        }
    }
}