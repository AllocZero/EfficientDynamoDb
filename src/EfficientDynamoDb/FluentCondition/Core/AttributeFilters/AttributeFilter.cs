using System;
using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Operators.Common;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    // TODO: Make two AttributeFilter implementations - default implementation is generic that forces property type as a method parameter and second one is current implementation that allows any parameter type 
    public class AttributeFilter<TEntity> : IAttributeFilter<TEntity>, ISizeOfAttributeFilter<TEntity>
    {
        private readonly Expression _expression;
        private readonly bool _useSize;

        internal AttributeFilter(Expression expression, bool useSize)
        {
            _expression = expression;
            _useSize = useSize;
        }

        public FilterBase LessThan<T>(T value) => new FilterLessThan<TEntity, T>(_expression, _useSize, value);
        public FilterBase LessThan<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterLessThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterLessThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase LessThanOrEqualTo<T>(T value) => new FilterLessThanOrEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase LessThanOrEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanOrEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase EqualTo<T>(T value) => new FilterEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase EqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase EqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase NotEqualTo<T>(T value) => new FilterNotEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase NotEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase NotEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>>property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThan<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThanOrEqualTo<T>(T value) => new FilterGreaterThanOrEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThanOrEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanOrEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase Between<T>(T min, T max) => new FilterBetweenValues<TEntity, T>(_expression, _useSize, min, max);

        public FilterBase Between<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, TProperty max) =>
            new FilterBetweenAttributeAndValue<TEntity, TProperty>(_expression, _useSize, minProperty, false, max);

        public FilterBase BetweenSizeOfAndValue<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, TProperty max) =>
            new FilterBetweenAttributeAndValue<TEntity, TProperty>(_expression, _useSize, minProperty, true, max);

        public FilterBase Between<TProperty>(TProperty min, Expression<Func<TEntity, TProperty>> maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, TProperty>(_expression, _useSize, min, maxProperty, false);

        public FilterBase BetweenValueAndSizeOf<TProperty>(TProperty min, Expression<Func<TEntity, TProperty>> maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, TProperty>(_expression, _useSize, min, maxProperty, true);

        public FilterBase Between<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, Expression<Func<TEntity, TProperty>> maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, false, maxProperty, false);

        public FilterBase BetweenSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, Expression<Func<TEntity, TProperty>> maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, true, maxProperty, true);

        public FilterBase BeginsWith(string prefix) => new FilterBeginsWithValue<TEntity>(_expression, _useSize, prefix);
        public FilterBase BeginsWith<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, false);
        public FilterBase BeginsWithSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, true);

        public FilterBase In<T>(params T[] values) => new FilterIn<TEntity, T>(_expression, _useSize, values);
        public FilterBase In<TProperty>(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, false);
        public FilterBase InSizeOf<TProperty>(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, true);

        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_expression);
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_expression);

        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_expression, _useSize, value);
        public FilterBase Contains<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterContains<TEntity>(_expression, _useSize, property, false);
        public FilterBase ContainsSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => new FilterContains<TEntity>(_expression, _useSize, property, true);

        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_expression, type);
    }
}