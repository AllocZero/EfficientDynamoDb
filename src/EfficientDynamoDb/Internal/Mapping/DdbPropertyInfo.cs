using System;
using System.Reflection;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Mapping.Converters;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal abstract class DdbPropertyInfo
    {
        public abstract void SetValue(object obj, in AttributeValue attributeValue);

        public abstract void SetDocumentValue(object obj, Document document);

        public abstract void Write(object obj, Utf8JsonWriter writer);
    }
    
    internal sealed class DdbPropertyInfo<T> : DdbPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; }
        
        public string AttributeName { get; }
        
        public DdbConverter<T> Converter { get; }
        
        public Func<object, T> Get { get; }
        
        public Action<object, T> Set { get; }

        public DdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DdbConverter<T> converter)
        {
            PropertyInfo = propertyInfo;
            AttributeName = attributeName;
            Converter = converter;

            Get = EmitMemberAccessor.CreatePropertyGetter<T>(propertyInfo);
            Set = EmitMemberAccessor.CreatePropertySetter<T>(propertyInfo);
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
    }
}