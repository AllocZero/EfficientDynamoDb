using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Operators.Common;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    // TODO: Make two AttributeFilter implementations - default implementation is generic that forces property type as a method parameter and second one is current implementation that allows any parameter type 
    public class AttributeFilter<TEntity> : IAttributeFilter, ISizeOfAttributeFilter
    {
        private readonly Expression _expression;
        private readonly bool _useSize;

        internal AttributeFilter(Expression expression, bool useSize)
        {
            _expression = expression;
            _useSize = useSize;
        }

        public FilterBase LessThan<T>(T value) => new FilterLessThan<TEntity, T>(_expression, _useSize, value);
        public FilterBase LessThan(Expression property) => new FilterLessThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanSizeOf(Expression property) => new FilterLessThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase LessThanOrEqualTo<T>(T value) => new FilterLessThanOrEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase LessThanOrEqualTo(Expression property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanOrEqualToSizeOf(Expression property) => new FilterLessThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase EqualTo<T>(T value) => new FilterEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase EqualTo(Expression property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase EqualToSizeOf(Expression property) => new FilterEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase NotEqualTo<T>(T value) => new FilterNotEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase NotEqualTo(Expression property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase NotEqualToSizeOf(Expression property) => new FilterNotEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThan(Expression property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanSizeOf(Expression property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThanOrEqualTo<T>(T value) => new FilterGreaterThanOrEqualTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThanOrEqualTo(Expression property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanOrEqualToSizeOf(Expression property) => new FilterGreaterThanOrEqualTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase Between<T>(T min, T max) => new FilterBetweenValues<TEntity, T>(_expression, _useSize, min, max);

        public FilterBase Between<T>(Expression minProperty, T max) =>
            new FilterBetweenAttributeAndValue<TEntity, T>(_expression, _useSize, minProperty, false, max);

        public FilterBase BetweenSizeOfAndValue<T>(Expression minProperty, T max) =>
            new FilterBetweenAttributeAndValue<TEntity, T>(_expression, _useSize, minProperty, true, max);

        public FilterBase Between<T>(T min, Expression maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, T>(_expression, _useSize, min, maxProperty, false);

        public FilterBase BetweenValueAndSizeOf<T>(T min, Expression maxProperty) =>
            new FilterBetweenValueAndAttribute<TEntity, T>(_expression, _useSize, min, maxProperty, true);

        public FilterBase Between(Expression minProperty, Expression maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, false, maxProperty, false);

        public FilterBase BetweenSizeOf(Expression minProperty, Expression maxProperty) =>
            new FilterBetweenAttributes<TEntity>(_expression, _useSize, minProperty, true, maxProperty, true);

        public FilterBase BeginsWith(string prefix) => new FilterBeginsWithValue<TEntity>(_expression, _useSize, prefix);
        public FilterBase BeginsWith(Expression property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, false);
        public FilterBase BeginsWithSizeOf(Expression property) => new FilterBeginsWithAttribute<TEntity>(_expression, _useSize, property, true);

        public FilterBase In<T>(params T[] values) => new FilterIn<TEntity, T>(_expression, _useSize, values);
        public FilterBase In(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, false);
        public FilterBase InSizeOf(params Expression[] properties) => new FilterIn<TEntity>(_expression, _useSize, properties, true);

        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_expression);
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_expression);

        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_expression, _useSize, value);
        public FilterBase Contains(Expression property) => new FilterContains<TEntity>(_expression, _useSize, property, false);
        public FilterBase ContainsSizeOf(Expression property) => new FilterContains<TEntity>(_expression, _useSize, property, true);

        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_expression, type);
    }
}