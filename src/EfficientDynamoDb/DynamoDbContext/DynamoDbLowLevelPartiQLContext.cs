using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Internal.Operations.ExecuteStatement;
using EfficientDynamoDb.Internal.Operations.ExecuteTransaction;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb
{
    internal class DynamoDbLowLevelPartiQLContext : IDynamoDbLowLevelPartiQLContext
    {
        internal DynamoDbContextConfig Config { get; }
        internal HttpApi Api { get; }

        internal DynamoDbLowLevelPartiQLContext(DynamoDbContextConfig config, HttpApi api)
        {
            Api = api;
            Config = config;
        }

        public async Task<ExecuteStatementResponse> ExecuteStatementAsync(ExecuteStatementRequest request, CancellationToken cancellationToken = default)
        {
            var httpContent = new ExecuteStatementRequestHttpContent(request);
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await DynamoDbLowLevelContext.ReadDocumentAsync(response, QueryParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            return ExecuteStatementResponseParser.Parse(result!);
        }

        public async Task<BatchExecuteStatementResponse> BatchExecuteStatementAsync(BatchExecuteStatementRequest request, CancellationToken cancellationToken = default)
        {
            var httpContent = new BatchExecuteStatementRequestHttpContent(request);
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await DynamoDbLowLevelContext.ReadDocumentAsync(response, TransactGetItemsParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            return BatchExecuteStatementResponseParser.Parse(result!);
        }

        public async Task<ExecuteTransactionResponse> ExecuteTransactionAsync(ExecuteTransactionRequest request, CancellationToken cancellationToken = default)
        {
            var httpContent = new ExecuteTransactionRequestHttpContent(request);
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await DynamoDbLowLevelContext.ReadDocumentAsync(response, TransactGetItemsParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            return ExecuteTransactionResponseParser.Parse(result!);
        }
    }
}
