using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal static class QueryResponseParser
    {
        public static QueryResponse Parse(Document response) =>
            new QueryResponse
            {
                Count = response.GetOptionalInt("Count"),
                ScannedCount = response.GetOptionalInt("ScannedCount"),
                Items = response.TryGetValue("Items", out var items) ? items._documentListValue.Items : Array.Empty<Document>(),
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacity(response),
                LastEvaluatedKey = ParseLastEvaluatedKey(response)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IReadOnlyDictionary<string, AttributeValue>? ParseLastEvaluatedKey(Document response)
        {
            if(!response.TryGetValue("LastEvaluatedKey", out var attribute))
                return null;

            var document = attribute.AsDocument();
            return document.Count > 0 ? document : null;
        }
    }
}