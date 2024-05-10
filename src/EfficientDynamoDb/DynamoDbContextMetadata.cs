using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Converters.Collections;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Collections;
using EfficientDynamoDb.Internal.Converters.Documents;
using EfficientDynamoDb.Internal.Converters.Primitives;
using EfficientDynamoDb.Internal.Converters.Primitives.Binary;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb
{
    public class DynamoDbContextMetadata
    {
        private static readonly DdbConverter[] InternalConverters =
        {
            new ListDdbConverterFactory(),
            new DictionaryDdbConverterFactory(), new IDictionaryDdbConverterFactory(), new IReadOnlyDictionaryDdbConverterFactory(),
            new NumberSetDdbConverterFactory(), new NumberISetDdbConverterFactory(), new StringSetDdbConverterFactory(), new StringISetDdbConverterFactory(),
            new IReadOnlyCollectionDdbConverterFactory(), new IReadOnlyListDdbConverterFactory(), new IListDdbConverterFactory(),
            new DocumentDdbConverterFactory(), new AttributeValueDdbConverterFactory(), new BinaryDdbConverterFactory(), new ArrayDdbConverterFactory(),
            new BinaryToMemoryDdbConverterFactory(), new BinaryToReadOnlyMemoryDdbConverterFactory(),
        };
        
        private readonly IReadOnlyCollection<DdbConverter> _converters;
        private readonly ConcurrentDictionary<Type, DdbConverter> _factoryConvertersCache = new ConcurrentDictionary<Type, DdbConverter>();
        private readonly ConcurrentDictionary<(Type PropertyType, Type? ConverterType), DdbConverter> _cache =
            new ConcurrentDictionary<(Type PropertyType, Type? ConverterType), DdbConverter>();
        
        // Cache class info per class-converter pair, because class info can be different when non-default converter is applied to the property
        private readonly ConcurrentDictionary<(Type ClassType, Type? ConverterType), DdbClassInfo> _classInfoCache = new ConcurrentDictionary<(Type ClassType, Type? ConverterType), DdbClassInfo>();
        
        public DynamoDbContextMetadata(IReadOnlyCollection<DdbConverter> converters)
        {
            _converters = converters;

            foreach (var converter in converters)
            {
                var converterType = converter.GetType();
                if (converterType.IsSubclassOf(typeof(DdbConverterFactory)))
                    continue;

                // Cache converter instance so it is not instantiated twice
                _factoryConvertersCache.TryAdd(converterType, converter);
            }
        }

        public DdbConverter<T> GetOrAddConverter<T>() => (DdbConverter<T>) GetOrAddConverter(typeof(T), null);

        public DdbConverter GetOrAddConverter(Type propertyType) => GetOrAddConverter(propertyType, null);
        
        public DdbConverter GetOrAddConverter(Type propertyType, Type? converterType) => _cache.GetOrAdd((propertyType, converterType), x => GetOrAddCachedConverter(x.PropertyType, x.ConverterType));

        internal DdbClassInfo GetOrAddClassInfo<T>() where T : class => GetOrAddClassInfo(typeof(T));
        
        internal DdbClassInfo GetOrAddClassInfo(Type classType)
        {
            return _classInfoCache.GetOrAdd((classType, null), (x, metadata) =>
            {
                var converterType = x.ClassType.GetCustomAttribute<DynamoDbConverterAttribute>(true)?.ConverterType;
                var converter = metadata.GetOrAddConverter(x.ClassType, converterType);

                return GetOrAddClassInfo(x.ClassType, converter.GetType());
            }, this);
        }

        internal DdbClassInfo GetOrAddClassInfo(Type classType, Type converterType)
        {
            return _classInfoCache.GetOrAdd((classType, converterType), (x, metadata) =>
            {
                var converter = metadata.GetOrAddConverter(x.ClassType, x.ConverterType);
                return new DdbClassInfo(x.ClassType, metadata, converter);
            }, this);
        }
        
        internal DdbClassInfo GetOrAddClassInfo(Type classType, DdbConverter converter)
        {
            var converterType = converter.GetType();
            
            return _classInfoCache.GetOrAdd((classType, converterType), (x, args) => new DdbClassInfo(x.ClassType, args.Metadata, args.Converter), (Metadata: this, Converter: converter));
        }
        
        private DdbConverter GetOrAddCachedConverter(Type propertyType, Type? converterType)
        {
            converterType ??= propertyType.GetCustomAttribute<DynamoDbConverterAttribute>(true)?.ConverterType;

            DdbConverter? converter = null;
            
            if (converterType != null)
            {
                converter = GetOrAddKnownConverter(propertyType, converterType);
            }
            else
            {
                converter = FindConverter(_converters, this, propertyType);
                converter ??= FindConverter(InternalConverters, this, propertyType);
                converter ??= DefaultDdbConverterFactory.CreateFromType(propertyType);
                
                // Check nested object converter in the end to make sure it does not override other converters
                if (converter == null && propertyType.IsClass)
                    converter = GetOrAddNestedObjectConverter(propertyType);

                if (converter == null)
                    throw new DdbException($"Type '{propertyType.Name}' requires an explicit ddb converter.");
            }

            if (!propertyType.IsValueType)
                return converter;
            
            var type = Nullable.GetUnderlyingType(propertyType);
            if (type is null)
                return converter;
            
            if (converter.Type == propertyType)
                return converter;
            
            var nullableConverterType = typeof(NullableValueTypeDdbConverter<>).MakeGenericType(type);
            return (DdbConverter) Activator.CreateInstance(nullableConverterType, converter);
        }

        private DdbConverter GetOrAddNestedObjectConverter(Type propertyType)
        {
            var converterType = typeof(ObjectDdbConverter<>).MakeGenericType(propertyType);

            return _factoryConvertersCache.GetOrAdd(propertyType, (x, metadata) => (DdbConverter) Activator.CreateInstance(converterType, metadata)!, this)!;
        }

        private DdbConverter? FindConverter(IReadOnlyCollection<DdbConverter> converters, DynamoDbContextMetadata metadata, Type propertyType)
        {
            foreach (var customConverter in converters)
            {
                if (!customConverter.CanConvert(propertyType)) 
                    continue;

                return customConverter is DdbConverterFactory factory
                    ? _factoryConvertersCache.GetOrAdd(propertyType, (x, data) => data.factory.CreateConverter(x, data.metadata), (factory, metadata))
                    : customConverter;
            }

            return null;
        }

        private DdbConverter GetOrAddKnownConverter(Type propertyType, Type converterType)
        {
            if (!converterType.IsGenericTypeDefinition)
                return _factoryConvertersCache.GetOrAdd(converterType, CreateConverter);

            var arguments = propertyType.IsArray ? new [] {propertyType.GetElementType()!} : propertyType.GenericTypeArguments;
            var fullConverterType = converterType.MakeGenericType(arguments);

            return _factoryConvertersCache.GetOrAdd(fullConverterType, CreateConverter);
        }
        
        private DdbConverter CreateConverter(Type converterType)
        {
            var constructor = converterType.GetConstructors()[0];
            var constructorParams = constructor.GetParameters();

            if (constructorParams.Length == 0) 
                return (DdbConverter) Activator.CreateInstance(converterType)!;

            var parameters = new object[constructorParams.Length];
            for (var i = 0; i < constructorParams.Length; i++)
            {
                var parameter = constructorParams[i];

                if (parameter.ParameterType == typeof(DynamoDbContextMetadata))
                {
                    parameters[i] = this;
                }
                else
                {
                    if (!parameter.ParameterType.IsSubclassOf(typeof(DdbConverter))) 
                        throw new DdbException("Can't create converter that contains non converter constructor parameters.");

                    parameters[i] = GetOrAddConverter(parameter.ParameterType.GenericTypeArguments[0], null);
                }
            }

            return (DdbConverter) Activator.CreateInstance(converterType, parameters)!;
        }
    }
}