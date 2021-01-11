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
    internal sealed class ObjectDdbConverter<T> : DdbResumableConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;
        
        internal override DdbClassType ClassType => DdbClassType.Object;

        public ObjectDdbConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>(_metadata);

        internal override bool TryRead(ref DdbReader reader, out T value)
        {
            Unsafe.SkipInit(out value);
            var success = false;
            
            reader.State.Push();
            try
            {
                ref var current = ref reader.State.GetCurrent();
                object entity;
                
                if (reader.State.UseFastPath)
                {
                    entity = current.ClassInfo!.Constructor!();
                    
                    while (true)
                    {
                        // Property name
                        reader.JsonReaderValue.ReadWithVerify();

                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                            break;
                        
                        if (!current.ClassInfo!.JsonProperties.TryGetValue(ref reader.JsonReaderValue, out var propertyInfo))
                        {
                            reader.JsonReaderValue.Skip();
                            continue;
                        }

                        current.PropertyInfo = propertyInfo;

                        // Start object
                        reader.JsonReaderValue.ReadWithVerify();

                        // Attribute type
                        reader.JsonReaderValue.ReadWithVerify();

                        var attributeType = current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                        // Attribute value
                        reader.JsonReaderValue.ReadWithVerify();

                        if(attributeType != AttributeType.Null)
                            propertyInfo.TryReadAndSetMember(entity, ref reader);

                        // End object
                        reader.JsonReaderValue.ReadWithVerify();
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
                            if (!reader.JsonReaderValue.Read())
                                return success = false;
                        }
                        

                        DdbPropertyInfo? propertyInfo;
                        if (current.PropertyState < DdbStackFramePropertyState.Name)
                        {
                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                                break;
                            
                            current.ClassInfo!.JsonProperties.TryGetValue(ref reader.JsonReaderValue, out propertyInfo);
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
                                if (!reader.JsonReaderValue.TrySkip())
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.None;
                                current.PropertyInfo = null;
                                continue;
                            }

                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValueStart;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueType)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;
                            
                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);
                            current.PropertyState = DdbStackFramePropertyState.ReadValueType;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(propertyInfo!.ConverterBase.CanSeek, ref reader))
                                return success = false;
                            
                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            if (current.AttributeType != AttributeType.Null && !propertyInfo!.TryReadAndSetMember(entity, ref reader))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }
                        
                        // End object
                        if (!reader.JsonReaderValue.Read())
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
                reader.State.Pop(success);
            }
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