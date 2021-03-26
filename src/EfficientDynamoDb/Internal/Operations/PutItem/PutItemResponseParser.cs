using System.Linq;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.PutItem;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal static class PutItemResponseParser
    {
        public static PutItemResponse Parse(Document? response)
        {
            if (response == null)
                return new PutItemResponse();

            return new PutItemResponse
            {
                Attributes = ParseAttributes(response),
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacity(response),
                ItemCollectionMetrics = ItemCollectionMetricsParser.ParseItemCollectionMetrics(response)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Document? ParseAttributes(Document response) =>
            response.TryGetValue("Attributes", out var attributeValue) ? attributeValue.AsDocument() : null;
    }
}