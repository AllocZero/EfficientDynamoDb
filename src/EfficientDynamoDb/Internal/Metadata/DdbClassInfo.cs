using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Metadata
{
    internal sealed class DdbClassInfo
    {
        public Type Type { get; }
        
        public DdbClassType ClassType { get; }
        
        public Type? ElementType { get; }
        
        public DdbClassInfo? ElementClassInfo { get; }
        
        public DdbConverter ConverterBase { get; }
        
        public Type ConverterType { get; }
        
        public Dictionary<string, DdbPropertyInfo> AttributesMap { get; }
        
        public Dictionary<string, DdbPropertyInfo> PropertiesMap { get; }
        
        public JsonReaderDictionary<DdbPropertyInfo> JsonProperties { get; }
        
        public DdbPropertyInfo[] Properties { get; }
        
        public Func<object>? Constructor { get; }
        
        public string? TableName { get; }
        
        public DdbPropertyInfo? PartitionKey { get; }
        
        public DdbPropertyInfo? SortKey { get; }
        
        public DdbPropertyInfo? Version { get; }

        public DdbClassInfo(Type type, DynamoDbContextMetadata metadata, DdbConverter converter)
        {
            Type = type;
            
            var properties = new Dictionary<string, DdbPropertyInfo>();
            var jsonProperties = new JsonReaderDictionary<DdbPropertyInfo>();
            
            ConverterBase = converter;
            ConverterType = converter.GetType();
            ClassType = ConverterBase.ClassType;

            switch (ClassType)
            {
                case DdbClassType.Object:
                {
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

                            if (properties.ContainsKey(attribute.Name))
                                continue;

                            var propertyConverter = metadata.GetOrAddConverter(propertyInfo.PropertyType, attribute.DdbConverterType);

                            var ddbPropertyInfo = propertyConverter.CreateDdbPropertyInfo(propertyInfo, attribute.Name, attribute.AttributeType, metadata);
                            properties.Add(attribute.Name, ddbPropertyInfo);
                            jsonProperties.Add(attribute.Name, ddbPropertyInfo);

                            switch (attribute.AttributeType)
                            {
                                case DynamoDbAttributeType.PartitionKey:
                                    if (PartitionKey != null)
                                        throw new DdbException($"An entity {Type.FullName} contains multiple partition key attributes");
                                    PartitionKey = ddbPropertyInfo;
                                    break;
                                case DynamoDbAttributeType.SortKey:
                                    if (SortKey != null)
                                        throw new DdbException($"An entity {Type.FullName} contains multiple sort key attributes");
                                    SortKey = ddbPropertyInfo;
                                    break;
                            }

                            if (Version == null && propertyInfo.GetCustomAttribute<DynamoDBVersionAttribute>() != null)
                                Version = ddbPropertyInfo;
                        }

                        TableName ??= currentType.GetCustomAttribute<DynamoDBTableAttribute>()?.TableName;
                    }
                    Constructor = EmitMemberAccessor.CreateConstructor(type) ?? throw new InvalidOperationException($"Can't generate constructor delegate for type '{type}'.");
                    
                    break;
                }
                case DdbClassType.Enumerable:
                case DdbClassType.Dictionary:
                {
                    ElementType = ConverterBase.ElementType;
                    ElementClassInfo = metadata.GetOrAddClassInfo(ElementType!);
                    break;
                }
            }


            AttributesMap = properties;
            PropertiesMap = properties.Values.ToDictionary(x => x.PropertyInfo.Name);
            JsonProperties = jsonProperties;
            Properties = properties.Values.ToArray();
        }
    }
}