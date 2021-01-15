using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public abstract class FilterBase
    {
        // TODO: Get rid of hashset
        internal abstract void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount);

        internal abstract void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount);

        public static FilterBase operator &(FilterBase left, FilterBase right) => Joiner.And(left, right);

        public static FilterBase operator |(FilterBase left, FilterBase right) => Joiner.Or(left, right);
    }

    internal abstract class FilterBase<TEntity> : FilterBase
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
                    $"Property {PropertyName} does not exist in entity {entityType.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");

            return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }
    }
}