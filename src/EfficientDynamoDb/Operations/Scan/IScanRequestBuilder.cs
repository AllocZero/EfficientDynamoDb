using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.Scan
{
    /// <summary>
    /// Represents a builder for the Scan operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    public interface IScanEntityRequestBuilder<TEntity> : ITableBuilder<IScanEntityRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the index name to use for the Scan operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to scan.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html
        /// </remarks>
        IScanEntityRequestBuilder<TEntity> WithLimit(int limit);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        /// <summary>
        /// Specify the select mode for the Scan operation.
        /// </summary>
        /// <param name="selectMode"></param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Instead of using <see cref="Select.SpecificAttributes"/>, use one of the following without specifying the <paramref name="selectMode"/> parameter:
        /// <list type="bullet">
        /// <item><description><see cref="WithProjectedAttributes"/></description></item>
        /// <item><description><see cref="AsProjections{TProjection}()"/></description></item>
        /// <item><description><see cref="AsProjections{TProjection}(System.Linq.Expressions.Expression{System.Func{TProjection,object}}[])"/></description></item>
        /// </list>
        /// </remarks>
        IScanEntityRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Scan operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Scan operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        /// <summary>
        /// Specifies the pagination token for the Scan operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
        
        /// <summary>
        /// Projects the retrieved items to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved items to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity, TProjection> AsProjections<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved items.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);
        
        /// <summary>
        /// Represents the returned items as <see cref="Document"/>.
        /// </summary>
        /// <returns>Scan operation builder suitable for document response.</returns>
        IScanDocumentRequestBuilder<TEntity> AsDocuments();
        
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<TEntity> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToPagedAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable containing the retrieved items from all segments.</returns>
        IAsyncEnumerable<TEntity> ToParallelAsyncEnumerable(int totalSegments);
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable of pages from all segments, where each page is a read-only list of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TEntity>> ToParallelPagedAsyncEnumerable(int totalSegments);
        
        /// <summary>
        /// Executes the Scan operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<TEntity>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Scan operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<ScanEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Represents a builder for the projected Scan operation.
    /// </summary>
    /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
    /// <typeparam name="TProjection">Type of the projection.</typeparam>
    public interface IScanEntityRequestBuilder<TEntity, TProjection> : ITableBuilder<IScanEntityRequestBuilder<TEntity, TProjection>> where TEntity : class where TProjection : class
    {
        /// <summary>
        /// Specifies the index name to use for the Scan operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity, TProjection> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity, TProjection> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to scan.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html
        /// </remarks>
        IScanEntityRequestBuilder<TEntity, TProjection> WithLimit(int limit);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity, TProjection> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        // TODO: Consider removing this method because select mode automatically set to SpecificAttributes when projection is specified.
        IScanEntityRequestBuilder<TEntity, TProjection> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity, TProjection> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Scan operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Scan operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Scan operation builder.</returns>
        IScanEntityRequestBuilder<TEntity, TProjection> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        /// <summary>
        /// Specifies the pagination token for the Scan operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IScanEntityRequestBuilder<TEntity, TProjection> WithPaginationToken(string? paginationToken);
        
        /// <summary>
        /// Represents the returned items as <see cref="Document"/>.
        /// </summary>
        /// <returns>Scan operation builder suitable for document response.</returns>
        IScanDocumentRequestBuilder<TEntity> AsDocuments();
       
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<TProjection> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToPagedAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable containing the retrieved items from all segments.</returns>
        IAsyncEnumerable<TProjection> ToParallelAsyncEnumerable(int totalSegments);
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable of pages from all segments, where each page is a read-only list of items.</returns>
        IAsyncEnumerable<IReadOnlyList<TProjection>> ToParallelPagedAsyncEnumerable(int totalSegments);

        /// <summary>
        /// Executes the Scan operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<TProjection>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Scan operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<ScanEntityResponse<TProjection>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Represents a builder for the Scan operation with an entity type constraint and a document response.
    /// Provides methods for configuring options and executing the operation with a <see cref="Document"/> representation of the response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IScanDocumentRequestBuilder<TEntity> : ITableBuilder<IScanDocumentRequestBuilder<TEntity>> where TEntity : class
    {
        /// <summary>
        /// Specifies the index name to use for the Scan operation.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>Scan operation builder.</returns>
        IScanDocumentRequestBuilder<TEntity> FromIndex(string indexName);

        /// <summary>
        /// Specifies whether consistent read should be used.
        /// </summary>
        /// <param name="useConsistentRead">True, if consistent reads should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// DynamoDB doesn't support consistent reads on global secondary indexes (GSI).
        /// Trying to use consistent reads on a GSI will result in an error.
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithConsistentRead(bool useConsistentRead);

        /// <summary>
        /// Specifies the maximum number of items to scan.
        /// </summary>
        /// <param name="limit">Maximum number of items to scan.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// The actual number of items returned may be less than the specified <paramref name="limit" /> when filter expression is present or if the scan operation exceeds the 1 MB limit or retrieved data.
        /// Refer to DynamoDB docs for more information: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithLimit(int limit);
        
        /// <summary>
        /// Projects the retrieved items to the specified type.
        /// </summary>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties that are present in the <typeparamref name="TProjection"/> type will be retrieved.
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        /// <summary>
        /// Projects the retrieved items to the specified type. Only the specified properties will be retrieved.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <typeparam name="TProjection">Type of the projection.</typeparam>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties of the <see cref="TProjection"/> will have the default values.
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        /// <summary>
        /// Projects the specific attributes of the retrieved items.
        /// </summary>
        /// <param name="properties">Attributes to project.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Only the properties specified in <paramref name="properties"/> will be retrieved.
        /// Other properties will have the default values.
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithProjectedAttributes(params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// Specifies the consumed capacity details to include in the response.
        /// </summary>
        /// <param name="consumedCapacityMode">The <see cref="ReturnConsumedCapacity"/> option.</param>
        /// <returns>Scan operation builder.</returns>
        IScanDocumentRequestBuilder<TEntity> ReturnConsumedCapacity(ReturnConsumedCapacity consumedCapacityMode);

        /// <summary>
        /// Specify the select mode for the Scan operation.
        /// </summary>
        /// <param name="selectMode"></param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Instead of using <see cref="Select.SpecificAttributes"/>, use one of the following without specifying the <paramref name="selectMode"/> parameter:
        /// <list type="bullet">
        /// <item><description><see cref="WithProjectedAttributes"/></description></item>
        /// <item><description><see cref="WithProjectedAttributes{TProjection}()"/></description></item>
        /// <item><description><see cref="WithProjectedAttributes{TProjection}(System.Linq.Expressions.Expression{System.Func{TProjection,object}}[])"/></description></item>
        /// </list>
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithSelectMode(Select selectMode);

        /// <summary>
        /// Specifies if backward search should be used.
        /// </summary>
        /// <param name="useBackwardSearch">True, if backward search should be used. Otherwise, false.</param>
        /// <returns>Scan operation builder.</returns>
        IScanDocumentRequestBuilder<TEntity> BackwardSearch(bool useBackwardSearch);

        /// <summary>
        /// Specifies the filter expression for the Scan operation.
        /// </summary>
        /// <param name="filterExpressionBuilder">Filter expression.</param>
        /// <returns>Scan operation builder.</returns>
        IScanDocumentRequestBuilder<TEntity> WithFilterExpression(FilterBase filterExpressionBuilder);

        /// <summary>
        /// Specifies the filter expression function for the Scan operation.
        /// </summary>
        /// <param name="filterSetup">The filter expression function.</param>
        /// <returns>Scan operation builder.</returns>
        IScanDocumentRequestBuilder<TEntity> WithFilterExpression(Func<EntityFilter<TEntity>, FilterBase> filterSetup);
        
        /// <summary>
        /// Specifies the pagination token for the Scan operation.
        /// </summary>
        /// <param name="paginationToken">The pagination token to use.</param>
        /// <returns>Scan operation builder.</returns>
        /// <remarks>
        /// Passing <c>null</c> for <paramref name="paginationToken"/> will result in the same behavior as not specifying the pagination token at all.
        /// </remarks>
        IScanDocumentRequestBuilder<TEntity> WithPaginationToken(string? paginationToken);
       
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// </summary>
        /// <returns>Async enumerable with retrieved items.</returns>
        IAsyncEnumerable<Document> ToAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// </summary>
        /// <returns>Async enumerable of pages of items.</returns>
        IAsyncEnumerable<IReadOnlyList<Document>> ToPagedAsyncEnumerable();
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a single retrieved item.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable containing the retrieved items from all segments.</returns>
        IAsyncEnumerable<Document> ToParallelAsyncEnumerable(int totalSegments);
        
        /// <summary>
        /// Executes the Scan operation in parallel and returns the result as an async enumerable, with each item in the sequence representing a page of DynamoDB items.
        /// The Scan operation is divided into the specified number of total segments.
        /// </summary>
        /// <param name="totalSegments">The total number of segments to divide the Scan operation into.</param>
        /// <returns>An async enumerable of pages from all segments, where each page is a read-only list of items.</returns>
        IAsyncEnumerable<IReadOnlyList<Document>> ToParallelPagedAsyncEnumerable(int totalSegments);
        
        /// <summary>
        /// Executes the Scan operation and returns the page of data with pagination token.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<PagedResult<Document>> ToPageAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the Scan operation and returns the deserialized response.
        /// </summary>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<ScanEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default);
    }
}