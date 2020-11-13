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
        private readonly StringAttributeValue _anotherValue;

        public AttributeValue(StringAttributeValue stringValue)
        {
            _type = AttributeType.String;
            _anotherValue = default;
            _stringValue = stringValue;
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
            }
        }
    }
}