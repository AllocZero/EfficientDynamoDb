using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Factories
{
    public static class Filter
    {
        public static EntityFilter<TEntity> ForEntity<TEntity>() => EntityFilter<TEntity>.Instance;
    }

    public class EntityFilter<TEntity>
    {
        internal static readonly EntityFilter<TEntity> Instance = new EntityFilter<TEntity>();
        
        public AttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => Filter<TEntity>.On(property);
    }

    public static class Filter<TEntity>
    {
        public static AttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            var propertyName = GetMemberName(property);

            return new AttributeFilter<TEntity>(propertyName);
        }

        // TODO: Add support for nested filters like "x => x.SomeProperty.NestedProperty"
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetMemberName(Expression expression) =>
            expression.NodeType switch
            {
                ExpressionType.Lambda => ((MemberExpression) ((LambdaExpression) expression).Body).Member.Name,
                ExpressionType.MemberAccess => ((MemberExpression) expression).Member.Name,
                ExpressionType.Convert => GetMemberName(((UnaryExpression) expression).Operand),
                _ => throw new NotSupportedException($"Node type {expression.NodeType.ToString()} is not supported")
            };
    }
}