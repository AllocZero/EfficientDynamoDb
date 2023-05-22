using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    public interface IBatchGetTableBuilder
    {
        internal BuilderNode GetNode();

        internal Type GetTableType();
    }
    
    /// <summary>
    /// Provides functionality to build a GetItem operation in batch.
    /// Allows configuring read consistency and projections.
    /// </summary>
    public interface IBatchGetTableBuilder<TTableEntity> : IBatchGetTableBuilder, ITableBuilder<IBatchGetTableBuilder<TTableEntity>> where TTableEntity : class
    {
        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        IBatchGetTableBuilder<TTableEntity> WithConsistentRead(bool useConsistentRead);
        
        /// <summary>
        /// Projects the retrieved item to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved item to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved item.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes(params Expression<Func<TTableEntity, object>>[] properties);

        /// <summary>
        /// Specifies the items to retrieve in batch.
        /// </summary>
        /// <param name="items">BatchGet item builders.</param>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Batch"/> static class to construct item builders.
        /// </remarks>
        IBatchGetTableBuilder<TTableEntity> WithItems(params IBatchGetItemBuilder[] items);
        
        /// <summary>
        /// Specifies the items to retrieve in batch.
        /// </summary>
        /// <param name="items">BatchGet item builders.</param>
        /// <returns>Table-specific batch GetItem operation builder.</returns>
        /// <remarks>
        /// Use <see cref="Batch"/> static class to construct item builders.
        /// </remarks>
        IBatchGetTableBuilder<TTableEntity> WithItems(IEnumerable<IBatchGetItemBuilder> items);
    }
}