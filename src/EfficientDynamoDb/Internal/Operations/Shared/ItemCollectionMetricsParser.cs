using System.Collections.Generic;
using System.Linq;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Misc;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal static class ItemCollectionMetricsParser
    {
        public static ItemCollectionMetrics? ParseItemCollectionMetrics(Document response)
        {
            if(!response.TryGetValue("ItemCollectionMetrics", out var metrics))
                return null;

            var metricsDocument = metrics.AsDocument();

            return ParseSingle(metricsDocument);
        }
        
        public static IReadOnlyDictionary<string, ItemCollectionMetrics>? ParseMultipleItemCollectionMetrics(Document response)
        {
            if(!response.TryGetValue("ItemCollectionMetrics", out var metricsAttribute))
                return null;

            var metricsRootDocument = metricsAttribute.AsDocument();
            var metrics = new Dictionary<string, ItemCollectionMetrics>(metricsRootDocument.Count);

            foreach (var pair in metricsRootDocument)
                metrics.Add(pair.Key, ParseSingle(pair.Value.AsDocument()));

            return metrics;
        }

        private static ItemCollectionMetrics ParseSingle(Document metricsDocument)
        {
            var itemCollectionKey = metricsDocument["ItemCollectionKey"].AsDocument().First();
            var estimates = metricsDocument["SizeEstimateRangeGB"].ToFloatArray();
            return new ItemCollectionMetrics(new DdbAttribute(itemCollectionKey.Key, itemCollectionKey.Value), new Range<float>(estimates[0], estimates[1]));
        }
    }
}