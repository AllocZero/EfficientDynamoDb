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

using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Context object for using the DataModel mode of DynamoDB.
    /// Used to interact with the service, save/load objects, etc.
    /// </summary>
    public partial class DynamoDBContext : IDynamoDBContext
    {
        #region Save/serialize

        /// <summary>
        /// Saves an object to DynamoDB.
        /// 
        /// Uses DynamoDBContextConfig configured on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to save as.</typeparam>
        /// <param name="value">Object to save.</param>
        public void Save<T>(T value)
        {
            Save(value, null);
        }

        /// <summary>
        /// Saves an object to DynamoDB using passed-in configs.
        /// 
        /// Passed-in config overrides DynamoDBContextConfig on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to save as.</typeparam>
        /// <param name="value">Object to save.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        public void Save<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            SaveHelper<T>(value, operationConfig);
        }

        #endregion

        #region Load/deserialize

        /// <summary>
        /// Loads an object from DynamoDB for the given hash primary key.
        /// 
        /// Uses DynamoDBContextConfig configured on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(object hashKey)
        {
            return Load<T>(hashKey, null, null);
        }

        /// <summary>
        /// Loads an object from DynamoDB for the given hash primary key and using the given config.
        /// 
        /// Passed-in config overrides DynamoDBContextConfig on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(object hashKey, DynamoDBOperationConfig operationConfig)
        {
            return Load<T>(hashKey, null, operationConfig);
        }

        /// <summary>
        /// Loads an object from DynamoDB for the given hash-and-range primary key.
        /// 
        /// Uses DynamoDBContextConfig configured on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="rangeKey">Range key element of the target item.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(object hashKey, object rangeKey)
        {
            return Load<T>(hashKey, rangeKey, null);
        }

        /// <summary>
        /// Loads an object from DynamoDB for the given hash-and-range primary key and using the given config.
        /// 
        /// Passed-in config overrides DynamoDBContextConfig on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="hashKey">Hash key element of the target item.</param>
        /// <param name="rangeKey">Range key element of the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig)
        {
            return LoadHelper<T>(hashKey, rangeKey, operationConfig);
        }

        /// <summary>
        /// Loads an object from DynamoDB for the given key.
        /// The keyObject is a partially-specified instance, where the
        /// hash/range properties are equal to the key of the item you
        /// want to load.
        /// 
        /// Uses DynamoDBContextConfig configured on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="keyObject">Key object defining the the target item.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(T keyObject)
        {
            return Load<T>(keyObject, null);
        }

        /// <summary>
        /// Loads an object from DynamoDB for the given key and using the given config.
        /// The keyObject is a partially-specified instance, where the
        /// hash/range properties are equal to the key of the item you
        /// want to load.
        /// 
        /// Passed-in config overrides DynamoDBContextConfig on the context.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="keyObject">Key object defining the the target item.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        /// <returns>
        /// Object of type T, populated with properties of item loaded from DynamoDB.
        /// </returns>
        public T Load<T>(T keyObject, DynamoDBOperationConfig operationConfig)
        {
            return LoadHelper<T>(keyObject, operationConfig);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to given object.
        /// 
        /// Uses DynamoDBContextConfig configured on the context.
        /// If SkipVersionCheck=false, will check version of object before deleting.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Object to delete.</param>
        public void Delete<T>(T value)
        {
            Delete<T>(value, null);
        }

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to given object.
        /// 
        /// Passed-in config overrides DynamoDBContextConfig on the context.
        /// If SkipVersionCheck=false, will check version of object before deleting.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Object to delete.</param>
        /// <param name="operationConfig">Overriding configuration.</param>
        public void Delete<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            DeleteHelper<T>(value, operationConfig);
        }

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to a given hash primary key.
        /// 
        /// No version check is done prior to delete.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        public void Delete<T>(object hashKey)
        {
            Delete<T>(hashKey, null);
        }

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to a given hash primary key.
        /// 
        /// No version check is done prior to delete.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        public void Delete<T>(object hashKey, DynamoDBOperationConfig operationConfig)
        {
            Delete<T>(hashKey, null, operationConfig);
        }

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to a given hash-and-range primary key.
        /// 
        /// No version check is done prior to delete.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="rangeKey">Range key element of the object to delete.</param>
        public void Delete<T>(object hashKey, object rangeKey)
        {
            Delete<T>(hashKey, rangeKey, null);
        }

        /// <summary>
        /// Deletes an item in DynamoDB corresponding to a given hash-and-range primary key.
        /// 
        /// No version check is done prior to delete.
        /// Type must be marked up with DynamoDBTableAttribute and at least
        /// one public field/property with DynamoDBHashKeyAttribute.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKey">Hash key element of the object to delete.</param>
        /// <param name="rangeKey">Range key element of the object to delete.</param>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        public void Delete<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig)
        {
            DeleteHelper<T>(hashKey, rangeKey, operationConfig);
        }

        #endregion

        #region BatchGet

        /// <summary>
        /// Issues a batch-get request with multiple batches.
        /// 
        /// Results are stored in the individual batches.
        /// </summary>
        /// <param name="batches">
        /// Configured BatchGet objects
        /// </param>
        public void ExecuteBatchGet(params BatchGet[] batches)
        {
            MultiTableBatchGet superBatch = new MultiTableBatchGet(batches);
            superBatch.Execute();
        }

        #endregion

        #region Batch Write

        /// <summary>
        /// Issues a batch-write request with multiple batches.
        /// </summary>
        /// <param name="batches">
        /// Configured BatchWrite objects
        /// </param>
        public void ExecuteBatchWrite(params BatchWrite[] batches)
        {
            MultiTableBatchWrite superBatch = new MultiTableBatchWrite(batches);
            superBatch.Execute();
        }

        #endregion

        #region Scan

        /// <summary>
        /// Executes a Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="conditions">
        /// Conditions that the results should meet.
        /// </param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Scan<T>(params ScanCondition[] conditions)
        {
            if (conditions == null)
                throw new ArgumentNullException("conditions");

            return Scan<T>(conditions, null);
        }

        /// <summary>
        /// Executes a Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="conditions">
        /// Conditions that the results should meet.
        /// </param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Scan<T>(IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig)
        {
            if (conditions == null)
                throw new ArgumentNullException("conditions");

            var scan = ConvertScan<T>(conditions, operationConfig);
            return FromSearch<T>(scan);
        }

        /// <summary>
        /// Executes a Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="scanConfig">Scan request object.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> FromScan<T>(ScanOperationConfig scanConfig)
        {
            return FromScan<T>(scanConfig, null);
        }

        /// <summary>
        /// Executes a Scan operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="scanConfig">Scan request object.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> FromScan<T>(ScanOperationConfig scanConfig, DynamoDBOperationConfig operationConfig)
        {
            if (scanConfig == null) throw new ArgumentNullException("scanConfig");

            var scan = ConvertFromScan<T>(scanConfig, operationConfig);
            return FromSearch<T>(scan);
        }

        #endregion

        #region Query

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
        /// that match the specified hash primary key.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKeyValue">Hash key of the items to query.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Query<T>(object hashKeyValue)
        {
            var query = ConvertQueryByValue<T>(hashKeyValue, null, null);
            return FromSearch<T>(query);
        }

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
        /// that match the specified hash primary key.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="hashKeyValue">Hash key of the items to query.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Query<T>(object hashKeyValue, DynamoDBOperationConfig operationConfig)
        {
            var query = ConvertQueryByValue<T>(hashKeyValue, null, operationConfig);
            return FromSearch<T>(query);
        }

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
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
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Query<T>(object hashKeyValue, QueryOperator op, params object[] values)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentOutOfRangeException("values");

            return Query<T>(hashKeyValue, op, values, null);
        }

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
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
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> Query<T>(object hashKeyValue, QueryOperator op, IEnumerable<object> values, DynamoDBOperationConfig operationConfig)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var query = ConvertQueryByValue<T>(hashKeyValue, op, values, operationConfig);
            return FromSearch<T>(query);
        }

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="queryConfig">Query request object.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> FromQuery<T>(QueryOperationConfig queryConfig)
        {
            return FromQuery<T>(queryConfig, null);
        }

        /// <summary>
        /// Executes a Query operation against DynamoDB, finding items
        /// that match the specified conditions.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="queryConfig">Query request object.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Lazy-loaded collection of results.</returns>
        public IEnumerable<T> FromQuery<T>(QueryOperationConfig queryConfig, DynamoDBOperationConfig operationConfig)
        {
            if (queryConfig == null) throw new ArgumentNullException("queryConfig");

            var search = ConvertFromQuery<T>(queryConfig, operationConfig);
            return FromSearch<T>(search);
        }

        #endregion

        #region Table methods

        /// <summary>
        /// Retrieves the target table for the specified type
        /// </summary>
        /// <typeparam name="T">Type to retrieve table for</typeparam>
        /// <returns>Table object</returns>
        public Table GetTargetTable<T>()
        {
            return GetTargetTable<T>(null);
        }

        /// <summary>
        /// Retrieves the target table for the specified type
        /// </summary>
        /// <typeparam name="T">Type to retrieve table for</typeparam>
        /// <returns>Table object</returns>
        public Table GetTargetTable<T>(DynamoDBOperationConfig operationConfig)
        {
            return GetTargetTableInternal<T>(operationConfig);
        }

        #endregion
    }
}
