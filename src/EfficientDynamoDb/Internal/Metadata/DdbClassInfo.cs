using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;

namespace EfficientDynamoDb.Internal.Metadata
{
    internal sealed class DdbClassInfo
    {
        public Type Type { get; }
        
        public Dictionary<string, DdbPropertyInfo> PropertiesMap { get; }
        
        public DdbPropertyInfo[] Properties { get; }
        
        public Func<object> Constructor { get; }
        
        public string? TableName { get; }

        public DdbClassInfo(Type type, DynamoDbContextMetadata metadata)
        {
            Type = type;
            Constructor = EmitMemberAccessor.CreateConstructor(type) ?? throw new InvalidOperationException($"Can't generate constructor delegate for type '{type}'.");

            var properties = new Dictionary<string, DdbPropertyInfo>();
            
            // TODO: Handle property overrides
            for (var currentType = type; currentType != null; currentType = currentType.BaseType)
            {
                const BindingFlags bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly;

                foreach (PropertyInfo propertyInfo in currentType.GetProperties(bindingFlags))
                {
                    var attribute = propertyInfo.GetCustomAttribute<DynamoDBPropertyAttribute>();
                    if (attribute == null)
                        continue;
                    
                    if(properties.ContainsKey(attribute.Name))
                        continue;
                    
                    var converter = metadata.GetOrAddConverter(propertyInfo.PropertyType, attribute.DdbConverterType);

                    properties.Add(attribute.Name, converter.CreateDdbPropertyInfo(propertyInfo, attribute.Name));
                }

                TableName ??= currentType.GetCustomAttribute<DynamoDBTableAttribute>()?.TableName;
            }

            PropertiesMap = properties;
            Properties = properties.Values.ToArray();
        }
    }
}