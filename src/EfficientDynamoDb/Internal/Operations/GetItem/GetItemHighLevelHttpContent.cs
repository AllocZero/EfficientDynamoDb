using System.Text.Json;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemHighLevelHttpContent<TPk> : GetItemHttpContentBase<GetItemHighLevelRequest<TPk>>
    {
        private readonly DdbPropertyInfo<TPk> _pk;

        public GetItemHighLevelHttpContent(GetItemHighLevelRequest<TPk> request, string? tablePrefix, DdbPropertyInfo<TPk> partitionKey) : base(request, tablePrefix)
        {
            _pk = partitionKey;
        }

        protected override void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();

            _pk.Converter.Write(writer, _pk.AttributeName, ref Request.PartitionKey);
            
            writer.WriteEndObject();
        }
    }
    
    internal sealed class GetItemHighLevelHttpContent<TPk, TSk> : GetItemHttpContentBase<GetItemHighLevelRequest<TPk, TSk>>
    {
        private readonly DdbPropertyInfo<TPk> _pk;
        private readonly DdbPropertyInfo<TSk> _sk;

        public GetItemHighLevelHttpContent(GetItemHighLevelRequest<TPk, TSk> request, string? tablePrefix, DdbPropertyInfo<TPk> partitionKey,
            DdbPropertyInfo<TSk> sortKey) : base(request, tablePrefix)
        {
            _pk = partitionKey;
            _sk = sortKey;
        }

        protected override void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();

            _pk.Converter.Write(writer, _pk.AttributeName, ref Request.PartitionKey);
            _sk.Converter.Write(writer, _sk.AttributeName, ref Request.SortKey);
            
            writer.WriteEndObject();
        }
    }
}