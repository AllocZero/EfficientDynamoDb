using System.Linq.Expressions;
using System.Text;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Core
{
    public abstract class FilterBase
    {
        internal abstract void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor);

        internal abstract void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor);

        public static FilterBase operator &(FilterBase left, FilterBase right) => Joiner.And(left, right);

        public static FilterBase operator |(FilterBase left, FilterBase right) => Joiner.Or(left, right);
    }

    internal abstract class FilterBase<TEntity> : FilterBase
    {
        protected readonly Expression Expression;

        internal FilterBase(Expression expression)
        {
            Expression = expression;
        }

        protected DdbConverter<TProperty> GetPropertyConverter<TProperty>(DdbExpressionVisitor visitor)
        {
            return (DdbConverter<TProperty>) visitor.ClassInfo.ConverterBase;
            
            // var propertyName = visitor.CachedAttributeNames[^1];
            // if (!visitor.ClassInfo.PropertiesMap.TryGetValue(propertyName, out var propertyInfo))
            //     throw new DdbException(
            //         $"Property {propertyName} does not exist in entity {visitor.ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDbPropertyAttribute)} attribute");
            //
            // return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }
        
        protected void WriteEncodedExpressionName(StringBuilder encodedExpressionName, bool useSize, ref NoAllocStringBuilder builder)
        {
            if (useSize)
            {
                builder.Append("size(");
                builder.Append(encodedExpressionName);
                builder.Append(')');
            }
            else
            {
                builder.Append(encodedExpressionName);
            }
        }
    }
}