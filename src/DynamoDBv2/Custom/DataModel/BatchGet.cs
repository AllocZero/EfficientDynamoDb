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
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using System.Threading;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Represents a non-generic object for retrieving a batch of items
    /// from a single DynamoDB table
    /// </summary>
    public abstract partial class BatchGet
    {
        #region Internal/protected properties

        /// <summary>
        /// Gets and sets the UntypedResults property.
        /// </summary>
        protected List<object> UntypedResults { get; set; }
        internal DynamoDBContext Context { get; set; }
        internal DynamoDBFlatConfig Config { get; set; }
        internal List<Key> Keys { get; set; }
        internal DocumentBatchGet DocumentBatch { get; set; }
        internal ItemStorageConfig ItemStorageConfig { get; set; }

        #endregion


        #region Constructor

        internal BatchGet(DynamoDBContext context, DynamoDBFlatConfig config)
        {
            Context = context;
            Config = config;
            Keys = new List<Key>();
        }

        #endregion


        #region Public properties

        /// <summary>
        /// List of results retrieved from DynamoDB.
        /// Populated after Execute is called.
        /// </summary>
        public List<object> Results { get { return UntypedResults; } }

        /// <summary>
        /// If set to true, a consistent read is issued. Otherwise eventually-consistent is used.
        /// </summary>
        public bool ConsistentRead { get; set; }

        #endregion


        #region Protected methods

        /// <summary>
        /// Executes a server call to batch-get the items requested.
        /// Populates Results with the retrieved items.
        /// </summary>
        internal protected abstract void ExecuteHelper();

#if AWS_ASYNC_API
        /// <summary>
        /// Executes an asynchronous server call to batch-get the items requested.
        /// Populates Results with the retrieved items.
        /// </summary>
        internal protected abstract Task ExecuteHelperAsync(CancellationToken cancellationToken);
#endif
        #endregion


        #region Internal methods

        internal abstract void CreateDocumentBatch();
        internal abstract void PopulateResults(List<Document> items);

        #endregion
    }

    /// <summary>
    /// Represents a strongly-typed object for retrieving a batch of items
    /// from a single DynamoDB table
    /// </summary>
    public class BatchGet<T> : BatchGet
    {
        #region Public properties

        /// <summary>
        /// List of results retrieved from DynamoDB.
        /// Populated after Execute is called.
        /// </summary>
        new public List<T> Results { get { return TypedResults; } }

        #endregion


        #region Public methods

        /// <summary>
        /// Add a single item to get, identified by its hash primary key.
        /// </summary>
        /// <param name="hashKey">Hash key of the item to get</param>
        public void AddKey(object hashKey)
        {
            AddKey(hashKey, null);
        }

        /// <summary>
        /// Add a single item to get, identified by its hash-and-range primary key.
        /// </summary>
        /// <param name="hashKey">Hash key of the item to get</param>
        /// <param name="rangeKey">Range key of the item to get</param>
        public void AddKey(object hashKey, object rangeKey)
        {
            Key key = Context.MakeKey(hashKey, rangeKey, ItemStorageConfig, Config);
            Keys.Add(key);
        }

        /// <summary>
        /// Add a single item to get.
        /// </summary>
        /// <param name="keyObject">Object key of the item to get</param>
        public void AddKey(T keyObject)
        {
            Key key = Context.MakeKey(keyObject, ItemStorageConfig, Config);
            Keys.Add(key);
        }

        /// <summary>
        /// Creates a MultiTableBatchGet object that is a combination
        /// of the current BatchGet and the specified BatchGets
        /// </summary>
        /// <param name="otherBatches">Other BatchGet objects</param>
        /// <returns>
        /// MultiTableBatchGet consisting of the multiple BatchGet objects:
        /// the current batch and the passed-in batches.
        /// </returns>
        public MultiTableBatchGet Combine(params BatchGet[] otherBatches)
        {
            return new MultiTableBatchGet(this, otherBatches);
        }

        #endregion


        #region Constructor

        internal BatchGet(DynamoDBContext context, DynamoDBFlatConfig config)
            : base(context, config)
        {
            ItemStorageConfig = context.StorageConfigCache.GetConfig<T>(config);
        }

        #endregion


        #region Internal/protected/private members

        /// <summary>
        /// Executes the batch get
        /// </summary>
        internal protected override void ExecuteHelper()
        {
            CreateDocumentBatch();
            DocumentBatch.ExecuteHelper();
            PopulateResults(DocumentBatch.Results);
        }

#if AWS_ASYNC_API
        /// <summary>
        /// Executes the batch get asynchronously
        /// </summary>
        internal protected override async Task ExecuteHelperAsync(CancellationToken cancellationToken)
        {
            CreateDocumentBatch();
            await DocumentBatch.ExecuteHelperAsync(cancellationToken).ConfigureAwait(false);
            PopulateResults(DocumentBatch.Results);
        }
#endif

        /// <summary>
        /// Gets and sets the TypedResults property.
        /// </summary>
        protected List<T> TypedResults { get; set; }
        internal override void CreateDocumentBatch()
        {
            var storageConfig = Context.StorageConfigCache.GetConfig<T>(Config);
            var table = Context.GetTargetTable(storageConfig, Config);

            DocumentBatchGet docBatch = new DocumentBatchGet(table)
            {
                AttributesToGet = storageConfig.AttributesToGet,
                ConsistentRead = this.ConsistentRead
            };
            docBatch.Keys.AddRange(Keys);

            DocumentBatch = docBatch;
        }
        internal override void PopulateResults(List<Document> items)
        {
            UntypedResults = new List<object>();
            TypedResults = new List<T>();
            foreach (var doc in items)
            {
                var item = Context.FromDocumentHelper<T>(doc, Config);
                TypedResults.Add(item);
                UntypedResults.Add(item);
            }
        }

        #endregion
    }

    /// <summary>
    /// Class for retrieving a batch of items from multiple DynamoDB tables,
    /// using multiple strongly-typed BatchGet objects
    /// </summary>
    public partial class MultiTableBatchGet
    {
        #region Private members

        private List<BatchGet> allBatches = new List<BatchGet>();

        #endregion


        #region Constructor

        /// <summary>
        /// Constructs a MultiTableBatchGet object from a number of
        /// BatchGet objects
        /// </summary>
        /// <param name="batches">Collection of BatchGet objects</param>
        public MultiTableBatchGet(params BatchGet[] batches)
        {
            allBatches = new List<BatchGet>(batches);
        }

        internal MultiTableBatchGet(BatchGet first, params BatchGet[] rest)
        {
            allBatches = new List<BatchGet>();
            allBatches.Add(first);
            allBatches.AddRange(rest);
        }

        #endregion


        #region Public properties

        /// <summary>
        /// Gets the total number of primary keys to be loaded from DynamoDB,
        /// across all batches
        /// </summary>
        public int TotalKeys
        {
            get
            {
                int count = 0;
                foreach (var batch in allBatches)
                {
                    count += batch.Keys.Count;
                }
                return count;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Add a BatchGet object to the multi-table batch request
        /// </summary>
        /// <param name="batch">BatchGet to add</param>
        public void AddBatch(BatchGet batch)
        {
            allBatches.Add(batch);
        }

        internal void ExecuteHelper()
        {
            MultiTableDocumentBatchGet superBatch = new MultiTableDocumentBatchGet();
            foreach (var batch in allBatches)
            {
                batch.CreateDocumentBatch();
                superBatch.AddBatch(batch.DocumentBatch);
            }

            superBatch.ExecuteHelper();

            foreach (var batch in allBatches)
            {
                batch.PopulateResults(batch.DocumentBatch.Results);
            }
        }

#if AWS_ASYNC_API
        internal async Task ExecuteHelperAsync(CancellationToken cancellationToken)
        {
            MultiTableDocumentBatchGet superBatch = new MultiTableDocumentBatchGet();
            foreach (var batch in allBatches)
            {
                batch.CreateDocumentBatch();
                superBatch.AddBatch(batch.DocumentBatch);
            }

            await superBatch.ExecuteHelperAsync(cancellationToken).ConfigureAwait(false);

            foreach (var batch in allBatches)
            {
                batch.PopulateResults(batch.DocumentBatch.Results);
            }
        }
#endif

        #endregion
    }
}
