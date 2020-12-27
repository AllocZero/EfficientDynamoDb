using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class ArrayDdbConverter<T> : DdbConverter<T[]>
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        private readonly DdbConverter<T> _elementConverter;

        public override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        public ArrayDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override T[] Read(in AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new T[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                entities[i] = _elementConverter.Read(in item);
            }

            return entities;
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out T[] value)
        {
            if (state.UseFastPath)
            {
                ref var current = ref state.GetCurrent();
                
                // TODO: Handle missing hint case
                var i = 0;
                value = new T[current.BufferLengthHint];

                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    // Start object
                    reader.ReadWithVerify();
                    
                    // Attribute type
                    reader.ReadWithVerify();
                    
                    _elementConverter.TryRead(ref reader, ref state, DdbJsonReader.GetDdbAttributeType(ref reader), out var element);
                    value[i++] = element;
                    
                    // End object
                    reader.ReadWithVerify();
                    
                    reader.ReadWithVerify();
                }
                
                return true;
            }
            
            throw new NotImplementedException();
        }

        public override AttributeValue Write(ref T[] value)
        {
            var array = new AttributeValue[value.Length];

            for (var i = 0; i < value.Length; i++)
                array[i] = _elementConverter.Write(ref value[i]);

            return new ListAttributeValue(array);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T[] value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref T[] value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref T[] value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("L");

            writer.WriteStartArray();

            for (var i = 0; i < value.Length; i++)
                _elementConverter.Write(writer, ref value[i]);

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }

    internal sealed class ArrayDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsArray)
                return true;

            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(IReadOnlyCollection<>) || genericType == typeof(IReadOnlyList<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var elementType = typeToConvert.IsArray ? typeToConvert.GetElementType()! : typeToConvert.GetGenericArguments()[0]!;
            var converterType = typeof(ArrayDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}