using EfficientDynamoDb.Internal.Operations.BatchExecuteStatement;
using EfficientDynamoDb.Operations.BatchExecuteStatement;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        public async Task<List<TEntity?>> BatchExecuteStatementAsync<TEntity>(BatchExecuteStatementRequest request, CancellationToken cancellationToken = default) where TEntity : class
        {
            var httpContent = new BatchExecuteStatementRequestHttpContent(request);
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<BatchExecuteStatementEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            var entities = new List<TEntity?>(result.Responses.Count);
            foreach (var item in result.Responses)
                entities.Add(item.Item);

            return entities;
        }
    }
}
