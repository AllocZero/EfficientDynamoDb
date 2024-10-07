using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Operations.ExecuteTransaction
{
    internal static class ExecuteTransactionResponseParser
    {
        public static ExecuteTransactionResponse Parse(Document response)
            => new ExecuteTransactionResponse { Responses = ParseResponses(response), ConsumedCapacity = CapacityParser.ParseFullConsumedCapacities(response) };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<ItemResponse> ParseResponses(Document response)
        {
            if (!response.TryGetValue("Responses", out var responsesAttr))
                return null!;

            var responsesList = new List<ItemResponse>();

            foreach (var item in responsesAttr.AsListAttribute().Items)
            {
                responsesList.Add(ParseItemResponse(item.AsDocument()));
            }

            return responsesList;
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
