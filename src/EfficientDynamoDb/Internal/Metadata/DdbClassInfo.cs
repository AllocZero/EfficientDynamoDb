using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
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
        
        public Dictionary<string, DdbPropertyInfo> PropertiesMap { get; }
        
        public JsonReaderDictionary<DdbPropertyInfo> JsonProperties { get; }
        
        public DdbPropertyInfo[] Properties { get; }
        
        public Func<object>? Constructor { get; }
        
        public string? TableName { get; }
        
        public DdbPropertyInfo? PartitionKey { get; }
        
        public DdbPropertyInfo? SortKey { get; }

        public DdbClassInfo(Type type, DynamoDbContextMetadata metadata, Type? converterType = null)
        {
            Type = type;
            
            var properties = new Dictionary<string, DdbPropertyInfo>();
            var jsonProperties = new JsonReaderDictionary<DdbPropertyInfo>();
            
            ConverterBase = metadata.GetOrAddConverter(type, converterType);
            ClassType = ConverterBase.ClassType;

            switch (ClassType)
            {
                case DdbClassType.Object:
                {
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

                            if (properties.ContainsKey(attribute.Name))
                                continue;

                            var converter = metadata.GetOrAddConverter(propertyInfo.PropertyType, attribute.DdbConverterType);

                            var ddbPropertyInfo = converter.CreateDdbPropertyInfo(propertyInfo, attribute.Name, metadata);
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
                        }

                        TableName ??= currentType.GetCustomAttribute<DynamoDBTableAttribute>()?.TableName;
                    }
                    Constructor = EmitMemberAccessor.CreateConstructor(type) ?? throw new InvalidOperationException($"Can't generate constructor delegate for type '{type}'.");
                    
                    break;
                }
                case DdbClassType.Enumerable:
                {
                    ElementType = ConverterBase.ElementType;
                    ElementClassInfo = metadata.GetOrAddClassInfo(ElementType!);
                    break;
                }
            }


            PropertiesMap = properties;
            JsonProperties = jsonProperties;
            Properties = properties.Values.ToArray();
        }
    }
}