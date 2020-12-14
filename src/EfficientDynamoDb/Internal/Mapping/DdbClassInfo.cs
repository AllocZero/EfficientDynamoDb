using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EfficientDynamoDb.Internal.Mapping.Attributes;
using EfficientDynamoDb.Internal.Mapping.Converters;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal sealed class DdbClassInfo
    {
        public Type Type { get; }
        
        public Dictionary<string, DdbPropertyInfo> PropertiesMap { get; }
        
        public DdbPropertyInfo[] Properties { get; }
        
        public Func<object> Constructor { get; }

        public DdbClassInfo(Type type)
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

                    // TODO: Handle missing converter case
                    // TODO: Cache converters
                    var converter = attribute.DdbConverterType != null
                        ? (DdbConverter) Activator.CreateInstance(attribute.DdbConverterType)
                        : DefaultDdbConverterFactory.Create(propertyInfo.PropertyType);
                    
                    properties.Add(attribute.Name, converter.CreateDdbPropertyInfo(propertyInfo, attribute.Name));
                }
            }

            PropertiesMap = properties;
            Properties = properties.Values.ToArray();
        }
    }
}