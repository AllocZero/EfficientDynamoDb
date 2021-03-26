using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.TransactWriteItems;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.TransactWriteItems;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        public ITransactWriteItemsRequestBuilder TransactWrite() => new TransactWriteItemsRequestBuilder(this);
        
        internal async Task TransactWriteItemsAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactWriteItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            await ReadAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TransactWriteItemsEntityResponse> TransactWriteItemsResponseAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactWriteItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<TransactWriteItemsEntityResponse>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}