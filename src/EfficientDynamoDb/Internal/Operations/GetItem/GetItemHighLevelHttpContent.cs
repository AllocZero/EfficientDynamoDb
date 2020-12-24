using System.Text.Json;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemHighLevelHttpContent : GetItemHttpContentBase<GetItemHighLevelRequest>
    {
        private readonly DdbPropertyInfo _partitionKey;
        private readonly DdbPropertyInfo? _sortKey;

        public GetItemHighLevelHttpContent(GetItemHighLevelRequest request, string? tablePrefix, DdbPropertyInfo partitionKey) : base(request, tablePrefix)
        {
            _partitionKey = partitionKey;
        }

        public GetItemHighLevelHttpContent(GetItemHighLevelRequest request, string? tablePrefix, DdbPropertyInfo partitionKey, DdbPropertyInfo sortKey) : base(request, tablePrefix)
        {
            _partitionKey = partitionKey;
            _sortKey = sortKey;
        }

        protected override void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();

            _partitionKey.WriteValue(Request.PartitionKey, writer);
            if (_sortKey != null && Request.SortKey != null)
                _sortKey.WriteValue(Request.SortKey, writer);
            
            writer.WriteEndObject();
        }
    }
}