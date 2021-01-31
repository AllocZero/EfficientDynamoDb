using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.FluentCondition.Operators.Update;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.UpdateItem;
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
    

    public interface IAttributeUpdate<TEntity> where TEntity : class
    {
        public IUpdateRequestBuilder<TEntity> Assign<T>(T value);
        public IUpdateRequestBuilder<TEntity> Assign(Expression property);
        public IUpdateRequestBuilder<TEntity> Assign<T>(Expression property, T fallbackValue);

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(TLeft left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(TLeft left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> Append(Expression property);
        public IUpdateRequestBuilder<TEntity> Append<T>(Expression property, T fallbackValue); // SET #a = list_append(#a, if_not_exists(#b, :fallbackVal))
        public IUpdateRequestBuilder<TEntity> Append<T>(T value);
        
        public IUpdateRequestBuilder<TEntity> Prepend(Expression property);
        public IUpdateRequestBuilder<TEntity> Prepend<T>(Expression property, T fallbackValue); // SET #a = list_append(if_not_exists(#b, :fallbackVal), #a)
        public IUpdateRequestBuilder<TEntity> Prepend<T>(T value);
        
        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(Expression left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, Expression right, TRight rightFallbackValue);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, TRight right, TRight rightFallbackValue);
        
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(TLeft left, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right);
        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue);

        public IUpdateRequestBuilder<TEntity> Insert<T>(T value);
        public IUpdateRequestBuilder<TEntity> Insert(Expression property);
        public IUpdateRequestBuilder<TEntity> Insert<T>(Expression property, T fallbackValue); // ADD #a if_not_exists(#b, :fallbackVal)

        public IUpdateRequestBuilder<TEntity> Remove(); // REMOVE #a
        
        public IUpdateRequestBuilder<TEntity> RemoveAt(int index); // REMOVE #a[0]
        
        public IUpdateRequestBuilder<TEntity> Remove<T>(T value); // DELETE #set_a :val
        public IUpdateRequestBuilder<TEntity> Remove(Expression property); // DELETE #set_a #b
        public IUpdateRequestBuilder<TEntity> Remove<T>(Expression property, T fallbackValue); // DELETE #set_a if_not_exists(#b, :fallbackVal)
    }

    internal class AttributeUpdate<TEntity> : IAttributeUpdate<TEntity> where TEntity : class
    {
        private readonly UpdateRequestBuilder<TEntity> _requestBuilder;
        private readonly Expression _expression;

        internal AttributeUpdate(UpdateRequestBuilder<TEntity> requestBuilder, Expression expression)
        {
            _expression = expression;
            _requestBuilder = requestBuilder;
        }

        public IUpdateRequestBuilder<TEntity> Assign<T>(T value) => _requestBuilder.Create(new UpdateAssign<TEntity, T>(_expression, value));

        public IUpdateRequestBuilder<TEntity> Assign(Expression property) => _requestBuilder.Create(new UpdateAssign<TEntity>(_expression, property));

        public IUpdateRequestBuilder<TEntity> Assign<T>(Expression property, T fallbackValue) =>
            _requestBuilder.Create(new UpdateAssignFallback<TEntity, T>(_expression, property, fallbackValue));

        public IUpdateRequestBuilder<TEntity> AssignSum(Expression left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSum<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction(Expression left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignSubtraction<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Append(Expression property) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Append<T>(Expression property, T fallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Append<T>(T value) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Prepend(Expression property) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Prepend<T>(Expression property, T fallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Prepend<T>(T value) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat(Expression left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(Expression left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(Expression left, TLeft leftFallbackValue, TRight right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TRight>(Expression left, TRight right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(TLeft left, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft>(TLeft left, TLeft leftFallbackValue, Expression right) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> AssignConcat<TLeft, TRight>(TLeft left, Expression right, TRight rightFallbackValue) => throw new System.NotImplementedException();

        public IUpdateRequestBuilder<TEntity> Insert<T>(T value) => _requestBuilder.Create(new UpdateInsert<TEntity, T>(_expression, value));

        public IUpdateRequestBuilder<TEntity> Insert(Expression property) => _requestBuilder.Create(new UpdateInsert<TEntity>(_expression, property));

        public IUpdateRequestBuilder<TEntity> Insert<T>(Expression property, T fallbackValue) =>
            _requestBuilder.Create(new UpdateInsertFallback<TEntity, T>(_expression, property, fallbackValue));

        public IUpdateRequestBuilder<TEntity> Remove() => _requestBuilder.Create(new UpdateRemove<TEntity>(_expression));

        public IUpdateRequestBuilder<TEntity> RemoveAt(int index) => _requestBuilder.Create(new UpdateRemoveAt<TEntity>(_expression, index));

        public IUpdateRequestBuilder<TEntity> Remove<T>(T value) => _requestBuilder.Create(new UpdateRemoveFromSet<TEntity, T>(_expression, value));

        public IUpdateRequestBuilder<TEntity> Remove(Expression property) => _requestBuilder.Create(new UpdateRemoveFromSet<TEntity>(_expression, property));

        public IUpdateRequestBuilder<TEntity> Remove<T>(Expression property, T fallbackValue) =>
            _requestBuilder.Create(new UpdateRemoveFromSetFallback<TEntity, T>(_expression, property, fallbackValue));
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