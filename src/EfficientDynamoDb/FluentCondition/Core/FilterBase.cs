using System;
using System.Linq;
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

        public static FilterBase operator !(FilterBase operand) => Joiner.Not(operand);
    }

    internal abstract class FilterBase<TEntity> : FilterBase
    {
        protected readonly Expression Expression;

        internal FilterBase(Expression expression)
        {
            Expression = expression;
        }

        protected DdbConverter<TProperty> GetPropertyConverter<TProperty>(DdbExpressionVisitor visitor, bool useSize)
        {
            if (useSize)
                return visitor.Metadata.GetOrAddConverter<TProperty>();
            
            if (visitor.ClassInfo.ConverterBase is DdbConverter<TProperty> converter)
                return converter;

            throw BuildCastException<TProperty>(visitor.ClassInfo.ConverterBase.Type);
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
        
         private static InvalidCastException BuildCastException<TProperty>(Type propertyType)
        {
            var conditionTypeName = GetFriendlyTypeName(typeof(TProperty));
            var propertyTypeName = GetFriendlyTypeName(propertyType);

            return new InvalidCastException(
                $"""Cannot cast type "{conditionTypeName}" provided in condition to property type "{propertyTypeName}". Consider casting "{conditionTypeName}" to "{propertyTypeName}" manually in filter expression."""
            );
        }
        
        private static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var typeNameSpan = type.Name.AsSpan();
            var backtickIndex = typeNameSpan.IndexOf('`');
            var genericTypeName = backtickIndex == -1 ? typeNameSpan : typeNameSpan[..backtickIndex];
            
            var genericArguments = type.GetGenericArguments();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"Nullable<{GetFriendlyTypeName(genericArguments[0])}>";
            }
            
            var argumentNames = string.Join(", ", genericArguments.Select(GetFriendlyTypeName));
            return $"{genericTypeName}<{argumentNames}>";
        }
    }
}