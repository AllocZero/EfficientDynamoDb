using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Core;
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


        protected abstract void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount);

        protected abstract void WriteAttributeValuesInternal(Utf8JsonWriter writer, ref int valuesCount);

        void IFilter.WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
            => WriteExpressionStatementInternal(ref builder, cachedNames, ref valuesCount);

        void IFilter.WriteAttributeValues(Utf8JsonWriter writer, ref int valuesCount) => WriteAttributeValuesInternal(writer, ref valuesCount);
    }
}