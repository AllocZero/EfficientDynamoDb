using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Mapping.Converters;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal abstract class DdbPropertyInfo
    {
        public abstract void SetValue(object obj, in AttributeValue attributeValue);
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
            var value = Converter.Read(attributeValue);

            Set!(obj, value);
        }
    }
}