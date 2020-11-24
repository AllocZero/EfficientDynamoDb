using EfficientDynamoDb.DocumentModel.Misc;

namespace EfficientDynamoDb.DocumentModel
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