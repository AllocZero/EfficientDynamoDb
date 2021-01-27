using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
    internal sealed class JsonListDdbConverter<T> : DdbResumableConverter<List<T>>
    {
        private static readonly Type ElementTypeValue = typeof(T);

        private readonly DdbConverter<T> _elementConverter;

        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        public JsonListDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override List<T> Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref List<T> value)
        {
            throw new NotSupportedException("Should never be called.");
        }

        internal override bool TryRead(ref DdbReader reader, out List<T> value)
        {
            var bufferHint = reader.State.GetCurrent().BufferLengthHint;

            var success = false;
            reader.State.Push();

            try
            {
                ref var current = ref reader.State.GetCurrent();
                List<T> list;

                if (reader.State.UseFastPath)
                {
                    value = list = new List<T>(bufferHint);

                    reader.JsonReaderValue.ReadWithVerify();

                    if (_elementConverter.UseDirectRead)
                    {
                        while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                        {
                            list.Add(_elementConverter.Read(ref reader));

                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }
                    else
                    {
                        while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                        {
                            _elementConverter.TryRead(ref reader, out var element);
                            list.Add(element);

                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }

                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = value = list = new List<T>(bufferHint);
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        value = list = (List<T>) current.ReturnValue!;
                    }

                    if (_elementConverter.UseDirectRead)
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!SingleValueReadWithReadAhead(_elementConverter.CanSeek, ref reader))
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;

                                if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                    break;
                            }

                            list[current.CollectionIndex++] = _elementConverter.Read(ref reader);

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!reader.JsonReaderValue.Read())
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;

                                if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                    break;
                            }

                            if (!_elementConverter.TryRead(ref reader, out var element))
                                return success = false;

                            list.Add(element);

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
                    }

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
    }
}