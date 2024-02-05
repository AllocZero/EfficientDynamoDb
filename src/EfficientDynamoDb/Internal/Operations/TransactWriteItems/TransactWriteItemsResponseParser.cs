using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.TransactWriteItems;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    internal static class TransactWriteItemsResponseParser
    {
        public static TransactWriteItemsResponse Parse(Document? response)
        {
            return response == null
                ? new TransactWriteItemsResponse(null, null)
                : new TransactWriteItemsResponse(CapacityParser.ParseFullConsumedCapacities(response), ItemCollectionMetricsParser.ParseMultipleItemCollectionMetrics(response));
        }
    }
}