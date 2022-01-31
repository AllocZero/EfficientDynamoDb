using System;
using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Operators.Common;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    // TODO: Make two AttributeFilter implementations - default implementation is generic that forces property type as a method parameter and second one is current implementation that allows any parameter type 
    public class AttributeFilter<TEntity, TProperty> : IAttributeFilter<TEntity, TProperty>, ISizeOfAttributeFilter<TEntity, TProperty>
    {
        private readonly Expression _expression;
        private readonly bool _useSize;

        internal AttributeFilter(Expression expression, bool useSize)
        {
            _expression = expression;
            _useSize = useSize;
        }

        public FilterBase LessThan(TProperty value) => new FilterLessThan<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase LessThan<T>(Expression<Func<TEntity, T>> property) => new FilterLessThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterLessThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase LessThanOrEqualTo(TProperty value) => new FilterLessThanOrEqualTo<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase LessThanOrEqualTo<T>(Expression<Func<TEntity, T>> property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanOrEqualToSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase EqualTo(TProperty value) => new FilterEqualTo<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase EqualTo<T>(Expression<Func<TEntity, T>> property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase EqualToSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase NotEqualTo(TProperty value) => new FilterNotEqualTo<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase NotEqualTo<T>(Expression<Func<TEntity, T>> property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase NotEqualToSizeOf<T>(Expression<Func<TEntity, T>>property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThan(TProperty value) => new FilterGreaterThan<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase GreaterThan<T>(Expression<Func<TEntity, T>> property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThanOrEqualTo(TProperty value) => new FilterGreaterThanOrEqualTo<TEntity, TProperty>(_expression, _useSize, value);
        public FilterBase GreaterThanOrEqualTo<T>(Expression<Func<TEntity, T>> property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanOrEqualToSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase Between(TProperty min, TProperty max) => new FilterBetweenValues<TEntity, TProperty>(_expression, _useSize, min, max);

        public FilterBase Between<T>(Expression<Func<TEntity, T>> minProperty, TProperty max) =>
            new FilterBetweenAttributeAndValue<TEntity, TProperty>(_expression, _useSize, minProperty, false, max);

        public FilterBase BetweenSizeOfAndValue<T>(Expression<Func<TEntity, T>> minProperty, TProperty max) =>
            new FilterBetweenAttributeAndValue<TEntity, TProperty>(_expression, _useSize, minProperty, true, max);

        public FilterBase Between<T>(TProperty min, Expression<Func<TEntity, T>> maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, TProperty>(_expression, _useSize, min, maxProperty, false);

        public FilterBase BetweenValueAndSizeOf<T>(TProperty min, Expression<Func<TEntity, T>> maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, TProperty>(_expression, _useSize, min, maxProperty, true);

        public FilterBase Between<T>(Expression<Func<TEntity, T>> minProperty, Expression<Func<TEntity, T>> maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, false, maxProperty, false);

        public FilterBase BetweenSizeOf<T>(Expression<Func<TEntity, T>> minProperty, Expression<Func<TEntity, T>> maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, true, maxProperty, true);

        public FilterBase BeginsWith(string prefix) => new FilterBeginsWithValue<TEntity>(_expression, _useSize, prefix);
        public FilterBase BeginsWith<T>(Expression<Func<TEntity, T>> property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, false);
        public FilterBase BeginsWithSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, true);

        public FilterBase In(params TProperty[] values) => new FilterIn<TEntity, TProperty>(_expression, _useSize, values);
        public FilterBase In(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, false);
        public FilterBase InSizeOf(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, true);

        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_expression);
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_expression);

        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_expression, _useSize, value);
        public FilterBase Contains<T>(Expression<Func<TEntity, T>> property) => new FilterContains<TEntity>(_expression, _useSize, property, false);
        public FilterBase ContainsSizeOf<T>(Expression<Func<TEntity, T>> property) => new FilterContains<TEntity>(_expression, _useSize, property, true);

        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_expression, type);
    }
}