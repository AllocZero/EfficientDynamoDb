using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder.GetItemHttpContents
{
    public class GetItemHttpContent<TPkAttribute> : GetItemHttpContentBase where TPkAttribute : IAttributeValue
    {
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue) : base(tableName)
        {
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
        }
        
        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();

            return default;
        }
    }

    public class GetItemHttpContent<TPkAttribute, TSkAttribute> : GetItemHttpContentBase
        where TPkAttribute : IAttributeValue
        where TSkAttribute : IAttributeValue
    {
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;
        private readonly string _skName;
        private readonly TSkAttribute _skAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue, string skName, TSkAttribute skAttributeValue) : base(tableName)
        {
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
            _skName = skName;
            _skAttributeValue = skAttributeValue;
        }
        
        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WritePropertyName(_skName);
            _skAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();

            return default;
        }
    }
}