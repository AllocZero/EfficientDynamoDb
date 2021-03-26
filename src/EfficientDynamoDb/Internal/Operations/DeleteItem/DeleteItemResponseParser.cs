using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.DeleteItem;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal static class DeleteItemResponseParser
    {
        public static DeleteItemResponse Parse(Document? response)
        {
            if (response == null)
                return new DeleteItemResponse();

            return new DeleteItemResponse
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