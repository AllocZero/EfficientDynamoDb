using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly struct AttributeValue
    {
        public static readonly AttributeValue Null = new NullAttributeValue(true);
        
        // TODO: Handle x86 case
        [FieldOffset(8)] 
        private readonly AttributeType _type;

        [FieldOffset(0)]
        private readonly StringAttributeValue _stringValue;
        
        [FieldOffset(0)]
        private readonly NumberAttributeValue _numberValue;
        
        [FieldOffset(0)]
        private readonly BoolAttributeValue _boolValue;

        [FieldOffset(0)]
        internal readonly MapAttributeValue _mapValue;

        [FieldOffset(0)] 
        private readonly ListAttributeValue _listValue;

        [FieldOffset(0)] 
        private readonly NullAttributeValue _nullValue;
        
        [FieldOffset(0)] 
        private readonly StringSetAttributeValue _stringSetValue;
        
        [FieldOffset(0)] 
        private readonly NumberSetAttributeValue _numberSetValue;

        [FieldOffset(0)] 
        internal readonly DocumentListAttributeValue _documentListValue;

        [FieldOffset(0)] 
        private readonly BinaryAttributeValue _binaryValue;

        [FieldOffset(0)] 
        private readonly BinarySetAttributeValue _binarySetValue;
        
        public AttributeType Type => _type;
        
        public AttributeValue(StringAttributeValue stringValue)
        {
            this = default;
            _type = AttributeType.String;
            _stringValue = stringValue;
        }

        public AttributeValue(BoolAttributeValue boolValue)
        {
            this = default;
            _type = AttributeType.Bool;
            _boolValue = boolValue;
        }
        
        public AttributeValue(MapAttributeValue mapValue)
        {
            this = default;
            _type = AttributeType.Map;
            _mapValue = mapValue;
        }

        public AttributeValue(ListAttributeValue listValue)
        {
            this = default;
            _type = AttributeType.List;
            _listValue = listValue;
        }
        
        public AttributeValue(NumberAttributeValue numberValue)
        {
            this = default;
            _type = AttributeType.Number;
            _numberValue = numberValue;
        }
        
        public AttributeValue(NullAttributeValue nullValue)
        {
            this = default;
            _type = AttributeType.Null;
            _nullValue = nullValue;
        }
        
        public AttributeValue(StringSetAttributeValue stringSetValue)
        {
            this = default;
            _type = AttributeType.StringSet;
            _stringSetValue = stringSetValue;
        }
        
        public AttributeValue(NumberSetAttributeValue numberSetValue)
        {
            this = default;
            _type = AttributeType.NumberSet;
            _numberSetValue = numberSetValue;
        }
        
        internal AttributeValue(DocumentListAttributeValue documentListValue)
        {
            this = default;
            _type = AttributeType.List;
            _documentListValue = documentListValue;
        }
        
        internal AttributeValue(BinaryAttributeValue binaryValue)
        {
            this = default;
            _type = AttributeType.Binary;
            _binaryValue = binaryValue;
        }

        internal AttributeValue(BinarySetAttributeValue binarySetValue)
        {
            this = default;
            _type = AttributeType.BinarySet;
            _binarySetValue = binarySetValue;
        }
        
        public bool IsNull => _type == AttributeType.Null && _nullValue.IsNull;

        public StringAttributeValue AsStringAttribute()
        {
            AssertType(AttributeType.String);
            return _stringValue;
        }

        public BoolAttributeValue AsBoolAttribute()
        {
            AssertType(AttributeType.Bool);
            return _boolValue;
        }

        public MapAttributeValue AsMapAttribute()
        {
            AssertType(AttributeType.Map);
            return _mapValue;
        }

        public NumberAttributeValue AsNumberAttribute()
        {
            AssertType(AttributeType.Number);
            return _numberValue;
        }

        public StringSetAttributeValue AsStringSetAttribute()
        {
            AssertType(AttributeType.StringSet);
            return _stringSetValue;
        }

        public NumberSetAttributeValue AsNumberSetAttribute()
        {
            AssertType(AttributeType.NumberSet);
            return _numberSetValue;
        }

        public ListAttributeValue AsListAttribute()
        {
            AssertType(AttributeType.List);
            return _listValue;
        }

        public NullAttributeValue AsNullAttribute()
        {
            AssertType(AttributeType.StringSet);
            return _nullValue;
        }

        public BinaryAttributeValue AsBinaryAttribute()
        {
            AssertType(AttributeType.Binary);
            return _binaryValue;
        }

        public BinarySetAttributeValue AsBinarySetAttribute()
        {
            AssertType(AttributeType.BinarySet);
            return _binarySetValue;
        }
        
        public Document AsDocument() => AsMapAttribute().Value;

        public string AsString() => AsStringAttribute().Value;

        public bool AsBool() => AsBoolAttribute().Value;

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
                case AttributeType.Null:
                    _nullValue.Write(writer);
                    break;
                case AttributeType.List:
                    _listValue.Write(writer);
                    break;
                case AttributeType.StringSet:
                    _stringSetValue.Write(writer);
                    break;
                case AttributeType.NumberSet:
                    _numberSetValue.Write(writer);
                    break;
                case AttributeType.Binary:
                    _binaryValue.Write(writer);
                    break;
                case AttributeType.BinarySet:
                    _binarySetValue.Write(writer);
                    break;
            }
        }

        public override string ToString()
        {
            return _type switch
            {
                AttributeType.String => _stringValue.ToString(),
                AttributeType.Number => _numberValue.ToString(),
                AttributeType.Bool => _boolValue.ToString(),
                AttributeType.Map => _mapValue.ToString(),
                AttributeType.Null => _nullValue.ToString(),
                AttributeType.List => _listValue.ToString(),
                AttributeType.StringSet => _stringSetValue.ToString(),
                AttributeType.NumberSet => _numberSetValue.ToString(),
                AttributeType.Binary => _binaryValue.ToString(),
                AttributeType.BinarySet => _binarySetValue.ToString(),
                _ => Type.ToString()
            };
        }

        public static implicit operator AttributeValue(string value)
        {
            return new AttributeValue(new StringAttributeValue(value));
        }

        public static implicit operator AttributeValue(int value)
        {
            return new AttributeValue(new NumberAttributeValue(value.ToString()));
        }
        
        public static implicit operator AttributeValue(bool value)
        {
            return new AttributeValue(new BoolAttributeValue(value));
        }

        public static implicit operator AttributeValue(Document value)
        {
            return new AttributeValue(new MapAttributeValue(value));
        }
        
        public static implicit operator AttributeValue(MapAttributeValue value) => new AttributeValue(value);

        public static implicit operator AttributeValue(StringAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(NumberAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(BoolAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(NullAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(StringSetAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(NumberSetAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(ListAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(BinaryAttributeValue value) => new AttributeValue(value);
        
        public static implicit operator AttributeValue(BinarySetAttributeValue value) => new AttributeValue(value);
    }
}