using System;
using System.Collections.Generic;
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
        internal readonly StringAttributeValue _stringValue;
        
        [FieldOffset(0)]
        private readonly NumberAttributeValue _numberValue;
        
        [FieldOffset(0)]
        private readonly BoolAttributeValue _boolValue;

        [FieldOffset(0)]
        private readonly MapAttributeValue _mapValue;

        [FieldOffset(0)] 
        private readonly ListAttributeValue _listValue;

        [FieldOffset(0)] 
        private readonly NullAttributeValue _nullValue;
        
        [FieldOffset(0)] 
        private readonly StringSetAttributeValue _stringSetValue;
        
        [FieldOffset(0)] 
        private readonly NumberSetAttributeValue _numberSetValue;

        public AttributeValue(StringAttributeValue stringValue)
        {
            _type = AttributeType.String;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _stringValue = stringValue;
        }

        public AttributeValue(BoolAttributeValue boolValue)
        {
            _type = AttributeType.Bool;
            _stringValue = default;
            _mapValue = default;
            _listValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _boolValue = boolValue;
        }
        
        public AttributeValue(MapAttributeValue mapValue)
        {
            _type = AttributeType.Map;
            _stringValue = default;
            _boolValue = default;
            _listValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _mapValue = mapValue;
        }

        public AttributeValue(ListAttributeValue listValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _listValue = listValue;
        }
        
        public AttributeValue(NumberAttributeValue numberValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _numberValue = numberValue;
        }
        
        public AttributeValue(NullAttributeValue nullValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _nullValue = default;
            _numberValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _nullValue = nullValue;
        }
        
        public AttributeValue(StringSetAttributeValue stringSetValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _nullValue = default;
            _numberValue = default;
            _numberSetValue = default;
            _nullValue = default;
            _stringSetValue = stringSetValue;
        }
        
        public AttributeValue(NumberSetAttributeValue numberSetValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _nullValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = numberSetValue;
        }

        public bool IsNull() => _nullValue.IsNull;

        public string AsString()
        {
            AssertType(AttributeType.String);
            return _stringValue.Value;
        }

        public int AsInt()
        {
            AssertType(AttributeType.Number);
            return int.Parse(_numberValue.Value, CultureInfo.InvariantCulture);
        }

        public bool AsBool()
        {
            AssertType(AttributeType.Bool);
            return _boolValue.Value;
        }

        public Document AsDocument()
        {
            AssertType(AttributeType.Map);
            return _mapValue.Value;
        }

        public AttributeValue[] AsArray()
        {
            AssertType(AttributeType.List);
            return _listValue.Items;
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
                case AttributeType.Number:
                    _numberValue.Write(writer);
                    break;
                case AttributeType.Bool:
                    _boolValue.Write(writer);
                    break;
                case AttributeType.Map:
                    _mapValue.Write(writer);
                    break;
            }
        }
    }
}