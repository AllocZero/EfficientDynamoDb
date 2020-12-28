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

namespace EfficientDynamoDb.Internal.Converters.Json
{
   internal sealed class JsonObjectDdbConverter<T> : DdbResumableConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;
        
        internal override DdbClassType ClassType => DdbClassType.Object;

        public JsonObjectDdbConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref T value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        internal override bool TryRead(ref DdbReader reader, out T value)
        {
            Unsafe.SkipInit(out value);
            var success = false;
            
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

                    // Attribute value
                    reader.JsonReaderValue.ReadWithVerify();

                    propertyInfo.TryReadAndSetMember(entity, ref reader, AttributeType.Unknown);
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

                    if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                    {
                        if (propertyInfo == null)
                        {
                            if (!reader.JsonReaderValue.TrySkip())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.None;
                            current.PropertyInfo = null;
                            continue;
                        }

                        if (!SingleValueReadWithReadAhead(propertyInfo!.ConverterBase.UseDirectRead, ref reader))
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.ReadValue;
                    }

                    if (!propertyInfo!.TryReadAndSetMember(entity, ref reader, AttributeType.Unknown))
                        return success = false;

                    current.PropertyState = DdbStackFramePropertyState.None;
                }

                value = (T) entity;
                return success = true;
            }
        }
    }
}