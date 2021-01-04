using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
   internal sealed class JsonIReadOnlyDictionaryDdbConverter<TKey, TValue> : DdbResumableConverter<IReadOnlyDictionary<TKey, TValue>>
    {
        private static readonly Type ElementTypeValue = typeof(TValue);

        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        public override Type? ElementType => ElementTypeValue;

        private readonly DdbConverter<TKey> _keyConverter;
        private readonly IDictionaryKeyConverter<TKey> _keyDictionaryConverter;
        private readonly DdbConverter<TValue> _valueConverter;
        
        public JsonIReadOnlyDictionaryDdbConverter(DynamoDbContextMetadata metadata)
        {
            _keyConverter = metadata.GetOrAddConverter<TKey>();
            _valueConverter = metadata.GetOrAddConverter<TValue>();
            _keyDictionaryConverter = _keyConverter as IDictionaryKeyConverter<TKey> ??
                                      throw new DdbException($"{_keyConverter.GetType().Name} must implement IDictionaryKeyConverter in order to store value as a dictionary key.");
        }

        public override IReadOnlyDictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            throw new NotSupportedException("Should never be called.");
        }

        public override AttributeValue Write(ref IReadOnlyDictionary<TKey, TValue> value)
        {
            throw new NotSupportedException("Should never be called.");
        }
        
         internal override bool TryRead(ref DdbReader reader, out IReadOnlyDictionary<TKey, TValue> value)
        {
            Unsafe.SkipInit(out value);
            var success = false;

            reader.State.Push();
            try
            {
                ref var current = ref reader.State.GetCurrent();
                Dictionary<TKey, TValue> entity;

                if (reader.State.UseFastPath)
                {
                    entity = new Dictionary<TKey, TValue>();

                    if (_valueConverter.UseDirectRead)
                    {
                        while (true)
                        {
                            // Property name
                            reader.JsonReaderValue.ReadWithVerify();

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                                break;

                            var pairKey = _keyConverter.Read(ref reader);
                            
                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();

                            entity.Add(pairKey, _valueConverter.Read(ref reader));
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            // Property name
                            reader.JsonReaderValue.ReadWithVerify();

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                                break;

                            var pairKey = _keyConverter.Read(ref reader);
                            
                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();

                            _valueConverter.TryRead(ref reader, out var pairValue);
                            entity.Add(pairKey, pairValue);
                        }
                    }

                    value = entity;

                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = entity = new Dictionary<TKey, TValue>();
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        entity = (Dictionary<TKey, TValue>) current.ReturnValue!;
                    }

                    while (true)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadName)
                        {
                            // Property name
                            if (!reader.JsonReaderValue.Read())
                                return success = false;
                        }

                        TKey pairKey;
                        if (current.PropertyState < DdbStackFramePropertyState.Name)
                        {
                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                                break;

                            pairKey = _keyConverter.Read(ref reader);
                            current.PropertyState = DdbStackFramePropertyState.Name;
                        }
                        else
                        {
                            pairKey = (TKey) current.DictionaryKey!;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(_valueConverter.CanSeek, ref reader))
                            {
                                current.DictionaryKey = pairKey;
                                return success = false;
                            }

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            TValue pairValue;
                            if (_valueConverter.UseDirectRead)
                            {
                                pairValue = _valueConverter.Read(ref reader);
                            }
                            else
                            {
                                if (!_valueConverter.TryRead(ref reader, out pairValue))
                                {
                                    current.DictionaryKey = pairKey;
                                    return success = false;
                                }
                            }

                            entity.Add(pairKey, pairValue);

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }

                        current.PropertyState = DdbStackFramePropertyState.None;
                        current.DictionaryKey = null;
                        current.PropertyInfo = null;
                    }

                    value = entity;
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