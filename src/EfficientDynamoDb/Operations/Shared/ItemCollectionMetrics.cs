using EfficientDynamoDb.Operations.Shared.Misc;

namespace EfficientDynamoDb.Operations.Shared
{
    public class ItemCollectionMetrics
    {
        public DdbAttribute ItemCollectionKey { get; }
        
        public Range<float> SizeEstimateRangeGb { get; }

        public ItemCollectionMetrics(DdbAttribute itemCollectionKey, Range<float> sizeEstimateRangeGb)
        {
            ItemCollectionKey = itemCollectionKey;
            SizeEstimateRangeGb = sizeEstimateRangeGb;
        }
    }
}