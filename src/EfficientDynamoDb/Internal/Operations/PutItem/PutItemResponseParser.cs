using System.Linq;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Misc;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    public static class PutItemResponseParser
    {
        public static PutItemResponse Parse(Document? response)
        {
            if (response == null)
                return new PutItemResponse();

            return new PutItemResponse
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
            var estimates = metricsDocument["SizeEstimateRangeGB"].ToFloatArray();
            return new ItemCollectionMetrics(new DdbAttribute(itemCollectionKey.Key, itemCollectionKey.Value), new Range<float>(estimates[0], estimates[1]));
        }
    }
}