using System;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    internal abstract class FilterBase<TEntity> : IFilter
    {
        private readonly string _propertyName;

        internal FilterBase(string propertyName)
        {
            _propertyName = propertyName;
        }
        
        protected DdbPropertyInfo GetPropertyInfo(DynamoDbContextMetadata metadata)
        {
            var entityType = typeof(TEntity);
            var classInfo = metadata.GetOrAddClassInfo(entityType);

            if (!classInfo.PropertiesMap.TryGetValue(_propertyName, out var propertyInfo))
                throw new InvalidOperationException(
                    $"Property {_propertyName} is not exist in entity {entityType.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");

            return propertyInfo;
        }
    }
}