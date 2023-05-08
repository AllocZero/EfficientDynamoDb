using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.GetItem
{

    /// <summary>
    /// Represents a builder for the GetItem operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface IGetItemEntityRequestBuilder<TEntity> : ITableBuilder<IGetItemEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>GetItem operation builder.</returns>
        IGetItemEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>GetItem operation builder.</returns>
        IGetItemEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        /// <summary>
        /// Specifies partition and sort keys of the item to get.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <param name="sk">Sort key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <typeparam name="TSk">Type of the sort key.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <summary>
        /// Specifies the partition key of the item to get.
        /// </summary>
        /// <param name="pk">Partition key of the item.</param>
        /// <typeparam name="TPk">Type of the partition key.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        /// <summary>
        /// Projects the retrieved item to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity, TProjection> AsProjection<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved item to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity, TProjection> AsProjection<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved item.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>GetItem operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// Represents the returned item as <see cref="Document"/>.
        /// </summary>
        /// <returns>GetItem operation builder suitable for document response.</returns>
        IGetItemDocumentRequestBuilder<TEntity> AsDocument();

        /// <summary>
        /// Executes the GetItem operation and returns the item.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Result will be null if the item does not exist.
        /// </remarks>
        Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the GetItem operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<GetItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Represents a builder for the projected GetItem operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    /// <typeparam name="TProjection">Type of the projection.</typeparam>
    public interface IGetItemEntityRequestBuilder<TEntity, TProjection> : ITableBuilder<IGetItemEntityRequestBuilder<TEntity, TProjection>> where TEntity : class where TProjection : class
    {
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithConsistentRead"/>
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.ReturnConsumedCapacity"/>
        IGetItemEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithPrimaryKey{TPk,TSk}"/>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithPrimaryKey{TPk}"/>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IGetItemEntityRequestBuilder<TEntity, TProjection> WithPrimaryKey<TPk>(TPk pk);

        /// <summary>
        /// Represents the returned item as <see cref="Document"/>.
        /// </summary>
        /// <returns>GetItem operation builder suitable for document response.</returns>
        IGetItemDocumentRequestBuilder<TEntity> AsDocument();

        /// <summary>
        /// Executes the GetItem operation and returns the projected item.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Result will be null if the item does not exist.
        /// </remarks>
        Task<TProjection?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the GetItem operation and returns the deserialized response with projected item.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<GetItemEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    public interface IGetItemDocumentRequestBuilder<TEntity> : ITableBuilder<IGetItemDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithConsistentRead"/>
        IGetItemDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Projects the retrieved item to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved item to the specified type. Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved item.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>GetItem operation builder.</returns>
        IGetItemDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.ReturnConsumedCapacity"/>
        IGetItemDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);
        
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithPrimaryKey{TPk,TSk}"/>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only partition key, use <see cref="WithPrimaryKey{TPk}"/> instead.
        /// </remarks>
        IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        /// <inheritdoc cref="IGetItemEntityRequestBuilder{TEntity}.WithPrimaryKey{TPk}"/>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="WithPrimaryKey{TPk,TSk}"/> instead.
        /// </remarks>
        IGetItemDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);
        
        /// <summary>
        /// Executes the GetItem operation and returns the item attributes.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Result will be null if the item does not exist.
        /// </remarks>
        Task<Document?> ToItemAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the GetItem operation and returns the deserialized response with item attributes.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<GetItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}