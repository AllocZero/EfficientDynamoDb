using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class GuidDdbConverter : DdbConverter<Guid>, IDictionaryKeyConverter<Guid>, ISetValueConverter<Guid>
    {
        public GuidDdbConverter() : base(true)
        {
        }

        public override Guid Read(in AttributeValue attributeValue) => Guid.Parse(attributeValue.AsString());

        public override Guid Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out Guid value, out _))
                throw new DdbException($"Couldn't parse Guid ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        public override AttributeValue Write(ref Guid value) => new AttributeValue(new StringAttributeValue(value.ToString()));
        
        public override void Write(in DdbWriter writer, ref Guid value) => WriteInternal(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref Guid value) => writer.JsonWriter.WritePropertyName(value);

        public string WriteStringValue(ref Guid value) => value.ToString();

        public void WriteStringValue(in DdbWriter writer, ref Guid value) => writer.JsonWriter.WriteStringValue(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInternal(Utf8JsonWriter writer, ref Guid value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, value);
            writer.WriteEndObject();
        }
    }
}