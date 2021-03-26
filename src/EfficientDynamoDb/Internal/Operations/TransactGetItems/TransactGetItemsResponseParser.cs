using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.TransactGetItems;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    internal static class TransactGetItemsResponseParser
    {
        public static TransactGetItemsResponse Parse(Document response)
        {
            var responsesArray = response.TryGetValue("Responses", out var responsesAttribute)
                ? (IReadOnlyList<AttributeValue>) responsesAttribute.AsListAttribute().Items
                : Array.Empty<AttributeValue>();

            var items = new Document[responsesArray.Count];
            for (var i = 0; i < responsesArray.Count; i++)
                items[i] = responsesArray[i].AsDocument()["Item"].AsDocument();

            return new TransactGetItemsResponse(items, CapacityParser.ParseTableConsumedCapacities(response));
        }
    }
}