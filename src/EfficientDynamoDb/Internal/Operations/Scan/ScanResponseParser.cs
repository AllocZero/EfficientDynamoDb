using System;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.Scan
{
    public class ScanResponseParser
    {
        public static ScanResponse Parse(Document response) =>
            new ScanResponse
            {
                Count = response.GetOptionalInt("Count"),
                ScannedCount = response.GetOptionalInt("ScannedCount"),
                Items = response.TryGetValue("Items", out var items) ? items._documentListValue.Items : Array.Empty<Document>(),
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacity(response),
                LastEvaluatedKey = QueryResponseParser.ParseLastEvaluatedKey(response)
            };
    }
}