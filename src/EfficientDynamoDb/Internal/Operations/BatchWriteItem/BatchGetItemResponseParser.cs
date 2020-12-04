using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal static class BatchWriteItemResponseParser
    {
        public static BatchWriteItemResponse Parse(Document response) =>
            new BatchWriteItemResponse(CapacityParser.ParseTableConsumedCapacities(response), ParseFailedItems(response));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyDictionary<string, IReadOnlyList<BatchWriteOperation>>? ParseFailedItems(Document response)
        {
            if (!response.TryGetValue("UnprocessedKeys", out var failedItems))
                return null;

            var failedItemsDocument = failedItems.AsDocument();
            var resultDict = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>(failedItemsDocument.Count);
            foreach (var tableData in failedItemsDocument)
            {
                var items = tableData.Value._documentListValue.Items;
                var parsedObjects = new BatchWriteOperation[items.Length];
                for (var i = 0; i < items.Length; i++)
                {
                    parsedObjects[i] = Convert(items[i])!;
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