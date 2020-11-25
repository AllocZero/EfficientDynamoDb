using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Operations.DeleteItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    public static class DeleteItemResponseParser
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