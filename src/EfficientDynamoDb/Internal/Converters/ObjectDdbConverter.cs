using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters
{
    internal sealed class ObjectDdbConverter<T> : DdbConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;
        
        internal override DdbClassType ClassType => DdbClassType.Object;

        public ObjectDdbConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>(_metadata);

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, out T value)
        {
            Unsafe.SkipInit(out value);
            var success = false;
            
            state.Push();
            try
            {
                ref var current = ref state.GetCurrent();
                object entity;
                if (state.UseFastPath)
                {
                    entity = current.ClassInfo!.Constructor!();
                    
                    while (true)
                    {
                        // Property name
                        reader.ReadWithVerify();

                        if (reader.TokenType == JsonTokenType.EndObject)
                            break;
                        
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

                        var attributeType = current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader);

                        // Attribute value
                        reader.ReadWithVerify();

                        propertyInfo.TryReadAndSetMember(entity, ref state, ref reader, attributeType);

                        // End object
                        reader.ReadWithVerify();
                    }

                    value = (T) entity;

                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = entity = current.ClassInfo!.Constructor!();
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        entity = current.ReturnValue!;
                    }

                    while (true)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadName)
                        {
                            // Property name
                            if (!reader.Read())
                                return success = false;
                        }
                        

                        DdbPropertyInfo? propertyInfo;
                        if (current.PropertyState < DdbStackFramePropertyState.Name)
                        {
                            if (reader.TokenType == JsonTokenType.EndObject)
                                break;
                            
                            current.ClassInfo!.JsonProperties.TryGetValue(ref reader, out propertyInfo);
                            current.PropertyInfo = propertyInfo;
                            current.PropertyState = DdbStackFramePropertyState.Name;
                        }
                        else
                        {
                            propertyInfo = current.PropertyInfo;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueStart)
                        {
                            if (propertyInfo == null)
                            {
                                if (!reader.TrySkip())
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.None;
                                current.PropertyInfo = null;
                                continue;
                            }

                            if (!reader.Read())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValueStart;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueType)
                        {
                            if (!reader.Read())
                                return success = false;
                            
                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader);
                            current.PropertyState = DdbStackFramePropertyState.ReadValueType;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(propertyInfo!.ConverterBase.UseDirectRead, ref reader, ref state))
                                return success = false;
                            
                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            if (!propertyInfo!.TryReadAndSetMember(entity, ref state, ref reader, current.AttributeType))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }
                        
                        // End object
                        if (!reader.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.None;
                        current.PropertyInfo = null;
                    }

                    value = (T) entity;
                    return success = true;
                }
            }
            finally
            {
                state.Pop(success);
            }
        }

        public override AttributeValue Write(ref T value) => new MapAttributeValue(value.ToDocument(_metadata));

        public override void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref T value) => WriteInlined(writer, ref value);

        public override T Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            throw new NotSupportedException("Should never be called.");
        }

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