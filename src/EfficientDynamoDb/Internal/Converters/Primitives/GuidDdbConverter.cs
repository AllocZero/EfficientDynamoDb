using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class GuidDdbConverter : DdbConverter<Guid>
    {
        public override Guid Read(in AttributeValue attributeValue) => Guid.Parse(attributeValue.AsString());

        public override Guid Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out Guid value, out _))
                throw new DdbException($"Couldn't parse Guid ddb value from '{reader.GetString()}'.");

            return value;
        }

        public override AttributeValue Write(ref Guid value) => new StringAttributeValue(value.ToString());

        public override void Write(Utf8JsonWriter writer, string attributeName, ref Guid value)
        {
            writer.WritePropertyName(attributeName);

            WriteInternal(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref Guid value) => WriteInternal(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInternal(Utf8JsonWriter writer, ref Guid value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, value);
            writer.WriteEndObject();
        }
    }
}