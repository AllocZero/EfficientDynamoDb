using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class UShortDdbConverter : NumberDdbConverter<ushort>, IDictionaryKeyConverter<ushort>, ISetValueConverter<ushort>
    {
        public override ushort Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();
        
        public override void Write(in DdbWriter writer, ref ushort value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref ushort value) => writer.JsonWriter.WritePropertyName(value);
        
        public void WriteStringValue(in DdbWriter writer, ref ushort value) => writer.JsonWriter.WriteStringValue(value);

        public override ushort Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out ushort value, out _))
                throw new DdbException($"Couldn't parse ushort ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref ushort value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}