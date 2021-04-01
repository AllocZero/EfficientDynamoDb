using System.Linq.Expressions;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    public interface ISizeOfAttributeFilter
    {
        public FilterBase LessThan<T>(T value);
        public FilterBase LessThan(Expression property);
        public FilterBase LessThanSizeOf(Expression property);

        public FilterBase LessThanOrEqualTo<T>(T value);
        public FilterBase LessThanOrEqualTo(Expression property);
        public FilterBase LessThanOrEqualToSizeOf(Expression property);

        public FilterBase EqualTo<T>(T value);
        public FilterBase EqualTo(Expression property);
        public FilterBase EqualToSizeOf(Expression property);

        public FilterBase NotEqualTo<T>(T value);
        public FilterBase NotEqualTo(Expression property);
        public FilterBase NotEqualToSizeOf(Expression property);

        public FilterBase GreaterThan<T>(T value);
        public FilterBase GreaterThan(Expression property);
        public FilterBase GreaterThanSizeOf(Expression property);

        public FilterBase GreaterThanOrEqualTo<T>(T value);
        public FilterBase GreaterThanOrEqualTo(Expression property);
        public FilterBase GreaterThanOrEqualToSizeOf(Expression property);

        public FilterBase Between<T>(T min, T max);
        public FilterBase Between<T>(Expression minProperty, T max);
        public FilterBase BetweenSizeOfAndValue<T>(Expression minProperty, T max);
        public FilterBase Between<T>(T min, Expression maxProperty);
        public FilterBase BetweenValueAndSizeOf<T>(T min, Expression maxProperty);
        public FilterBase Between(Expression minProperty, Expression maxProperty);
        public FilterBase BetweenSizeOf(Expression minProperty, Expression maxProperty);

        public FilterBase BeginsWith(string prefix);
        public FilterBase BeginsWith(Expression property);
        public FilterBase BeginsWithSizeOf(Expression property);

        public FilterBase In<T>(params T[] values);
        public FilterBase In(params Expression[] properties);
        public FilterBase InSizeOf(params Expression[] properties);

        public FilterBase Contains<T>(T value);
        public FilterBase Contains(Expression property);
        public FilterBase ContainsSizeOf(Expression property);
    }
}