using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.TransactWriteItems;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.TransactWriteItems;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for TransactWrite operation.
        /// </summary>
        /// <returns>TransactWrite operation builder.</returns>
        public ITransactWriteItemsRequestBuilder TransactWrite() => new TransactWriteItemsRequestBuilder(this);
        
        internal async Task<OpResult> TransactWriteItemsAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactWriteItemsHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            await ReadAsync<object>(response, cancellationToken).ConfigureAwait(false);
            return new();
        }

        internal async Task<OpResult<TransactWriteItemsEntityResponse>> TransactWriteItemsResponseAsync(BuilderNode node,
            CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactWriteItemsHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<TransactWriteItemsEntityResponse>(response, cancellationToken).ConfigureAwait(false);
            return new(result);
        }
    }
}