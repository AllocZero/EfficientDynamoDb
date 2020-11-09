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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using System.Globalization;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using System.Threading;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Represents a non-generic object for writing/deleting a batch of items
    /// in a single DynamoDB table
    /// </summary>
    public abstract partial class BatchWrite
    {
        #region Internal/protected properties

        internal DynamoDBContext Context { get; set; }
        internal DynamoDBFlatConfig Config { get; set; }
        internal DocumentBatchWrite DocumentBatch { get; set; }

        #endregion


        #region Constructor

        internal BatchWrite(DynamoDBContext context, DynamoDBFlatConfig config)
        {
            Context = context;
            Config = config;
        }

        #endregion


        #region Protected methods

        /// <summary>
        /// Executes a server call to batch-write/delete the items requested.
        /// </summary>
        internal protected abstract void ExecuteHelper();

#if AWS_ASYNC_API
        /// <summary>
        /// Executes an asynchronous server call to batch-write/delete the items requested.
        /// </summary>
        internal protected abstract Task ExecuteHelperAsync(CancellationToken cancellationToken);
#endif

        #endregion
    }

    /// <summary>
    /// Represents a strongly-typed object for writing/deleting a batch of items
    /// in a single DynamoDB table
    /// </summary>
    public class BatchWrite<T> : BatchWrite
    {
        #region Public combine methods

        /// <summary>
        /// Creates a MultiTableBatchWrite object that is a combination
        /// of the current BatchWrite and the specified BatchWrites
        /// </summary>
        /// <param name="otherBatches">Other BatchWrite objects</param>
        /// <returns>
        /// MultiTableBatchWrite consisting of the multiple BatchWrite objects:
        /// the current batch and the passed-in batches.
        /// </returns>
        public MultiTableBatchWrite Combine(params BatchWrite[] otherBatches)
        {
            return new MultiTableBatchWrite(this, otherBatches);
        }

        #endregion


        #region Public Put methods

        /// <summary>
        /// Add a number of items to be put in the current batch operation
        /// </summary>
        /// <param name="values">Items to put</param>
        public void AddPutItems(IEnumerable<T> values)
        {
            if (values == null) return;

            foreach (T item in values)
            {
                AddPutItem(item);
            }
        }

        /// <summary>
        /// Add a single item to be put in the current batch operation
        /// </summary>
        /// <param name="item"></param>
        public void AddPutItem(T item)
        {
            if (item == null) return;

            ItemStorage storage = Context.ObjectToItemStorageHelper(item, StorageConfig, Config, keysOnly: false, ignoreNullValues: true);
            if (storage == null) return;
            DocumentBatch.AddDocumentToPut(storage.Document);
        }

        #endregion


        #region Public Delete methods

        /// <summary>
        /// Add a number of items to be deleted in the current batch operation
        /// </summary>
        /// <param name="values">Items to be deleted</param>
        public void AddDeleteItems(IEnumerable<T> values)
        {
            if (values == null) return;

            foreach (T item in values)
            {
                AddDeleteItem(item);
            }
        }

        /// <summary>
        /// Add a single item to be deleted in the current batch operation.
        /// </summary>
        /// <param name="item">Item to be deleted</param>
        public void AddDeleteItem(T item)
        {
            if (item == null) return;

            ItemStorage storage = Context.ObjectToItemStorageHelper(item, StorageConfig, Config, keysOnly: true, ignoreNullValues: true);
            if (storage == null) return;
            DocumentBatch.AddItemToDelete(storage.Document);
        }

        /// <summary>
        /// Add a single item to be deleted in the current batch operation.
        /// Item is identified by its hash primary key.
        /// </summary>
        /// <param name="hashKey">Hash key of the item to delete</param>
        public void AddDeleteKey(object hashKey)
        {
            AddDeleteKey(hashKey, null);
        }

        /// <summary>
        /// Add a single item to be deleted in the current batch operation.
        /// Item is identified by its hash-and-range primary key.
        /// </summary>
        /// <param name="hashKey">Hash key of the item to delete</param>
        /// <param name="rangeKey">Range key of the item to delete</param>
        public void AddDeleteKey(object hashKey, object rangeKey)
        {
            DocumentBatch.AddKeyToDelete(Context.MakeKey(hashKey, rangeKey, StorageConfig, Config));
        }

        #endregion


        #region Constructor

        internal BatchWrite(DynamoDBContext context, DynamoDBFlatConfig config)
            : base(context, config)
        {
            Type type = typeof(T);
            StorageConfig = context.StorageConfigCache.GetConfig(type, config);

            if (StorageConfig.HasVersion)
            {
                if (!Config.SkipVersionCheck.GetValueOrDefault(false))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Object {0} has a versioning field, which is not supported for this operation. To ignore versioning, use the DynamoDBContextConfig.SkipVersionCheck property.",
                        type.Name));
            }

            Table table = Context.GetTargetTable(StorageConfig, Config);
            DocumentBatch = table.CreateBatchWrite();
        }

        #endregion


        #region Internal/protected/private members

        internal ItemStorageConfig StorageConfig { get; set; }

        /// <summary>
        /// Execute the batch write.
        /// </summary>
        internal protected override void ExecuteHelper()
        {
            DocumentBatch.ExecuteHelper();
        }

#if AWS_ASYNC_API
        /// <summary>
        /// Execute the batch write asynchronously.
        /// </summary>
        internal protected override Task ExecuteHelperAsync(CancellationToken cancellationToken)
        {
            return DocumentBatch.ExecuteHelperAsync(cancellationToken);
        }
#endif

        #endregion
    }

    /// <summary>
    /// Class for writing/deleting a batch of items in multiple DynamoDB tables,
    /// using multiple strongly-typed BatchWrite objects
    /// </summary>
    public partial class MultiTableBatchWrite
    {
        #region Private members

        private List<BatchWrite> allBatches = new List<BatchWrite>();

        #endregion


        #region Constructor

        /// <summary>
        /// Constructs a MultiTableBatchWrite object from a number of
        /// BatchWrite objects
        /// </summary>
        /// <param name="batches">Collection of BatchWrite objects</param>
        public MultiTableBatchWrite(params BatchWrite[] batches)
        {
            allBatches = new List<BatchWrite>(batches);
        }

        internal MultiTableBatchWrite(BatchWrite first, params BatchWrite[] rest)
        {
            allBatches = new List<BatchWrite>();
            allBatches.Add(first);
            allBatches.AddRange(rest);
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Add a BatchWrite object to the multi-table batch request
        /// </summary>
        /// <param name="batch">BatchGet to add</param>
        public void AddBatch(BatchWrite batch)
        {
            allBatches.Add(batch);
        }

        internal void ExecuteHelper()
        {
            MultiTableDocumentBatchWrite superBatch = new MultiTableDocumentBatchWrite();
            foreach (var batch in allBatches)
            {
                superBatch.AddBatch(batch.DocumentBatch);
            }
            superBatch.ExecuteHelper();
        }

#if AWS_ASYNC_API
        internal Task ExecuteHelperAsync(CancellationToken cancellationToken)
        {
            MultiTableDocumentBatchWrite superBatch = new MultiTableDocumentBatchWrite();
            foreach (var batch in allBatches)
            {
                superBatch.AddBatch(batch.DocumentBatch);
            }
            return superBatch.ExecuteHelperAsync(cancellationToken);
        }
#endif

        #endregion
    }
}
