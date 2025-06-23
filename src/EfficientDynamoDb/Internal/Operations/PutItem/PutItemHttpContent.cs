using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.PutItem;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHttpContent : PutItemHttpContentBase<PutItemRequest>
    {
        public PutItemHttpContent(PutItemRequest request, ITableNameFormatter? tableNameFormatter) : base(request, tableNameFormatter)
        {
        }

        protected override ValueTask WriteItemAsync(DdbWriter writer) =>
            writer.JsonWriter.WriteAttributesDictionaryAsync(writer.BufferWriter, Request.Item!);
    }
}