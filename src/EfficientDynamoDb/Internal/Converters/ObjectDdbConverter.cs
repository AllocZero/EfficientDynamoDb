using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class ObjectDdbConverter<T> : DdbConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;
        
        public override DdbClassType ClassType => DdbClassType.Object;

        public ObjectDdbConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>(_metadata);

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out T value)
        {
            if (state.UseFastPath)
            {
                state.PushObject();
                
                var classInfo = _metadata.GetOrAddClassInfo(typeof(T));
                
                var entity = classInfo.Constructor!();
                ref var current = ref state.GetCurrent();
                
                // Property name
                reader.ReadWithVerify();
                
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    if (!current.ClassInfo!.JsonProperties.TryGetValue(ref reader, out var propertyInfo))
                    {
                        reader.Skip();
                        continue;
                    }

                    state.GetCurrent().PropertyInfo = propertyInfo;
                    
                    // Start object
                    reader.ReadWithVerify();
                    
                    // Attribute type
                    reader.ReadWithVerify();

                    var propertyAttributeType = DdbJsonReader.GetDdbAttributeType(ref reader);
                    
                    // Attribute value
                    reader.ReadWithVerify();
                    
                    propertyInfo.TryReadAndSetMember(entity, ref state, ref reader, propertyAttributeType);
                    
                    // End object
                    reader.ReadWithVerify();
                    
                    // Next
                    reader.ReadWithVerify();
                }

                value = (T) entity;
                
                state.PopObject();
                
                return true;
            }
            
            throw new NotImplementedException();
        }

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
            var converterType = typeof(ObjectDdbConverter<>).MakeGenericType(typeToConvert);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata);
        }
    }
}