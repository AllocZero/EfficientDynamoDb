using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class NestedObjectConverter<T> : DdbConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;

        public NestedObjectConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>(_metadata);

        public override AttributeValue Write(ref T value) => new MapAttributeValue(value.ToDocument(_metadata));

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref T value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref T value)
        {
            foreach (var property in _metadata.GetOrAddClassInfo(typeof(T)).Properties)
                property.Write(value, writer);
        }
    }

    internal sealed class NestedObjectConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsClass;

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var converterType = typeof(NestedObjectConverter<>).MakeGenericType(typeToConvert);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata);
        }
    }
}