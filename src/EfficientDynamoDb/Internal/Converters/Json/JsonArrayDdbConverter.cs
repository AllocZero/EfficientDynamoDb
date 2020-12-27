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
    internal sealed class JsonArrayDdbConverter<T> : DdbConverter<T[]>
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        private readonly DdbConverter<T> _elementConverter;
        
        public override DdbClassType ClassType => DdbClassType.Enumerable;
        
        public override Type? ElementType => ElementTypeValue;

        public JsonArrayDdbConverter(DdbConverter<T> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        public override T[] Read(in AttributeValue attributeValue)
        {
            throw new NotImplementedException();
        }

        public override AttributeValue Write(ref T[] value)
        {
            throw new NotImplementedException();
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out T[] value)
        {
            if (state.UseFastPath)
            {
                ref var current = ref state.GetCurrent();

                var i = 0;
                value = new T[current.BufferLengthHint];
                
                state.PushObject();

                reader.ReadWithVerify();
                
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    _elementConverter.TryRead(ref reader, ref state, AttributeType.Unknown, out var element);
                    value[i++] = element;

                    reader.ReadWithVerify();
                }
                
                state.PopObject();
                
                return true;
            }
            
            throw new NotImplementedException();
        }
    }
}