using EfficientDynamoDb.Internal.Operations.ExecuteStatement;
using EfficientDynamoDb.Operations.ExecuteStatement;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        public async Task<List<TEntity>> ExecuteStatementAsync<TEntity>(ExecuteStatementRequest request, CancellationToken cancellationToken = default) where TEntity : class
        {
            var httpContent = new ExecuteStatementRequestHttpContent(request);
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<ExecuteStatementEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            var entities = new List<TEntity>(result.Items.Count);
            foreach (var item in result.Items)
                entities.Add(item);

            return entities;
        }
    }
}
