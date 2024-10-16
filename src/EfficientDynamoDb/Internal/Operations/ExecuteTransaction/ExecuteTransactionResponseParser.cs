using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using System;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Operations.ExecuteTransaction
{
    internal static class ExecuteTransactionResponseParser
    {
        public static ExecuteTransactionResponse Parse(Document response)
            => new ExecuteTransactionResponse { Responses = ParseResponses(response), ConsumedCapacity = CapacityParser.ParseFullConsumedCapacities(response) };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ItemResponse[] ParseResponses(Document response)
        {
            if (!response.TryGetValue("Responses", out var responsesAttr))
                return Array.Empty<ItemResponse>();

            var attributesList = responsesAttr.AsListAttribute().Items;
            var responses = new ItemResponse[attributesList.Count];
            for (var i = 0; i < attributesList.Count; i++)
                responses[i] = ParseItemResponse(attributesList[i].AsDocument());

            return responses;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ItemResponse ParseItemResponse(Document document)
        {
            var response = new ItemResponse();
            document.TryGetValue("Item", out var itemAttr);
            response.Item = itemAttr.AsDocument();
            return response;
        }
    }
}
