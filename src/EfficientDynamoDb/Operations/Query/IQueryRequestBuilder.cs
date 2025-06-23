using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Query
{
    /// <summary>
    /// Represents a builder for the Query operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface IQueryEntityRequestBuilder<TEntity> : ITableBuilder<IQueryEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IReadOnlyList<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<QueryEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<TEntity> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToPagedAsyncEnumerable();

        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Represents the returned items as <see cref="Document"/>.
        /// </summary>
        /// <returns>Query operation builder suitable for document response.</returns>
        IQueryDocumentRequestBuilder<TEntity> AsDocuments();
        
        /// <summary>
        /// Specifies the key expression for the Query operation.
        /// </summary>
        /// <param name="keyExpressionBuilder">Key expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder);

        /// <summary>
        /// Specifies the key expression function for the Query operation.
        /// </summary>
        /// <param name="keySetup">The key expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        /// <summary>
        /// Specifies the index name to use for the Query operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to query.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity> WithLimit(int limit);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        /// <summary>
        /// Specify the select mode for the Query operation.
        /// </summary>
        /// <param name="selectMode"></param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Instead of using <see cref="Select.SpecificAttributes"/>, use one of the following without specifying the <paramref name="selectMode"/> parameter:
        /// <list type="bullet">
        /// <item><description><see cref="WithProjectedAttributes"/></description></item>
        /// <item><description><see cref="AsProjections{TProjection}()"/></description></item>
        /// <item><description><see cref="AsProjections{TProjection}(System.Linq.Expressions.Expression{System.Func{TProjection,object}}[])"/></description></item>
        /// </list>
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Query operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Query operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        /// <summary>
        /// Specifies the pagination token for the Query operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
        
        /// <summary>
        /// Projects the retrieved items to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class;
        
        /// <summary>
        /// Projects the retrieved items to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved items.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        /// <summary>
        /// Suppresses throwing of the exceptions related to the Query operation.
        /// </summary>
        /// <returns></returns>
        ISuppressedQueryEntityRequestBuilder<TEntity> SuppressThrowing();
    }

    /// <summary>
    /// Represents a builder for the Query operation that suppresses DynamoDB-related exceptions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface ISuppressedQueryEntityRequestBuilder<TEntity> : ITableBuilder<ISuppressedQueryEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<IReadOnlyList<TEntity>>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<QueryEntityResponse<TEntity>>> ToResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<PagedResult<TEntity>>> ToPageAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a builder for the projected Query operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    /// <typeparam name="TProjection">Type of the projection.</typeparam>
    public interface IQueryEntityRequestBuilder<TEntity, TProjection> : ITableBuilder<IQueryEntityRequestBuilder<TEntity, TProjection>> where TEntity : class where TProjection : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IReadOnlyList<TProjection>> ToListAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<TProjection> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToPagedAsyncEnumerable();
        
        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<QueryEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Represents the returned items as <see cref="Document"/>.
        /// </summary>
        /// <returns>Query operation builder suitable for document response.</returns>
        IQueryDocumentRequestBuilder<TEntity> AsDocuments();
        
        /// <summary>
        /// Specifies the key expression for the Query operation.
        /// </summary>
        /// <param name="keyExpressionBuilder">Key expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(FilterBase keyExpressionBuilder);

        /// <summary>
        /// Specifies the key expression function for the Query operation.
        /// </summary>
        /// <param name="keySetup">The key expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        /// <summary>
        /// Specifies the index name to use for the Query operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to query.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        // TODO: Consider removing this method because select mode automatically set to SpecificAttributes when projection is specified.
        IQueryEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Query operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Query operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        /// <summary>
        /// Specifies the pagination token for the Query operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IQueryEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken);
        
        /// <summary>
        /// Suppresses throwing of the exceptions related to the Query operation.
        /// </summary>
        /// <returns></returns>
        ISuppressedQueryEntityRequestBuilder<TEntity, TProjection> SuppressThrowing();
    }

    /// <summary>
    /// Represents a builder for the projected Query operation that suppresses DynamoDB-related exceptions.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    /// <typeparam name="TProjection">Type of the projection.</typeparam>
    public interface ISuppressedQueryEntityRequestBuilder<TEntity, TProjection> : ITableBuilder<ISuppressedQueryEntityRequestBuilder<TEntity, TProjection>>
        where TEntity : class where TProjection : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<IReadOnlyList<TProjection>>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<QueryEntityResponse<TProjection>>> ToResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<PagedResult<TProjection>>> ToPageAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a builder for the Query operation with an entity type constraint and a document response.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryDocumentRequestBuilder<TEntity> : ITableBuilder<IQueryDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IReadOnlyList<Document>> ToListAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<QueryEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<Document> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Query operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<Document>> ToPagedAsyncEnumerable();
        
        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Projects the retrieved items to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;
        
        /// <summary>
        /// Projects the retrieved items to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved items.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        /// <summary>
        /// Specifies the key expression for the Query operation.
        /// </summary>
        /// <param name="keyExpressionBuilder">Key expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(FilterBase keyExpressionBuilder);

        /// <summary>
        /// Specifies the key expression function for the Query operation.
        /// </summary>
        /// <param name="keySetup">The key expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> WithKeyExpression(Func<EntityFilter<TEntity>, FilterBase> keySetup);

        /// <summary>
        /// Specifies the index name to use for the Query operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to query.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithLimit(int limit);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        /// <summary>
        /// Specify the select mode for the Query operation.
        /// </summary>
        /// <param name="selectMode"></param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Instead of using <see cref="Select.SpecificAttributes"/>, use one of the following without specifying the <paramref name="selectMode"/> parameter:
        /// <list type="bullet">
        /// <item><description><see cref="WithProjectedAttributes"/></description></item>
        /// <item><description><see cref="WithProjectedAttributes{TProjection}()"/></description></item>
        /// <item><description><see cref="WithProjectedAttributes{TProjection}(System.Linq.Expressions.Expression{System.Func{TProjection,object}}[])"/></description></item>
        /// </list>
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Query operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Query operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Query operation builder.</returns>
        IQueryDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);

        /// <summary>
        /// Specifies the pagination token for the Query operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Query operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IQueryDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
        
        /// <summary>
        /// Suppresses throwing of the exceptions related to the Query operation.
        /// </summary>
        /// <returns></returns>
        ISuppressedQueryDocumentRequestBuilder<TEntity> SuppressThrowing();
    }

    /// <summary>
    /// Represents a builder for the Query operation that suppresses DynamoDB-related exceptions.
    /// Provides methods for executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISuppressedQueryDocumentRequestBuilder<TEntity> : ITableBuilder<ISuppressedQueryDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Executes the Query operation and aggregates the results into a single list of items.
        /// This method will make at least one service call to DynamoDB and will continue making calls to fetch all available pages of results if necessary.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<IReadOnlyList<Document>>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<QueryEntityResponse<Document>>> ToResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the Query operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<OpResult<PagedResult<Document>>> ToPageAsync(CancellationToken cancellationToken = default);
    }
}