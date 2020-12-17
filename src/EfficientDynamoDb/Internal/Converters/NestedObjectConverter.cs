using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class NestedObjectConverter<T> : DdbConverter<T> where T : class
    {
        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>();

        public override AttributeValue Write(ref T value) => new MapAttributeValue(value.ToDocument());

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref T value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref T value)
        {
            foreach (var property in DdbClassInfoCache.GetOrAdd(typeof(T)).Properties)
                property.Write(value, writer);
        }
    }
}