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
   internal sealed class JsonObjectDdbConverter<T> : DdbConverter<T> where T : class
    {
        private readonly DynamoDbContextMetadata _metadata;
        
        public override DdbClassType ClassType => DdbClassType.Enumerable;

        public JsonObjectDdbConverter(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public override T Read(in AttributeValue attributeValue)
        {
            throw new NotImplementedException();
        }

        public override AttributeValue Write(ref T value)
        {
            throw new NotImplementedException();
        }

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out T value)
        {
            if (state.UseFastPath)
            {
                var classInfo = _metadata.GetOrAddClassInfo(typeof(T));
              
                // Object start
                reader.ReadWithVerify();

                var entity = classInfo.Constructor!();
                ref var current = ref state.GetCurrent();
                
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    // Property name
                    reader.ReadWithVerify();

                    if (!current.ClassInfo!.JsonProperties.TryGetValue(ref reader, out var propertyInfo))
                    {
                        reader.Skip();
                        continue;
                    }
                    
                    state.GetCurrent().PropertyInfo = propertyInfo;

                    // Property value
                    reader.ReadWithVerify();
                    
                    propertyInfo.TryReadAndSetMember(entity, ref state, ref reader, AttributeType.Unknown);
                }

                value = (T) entity;
                return true;
            }
            
            throw new NotImplementedException();
        }
    }
}