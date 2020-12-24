using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context.Operations.GetItem;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemHttpContent : GetItemHttpContentBase<GetItemRequest>
    {
        private readonly string _pkName;
        private readonly string? _skName;
        
        public GetItemHttpContent(GetItemRequest request, string? tablePrefix, string pkName, string? skName) : base(request, tablePrefix)
        {
            _pkName = pkName;
            _skName = skName;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            Request.Key!.PartitionKeyValue.Write(writer);

            if (Request.Key.SortKeyValue != null)
            {
                writer.WritePropertyName(_skName!);
                Request.Key.SortKeyValue.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
    }
}