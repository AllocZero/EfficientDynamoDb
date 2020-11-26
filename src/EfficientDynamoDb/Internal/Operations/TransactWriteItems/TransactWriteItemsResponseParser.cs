using EfficientDynamoDb.Context.Operations.TransactWriteItems;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    public static class TransactWriteItemsResponseParser
    {
        public static TransactWriteItemsResponse Parse(Document? response)
        {
            return response == null
                ? new TransactWriteItemsResponse(null, null)
                : new TransactWriteItemsResponse(CapacityParser.ParseFullConsumedCapacity(response), ItemCollectionMetricsParser.ParseMultipleItemCollectionMetrics(response));
        }
    }
}