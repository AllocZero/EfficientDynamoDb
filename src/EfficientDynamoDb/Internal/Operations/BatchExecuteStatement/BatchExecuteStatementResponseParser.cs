using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Operations.BatchExecuteStatement
{
    internal static class BatchExecuteStatementResponseParser
    {
        public static BatchExecuteStatementResponse Parse(Document response)
            => new BatchExecuteStatementResponse { Responses = ParseResponses(response), ConsumedCapacity = CapacityParser.ParseFullConsumedCapacities(response) };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<BatchStatementResponse> ParseResponses(Document response)
        {
            if (!response.TryGetValue("Responses", out var responsesAttr))
                return null!;

            var responsesList = new List<BatchStatementResponse>();

            foreach (var item in responsesAttr.AsListAttribute().Items)
            {
                responsesList.Add(ParseBatchStatementResponse(item.AsDocument()));
            }

            return responsesList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BatchStatementResponse ParseBatchStatementResponse(Document document)
        {
            var response = new BatchStatementResponse();

            if (document.TryGetValue("TableName", out var tableNameAttr))
            {
                response.TableName = tableNameAttr.AsString();
            }

            if (document.TryGetValue("Error", out var errorAttr))
            {
                response.Error = ParseBatchStatementError(errorAttr.AsDocument());
            }

            if (document.TryGetValue("Item", out var itemAttr))
            {
                response.Item = itemAttr.AsDocument();
            }

            return response;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BatchStatementError? ParseBatchStatementError(Document document)
        {
            var error = new BatchStatementError();

            if (document.TryGetValue("Code", out var codeAttr))
            {
                error.Code = codeAttr.AsString();
            }

            if (document.TryGetValue("Message", out var messageAttr))
            {
                error.Message = messageAttr.AsString();
            }

            if (document.TryGetValue("Item", out var itemAttr))
            {
                error.Item = itemAttr.AsDocument();
            }

            return error;
        }
    }
}
