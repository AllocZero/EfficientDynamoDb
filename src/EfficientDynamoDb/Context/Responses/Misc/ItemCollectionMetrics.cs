using System.Collections.Generic;
using EfficientDynamoDb.Context.Requests;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Responses.Misc
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