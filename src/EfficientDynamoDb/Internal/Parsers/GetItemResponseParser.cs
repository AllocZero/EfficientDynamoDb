using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Requests;
using EfficientDynamoDb.Context.Responses;
using EfficientDynamoDb.Context.Responses.GetItem;
using EfficientDynamoDb.Context.Responses.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Parsers
{
    public static class GetItemResponseParser
    {
        public static GetItemResponse Parse(Document response) => new GetItemResponse(response["Item"].AsDocument(), ParseConsumedCapacity(response));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static GetItemConsumedCapacity? ParseConsumedCapacity(Document response)
        {
            if (!response.TryGetValue("ConsumedCapacity", out var consumedCapacityAttribute))
                return null;

            var consumedCapacityDocument = consumedCapacityAttribute.AsDocument();
            var consumedCapacity = new GetItemConsumedCapacity
            {
                TableName = consumedCapacityDocument.TryGetValue("TableName", out var tableName) ? tableName.AsString() : null,
                CapacityUnits = consumedCapacityDocument.GetOptionalFloat("CapacityUnits"),
            };

            return consumedCapacity;
        }
    }
}