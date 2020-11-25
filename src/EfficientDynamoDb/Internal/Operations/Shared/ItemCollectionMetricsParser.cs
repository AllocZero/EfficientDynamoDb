using System.Linq;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Misc;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    public static class ItemCollectionMetricsParser
    {
        public static ItemCollectionMetrics? ParseItemCollectionMetrics(Document response)
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