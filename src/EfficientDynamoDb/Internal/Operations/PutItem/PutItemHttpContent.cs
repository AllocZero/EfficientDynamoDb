using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHttpContent : PutItemHttpContentBase<PutItemRequest>
    {
        public PutItemHttpContent(PutItemRequest request, string? tablePrefix) : base(request, tablePrefix)
        {
        }

        protected override ValueTask WriteItemAsync(DdbWriter writer) =>
            writer.JsonWriter.WriteAttributesDictionaryAsync(writer.BufferWriter, Request.Item!);
    }
}