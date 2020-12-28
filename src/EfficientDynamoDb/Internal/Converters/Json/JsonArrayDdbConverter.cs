using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    /// <summary>
    /// Internal converter for simple json arrays (not dynamodb arrays).
    /// </summary>
    internal sealed class JsonArrayDdbConverter<T> : DdbConverter<T[]>
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        private readonly DdbConverter<T> _elementConverter;
        
        internal override DdbClassType ClassType => DdbClassType.Enumerable;
        
        public override Type? ElementType => ElementTypeValue;

        public JsonArrayDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override T[] Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref T[] value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override T[] Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            throw new NotSupportedException("Should never be called.");
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, out T[] value)
        {
            var bufferHint = state.GetCurrent().BufferLengthHint;

            var success = false;
            state.Push();

            try
            {
                ref var current = ref state.GetCurrent();
                
                if (state.UseFastPath)
                {
                    var i = 0;
                    value = new T[bufferHint];

                    reader.ReadWithVerify();

                    if (_elementConverter.UseDirectRead)
                    {
                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            value[i++] =  _elementConverter.Read(ref reader, AttributeType.Unknown);

                            reader.ReadWithVerify();
                        }
                    }
                    else
                    {
                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            _elementConverter.TryRead(ref reader, ref state, out var element);
                            value[i++] = element;

                            reader.ReadWithVerify();
                        }
                    }
                
                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = value = new T[bufferHint];
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        value = (T[]) current.ReturnValue!;
                    }

                    if (_elementConverter.UseDirectRead)
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!SingleValueReadWithReadAhead(true, ref reader, ref state))
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;
                            
                                if (reader.TokenType == JsonTokenType.EndArray)
                                    break;
                            }
                            
                            value[current.CollectionIndex++] = _elementConverter.Read(ref reader, AttributeType.Unknown);

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!reader.Read())
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;
                            
                                if (reader.TokenType == JsonTokenType.EndArray)
                                    break;
                            }

                            if (!_elementConverter.TryRead(ref reader, ref state, out var element))
                                return success = false;
                        
                            value[current.CollectionIndex++] = element;

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
                    }

                    return success = true;
                }
            }
            finally
            {
                state.Pop(success);
            }
        }
    }
}