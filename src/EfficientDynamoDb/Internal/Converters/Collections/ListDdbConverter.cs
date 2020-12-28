using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class ListDdbConverter<T> : DdbResumableConverter<List<T>>
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        private readonly DdbConverter<T> _elementConverter;

        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        public ListDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override List<T> Read(in AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new List<T>(items.Length);

            for (var i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                entities.Add(_elementConverter.Read(in item));
            }

            return entities;
        }

        public override AttributeValue Write(ref List<T> value)
        {
            var array = new AttributeValue[value.Count];

            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                array[i] = _elementConverter.Write(ref item);
            }
            
            return new ListAttributeValue(array);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref List<T> value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref List<T> value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref List<T> value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("L");
            
            writer.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                _elementConverter.Write(writer, ref itemCopy);
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }

    internal sealed class ListDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || (!typeToConvert.IsClass && !typeToConvert.IsInterface))
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(List<>) || genericType == typeof(IList<>) || genericType == typeof(ICollection<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var elementType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(ListDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}