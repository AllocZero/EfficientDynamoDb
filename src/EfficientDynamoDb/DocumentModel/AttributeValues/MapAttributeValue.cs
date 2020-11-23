using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct MapAttributeValue : IAttributeValue
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
            // TODO: Refactor to check for PendingBytes and call Flush

            writer.WritePropertyName("M");
            
            writer.WriteStartObject();

            foreach (var pair in _value)
            {
                writer.WritePropertyName(pair.Key);
                pair.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }

        public override string ToString() => string.Join(", ", _value.Select(x => $"{x.Key}: {x.Value.ToString()}"));
    }
}