using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal static class GetItemResponseParser
    {
        public static GetItemResponse Parse(Document? response) =>
            response != null ? new GetItemResponse(ParseItem(response), ParseConsumedCapacity(response)) : new GetItemResponse(null, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Document? ParseItem(Document response) => response.TryGetValue("Item", out var attributeValue) ? attributeValue.AsDocument() : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TableConsumedCapacity? ParseConsumedCapacity(Document response)
        {
            if (!response.TryGetValue("ConsumedCapacity", out var consumedCapacityAttribute))
                return null;

            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            var consumedCapacity = new TableConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
            };

            return consumedCapacity;
        }
    }
}