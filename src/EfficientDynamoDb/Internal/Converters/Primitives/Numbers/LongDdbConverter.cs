using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;


namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class LongDdbConverter : NumberDdbConverter<long>, IDictionaryKeyConverter<long>, ISetValueConverter<long>
    {
        public override long Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();

        public override void Write(in DdbWriter writer, ref long value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref long value) => writer.JsonWriter.WritePropertyName(value);
        
        public void WriteStringValue(in DdbWriter writer, ref long value) => writer.JsonWriter.WriteStringValue(value);

        public override long Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out long value, out _))
                throw new DdbException($"Couldn't parse long ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref long value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}