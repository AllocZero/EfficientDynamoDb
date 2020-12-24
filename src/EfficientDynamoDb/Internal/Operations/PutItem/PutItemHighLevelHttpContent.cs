using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHighLevelHttpContent : PutItemHttpContentBase<HighLevelPutItemRequest>
    {
        private readonly DdbClassInfo _entityClassInfo;

        public PutItemHighLevelHttpContent(HighLevelPutItemRequest request, string? tablePrefix, DdbClassInfo entityClassInfo) : base(request, tablePrefix)
        {
            _entityClassInfo = entityClassInfo;
        }

        protected override ValueTask WriteItemAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter) =>
            writer.WriteEntityAsync(bufferWriter, _entityClassInfo, Request.Item!);
    }
}