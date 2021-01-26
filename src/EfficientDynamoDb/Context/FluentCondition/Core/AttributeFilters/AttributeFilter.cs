using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Operators.Common;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Core.AttributeFilters
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

        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase LessThanOrEqualsTo(Expression property) => new FilterLessThanOrEqualsTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase LessThanOrEqualsToSizeOf(Expression property) => new FilterLessThanOrEqualsTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase EqualsTo<T>(T value) => new FilterEqualsTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase EqualsTo(Expression property) => new FilterEqualsTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase EqualsToSizeOf(Expression property) => new FilterEqualsTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase NotEqualsTo<T>(T value) => new FilterNotEqualsTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase NotEqualsTo(Expression property) => new FilterNotEqualsTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase NotEqualsToSizeOf(Expression property) => new FilterNotEqualsTo<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThan(Expression property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanSizeOf(Expression property) => new FilterGreaterThan<TEntity>(_expression, _useSize, property, true);

        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<TEntity, T>(_expression, _useSize, value);
        public FilterBase GreaterThanOrEqualsTo(Expression property) => new FilterGreaterThanOrEqualsTo<TEntity>(_expression, _useSize, property, false);
        public FilterBase GreaterThanOrEqualsToSizeOf(Expression property) => new FilterGreaterThanOrEqualsTo<TEntity>(_expression, _useSize, property, true);

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