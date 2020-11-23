using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NumberAttributeValue : IAttributeValue
    {
        [FieldOffset(0)]
        private readonly string _value;

        public string Value => _value;

        public NumberAttributeValue(string value)
        {
            _value = value;
        }

        public int ToInt() => int.Parse(_value, CultureInfo.InvariantCulture);
        
        public float ToFloat() => float.Parse(_value, CultureInfo.InvariantCulture);

        public double ToDouble() => double.Parse(_value, CultureInfo.InvariantCulture);

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString("N", _value);
            writer.WriteEndObject();
        }
        
        public override string ToString() => _value;
    }
}