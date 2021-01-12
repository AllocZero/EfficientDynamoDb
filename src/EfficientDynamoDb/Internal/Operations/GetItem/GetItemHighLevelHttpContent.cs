using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.DocumentModel.Converters;
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

        protected override void WritePrimaryKey(in DdbWriter writer)
        {
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            _pk.Converter.Write(in writer, _pk.AttributeName, ref Request.PartitionKey);
            
            writer.JsonWriter.WriteEndObject();
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

        protected override void WritePrimaryKey(in DdbWriter writer)
        {
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            _pk.Converter.Write(in writer, _pk.AttributeName, ref Request.PartitionKey);
            _sk.Converter.Write(in writer, _sk.AttributeName, ref Request.SortKey);
            
            writer.JsonWriter.WriteEndObject();
        }
    }
}