using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class DictionaryDdbConverter<TKey, TValue> : DdbResumableConverter<Dictionary<TKey, TValue>> where TKey : struct
    {
        private static readonly Type ElementTypeValue = typeof(TValue);
        
        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        public override Type? ElementType => ElementTypeValue;

        private readonly DdbConverter<TKey> _keyConverter;
        private readonly IDictionaryKeyConverter<TKey> _keyDictionaryConverter;
        private readonly DdbConverter<TValue> _valueConverter;

        public DictionaryDdbConverter(DynamoDbContextMetadata metadata)
        {
            _keyConverter = metadata.GetOrAddConverter<TKey>();
            _valueConverter = metadata.GetOrAddConverter<TValue>();
            _keyDictionaryConverter = _keyConverter as IDictionaryKeyConverter<TKey> ??
                                      throw new DdbException($"{_keyConverter.GetType().Name} must implement IDictionaryKeyConverter in order to store value as a dictionary key.");
        }

        public override Dictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                dictionary.Add(_keyConverter.Read(new AttributeValue(new StringAttributeValue(pair.Key))), _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<TKey, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairKey = pair.Key;
                var pairValue = pair.Value;
                document.Add(_keyConverter.Write(ref pairKey).GetString(), _valueConverter.Write(ref pairValue));
            }

            return document;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref Dictionary<TKey, TValue> value)
        {
            writer.WritePropertyName(attributeName);
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.Map);
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                var keyCopy = pair.Key;
                var valueCopy = pair.Value;
                
                _keyDictionaryConverter.WritePropertyName(writer, ref keyCopy);
                _valueConverter.Write(writer, ref valueCopy);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        
        internal override bool TryRead(ref DdbReader reader, out Dictionary<TKey, TValue> value)
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
                        
                            // Start object
                            reader.JsonReaderValue.ReadWithVerify();

                            // Attribute type
                            reader.JsonReaderValue.ReadWithVerify();

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();

                            entity.Add(pairKey,  _valueConverter.Read(ref reader));

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

                            var pairKey = _keyConverter.Read(ref reader);
                        
                            // Start object
                            reader.JsonReaderValue.ReadWithVerify();

                            // Attribute type
                            reader.JsonReaderValue.ReadWithVerify();

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                            // Attribute value
                            reader.JsonReaderValue.ReadWithVerify();

                            _valueConverter.TryRead(ref reader, out var pairValue);
                            entity.Add(pairKey, _valueConverter.Read(ref reader));

                            // End object
                            reader.JsonReaderValue.ReadWithVerify();
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
                            if (!SingleValueReadWithReadAhead(_valueConverter.UseDirectRead, ref reader))
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
                        
                        // End object
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

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

    internal sealed class DictionaryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || (!typeToConvert.IsClass && !typeToConvert.IsInterface))
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(Dictionary<,>);
            return isDictionary;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var exactConverterType = typeof(DictionaryDdbConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments[0], typeToConvert.GenericTypeArguments[1]);

            return (DdbConverter) Activator.CreateInstance(exactConverterType, metadata);
        }
    }
}