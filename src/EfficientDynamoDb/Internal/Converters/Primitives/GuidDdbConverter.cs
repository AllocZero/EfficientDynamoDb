using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class GuidDdbConverter : DdbConverter<Guid>
    {
        public override Guid Read(in AttributeValue attributeValue) => Guid.Parse(attributeValue.AsString());

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