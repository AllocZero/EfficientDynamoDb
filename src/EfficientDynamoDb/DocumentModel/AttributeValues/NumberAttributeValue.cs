using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NumberAttributeValue
    {
        [FieldOffset(0)]
        private readonly string _value;

        public string Value => _value;

        public NumberAttributeValue(string value)
        {
            _value = value;
        }

        public int ToInt() => int.Parse(_value, CultureInfo.InvariantCulture);
        
        public uint ToUInt() => uint.Parse(_value, CultureInfo.InvariantCulture);
        
        public float ToFloat() => float.Parse(_value, CultureInfo.InvariantCulture);

        public double ToDouble() => double.Parse(_value, CultureInfo.InvariantCulture);
        
        public long ToLong() => long.Parse(_value, CultureInfo.InvariantCulture);
        
        public ulong ToULong() => ulong.Parse(_value, CultureInfo.InvariantCulture);
        
        public short ToShort() => short.Parse(_value, CultureInfo.InvariantCulture);
        
        public ushort ToUShort() => ushort.Parse(_value, CultureInfo.InvariantCulture);
        
        public byte ToByte() => byte.Parse(_value, CultureInfo.InvariantCulture);
        
        public decimal ToDecimal() => decimal.Parse(_value, CultureInfo.InvariantCulture);

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, _value);
            writer.WriteEndObject();
        }
        
        public override string ToString() => _value;
    }
}