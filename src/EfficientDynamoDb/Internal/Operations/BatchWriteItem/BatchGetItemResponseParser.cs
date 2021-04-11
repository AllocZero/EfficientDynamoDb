using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchWriteItem;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal static class BatchWriteItemResponseParser
    {
        public static BatchWriteItemResponse Parse(Document? response) =>
            response == null
                ? new BatchWriteItemResponse(null, null, null)
                : new BatchWriteItemResponse(CapacityParser.ParseTableConsumedCapacities(response), ItemCollectionMetricsParser.ParseMultipleItemCollectionMetrics(response),
                    ParseFailedItems(response));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyDictionary<string, IReadOnlyList<BatchWriteOperation>>? ParseFailedItems(Document response)
        {
            if (!response.TryGetValue("UnprocessedItems", out var failedItems))
                return null;

            var failedItemsDocument = failedItems.AsDocument();
            var resultDict = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>(failedItemsDocument.Count);
            foreach (var tableData in failedItemsDocument)
            {
                var items = tableData.Value.AsListAttribute().Items;
                var parsedObjects = new BatchWriteOperation[items.Count];
                for (var i = 0; i < items.Count; i++)
                {
                    parsedObjects[i] = Convert(items[i].AsDocument())!;
                }

                resultDict[tableData.Key] = parsedObjects;
            }

            return resultDict;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BatchWriteOperation? Convert(Document item)
        {
            if (item.TryGetValue("DeleteRequest", out var deleteAttributeValue))
            {
                return new BatchWriteOperation(new BatchWriteDeleteRequest(deleteAttributeValue.AsDocument()["Key"].AsDocument()));
            }
            
            if (item.TryGetValue("PutRequest", out var putAttributeValue))
            {
                return new BatchWriteOperation(new BatchWritePutRequest(putAttributeValue.AsDocument()["Item"].AsDocument()));
            }

            // Should never happen
            return null;
        }
    }
}