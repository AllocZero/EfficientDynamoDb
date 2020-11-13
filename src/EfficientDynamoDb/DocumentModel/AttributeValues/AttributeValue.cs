using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly struct AttributeValue : IAttributeValue
    {
        // TODO: Handle x86 case
        [FieldOffset(8)] 
        private readonly AttributeType _type;

        [FieldOffset(0)]
        private readonly StringAttributeValue _stringValue;
        
        [FieldOffset(0)]
        private readonly BoolAttributeValue _boolValue;

        [FieldOffset(0)]
        private readonly DocumentAttributeValue _documentValue;

        public AttributeValue(StringAttributeValue stringValue)
        {
            _type = AttributeType.String;
            _boolValue = default;
            _documentValue = default;
            _stringValue = stringValue;
        }

        public AttributeValue(BoolAttributeValue boolValue)
        {
            _type = AttributeType.Bool;
            _stringValue = default;
            _documentValue = default;
            _boolValue = boolValue;
        }
        
        public AttributeValue(DocumentAttributeValue documentValue)
        {
            _type = AttributeType.Map;
            _stringValue = default;
            _boolValue = default;
            _documentValue = documentValue;
        }

        public string AsString()
        {
            AssertType(AttributeType.String);
            return _stringValue.Value;
        }

        public int AsInt()
        {
            AssertType(AttributeType.String);
            return int.Parse(_stringValue.Value, CultureInfo.InvariantCulture);
        }

        public bool AsBool()
        {
            AssertType(AttributeType.Bool);
            return _boolValue.Value;
        }

        public Document AsDocument()
        {
            AssertType(AttributeType.Map);
            return _documentValue.Value;
        }

        private void AssertType(AttributeType expectedType)
        {
            if (expectedType != _type)
                throw new InvalidOperationException($"Attribute contains '{_type}' value instead of '{expectedType}'.");
        }

        public void Write(Utf8JsonWriter writer)
        {
            switch (_type)
            {
                case AttributeType.String:
                    _stringValue.Write(writer);
                    break;
                case AttributeType.Bool:
                    _boolValue.Write(writer);
                    break;
                case AttributeType.Map:
                    _documentValue.Write(writer);
                    break;
            }
        }
    }
}