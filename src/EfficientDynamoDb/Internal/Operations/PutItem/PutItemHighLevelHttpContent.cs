using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHighLevelHttpContent : PutItemHttpContentBase<HighLevelPutItemRequest>
    {
        private readonly DynamoDbContextMetadata _metadata;

        public PutItemHighLevelHttpContent(HighLevelPutItemRequest request, string? tablePrefix, DynamoDbContextMetadata metadata) : base(request, tablePrefix)
        {
            _metadata = metadata;
        }

        protected override ValueTask WriteItemAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter) =>
            writer.WriteEntityAsync(bufferWriter, Request.Item!, _metadata);
    }
}