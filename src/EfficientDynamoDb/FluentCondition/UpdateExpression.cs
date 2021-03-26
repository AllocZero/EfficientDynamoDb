using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.FluentCondition.Operators.Update;
using EfficientDynamoDb.FluentCondition.Operators.Update.AssignConcat;
using EfficientDynamoDb.FluentCondition.Operators.Update.AssignMath;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.FluentCondition
{
    // SET #a = if_not_exists(#a, :val)
    // SET #a = #a + :val


    // Update.On(x => x.A).Insert(x => x.B)

    // Joiner.Combine(Update.On(x => x.A).Assign(x => x.B), Update.On(x => x.A).Assign(x => x.B))

    // context.Update().When(condition)
    // context.Update().Property(x => x.A).Assign(x => x.B)
    //                 .On(x => x.A).AssignSum(x => x.B, x => x.C)

    // Update.On(x => x.A).Assign(x => x.B)
    //       .On(x => x.A).AssignSum(x => x.B, x => x.C)

    // Update.On(x => x.A).Assign(x => x.B)
    // Update.On(x => x.A).AssignSum(x => x.B, x => x.C)
    // Update.On(x => x.A).AssignSum(x => x.B, 5)
    // Update.On(x => x.A).AssignSubtraction(x => x.B, x => x.C)

    // Update.On(x => x.A).Set(x => x.B)
    // Update.On(x => x.A).SetSum(x => x.B)
    // Update.On(x => x.A).SetSubtraction(x => x.B)

    // Update.On(x => x.A).Append(x => x.B)
    // Update.On(x => x.A).Prepend(x => x.B)
    // Update.On(x => x.A).AssignConcat(x => x.A, x => x.B)
    // Update.On(x => x.A).AssignConcat(x => x.B, x => x.A)
    // Update.On(x => x.A).AssignConcat(x => x.B, x => x.C)

    public interface IUpdateItemBuilder<out TUpdateRequestBuilder>
    {
        internal TUpdateRequestBuilder Create(UpdateBase update, BuilderNodeType nodeType);
    }
    
    public interface IAttributeUpdate<out TUpdateItemBuilder, TEntity, TProperty> where TEntity : class where TUpdateItemBuilder : IUpdateItemBuilder<TUpdateItemBuilder>
    {
        public TUpdateItemBuilder Assign(TProperty value);
        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property);
        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property);
        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(#a, if_not_exists(#b, :fallbackVal))
        public TUpdateItemBuilder Append(TProperty value);

        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property);
        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(if_not_exists(#b, :fallbackVal), #a)
        public TUpdateItemBuilder Prepend(TProperty value);
        
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public TUpdateItemBuilder Insert(TProperty value);
        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property);
        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // ADD #a if_not_exists(#b, :fallbackVal)

        public TUpdateItemBuilder Remove(); // REMOVE #a
        // public TUpdateItemBuilder RemoveAt(int index); // REMOVE #a[0]

        public TUpdateItemBuilder Remove(TProperty value); // DELETE #set_a :val
        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property); // DELETE #set_a #b
        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // DELETE #set_a if_not_exists(#b, :fallbackVal)
    }

    internal class AttributeUpdate<TUpdateItemBuilder, TEntity, TProperty> : IAttributeUpdate<TUpdateItemBuilder, TEntity, TProperty> where TEntity : class where TUpdateItemBuilder : IUpdateItemBuilder<TUpdateItemBuilder>
    {
        private readonly TUpdateItemBuilder _requestBuilder;
        private readonly Expression<Func<TEntity, TProperty>> _expression;

        internal AttributeUpdate(TUpdateItemBuilder requestBuilder, Expression<Func<TEntity, TProperty>> expression)
        {
            _expression = expression;
            _requestBuilder = requestBuilder;
        }

        public TUpdateItemBuilder Assign(TProperty value) => _requestBuilder.Create(new UpdateAssign<TEntity, TProperty>(_expression, value), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateAssign<TEntity>(_expression, property), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateAssignFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) => _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) => _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right,
                rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property) => AssignConcat(_expression, property);

        public TUpdateItemBuilder Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(_expression, property, fallbackValue);

        public TUpdateItemBuilder Append(TProperty value) => AssignConcat(_expression, value);

        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property) => AssignConcat(property, _expression);

        public TUpdateItemBuilder Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(property, fallbackValue, _expression);

        public TUpdateItemBuilder Prepend(TProperty value) => AssignConcat(value, _expression);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributes<TEntity>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesLeftFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesRightFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignConcatAttributesFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right, rightFallbackValue),
            BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValueFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValueFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public TUpdateItemBuilder Insert(TProperty value) => _requestBuilder.Create(new UpdateInsert<TEntity, TProperty>(_expression, value), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateInsert<TEntity>(_expression, property), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateInsertFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.AddUpdate);

        public TUpdateItemBuilder Remove() => _requestBuilder.Create(new UpdateRemove<TEntity>(_expression), BuilderNodeType.RemoveUpdate);

        // public TUpdateItemBuilder RemoveAt(int index) => _requestBuilder.Create(new UpdateRemoveAt<TEntity>(_expression, index), BuilderNodeType.RemoveUpdate);

        public TUpdateItemBuilder Remove(TProperty value) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity, TProperty>(_expression, value), BuilderNodeType.DeleteUpdate);

        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity>(_expression, property), BuilderNodeType.DeleteUpdate);

        public TUpdateItemBuilder Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateRemoveFromSetFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.DeleteUpdate);
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
            return (DdbConverter<TProperty>) visitor.ClassInfo.ConverterBase;

            // var propertyName = visitor.CachedAttributeNames[^1];
            // if (!visitor.ClassInfo.PropertiesMap.TryGetValue(propertyName, out var propertyInfo))
            //     throw new DdbException(
            //         $"Property {propertyName} does not exist in entity {visitor.ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDbPropertyAttribute)} attribute");
            //
            // return ((DdbPropertyInfo<TProperty>) propertyInfo).Converter;
        }

        protected static void WriteIfNotExistsBlock<TEntity>(ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, Expression property,
            ref int valuesCount)
        {
            builder.Append("if_not_exists(");

            visitor.Visit<TEntity>(property);
            builder.Append(visitor.Builder);

            builder.Append(",:v");
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        protected void WriteAttributeValue<TEntity, TProperty>(ref NoAllocStringBuilder builder, in DdbWriter writer, ref TProperty value,
            DdbExpressionVisitor visitor, ref int valuesCount)
        {
            visitor.Visit<TEntity>(Expression);
            var converter = GetPropertyConverter<TProperty>(visitor);

            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref value);
        }
    }
}