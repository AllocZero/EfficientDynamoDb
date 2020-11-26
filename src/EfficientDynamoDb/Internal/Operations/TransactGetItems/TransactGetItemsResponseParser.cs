using System;
using EfficientDynamoDb.Context.Operations.TransactGetItems;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    public static class TransactGetItemsResponseParser
    {
        public static TransactGetItemsResponse Parse(Document response)
        {
            var responsesArray = response.TryGetValue("Responses", out var responsesAttribute) ? responsesAttribute.AsListAttribute().Items : Array.Empty<AttributeValue>();

            var items = new Document[responsesArray.Length];
            for (var i = 0; i < responsesArray.Length; i++)
                items[i] = responsesArray[i].AsDocument()["Item"].AsDocument();

            return new TransactGetItemsResponse(items, CapacityParser.ParseTableConsumedCapacities(response));
        }
    }
}