using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.TransactWriteItems;
using EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders;
using EfficientDynamoDb.Internal.Operations.TransactWriteItems;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public ITransactWriteItemsRequestBuilder TransactWriteItems() => new TransactWriteItemsRequestBuilder(this);
        
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