using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal abstract class DictionaryDdbConverterBase<TDictionary, TKey, TValue> : DdbResumableConverter<TDictionary>
    {
        private static readonly Type ElementTypeValue = typeof(TValue);

        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        internal override Type? ElementType => ElementTypeValue;

        protected readonly DdbConverter<TKey> KeyConverter;
        protected readonly IDictionaryKeyConverter<TKey> KeyDictionaryConverter;
        protected readonly DdbConverter<TValue> ValueConverter;

        protected DictionaryDdbConverterBase(DynamoDbContextMetadata metadata)
        {
            KeyConverter = metadata.GetOrAddConverter<TKey>();
            ValueConverter = metadata.GetOrAddConverter<TValue>();
            KeyDictionaryConverter = KeyConverter as IDictionaryKeyConverter<TKey> ??
                                     throw new DdbException($"{KeyConverter.GetType().Name} must implement IDictionaryKeyConverter in order to store value as a dictionary key.");
        }

        protected abstract TDictionary ToResult(Dictionary<TKey, TValue> dictionary);
        
        internal override bool TryRead(ref DdbReader reader, out TDictionary value)
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

                    if (ValueConverter.UseDirectRead)
                    {
                        while (true)
                        {
                            // Property name
                            reader.JsonReaderValue.ReadWithVerify();

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                                break;

                            var pairKey = KeyConverter.Read(ref reader);

                            // Start object
                            reader.JsonReaderValue.ReadWithVerify();

                            // Attribute type
                            reader.JsonReaderValue.ReadWithVerify();

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();
                            
                            entity.Add(pairKey, ValueConverter.Read(ref reader));

                            // End object
                            reader.JsonReaderValue.ReadWithVerify();
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

                            var pairKey = KeyConverter.Read(ref reader);

                            // Start object
                            reader.JsonReaderValue.ReadWithVerify();

                            // Attribute type
                            reader.JsonReaderValue.ReadWithVerify();

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();

                            ValueConverter.TryRead(ref reader, out var pairValue);
                            entity.Add(pairKey, pairValue);

                            // End object
                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }

                    value = ToResult(entity);

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

                            pairKey = KeyConverter.Read(ref reader);
                            current.PropertyState = DdbStackFramePropertyState.Name;
                        }
                        else
                        {
                            pairKey = (TKey) current.DictionaryKey!;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueStart)
                        {
                            if (!reader.JsonReaderValue.Read())
                            {
                                current.DictionaryKey = pairKey;
                                return success = false;
                            }

                            current.PropertyState = DdbStackFramePropertyState.ReadValueStart;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueType)
                        {
                            if (!reader.JsonReaderValue.Read())
                            {
                                current.DictionaryKey = pairKey;
                                return success = false;
                            }

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);
                            current.PropertyState = DdbStackFramePropertyState.ReadValueType;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(ValueConverter.CanSeek, ref reader))
                            {
                                current.DictionaryKey = pairKey;
                                return success = false;
                            }

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            TValue pairValue;
                            if (ValueConverter.UseDirectRead)
                            {
                                pairValue = ValueConverter.Read(ref reader);
                            }
                            else
                            {
                                if (!ValueConverter.TryRead(ref reader, out pairValue))
                                {
                                    current.DictionaryKey = pairKey;
                                    return success = false;
                                }
                            }

                            entity.Add(pairKey, pairValue);

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }

                        // End object
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.None;
                        current.DictionaryKey = null;
                        current.PropertyInfo = null;
                    }

                    value = ToResult(entity);
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