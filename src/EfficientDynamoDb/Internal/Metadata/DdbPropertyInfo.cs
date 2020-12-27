using System;
using System.Reflection;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Metadata
{
    internal abstract class DdbPropertyInfo
    {
        public string AttributeName { get; }
        
        public abstract DdbClassInfo RuntimeClassInfo { get; }

        public abstract void SetValue(object obj, in AttributeValue attributeValue);

        public abstract void SetDocumentValue(object obj, Document document);

        public abstract void Write(object obj, Utf8JsonWriter writer);

        public abstract void WriteValue(object value, Utf8JsonWriter writer);

        protected DdbPropertyInfo(string attributeName) => AttributeName = attributeName;

        public abstract bool TryReadAndSetMember(object obj, ref DdbEntityReadStack state, ref Utf8JsonReader reader, AttributeType attributeType);
    }

    internal sealed class DdbPropertyInfo<T> : DdbPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; }
        
        public override DdbClassInfo RuntimeClassInfo { get; }
        
        public DdbConverter<T> Converter { get; }
        
        public Func<object, T> Get { get; }
        
        public Action<object, T> Set { get; }

        public DdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DdbConverter<T> converter, DynamoDbContextMetadata metadata) : base(attributeName)
        {
            PropertyInfo = propertyInfo;
            Converter = converter;

            Get = EmitMemberAccessor.CreatePropertyGetter<T>(propertyInfo);
            Set = EmitMemberAccessor.CreatePropertySetter<T>(propertyInfo);

            RuntimeClassInfo = metadata.GetOrAddClassInfo(propertyInfo.PropertyType);
        }

        public override void SetValue(object obj, in AttributeValue attributeValue)
        {
            var value = Converter.Read(in attributeValue);

            Set!(obj, value);
        }

        public override void SetDocumentValue(object obj, Document document)
        {
            var value = Get(obj);
            if (value is null)
                return;

            var attributeValue = Converter.Write(ref value);

            if (!attributeValue.IsNull)
                document.Add(AttributeName, attributeValue);
        }

        public override void Write(object obj, Utf8JsonWriter writer)
        {
            var value = Get(obj);
            if (value is null)
                return;
            
            Converter.Write(writer, AttributeName, ref value);
        }

        public override void WriteValue(object value, Utf8JsonWriter writer)
        {
            var castedValue = (T) value;
            Converter.Write(writer, AttributeName, ref castedValue);
        }

        public override bool TryReadAndSetMember(object obj, ref DdbEntityReadStack state, ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Converter.TryRead(ref reader, ref state, attributeType, out var value))
                return false;

            Set(obj, value);
            return true;
        }
    }
}