using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.FluentCondition
{
    public class UpdateExpression<TEntity>
    {
        private readonly Expression _expression;

        public UpdateExpression(Expression expression)
        {
            _expression = expression;
        }
        
        
    }
    
    internal abstract class UpdateBase
    {
        protected readonly Expression Expression;

        public UpdateBase(Expression expression)
        {
            Expression = expression;
        }
        
        // TODO: Get rid of hashset
        internal abstract void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor);

        internal abstract void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor);

        // public static UpdateBase operator &(UpdateBase left, UpdateBase right) => Joiner.And(left, right);

        // public static UpdateBase operator |(UpdateBase left, UpdateBase right) => Joiner.Or(left, right);
        
        protected DdbConverter<TProperty> GetPropertyConverter<TProperty>(DdbExpressionVisitor visitor)
        {
            var propertyName = visitor.CachedAttributeNames[^1];
            if (!visitor.ClassInfo.PropertiesMap.TryGetValue(propertyName, out var propertyInfo))
                throw new DdbException(
                    $"Property {propertyName} does not exist in entity {visitor.ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");

            return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }
    }
    
    internal class UpdateSet<TEntity, TProperty> : UpdateBase
    {
        private TProperty _value;

        public UpdateSet(Expression expression, TProperty value) : base(expression)
        {
            _value = value;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "SET #a = :v0"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" = :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
    
    internal class UpdateIncrement<TEntity, TProperty> : UpdateBase
    {
        private TProperty _value;

        public UpdateIncrement(Expression expression, TProperty value) : base(expression)
        {
            _value = value;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "SET #a = #a + :v0"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            var encodedExpressionName = visitor.GetEncodedExpressionName();
            builder.Append(encodedExpressionName);
            builder.Append(" = #");
            builder.Append(encodedExpressionName);
            builder.Append(" + :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
    
    internal class UpdateDecrement<TEntity, TProperty> : UpdateBase
    {
        private TProperty _value;

        public UpdateDecrement(Expression expression, TProperty value) : base(expression)
        {
            _value = value;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "SET #a = #a - :v0"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            var encodedExpressionName = visitor.GetEncodedExpressionName();
            builder.Append(encodedExpressionName);
            builder.Append(" = #");
            builder.Append(encodedExpressionName);
            builder.Append(" - :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
}