using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct MapAttributeValue
    {
        [FieldOffset(0)]
        private readonly Document _value;

        public Document Value => _value;

        public MapAttributeValue(Document value)
        {
            _value = value;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.Map);
            
            writer.WriteStartObject();

            foreach (var pair in _value)
            {
                writer.WritePropertyName(pair.Key);
                pair.Value.Write(writer);
            }
            
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        public override string ToString() => string.Join(", ", _value.Select(x => $"{x.Key}: {x.Value.ToString()}"));
    }
}