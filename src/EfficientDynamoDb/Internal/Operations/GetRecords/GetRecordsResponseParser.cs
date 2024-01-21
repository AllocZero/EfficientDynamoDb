using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Internal.Operations.GetRecords
{
    internal static class GetRecordsResponseParser
    {
        public static GetRecordsResponse Parse(Document response) => new GetRecordsResponse(ParseNextShardIterator(response),
            RecordsParser.ParseRecords(response));

        private static string? ParseNextShardIterator(Document response) => 
            response.TryGetValue("NextShardIterator", out var nextShardIterator) ? nextShardIterator.AsString() : null;
    }
}