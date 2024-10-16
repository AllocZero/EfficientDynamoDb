using EfficientDynamoDb.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteTransaction;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb
{
    public interface IDynamoDbLowLevelPartiQLContext
    {
        Task<ExecuteStatementResponse> ExecuteStatementAsync(ExecuteStatementRequest request, CancellationToken cancellationToken = default);

        Task<BatchExecuteStatementResponse> BatchExecuteStatementAsync(BatchExecuteStatementRequest request, CancellationToken cancellationToken = default);

        Task<ExecuteTransactionResponse> ExecuteTransactionAsync(ExecuteTransactionRequest request, CancellationToken cancellationToken = default);
    }
}
