using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchGetItem;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    internal static class BatchGetItemResponseParser
    {
        public static BatchGetItemResponse Parse(Document response) => new BatchGetItemResponse(CapacityParser.ParseTableConsumedCapacities(response),
            ParseProcessedItems(response), ParseFailedItems(response));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyDictionary<string, IReadOnlyList<Document>>? ParseProcessedItems(Document response)
        {
            if (!response.TryGetValue("Responses", out var processedItems))
                return null;
            
            var resultDict = new Dictionary<string, IReadOnlyList<Document>>();
            foreach (var tableData in processedItems.AsDocument())
            {
                resultDict[tableData.Key] = tableData.Value._documentListValue.Items;
            }

            return resultDict;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyDictionary<string, TableBatchGetItemRequest>? ParseFailedItems(Document response)
        {
            if (!response.TryGetValue("UnprocessedKeys", out var failedItems))
                return null;

            var failedItemsDocument = failedItems.AsDocument();
            
            var resultDict = new Dictionary<string, TableBatchGetItemRequest>(failedItemsDocument.Count);
            foreach (var tableData in failedItemsDocument) 
                resultDict[tableData.Key] = Convert(tableData.Value.AsDocument());

            return resultDict;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TableBatchGetItemRequest Convert(Document item)
        {
            var result = new TableBatchGetItemRequest();

            if (item.TryGetValue("ConsistentRead", out var temp))
                result.ConsistentRead = temp.AsBool();

            if (item.TryGetValue("ProjectionExpression", out temp))
                result.ProjectionExpression = temp.AsString();

            if (item.TryGetValue("ExpressionAttributeNames", out temp))
                result.ExpressionAttributeNames = temp.AsDocument().ToDictionary(x => x.Key, x => x.Value.AsString());

            if (item.TryGetValue("Keys", out temp))
            {
                var responseKeysArray = temp.AsListAttribute().Items;
                var parsedKeysArray = new Dictionary<string, AttributeValue>[responseKeysArray.Count];
                for (var i = 0; i < responseKeysArray.Count; i++)
                {
                    parsedKeysArray[i] = responseKeysArray[i].AsDocument();
                }

                result.Keys = parsedKeysArray;
            }

            return result;
        }
    }
}