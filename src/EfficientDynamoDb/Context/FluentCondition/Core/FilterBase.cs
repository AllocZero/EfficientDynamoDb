using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    internal abstract class FilterBase<TEntity> : IFilter
    {
        protected readonly string PropertyName;

        internal FilterBase(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected DdbConverter<TProperty> GetPropertyConverter<TProperty>(DynamoDbContextMetadata metadata)
        {
            var entityType = typeof(TEntity);
            var classInfo = metadata.GetOrAddClassInfo(entityType);

            if (!classInfo.PropertiesMap.TryGetValue(PropertyName, out var propertyInfo))
                throw new InvalidOperationException(
                    $"Property {PropertyName} is not exist in entity {entityType.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");

            return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }


        protected abstract void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount);

        protected abstract void WriteAttributeValuesInternal(Utf8JsonWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount);

        void IFilter.WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
            => WriteExpressionStatementInternal(ref builder, cachedNames, ref valuesCount);

        void IFilter.WriteAttributeValues(Utf8JsonWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount) =>
            WriteAttributeValuesInternal(writer, metadata, ref valuesCount);
    }
}