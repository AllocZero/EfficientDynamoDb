using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactGetItems
{
    public interface ITransactGetItemRequestBuilder
    {
        internal BuilderNode GetNode() => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITransactGetItemRequestBuilder)} must implement the {nameof(GetNode)} method.");

        internal Type GetEntityType() => throw new NotImplementedException(
            $"All internal implementations of {nameof(ITransactGetItemRequestBuilder)} must implement the {nameof(GetEntityType)} method.");
    }
    
    /// <summary>
    /// Defines the contract for building a TransactGetItem request in a transaction.
    /// </summary>
    /// <typeparam name="TEntity">The type of the DB entity.</typeparam>
    public interface ITransactGetItemRequestBuilder<TEntity> : ITransactGetItemRequestBuilder, ITableBuilder<ITransactGetItemRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the partition key of the item to get in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>TransactGetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        /// <summary>
        /// Specifies partition and sort keys of the item to get in transaction.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>TransactGetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        ITransactGetItemRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Projects the retrieved item to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>TransactGetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved item to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>TransactGetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved item.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>TransactGetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        ITransactGetItemRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
    }
}