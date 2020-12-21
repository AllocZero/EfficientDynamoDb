using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Collections;
using EfficientDynamoDb.Internal.Converters.Collections.Dictionaries;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContextMetadata
    {
        private static readonly DdbConverter[] InternalConverters =
            {new ArrayDdbConverterFactory(), new ListDdbConverterFactory(), new DictionaryDdbConverterFactory()};
        
        private readonly IReadOnlyCollection<DdbConverter> _converters;
        private readonly ConcurrentDictionary<Type, DdbConverter> _factoryConvertersCache = new ConcurrentDictionary<Type, DdbConverter>();
        private readonly ConcurrentDictionary<Type, DdbClassInfo> _classInfoCache = new ConcurrentDictionary<Type, DdbClassInfo>();
        
        public DynamoDbContextMetadata(IReadOnlyCollection<DdbConverter> converters)
        {
            _converters = converters;
        }
        
        public DdbConverter GetOrAddConverter(Type propertyType, Type? converterType)
        {
            DdbConverter? converter = null;
            
            if (converterType != null)
            {
                converter = DefaultDdbConverterFactory.Create(converterType);
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

            return converter;
        }
        
        internal DdbClassInfo GetOrAddClassInfo(Type classType) => _classInfoCache.GetOrAdd(classType, (x, metadata) => new DdbClassInfo(x, metadata), this);
        
        private DdbConverter GetOrAddNestedObjectConverter(Type propertyType)
        {
            var converterType = typeof(NestedObjectConverter<>).MakeGenericType(propertyType);

            return _factoryConvertersCache.GetOrAdd(propertyType, (x, metadata) => (DdbConverter) Activator.CreateInstance(converterType, metadata), this);
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
    }
}