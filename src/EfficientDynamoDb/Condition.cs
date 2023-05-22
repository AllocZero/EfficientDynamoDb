using System;
using System.Linq.Expressions;
using EfficientDynamoDb.FluentCondition.Core.AttributeFilters;

namespace EfficientDynamoDb
{
    /// <summary>
    /// Utility class for creating EntityFilter that may be cached for creating multiple conditions.
    /// </summary>
    public static class Condition
    {
        /// <summary>
        /// Creates a filter object that can be used to specify conditions for DynamoDb requests. .
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// Instance of <code>EntityFilter</code> may be cached and used for declaring multiple conditions for the same entity.
        /// </remarks>
        public static EntityFilter<TEntity> ForEntity<TEntity>() => EntityFilter<TEntity>.Instance;
    }

    /// <summary>
    /// Represents a cacheable filter object that can be used to specify conditions for DynamoDb requests.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public class EntityFilter<TEntity>
    {
        internal static readonly EntityFilter<TEntity> Instance = new EntityFilter<TEntity>();
        
        /// <summary>
        /// Specifies the property to run condition on.
        /// </summary>
        /// <param name="property">An expression identifying the property for condition.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>Attribute filter builder.</returns>
        public IAttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => Condition<TEntity>.On(property);
        
        /// <summary>
        /// Specifies the property to run condition on.
        /// The condition will be applied to the size of the DynamoDB item instead of the item value.
        /// </summary>
        /// <param name="property">An expression identifying the property for condition.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>SizeOf attribute filter builder.</returns>
        public ISizeOfAttributeFilter<TEntity> OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => Condition<TEntity>.OnSizeOf(property);
    }

    /// <summary>
    /// Utility class for creating a single condition.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public static class Condition<TEntity>
    {
        /// <summary>
        /// Specifies the property to run condition on.
        /// </summary>
        /// <param name="property">An expression identifying the property for condition.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>Attribute filter builder.</returns>
        public static IAttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => new AttributeFilter<TEntity>(property, false);

        /// <summary>
        /// Specifies the property to run condition on.
        /// The condition will be applied to the size of the DynamoDB item instead of the item value.
        /// </summary>
        /// <param name="property">An expression identifying the property for condition.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>SizeOf attribute filter builder.</returns>
        public static ISizeOfAttributeFilter<TEntity> OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) =>
            new AttributeFilter<TEntity>(property, true);
    }
}