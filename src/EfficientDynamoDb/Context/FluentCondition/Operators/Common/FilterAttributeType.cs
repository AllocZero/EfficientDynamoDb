using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterAttributeType<TEntity> : FilterBase<TEntity>
    {
        private readonly AttributeType _type;

        public FilterAttributeType(Expression expression, AttributeType type) : base(expression) => _type = type;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // attribute_type(#a,:v0)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("attribute_type(");
            builder.Append(visitor.Builder);
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.JsonWriter.WriteString(builder.GetBuffer(), _type.ToDdbTypeName());
        }
    }
}