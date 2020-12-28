using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class ShortDdbConverter : NumberDdbConverter<short>
    {
        public override short Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();

        public override void Write(Utf8JsonWriter writer, string attributeName, ref short value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref short value) => WriteInlined(writer, ref value);

        public override void WriteStringValue(Utf8JsonWriter writer, ref short value) => writer.WriteStringValue(value);

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