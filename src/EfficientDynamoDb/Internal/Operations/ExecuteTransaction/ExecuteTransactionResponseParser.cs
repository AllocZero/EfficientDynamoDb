using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Internal.Operations.ExecuteTransaction
{
    internal static class ExecuteTransactionResponseParser
    {
        public static ExecuteTransactionResponse Parse(Document response)
        {
            var responsesArray = response.TryGetValue("Responses", out var responsesAttribute)
                ? (IReadOnlyList<AttributeValue>)responsesAttribute.AsListAttribute().Items
                : Array.Empty<AttributeValue>();

            var items = new Document[responsesArray.Count];
            for (var i = 0; i < responsesArray.Count; i++)
                items[i] = responsesArray[i].AsDocument()["Item"].AsDocument();

            return new ExecuteTransactionResponse
            {
                Responses = items,
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacities(response)
            };
        }
    }
}
