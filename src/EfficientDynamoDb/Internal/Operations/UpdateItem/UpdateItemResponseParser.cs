using System.Linq;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Misc;
using EfficientDynamoDb.Operations.UpdateItem;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal static class UpdateItemResponseParser
    {
        public static UpdateItemResponse Parse(Document? response)
        {
            if (response == null)
                return new UpdateItemResponse();

            return new UpdateItemResponse
            {
                Attributes = ParseAttributes(response),
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacity(response),
                ItemCollectionMetrics = ParseItemCollectionMetrics(response)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Document? ParseAttributes(Document response) =>
            response.TryGetValue("Attributes", out var attributeValue) ? attributeValue.AsDocument() : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ItemCollectionMetrics? ParseItemCollectionMetrics(Document response)
        {
            if(!response.TryGetValue("ItemCollectionMetrics", out var metrics))
                return null;

            var metricsDocument = metrics.AsDocument();

            var itemCollectionKey = metricsDocument["ItemCollectionKey"].AsDocument().First();
            var estimates = metricsDocument["SizeEstimateRangeGB"].AsNumberSetAttribute().ToFloatArray();
            return new ItemCollectionMetrics(new DdbAttribute(itemCollectionKey.Key, itemCollectionKey.Value), new Range<float>(estimates[0], estimates[1]));
        }
    }
}