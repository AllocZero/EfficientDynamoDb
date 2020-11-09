/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
#pragma warning disable 1574

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Context object for using the DataModel mode of DynamoDB.
    /// Used to interact with the service, save/load objects, etc.
    /// </summary>
    public partial class DynamoDBContext : IDynamoDBContext
    {
        #region Save async

        /// <summary>
        /// Initiates the asynchronous execution of the Save operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Save"/>
        /// </summary>
        /// <typeparam name="T">Type to save as.</typeparam>
        /// <param name="value">Object to save.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task SaveAsync<T>(T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveHelperAsync(value, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Save operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Save"/>
        /// </summary>
        /// <typeparam name="T">Type to save as.</typeparam>
        /// <param name="value">Object to save.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task SaveAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveHelperAsync(value, operationConfig, cancellationToken);
        }

        #endregion

        #region Load async

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(object hashKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync<T>(hashKey, null, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(object hashKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync<T>(hashKey, null, operationConfig, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="rangeKey">Range key element of the target item.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(object hashKey, object rangeKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync<T>(hashKey, rangeKey, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="rangeKey">Range key element of the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync<T>(hashKey, rangeKey, operationConfig, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="keyObject">Key of the target item.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(T keyObject, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync(keyObject, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Load operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Load"/>
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="keyObject">Key of the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<T> LoadAsync<T>(T keyObject, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return LoadHelperAsync(keyObject, operationConfig, cancellationToken);
        }

        #endregion

        #region Delete async

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Object to delete.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync(value, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Object to delete.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync(value, operationConfig, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(object hashKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync<T>(hashKey, null, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(object hashKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync<T>(hashKey, null, operationConfig, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="rangeKey">Range key element of the object to delete.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(object hashKey, object rangeKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync<T>(hashKey, rangeKey, null, cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Delete operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.DynamoDBContext.Delete"/>
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="rangeKey">Range key element of the object to delete.</param>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task DeleteAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DeleteHelperAsync<T>(hashKey, rangeKey, operationConfig, cancellationToken);
        }

        #endregion

        #region BatchGet async

        /// <summary>
        /// Issues a batch-get request with multiple batches.
        /// 
        /// Results are stored in the individual batches.
        /// </summary>
        /// <param name="batches">
        /// Configured BatchGet objects
        /// </param>
        public Task ExecuteBatchGetAsync(params BatchGet[] batches)
        {
            return ExecuteBatchGetAsync(batches, default(CancellationToken));
        }

        /// <summary>
        /// Issues a batch-get request with multiple batches.
        /// 
        /// Results are stored in the individual batches.
        /// </summary>
        /// <param name="batches">
        /// Configured BatchGet objects
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public Task ExecuteBatchGetAsync(BatchGet[] batches, CancellationToken cancellationToken = default(CancellationToken))
        {
            MultiTableBatchGet superBatch = new MultiTableBatchGet(batches);
            return superBatch.ExecuteAsync(cancellationToken);
        }

        #endregion

        #region BatchWrite async

        /// <summary>
        /// Issues a batch-write request with multiple batches.
        /// </summary>
        /// <param name="batches">
        /// Configured BatchWrite objects
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public Task ExecuteBatchWriteAsync(BatchWrite[] batches, CancellationToken cancellationToken = default(CancellationToken))
        {
            MultiTableBatchWrite superBatch = new MultiTableBatchWrite(batches);
            return superBatch.ExecuteAsync(cancellationToken);
        }

        #endregion

        #region Scan async

        /// <summary>
        ///  Configures an async Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="conditions">
        /// Conditions that the results should meet.
        /// </param>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>AsyncSearch which can be used to retrieve DynamoDB data.</returns>
        public AsyncSearch<T> ScanAsync<T>(IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig = null)
        {
            var scan = ConvertScan<T>(conditions, operationConfig);
            return FromSearchAsync<T>(scan);
        }

        /// <summary>
        ///  Configures an async Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="scanConfig">Scan request object.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>AsyncSearch which can be used to retrieve DynamoDB data.</returns>
        public AsyncSearch<T> FromScanAsync<T>(ScanOperationConfig scanConfig, DynamoDBOperationConfig operationConfig = null)
        {
            if (scanConfig == null) throw new ArgumentNullException("scanConfig");

            var search = ConvertFromScan<T>(scanConfig, operationConfig);
            return FromSearchAsync<T>(search);
        }

        #endregion

        #region Query async

        /// <summary>
        /// Configures an async Query operation against DynamoDB, finding items
        /// that match the specified hash primary key.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKeyValue">Hash key of the items to query.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>AsyncSearch which can be used to retrieve DynamoDB data.</returns>
        public AsyncSearch<T> QueryAsync<T>(object hashKeyValue, DynamoDBOperationConfig operationConfig = null)
        {
            var query = ConvertQueryByValue<T>(hashKeyValue, null, operationConfig);
            return FromSearchAsync<T>(query);
        }

        /// <summary>
        /// Configures an async Query operation against DynamoDB, finding items
        /// that match the specified range element condition for a hash-and-range primary key.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKeyValue">Hash key of the items to query.</param>
        /// <param name="op">Operation of the condition.</param>
        /// <param name="values">
        /// Value(s) of the condition.
        /// For all operations except QueryOperator.Between, values should be one value.
        /// For QueryOperator.Between, values should be two values.
        /// </param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>AsyncSearch which can be used to retrieve DynamoDB data.</returns>
        public AsyncSearch<T> QueryAsync<T>(object hashKeyValue, QueryOperator op, IEnumerable<object> values, DynamoDBOperationConfig operationConfig = null)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var query = ConvertQueryByValue<T>(hashKeyValue, op, values, operationConfig);
            return FromSearchAsync<T>(query);
        }

        /// <summary>
        /// Configures an async Query operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="queryConfig">Query request object.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>AsyncSearch which can be used to retrieve DynamoDB data.</returns>
        public AsyncSearch<T> FromQueryAsync<T>(QueryOperationConfig queryConfig, DynamoDBOperationConfig operationConfig = null)
        {
            if (queryConfig == null) throw new ArgumentNullException("queryConfig");

            var search = ConvertFromQuery<T>(queryConfig, operationConfig);
            return FromSearchAsync<T>(search);
        }

        #endregion
    }
}
