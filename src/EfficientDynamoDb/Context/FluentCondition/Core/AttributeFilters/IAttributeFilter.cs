using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Core.AttributeFilters
{
    public interface IAttributeFilter
    {
        public FilterBase LessThan<T>(T value);
        public FilterBase LessThan(Expression property);
        public FilterBase LessThanSizeOf(Expression property);
        
        public FilterBase LessThanOrEqualsTo<T>(T value);
        public FilterBase LessThanOrEqualsTo(Expression property);
        public FilterBase LessThanOrEqualsToSizeOf(Expression property);
        
        public FilterBase EqualsTo<T>(T value);
        public FilterBase EqualsTo(Expression property);
        public FilterBase EqualsToSizeOf(Expression property);
        
        public FilterBase NotEqualsTo<T>(T value);
        public FilterBase NotEqualsTo(Expression property);
        public FilterBase NotEqualsToSizeOf(Expression property);
        
        public FilterBase GreaterThan<T>(T value);
        public FilterBase GreaterThan(Expression property);
        public FilterBase GreaterThanSizeOf(Expression property);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value);
        public FilterBase GreaterThanOrEqualsTo(Expression property);
        public FilterBase GreaterThanOrEqualsToSizeOf(Expression property);
        
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
        
        public FilterBase Exists();
        public FilterBase NotExists();
        
        public FilterBase Contains<T>(T value);
        public FilterBase Contains(Expression property);
        public FilterBase ContainsSizeOf(Expression property);
        
        public FilterBase OfType(AttributeType type);
    }
}