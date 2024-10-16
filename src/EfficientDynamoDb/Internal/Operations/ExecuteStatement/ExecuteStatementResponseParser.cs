using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.ExecuteStatement;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Operations.ExecuteStatement
{
    internal static class ExecuteStatementResponseParser
    {
        public static ExecuteStatementResponse Parse(Document response) =>
            new ExecuteStatementResponse
            {
                Items = response.TryGetValue("Items", out var items) ? items._documentListValue.Items : Array.Empty<Document>(),
                LastEvaluatedKey = ParseLastEvaluatedKey(response),
                NextToken = response.TryGetValue("NextToken", out var nextToken) ? nextToken.AsString() : string.Empty,
                ConsumedCapacity = CapacityParser.ParseFullConsumedCapacity(response),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IReadOnlyDictionary<string, AttributeValue>? ParseLastEvaluatedKey(Document response)
        {
            if (!response.TryGetValue("LastEvaluatedKey", out var attribute))
                return null;

            var document = attribute.AsDocument();
            return document.Count > 0 ? document : null;
        }
    }
}
