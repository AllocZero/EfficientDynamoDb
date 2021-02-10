using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.FluentCondition.Operators.Update;
using EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignConcat;
using EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignMath;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition
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


    public interface IAttributeUpdate<TEntity, TProperty> where TEntity : class
    {
        public IUpdateRequestBuilder<TEntity> Assign(TProperty value);
        public IUpdateRequestBuilder<TEntity> Assign(Expression<Func<TEntity, TProperty>> property);
        public IUpdateRequestBuilder<TEntity> Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public IUpdateRequestBuilder<TEntity> AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> Append(Expression<Func<TEntity, TProperty>> property);
        public IUpdateRequestBuilder<TEntity> Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(#a, if_not_exists(#b, :fallbackVal))
        public IUpdateRequestBuilder<TEntity> Append(TProperty value);

        public IUpdateRequestBuilder<TEntity> Prepend(Expression<Func<TEntity, TProperty>> property);
        public IUpdateRequestBuilder<TEntity> Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // SET #a = list_append(if_not_exists(#b, :fallbackVal), #a)
        public IUpdateRequestBuilder<TEntity> Prepend(TProperty value);
        
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right);
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right);

        public IUpdateRequestBuilder<TEntity> AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right);
        public IUpdateRequestBuilder<TEntity> AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> Insert(TProperty value);
        public IUpdateRequestBuilder<TEntity> Insert(Expression<Func<TEntity, TProperty>> property);
        public IUpdateRequestBuilder<TEntity> Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // ADD #a if_not_exists(#b, :fallbackVal)

        public IUpdateRequestBuilder<TEntity> Remove(); // REMOVE #a
        public IUpdateRequestBuilder<TEntity> RemoveAt(int index); // REMOVE #a[0]

        public IUpdateRequestBuilder<TEntity> Remove(TProperty value); // DELETE #set_a :val
        public IUpdateRequestBuilder<TEntity> Remove(Expression<Func<TEntity, TProperty>> property); // DELETE #set_a #b
        public IUpdateRequestBuilder<TEntity> Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue); // DELETE #set_a if_not_exists(#b, :fallbackVal)
    }

    internal class AttributeUpdate<TEntity, TProperty> : IAttributeUpdate<TEntity, TProperty> where TEntity : class
    {
        private readonly UpdateRequestBuilder<TEntity> _requestBuilder;
        private readonly Expression<Func<TEntity, TProperty>> _expression;

        internal AttributeUpdate(UpdateRequestBuilder<TEntity> requestBuilder, Expression<Func<TEntity, TProperty>> expression)
        {
            _expression = expression;
            _requestBuilder = requestBuilder;
        }

        public IUpdateRequestBuilder<TEntity> Assign(TProperty value) => _requestBuilder.Create(new UpdateAssign<TEntity, TProperty>(_expression, value), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> Assign(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateAssign<TEntity>(_expression, property), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> Assign(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateAssignFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSum(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Plus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignAttributesMath<TEntity>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) => _requestBuilder.Create(new UpdateAssignAttributesMathLeftFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) => _requestBuilder.Create(new UpdateAssignAttributesMathRightFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignAttributesMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right,
                rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignRightValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, leftFallbackValue, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMath<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignLeftValueMathFallback<TEntity, TProperty>(_expression, AssignMathOperator.Minus, left, right, rightFallbackValue), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> Append(Expression<Func<TEntity, TProperty>> property) => AssignConcat(_expression, property);

        public IUpdateRequestBuilder<TEntity> Append(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(_expression, property, fallbackValue);

        public IUpdateRequestBuilder<TEntity> Append(TProperty value) => AssignConcat(_expression, value);

        public IUpdateRequestBuilder<TEntity> Prepend(Expression<Func<TEntity, TProperty>> property) => AssignConcat(property, _expression);

        public IUpdateRequestBuilder<TEntity> Prepend(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            AssignConcat(property, fallbackValue, _expression);

        public IUpdateRequestBuilder<TEntity> Prepend(TProperty value) => AssignConcat(value, _expression);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributes<TEntity>(_expression, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesLeftFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, Expression<Func<TEntity, TProperty>> right,
            TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatAttributesRightFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue,
            Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) => _requestBuilder.Create(
            new UpdateAssignConcatAttributesFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right, rightFallbackValue),
            BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression<Func<TEntity, TProperty>> left, TProperty leftFallbackValue, TProperty right) =>
            _requestBuilder.Create(new UpdateAssignConcatRightValueFallback<TEntity, TProperty>(_expression, left, leftFallbackValue, right),
                BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValue<TEntity, TProperty>(_expression, left, right), BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> AssignConcat(TProperty left, Expression<Func<TEntity, TProperty>> right, TProperty rightFallbackValue) =>
            _requestBuilder.Create(new UpdateAssignConcatLeftValueFallback<TEntity, TProperty>(_expression, left, right, rightFallbackValue),
                BuilderNodeType.SetUpdate);

        public IUpdateRequestBuilder<TEntity> Insert(TProperty value) => _requestBuilder.Create(new UpdateInsert<TEntity, TProperty>(_expression, value), BuilderNodeType.AddUpdate);

        public IUpdateRequestBuilder<TEntity> Insert(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateInsert<TEntity>(_expression, property), BuilderNodeType.AddUpdate);

        public IUpdateRequestBuilder<TEntity> Insert(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
            _requestBuilder.Create(new UpdateInsertFallback<TEntity, TProperty>(_expression, property, fallbackValue), BuilderNodeType.AddUpdate);

        public IUpdateRequestBuilder<TEntity> Remove() => _requestBuilder.Create(new UpdateRemove<TEntity>(_expression), BuilderNodeType.RemoveUpdate);

        public IUpdateRequestBuilder<TEntity> RemoveAt(int index) => _requestBuilder.Create(new UpdateRemoveAt<TEntity>(_expression, index), BuilderNodeType.RemoveUpdate);

        public IUpdateRequestBuilder<TEntity> Remove(TProperty value) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity, TProperty>(_expression, value), BuilderNodeType.DeleteUpdate);

        public IUpdateRequestBuilder<TEntity> Remove(Expression<Func<TEntity, TProperty>> property) =>
            _requestBuilder.Create(new UpdateRemoveFromSet<TEntity>(_expression, property), BuilderNodeType.DeleteUpdate);

        public IUpdateRequestBuilder<TEntity> Remove(Expression<Func<TEntity, TProperty>> property, TProperty fallbackValue) =>
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
            //         $"Property {propertyName} does not exist in entity {visitor.ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");
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