using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Binary
{
    internal sealed class BinaryToMemoryDdbConverter : DdbConverter<Memory<byte>>
    {
        public override Memory<byte> Read(in AttributeValue attributeValue) => 
            attributeValue.AsBinaryAttribute().Value;

        public override AttributeValue Write(ref Memory<byte> value)
        {
            var array = MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)value, out var segment) && segment.Offset == 0 && segment.Count == value.Length
                ? segment.Array
                : value.ToArray();
            Debug.Assert(array != null);
            
            return new AttributeValue(new BinaryAttributeValue(array));
        }

        public override Memory<byte> Read(ref DdbReader reader) => 
            reader.JsonReaderValue.GetBytesFromBase64();

        public override void Write(in DdbWriter writer, ref Memory<byte> value) => 
            writer.WriteDdbBinary(value.Span);
    }

    internal sealed class BinaryToMemoryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert == typeof(Memory<byte>) || typeToConvert == typeof(Memory<byte>?);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => 
            new BinaryToMemoryDdbConverter();
    }
}