using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;

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
        
        protected override void WriteData(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();
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
        
        protected override void WriteData(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            // TODO: Consider flushing, review JsonSerializer.Write.Helpers.WriteAsyncCore
            // Flushing is not needed for GetItem because JSON should mostly be smaller than DefaultBufferSize, but will be needed for bigger request classes

            writer.WritePropertyName(_skName);
            _skAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();
        }
    }
}