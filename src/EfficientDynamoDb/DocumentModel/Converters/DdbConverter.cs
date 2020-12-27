using System;
using System.Reflection;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public abstract class DdbConverter
    {
        internal abstract DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata dynamoDbContextMetadata);

        public abstract bool CanConvert(Type typeToConvert);
        
        public abstract DdbClassType ClassType { get; }
        
        public abstract Type? ElementType { get; }
    }
    
    public abstract class DdbConverter<T> : DdbConverter
    {
        public override DdbClassType ClassType => DdbClassType.Value;

        public override Type? ElementType => null;

        public abstract T Read(in AttributeValue attributeValue);

        public abstract AttributeValue Write(ref T value);

        // TODO: Make abstract
        internal virtual bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType attributeType, out T value)
        {
            throw new NotImplementedException();
        }

        public virtual void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            var attributeValue = Write(ref value);
            if (attributeValue.IsNull)
                return;
            
            writer.WritePropertyName(attributeName);
            attributeValue.Write(writer);
        }

        public virtual void Write(Utf8JsonWriter writer, ref T value)
        {
            var attributeValue = Write(ref value);
            attributeValue.Write(writer);
        }

        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata metadata) => new DdbPropertyInfo<T>(propertyInfo, attributeName, this, metadata);
        
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);
    }
}