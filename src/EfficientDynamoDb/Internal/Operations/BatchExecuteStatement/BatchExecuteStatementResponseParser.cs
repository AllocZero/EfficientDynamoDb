using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using System;
using System.Collections.Generic;

namespace EfficientDynamoDb.Internal.Operations.BatchExecuteStatement
{
    internal static class BatchExecuteStatementResponseParser
    {
        public static BatchExecuteStatementResponse Parse(Document response)
        {
            var responsesArray = response.TryGetValue("Responses", out var responsesAttribute)
                ? (IReadOnlyList<AttributeValue>)responsesAttribute.AsListAttribute().Items
                : Array.Empty<AttributeValue>();

            var items = new Document[responsesArray.Count];
            for (var i = 0; i < responsesArray.Count; i++)
                items[i] = responsesArray[i].AsDocument()["Item"].AsDocument();

            return new BatchExecuteStatementResponse
            {
                Responses = items,
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacities(response)
            };
        }
    }
}
