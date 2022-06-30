using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class ShortDdbConverter : NumberDdbConverter<short>, IDictionaryKeyConverter<short>, ISetValueConverter<short>
    {
        public override short Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToShort();

        public override void Write(in DdbWriter writer, ref short value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref short value) => writer.JsonWriter.WritePropertyName(value);
        
        public void WriteStringValue(in DdbWriter writer, ref short value) => writer.JsonWriter.WriteStringValue(value);

        public override short Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out short value, out _))
                throw new DdbException($"Couldn't parse short ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref short value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}