using System;
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
        
        public AttributeType Type => _type;
        
        public AttributeValue(StringAttributeValue stringValue)
        {
            _type = AttributeType.String;
            _boolValue = default; // Looks ugly, but performance of :this() is yet to be benchmarked
            _mapValue = default;
            _listValue = default;
            _numberValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _documentListValue = default;
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
            _documentListValue = default;
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
            _documentListValue = default;
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
            _documentListValue = default;
            _listValue = listValue;
        }
        
        public AttributeValue(NumberAttributeValue numberValue)
        {
            _type = AttributeType.Number;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _nullValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _documentListValue = default;
            _numberValue = numberValue;
        }
        
        public AttributeValue(NullAttributeValue nullValue)
        {
            _type = AttributeType.List;
            _stringValue = default;
            _boolValue = default;
            _mapValue = default;
            _listValue = default;
            _numberValue = default;
            _stringSetValue = default;
            _numberSetValue = default;
            _documentListValue = default;
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
            _documentListValue = default;
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
            _stringSetValue = default;
            _documentListValue = default;
            _numberSetValue = numberSetValue;
        }
        
        internal AttributeValue(DocumentListAttributeValue documentListValue)
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
            _documentListValue = documentListValue;
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
        
        public Document AsDocument() => AsMapAttribute().Value;

        public string AsString() => AsStringAttribute().Value;
        
        public int ToInt() => AsNumberAttribute().ToInt();
        
        public float ToFloat() => AsNumberAttribute().ToFloat();

        public double ToDouble() => AsNumberAttribute().ToDouble();

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
            }
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
    }
}