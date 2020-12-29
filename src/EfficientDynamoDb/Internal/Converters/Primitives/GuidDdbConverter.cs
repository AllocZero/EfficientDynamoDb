using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class GuidDdbConverter : DdbConverter<Guid>, IDictionaryKeyConverter<Guid>, ISetValueConverter<Guid>
    {
        public override Guid Read(in AttributeValue attributeValue) => Guid.Parse(attributeValue.AsString());

        public override Guid Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out Guid value, out _))
                throw new DdbException($"Couldn't parse Guid ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        public override AttributeValue Write(ref Guid value) => new StringAttributeValue(value.ToString());

        public override void Write(Utf8JsonWriter writer, string attributeName, ref Guid value)
        {
            writer.WritePropertyName(attributeName);

            WriteInternal(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref Guid value) => WriteInternal(writer, ref value);

        public void WritePropertyName(Utf8JsonWriter writer, ref Guid value) => writer.WritePropertyName(value);

        public void WriteStringValue(Utf8JsonWriter writer, ref Guid value) => writer.WriteStringValue(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInternal(Utf8JsonWriter writer, ref Guid value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, value);
            writer.WriteEndObject();
        }
    }
}