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
using System.Threading;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Context object for using the DataModel mode of DynamoDB.
    /// Used to interact with the service, save/load objects, etc.
    /// </summary>
    public partial class DynamoDBContext : IDynamoDBContext
    {
        #region Private members

        private bool disposed;
        private bool ownClient;
        internal IAmazonDynamoDB Client { get; private set; }
        private Dictionary<string, Table> tablesMap;
        private readonly object tablesMapLock = new object();
        internal DynamoDBContextConfig Config { get; private set; }
        internal ItemStorageConfigCache StorageConfigCache { get; private set; }

        #endregion

        #region Public properties

        /// <summary>
        /// This cache is a way to store Converters for objects which provides a way to expand Context
        /// </summary>
        public Dictionary<Type, IPropertyConverter> ConverterCache { get; private set; }

        #endregion

        #region Constructors

#if !NETSTANDARD

        /// <summary>
        /// Constructs a DynamoDBContext object with a default AmazonDynamoDBClient
        /// client and a default DynamoDBContextConfig object for configuration.
        /// </summary>
        public DynamoDBContext()
            : this(new AmazonDynamoDBClient()) { }

        /// <summary>
        /// Constructs a DynamoDBContext object with a default AmazonDynamoDBClient
        /// client and a default DynamoDBContextConfig object for configuration.
        /// </summary>
        /// <param name="region">The region to configure the AmazonDynamoDBClient to use.</param>
        public DynamoDBContext(RegionEndpoint region)
            : this(new AmazonDynamoDBClient(region), true, new DynamoDBContextConfig()) { }

        /// <summary>
        /// Constructs a DynamoDBContext object with the specified configuration.
        /// Uses a default AmazonDynamoDBClient as the client.
        /// </summary>
        /// <param name="config"></param>
        public DynamoDBContext(DynamoDBContextConfig config)
            : this(new AmazonDynamoDBClient(), config) { }

        /// <summary>
        /// Constructs a DynamoDBContext object with the specified configuration.
        /// Uses a default AmazonDynamoDBClient as the client.
        /// </summary>
        /// <param name="region">The region to configure the AmazonDynamoDBClient to use.</param>
        /// <param name="config"></param>
        public DynamoDBContext(RegionEndpoint region, DynamoDBContextConfig config)
            : this(new AmazonDynamoDBClient(region), true, config) { }
#endif

        /// <summary>
        /// Constructs a DynamoDBContext object with the specified DynamoDB client.
        /// Uses default DynamoDBContextConfig object for configuration.
        /// </summary>
        /// <param name="client">Client to use for making calls</param>
        public DynamoDBContext(IAmazonDynamoDB client)
            : this(client, false, new DynamoDBContextConfig()) { }

        /// <summary>
        /// Constructs a DynamoDBContext object with the specified DynamoDB client
        /// and configuration.
        /// </summary>
        /// <param name="client">Client to use for making calls</param>
        /// <param name="config">Configuration to use</param>
        public DynamoDBContext(IAmazonDynamoDB client, DynamoDBContextConfig config)
            : this(client, false, config) { }

        private DynamoDBContext(IAmazonDynamoDB client, bool ownClient, DynamoDBContextConfig config)
        {
            if (client == null) throw new ArgumentNullException("client");

            this.ConverterCache = new Dictionary<Type, IPropertyConverter>();
            this.ConverterCache.Add(typeof(S3Link), new S3Link.S3LinkConverter(this));
            this.Client = client;
            this.tablesMap = new Dictionary<string, Table>();
            this.ownClient = ownClient;
            this.Config = config ?? new DynamoDBContextConfig();
            this.StorageConfigCache = new ItemStorageConfigCache(this);
        }

        #endregion

        #region Dispose Pattern Implementation

        /// <summary>
        /// Implements the Dispose pattern
        /// </summary>
        /// <param name="disposing">Whether this object is being disposed via a call to Dispose
        /// or garbage collected.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing && Client != null)
                {
                    if (ownClient)
                    {
                        Client.Dispose();
                    }
                    Client = null;
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Disposes of all managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The destructor for the client class.
        /// </summary>
        ~DynamoDBContext()
        {
            this.Dispose(false);
        }

        #endregion

        #region Factory Creates

        /// <summary>
        /// Creates a strongly-typed BatchGet object, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to get</typeparam>
        /// <returns>Empty strongly-typed BatchGet object</returns>
        public BatchGet<T> CreateBatchGet<T>()
        {
            return CreateBatchGet<T>(null);
        }

        /// <summary>
        /// Creates a strongly-typed BatchGet object, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to get</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchGet object</returns>
        public BatchGet<T> CreateBatchGet<T>(DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig config = new DynamoDBFlatConfig(operationConfig, this.Config);
            return new BatchGet<T>(this, config);
        }

        /// <summary>
        /// Creates a MultiTableBatchGet object, composed of multiple
        /// individual BatchGet objects.
        /// </summary>
        /// <param name="batches">Individual BatchGet objects</param>
        /// <returns>Composite MultiTableBatchGet object</returns>
        public MultiTableBatchGet CreateMultiTableBatchGet(params BatchGet[] batches)
        {
            return new MultiTableBatchGet(batches);
        }

        /// <summary>
        /// Creates a strongly-typed BatchWrite object, allowing
        /// a batch-write operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to write</typeparam>
        /// <returns>Empty strongly-typed BatchWrite object</returns>
        public BatchWrite<T> CreateBatchWrite<T>()
        {
            return CreateBatchWrite<T>(null);
        }

        /// <summary>
        /// Creates a strongly-typed BatchWrite object, allowing
        /// a batch-write operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to write</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchWrite object</returns>
        public BatchWrite<T> CreateBatchWrite<T>(DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig config = new DynamoDBFlatConfig(operationConfig, this.Config);
            return new BatchWrite<T>(this, config);
        }

        /// <summary>
        /// Creates a MultiTableBatchWrite object, composed of multiple
        /// individual BatchWrite objects.
        /// </summary>
        /// <param name="batches">Individual BatchWrite objects</param>
        /// <returns>Composite MultiTableBatchWrite object</returns>
        public MultiTableBatchWrite CreateMultiTableBatchWrite(params BatchWrite[] batches)
        {
            return new MultiTableBatchWrite(batches);
        }


        #endregion

        #region Save/serialize

        private void SaveHelper<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            if (value == null) return;

            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorage storage = ObjectToItemStorage(value, false, flatConfig);
            if (storage == null) return;

            Table table = GetTargetTable(storage.Config, flatConfig);
            if ((flatConfig.SkipVersionCheck.HasValue && flatConfig.SkipVersionCheck.Value) || !storage.Config.HasVersion)
            {
                table.UpdateHelper(storage.Document, table.MakeKey(storage.Document), null);
            }
            else
            {
                Document expectedDocument = CreateExpectedDocumentForVersion(storage);
                SetNewVersion(storage);
                var updateItemOperationConfig = new UpdateItemOperationConfig
                {
                    Expected = expectedDocument,
                    ReturnValues = ReturnValues.None,
                };
                table.UpdateHelper(storage.Document, table.MakeKey(storage.Document), updateItemOperationConfig);
                PopulateInstance(storage, value, flatConfig);
            }
        }

#if AWS_ASYNC_API 
        private async Task SaveHelperAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken)
        {
            if (value == null) return;

            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorage storage = ObjectToItemStorage(value, false, flatConfig);
            if (storage == null) return;

            Table table = GetTargetTable(storage.Config, flatConfig);
            if (
                (flatConfig.SkipVersionCheck.HasValue && flatConfig.SkipVersionCheck.Value)
                || !storage.Config.HasVersion)
            {
                await table.UpdateHelperAsync(storage.Document, table.MakeKey(storage.Document), null, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Document expectedDocument = CreateExpectedDocumentForVersion(storage);
                SetNewVersion(storage);
                await table.UpdateHelperAsync(
                    storage.Document,
                    table.MakeKey(storage.Document),
                    new UpdateItemOperationConfig { Expected = expectedDocument, ReturnValues = ReturnValues.None },
                    cancellationToken).ConfigureAwait(false);
                PopulateInstance(storage, value, flatConfig);
            }
        }
#endif

        /// <summary>
        /// Serializes an object to a Document.
        /// </summary>
        /// <typeparam name="T">Type to serialize as.</typeparam>
        /// <param name="value">Object to serialize.</param>
        /// <returns>Document with attributes populated from object.</returns>
        public Document ToDocument<T>(T value)
        {
            return ToDocument<T>(value, null);
        }

        /// <summary>
        /// Serializes an object to a Document.
        /// </summary>
        /// <typeparam name="T">Type to serialize as.</typeparam>
        /// <param name="value">Object to serialize.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Document with attributes populated from object.</returns>
        public Document ToDocument<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            if (value == null) return null;

            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            ItemStorage storage = ObjectToItemStorage<T>(value, false, flatConfig);
            if (storage == null) return null;

            return storage.Document;
        }

        #endregion

        #region Load/deserialize

        private T LoadHelper<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Key key = MakeKey(hashKey, rangeKey, storageConfig, flatConfig);
            return LoadHelper<T>(key, flatConfig, storageConfig);
        }

#if AWS_ASYNC_API 
        private Task<T> LoadHelperAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Key key = MakeKey(hashKey, rangeKey, storageConfig, flatConfig);
            return LoadHelperAsync<T>(key, flatConfig, storageConfig, cancellationToken);
        }
#endif

        private T LoadHelper<T>(T keyObject, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Key key = MakeKey<T>(keyObject, storageConfig, flatConfig);
            return LoadHelper<T>(key, flatConfig, storageConfig);
        }

#if AWS_ASYNC_API 
        private Task<T> LoadHelperAsync<T>(T keyObject, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Key key = MakeKey<T>(keyObject, storageConfig, flatConfig);
            return LoadHelperAsync<T>(key, flatConfig, storageConfig, cancellationToken);
        }
#endif

        private T LoadHelper<T>(Key key, DynamoDBFlatConfig flatConfig, ItemStorageConfig storageConfig)
        {
            GetItemOperationConfig getConfig = new GetItemOperationConfig
            {
                ConsistentRead = flatConfig.ConsistentRead.Value,
                AttributesToGet = storageConfig.AttributesToGet
            };

            Table table = GetTargetTable(storageConfig, flatConfig);
            ItemStorage storage = new ItemStorage(storageConfig);
            storage.Document = table.GetItemHelper(key, getConfig);

            T instance = DocumentToObject<T>(storage, flatConfig);
            return instance;
        }

#if AWS_ASYNC_API 
        private async Task<T> LoadHelperAsync<T>(Key key, DynamoDBFlatConfig flatConfig, ItemStorageConfig storageConfig, CancellationToken cancellationToken)
        {
            GetItemOperationConfig getConfig = new GetItemOperationConfig
            {
                ConsistentRead = flatConfig.ConsistentRead.Value,
                AttributesToGet = storageConfig.AttributesToGet
            };

            Table table = GetTargetTable(storageConfig, flatConfig);
            ItemStorage storage = new ItemStorage(storageConfig);
            storage.Document = await table.GetItemHelperAsync(key, getConfig, cancellationToken).ConfigureAwait(false);

            T instance = DocumentToObject<T>(storage, flatConfig);
            return instance;
        }
#endif

        /// <summary>
        /// Deserializes a document to an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="document">Document with properties to use.</param>
        /// <returns>
        /// Object of type T, populated with properties from the document.
        /// </returns>
        public T FromDocument<T>(Document document)
        {
            return FromDocument<T>(document, null);
        }

        /// <summary>
        /// Deserializes a document to an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="document">Document with properties to use.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>
        /// Object of type T, populated with properties from the document.
        /// </returns>
        public T FromDocument<T>(Document document, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            return FromDocumentHelper<T>(document, flatConfig);
        }

        internal T FromDocumentHelper<T>(Document document, DynamoDBFlatConfig flatConfig)
        {
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            ItemStorage storage = new ItemStorage(storageConfig);
            storage.Document = document;
            T instance = DocumentToObject<T>(storage, flatConfig);
            return instance;
        }

        /// <summary>
        /// Deserializes a collections of documents to a collection of instances of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="documents">Documents to deserialize.</param>
        /// <returns>
        /// Collection of items of type T, each populated with properties from a corresponding document.
        /// </returns>
        public IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents)
        {
            return FromDocuments<T>(documents, null);
        }

        /// <summary>
        /// Deserializes a collections of documents to a collection of instances of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="documents">Documents to deserialize.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>
        /// Collection of items of type T, each populated with properties from a corresponding document.
        /// </returns>
        public IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents, DynamoDBOperationConfig operationConfig)
        {
            foreach (var document in documents)
            {
                T item = FromDocument<T>(document, operationConfig);
                yield return item;
            }
        }

        internal IEnumerable<T> FromDocumentsHelper<T>(IEnumerable<Document> documents, DynamoDBFlatConfig flatConfig)
        {
            foreach (var document in documents)
            {
                T item = FromDocumentHelper<T>(document, flatConfig);
                yield return item;
            }
        }

        #endregion

        #region Delete

        private void DeleteHelper<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig config = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(config);
            Key key = MakeKey(hashKey, rangeKey, storageConfig, config);

            Table table = GetTargetTable(storageConfig, config);
            table.DeleteHelper(key, null);
        }

#if AWS_ASYNC_API 
        private Task DeleteHelperAsync<T>(object hashKey, object rangeKey, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken)
        {
            DynamoDBFlatConfig config = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(config);
            Key key = MakeKey(hashKey, rangeKey, storageConfig, config);

            Table table = GetTargetTable(storageConfig, config);
            return table.DeleteHelperAsync(key, null, cancellationToken);
        }
#endif

        private void DeleteHelper<T>(T value, DynamoDBOperationConfig operationConfig)
        {
            if (value == null) throw new ArgumentNullException("value");

            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            flatConfig.IgnoreNullValues = true;
            ItemStorage storage = ObjectToItemStorage<T>(value, true, flatConfig);
            if (storage == null) return;

            Table table = GetTargetTable(storage.Config, flatConfig);
            if (flatConfig.SkipVersionCheck.Value || !storage.Config.HasVersion)
            {
                table.DeleteHelper(table.MakeKey(storage.Document), null);
            }
            else
            {
                Document expectedDocument = CreateExpectedDocumentForVersion(storage);
                table.DeleteHelper(
                    table.MakeKey(storage.Document),
                    new DeleteItemOperationConfig { Expected = expectedDocument });
            }
        }

#if AWS_ASYNC_API

        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        private Task DeleteHelperAsync<T>(T value, DynamoDBOperationConfig operationConfig, CancellationToken cancellationToken)
        {
            if (value == null) throw new ArgumentNullException("value");

            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            flatConfig.IgnoreNullValues = true;
            ItemStorage storage = ObjectToItemStorage(value, true, flatConfig);
            if (storage == null) return CompletedTask;

            Table table = GetTargetTable(storage.Config, flatConfig);
            if (flatConfig.SkipVersionCheck.Value || !storage.Config.HasVersion)
            {
                return table.DeleteHelperAsync(table.MakeKey(storage.Document), null, cancellationToken);
            }
            else
            {
                Document expectedDocument = CreateExpectedDocumentForVersion(storage);
                return table.DeleteHelperAsync(
                    table.MakeKey(storage.Document),
                    new DeleteItemOperationConfig { Expected = expectedDocument },
                    cancellationToken);
            }
        }
#endif

        #endregion
    }
}
