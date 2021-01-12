using System;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters
{
    // TODO: Validate nullable behavior for dictionaries and lists
    internal sealed class NullableValueTypeDdbConverter<T> : DdbConverter<T?> where T : struct
    {
        private readonly DdbConverter<T> _converter;

        public NullableValueTypeDdbConverter(DdbConverter<T> converter) : base(true)
        {
            _converter = converter;
        }

        public override T? Read(in AttributeValue attributeValue) => _converter.Read(in attributeValue);
        
        public override T? Read(ref DdbReader reader) => _converter.Read(ref reader);

        public override AttributeValue Write(ref T? value)
        {
            var notNullableValue = value!.Value;
            return _converter.Write(ref notNullableValue);
        }

        public override void Write(in DdbWriter writer, string attributeName, ref T? value)
        {
            if (!value.HasValue)
                return;
            
            var attributeValue = Write(ref value);
            if (attributeValue.IsNull)
                return;
            
            writer.JsonWriter.WritePropertyName(attributeName);
            attributeValue.Write(writer.JsonWriter);
        }
    }
}