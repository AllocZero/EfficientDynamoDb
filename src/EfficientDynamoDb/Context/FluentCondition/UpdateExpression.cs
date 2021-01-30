using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.FluentCondition.Operators.Update;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;

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
    

    internal interface IAttributeUpdate
    {
        public UpdateBase Assign<T>(T value);
        public UpdateBase Assign(Expression property);
        public UpdateBase Assign<T>(Expression property, T fallbackValue);

        public UpdateBase AssignSum(Expression left, Expression right);
        public UpdateBase AssignSum<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignSum<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public UpdateBase AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public UpdateBase AssignSum<TRight>(Expression left, TRight right);
        public UpdateBase AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public UpdateBase AssignSum<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public UpdateBase AssignSum<TLeft>(TLeft left, Expression right);
        public UpdateBase AssignSum<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignSum<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);
        
        public UpdateBase AssignSubtraction(Expression left, Expression right);
        public UpdateBase AssignSubtraction<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignSubtraction<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public UpdateBase AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public UpdateBase AssignSubtraction<TRight>(Expression left, TRight right);
        public UpdateBase AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public UpdateBase AssignSubtraction<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public UpdateBase AssignSubtraction<TLeft>(TLeft left, Expression right);
        public UpdateBase AssignSubtraction<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignSubtraction<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);

        public UpdateBase Append(Expression property);
        public UpdateBase Append<T>(Expression property, T fallbackValue); // SET #a = list_append(#a, if_not_exists(#b, :fallbackVal))
        public UpdateBase Append<T>(T value);
        
        public UpdateBase Prepend(Expression property);
        public UpdateBase Prepend<T>(Expression property, T fallbackValue); // SET #a = list_append(if_not_exists(#b, :fallbackVal), #a)
        public UpdateBase Prepend<T>(T value);
        
        public UpdateBase AssignConcat(Expression left, Expression right);
        public UpdateBase AssignConcat<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignConcat<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public UpdateBase AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public UpdateBase AssignConcat<TRight>(Expression left, TRight right);
        public UpdateBase AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public UpdateBase AssignConcat<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public UpdateBase AssignConcat<TLeft>(TLeft left, Expression right);
        public UpdateBase AssignConcat<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public UpdateBase AssignConcat<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);

        public UpdateBase Insert<T>(T value);
        public UpdateBase Insert(Expression property);
        public UpdateBase Insert<T>(Expression property, T fallbackValue); // ADD #a if_not_exists(#b, :fallbackVal)

        public UpdateBase Remove(); // REMOVE #a
        
        public UpdateBase RemoveAt(int index); // REMOVE #a[0]
        
        public UpdateBase Remove<T>(T value); // DELETE #set_a :val
        public UpdateBase Remove(Expression property); // DELETE #set_a #b
        public UpdateBase Remove<T>(Expression property, T fallbackValue); // DELETE #set_a if_not_exists(#b, :fallbackVal)
    }

    internal class AttributeUpdate<TEntity> : IAttributeUpdate
    {
        private readonly Expression _expression;

        internal AttributeUpdate(Expression expression)
        {
            _expression = expression;
        }

        public UpdateBase Assign<T>(T value) => new UpdateAssign<TEntity, T>(_expression, value);

        public UpdateBase Assign(Expression property) => new UpdateAssign<TEntity>(_expression, property);

        public UpdateBase Assign<T>(Expression property, T fallbackValue) => new UpdateAssignFallback<TEntity, T>(_expression, property, fallbackValue);

        public UpdateBase AssignSum(Expression left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSum<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction(Expression left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignSubtraction<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase Append(Expression property) => throw new System.NotImplementedException();

        public UpdateBase Append<T>(Expression property, T fallbackValue) => throw new System.NotImplementedException();

        public UpdateBase Append<T>(T value) => throw new System.NotImplementedException();

        public UpdateBase Prepend(Expression property) => throw new System.NotImplementedException();

        public UpdateBase Prepend<T>(Expression property, T fallbackValue) => throw new System.NotImplementedException();

        public UpdateBase Prepend<T>(T value) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat(Expression left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public UpdateBase AssignConcat<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public UpdateBase Insert<T>(T value) => new UpdateInsert<TEntity, T>(_expression, value);

        public UpdateBase Insert(Expression property) => new UpdateInsert<TEntity>(_expression, property);

        public UpdateBase Insert<T>(Expression property, T fallbackValue) => new UpdateInsertFallback<TEntity, T>(_expression, property, fallbackValue);

        public UpdateBase Remove() => new UpdateRemove<TEntity>(_expression);

        public UpdateBase RemoveAt(int index) => new UpdateRemoveAt<TEntity>(_expression, index);

        public UpdateBase Remove<T>(T value) => new UpdateRemoveFromSet<TEntity, T>(_expression, value);

        public UpdateBase Remove(Expression property) => new UpdateRemoveFromSet<TEntity>(_expression, property);

        public UpdateBase Remove<T>(Expression property, T fallbackValue) => new UpdateRemoveFromSetFallback<TEntity, T>(_expression, property, fallbackValue);
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
}