using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Binary
{
    internal sealed class BinaryToReadOnlyMemoryDdbConverter : DdbConverter<ReadOnlyMemory<byte>>
    {
        public override ReadOnlyMemory<byte> Read(in AttributeValue attributeValue) => 
            attributeValue.AsBinaryAttribute().Value;

        public override AttributeValue Write(ref ReadOnlyMemory<byte> value)
        {
            var array = MemoryMarshal.TryGetArray(value, out var segment)
                ? segment.Array
                : value.ToArray();
            Debug.Assert(array != null);
            
            return new AttributeValue(new BinaryAttributeValue(array));
        }

        public override ReadOnlyMemory<byte> Read(ref DdbReader reader) => 
            reader.JsonReaderValue.GetBytesFromBase64();

        public override void Write(in DdbWriter writer, ref ReadOnlyMemory<byte> value) => 
            writer.WriteDdbBinary(value.Span);
    }
    
    internal sealed class BinaryToReadOnlyMemoryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => 
            typeToConvert == typeof(ReadOnlyMemory<byte>) || typeToConvert == typeof(ReadOnlyMemory<byte>?);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => 
            new BinaryToReadOnlyMemoryDdbConverter();
    }
}