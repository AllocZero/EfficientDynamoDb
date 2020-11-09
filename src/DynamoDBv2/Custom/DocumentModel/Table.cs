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

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Util;
using System.Linq;
using System.Threading;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime.Internal.Util;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// The Table class is the starting object when using the Document API. It is used to Get documents from the DynamnoDB table
    /// and write documents back to the DynamoDB table.
    /// </summary>
    public partial class Table
    {
        #region Private/internal members

        internal enum DynamoDBConsumer
        {
            DocumentModel, DataModel, SessionStateProvider
        }

        private static string TableInfoCacheIdentifier = typeof(Table).FullName;
        private TableConfig Config;

        internal Table.DynamoDBConsumer TableConsumer { get { return Config.Consumer; } }
        internal DynamoDBEntryConversion Conversion { get { return Config.Conversion; } }
        internal bool IsEmptyStringValueEnabled { get {return Config.IsEmptyStringValueEnabled; } }
        internal IEnumerable<string> StoreAsEpoch { get { return Config.AttributesToStoreAsEpoch; } }
        internal IEnumerable<string> KeyNames { get { return Keys.Keys; } }

#if NETSTANDARD
        internal AmazonDynamoDBClient DDBClient { get; private set; }
#else
        internal IAmazonDynamoDB DDBClient { get; private set; }
#endif

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the table.
        /// </summary>
        public string TableName { get { return Config.TableName; } }

        /// <summary>
        /// Keys of the table.
        /// </summary>
        public Dictionary<string, KeyDescription> Keys { get; private set; }

        /// <summary>
        /// Global secondary indexes of the table.
        /// </summary>
        public Dictionary<string, GlobalSecondaryIndexDescription> GlobalSecondaryIndexes { get; private set; }

        /// <summary>
        /// Local secondary indexes of the table.
        /// </summary>
        public Dictionary<string, LocalSecondaryIndexDescription> LocalSecondaryIndexes { get; private set; }

        /// <summary>
        /// Names of the local secondary indexes of the table.
        /// </summary>
        public List<string> LocalSecondaryIndexNames { get; private set; }

        /// <summary>
        /// Names of the global secondary indexes of the table.
        /// </summary>
        public List<string> GlobalSecondaryIndexNames { get; private set; }

        /// <summary>
        /// List of keys on the table marked HASH
        /// </summary>
        public List<string> HashKeys { get; private set; }

        /// <summary>
        /// List of keys on the table marked RANGE
        /// </summary>
        public List<string> RangeKeys { get; private set; }

        /// <summary>
        /// List of key attributes on the table.
        /// </summary>
        public List<AttributeDefinition> Attributes { get; set; }

        #endregion


        #region Private/internal methods

        private static DynamoDBEntryType GetType(string attributeType)
        {
            if (String.Equals(attributeType, "N"))
                return DynamoDBEntryType.Numeric;
            if (String.Equals(attributeType, "S"))
                return DynamoDBEntryType.String;
            if (String.Equals(attributeType, "B"))
                return DynamoDBEntryType.Binary;
            throw new InvalidOperationException("Unknown attribute type");
        }

        private void LoadTableInfo()
        {
            ClearTableData();

            var tableInfoCache = SdkCache.GetCache<string, TableDescription>(DDBClient, TableInfoCacheIdentifier, StringComparer.Ordinal);
            bool staleCacheData;
            TableDescription table = tableInfoCache.GetValue(TableName, this.DescribeTable, out staleCacheData);
            if (staleCacheData)
            {
                var logger = Logger.GetLogger(typeof(Table));
                logger.InfoFormat("Description for table [{0}] loaded from SDK Cache", TableName);
            }

            foreach (var key in table.KeySchema)
            {
                string keyName = key.AttributeName;
                AttributeDefinition attributeDefinition = table.AttributeDefinitions
                    .FirstOrDefault(a => string.Equals(a.AttributeName, keyName, StringComparison.Ordinal));
                if (attributeDefinition == null) throw new InvalidOperationException("No attribute definition found for key " + key.AttributeName);
                KeyDescription keyDescription = new KeyDescription
                {
                    IsHash = string.Equals(key.KeyType, "HASH", StringComparison.OrdinalIgnoreCase),
                    Type = GetType(attributeDefinition.AttributeType)
                };
                if (keyDescription.IsHash)
                    HashKeys.Add(keyName);
                else
                    RangeKeys.Add(keyName);
                Keys[keyName] = keyDescription;
            }

            if (table.LocalSecondaryIndexes != null)
            {
                foreach (var index in table.LocalSecondaryIndexes)
                {
                    LocalSecondaryIndexes[index.IndexName] = index;
                    LocalSecondaryIndexNames.Add(index.IndexName);
                }
            }

            if (table.GlobalSecondaryIndexes != null)
            {
                foreach (var index in table.GlobalSecondaryIndexes)
                {
                    GlobalSecondaryIndexes[index.IndexName] = index;
                    GlobalSecondaryIndexNames.Add(index.IndexName);
                }
            }

            foreach (var attribute in table.AttributeDefinitions)
            {
                Attributes.Add(attribute);
            }
        }

        internal Key MakeKey(IDictionary<string, DynamoDBEntry> doc)
        {
            Key key = new Key();
            foreach (var kvp in Keys)
            {
                string keyName = kvp.Key;
                KeyDescription description = kvp.Value;
                DynamoDBEntry value;
                if (!doc.TryGetValue(keyName, out value))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Document does not contain value for key {0}", keyName));
                value = value.ToConvertedEntry(Conversion);
                if (StoreAsEpoch.Contains(keyName))
                    value = Document.DateTimeToEpochSeconds(value, keyName);

                Primitive primitive = value.AsPrimitive();
                if (primitive == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Key attribute {0} must be a Primitive type", keyName));
                if (primitive.Type != description.Type)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Key attribute {0} must be of type {1}", keyName, description.Type));

                var attributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(Conversion, IsEmptyStringValueEnabled);
                key[keyName] = primitive.ConvertToAttributeValue(attributeConversionConfig);
            }
            return key;
        }

        internal Key MakeKey(Primitive hashKey, Primitive rangeKey)
        {
            Key newKey = new Key();

            if (HashKeys.Count != 1)
                throw new InvalidOperationException("Must have one hash key defined for the table " + TableName);
            string hashKeyName = HashKeys[0];
            KeyDescription hashKeyDescription = Keys[hashKeyName];
            if (this.StoreAsEpoch.Contains(hashKeyName))
                hashKey = KeyDateTimeToEpochSeconds(hashKey, hashKeyName);

            if (hashKeyDescription.Type != hashKey.Type)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Schema for table {0}, hash key {1}, is inconsistent with specified hash key value.", TableName, hashKeyName));

            var hashKeyAttributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(Conversion, IsEmptyStringValueEnabled);
            var hashKeyAttributeValue = hashKey.ConvertToAttributeValue(hashKeyAttributeConversionConfig);
            newKey[hashKeyName] = hashKeyAttributeValue;

            if ((rangeKey == null) != (RangeKeys.Count == 0))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Schema for table {0}, range key {1}, is inconsistent with specified range key value.", TableName, hashKeyName));
            }
            else if (rangeKey != null)
            {
                string rangeKeyName = RangeKeys[0];
                KeyDescription rangeKeyDescription = Keys[rangeKeyName];

                if (this.StoreAsEpoch.Contains(rangeKeyName))
                    rangeKey = KeyDateTimeToEpochSeconds(rangeKey, rangeKeyName);

                if (rangeKeyDescription.Type != rangeKey.Type)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Schema for table {0}, range key {1}, is inconsistent with specified range key value.", TableName, hashKeyName));

                var rangeKeyAttributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(Conversion, IsEmptyStringValueEnabled);
                var rangeKeyAttributeValue = rangeKey.ConvertToAttributeValue(rangeKeyAttributeConversionConfig);
                newKey[rangeKeyName] = rangeKeyAttributeValue;
            }

            return newKey;
        }
        private static Primitive KeyDateTimeToEpochSeconds(Primitive key, string attributeName)
        {
            DynamoDBEntry entry = Document.DateTimeToEpochSeconds(key, attributeName);
            Primitive converted = entry as Primitive;
            return converted;
        }

        internal void UserAgentRequestEventHandlerSync(object sender, RequestEventArgs args)
        {
            UserAgentRequestEventHandler(sender, args, false);
        }
        internal void UserAgentRequestEventHandlerAsync(object sender, RequestEventArgs args)
        {
            UserAgentRequestEventHandler(sender, args, true);
        }

        internal void AddRequestHandler(AmazonDynamoDBRequest request, bool isAsync)
        {
            ((Amazon.Runtime.Internal.IAmazonWebServiceRequest)request).AddBeforeRequestHandler(isAsync ?
                new RequestEventHandler(this.UserAgentRequestEventHandlerAsync) :
                new RequestEventHandler(this.UserAgentRequestEventHandlerSync)
                );
        }

        private void UserAgentRequestEventHandler(object sender, RequestEventArgs args, bool isAsync)
        {
            WebServiceRequestEventArgs wsArgs = args as WebServiceRequestEventArgs;
            if (wsArgs != null)
            {
                if (wsArgs.Headers.Keys.Contains(HeaderKeys.UserAgentHeader))
                {
                    string currentUserAgent = wsArgs.Headers[HeaderKeys.UserAgentHeader];
                    wsArgs.Headers[HeaderKeys.UserAgentHeader] =
                    currentUserAgent + " " + this.TableConsumer.ToString() + " " + (isAsync ? "TableAsync" : "TableSync");
                }
                else if (wsArgs.Headers.Keys.Contains(HeaderKeys.XAmzUserAgentHeader))
                {
                    string currentUserAgent = wsArgs.Headers[HeaderKeys.XAmzUserAgentHeader];
                    wsArgs.Headers[HeaderKeys.XAmzUserAgentHeader] =
                        currentUserAgent + " " + this.TableConsumer.ToString() + " " + (isAsync ? "TableAsync" : "TableSync");
                }
            }
        }

        /// <summary>
        /// Validates that the conditional properties on the config object are correctly set.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException"/>
        /// <param name="config"></param>
        private static void ValidateConditional(IConditionalOperationConfig config)
        {
            if (config == null)
                return;

            int conditionsSet = 0;
            conditionsSet += config.Expected != null ? 1 : 0;
            conditionsSet += config.ExpectedState != null ? 1 : 0;
            conditionsSet += config.ConditionalExpression != null && config.ConditionalExpression.ExpressionStatement != null ? 1 : 0;

            if (conditionsSet > 1)
                throw new InvalidOperationException("Only one of the conditonal properties Expected, ExpectedState and ConditionalExpression can be set.");
        }

        private void ClearTableData()
        {
            Keys = new Dictionary<string, KeyDescription>();
            HashKeys = new List<string>();
            RangeKeys = new List<string>();
            LocalSecondaryIndexes = new Dictionary<string, LocalSecondaryIndexDescription>();
            LocalSecondaryIndexNames = new List<string>();
            GlobalSecondaryIndexes = new Dictionary<string, GlobalSecondaryIndexDescription>();
            GlobalSecondaryIndexNames = new List<string>();
            Attributes = new List<AttributeDefinition>();
        }

        private TableDescription DescribeTable(string tableName)
        {
            DescribeTableRequest req = new DescribeTableRequest
            {
                TableName = TableName
            };
            this.AddRequestHandler(req, isAsync: false);

            var info = this.DDBClient.DescribeTable(req);

            if (info.Table == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Table name {0} does not exist", TableName));
            }

            return info.Table;
        }

        internal Table Copy()
        {
            return this.Copy(Config);
        }
        internal Table Copy(TableConfig newConfig)
        {
            return new Table(this.DDBClient, newConfig)
            {
                Keys = this.Keys,
                RangeKeys = this.RangeKeys,
                HashKeys = this.HashKeys,
                Attributes = this.Attributes,
                GlobalSecondaryIndexes = this.GlobalSecondaryIndexes,
                GlobalSecondaryIndexNames = this.GlobalSecondaryIndexNames,
                LocalSecondaryIndexes = this.LocalSecondaryIndexes,
                LocalSecondaryIndexNames = this.LocalSecondaryIndexNames
            };
        }

        internal Dictionary<string, AttributeValue> ToAttributeMap(Document doc, DynamoDBEntryConversion conversion)
        {
            return doc.ToAttributeMap(conversion, this.StoreAsEpoch, Config.IsEmptyStringValueEnabled);
        }

        #endregion


        #region Constructor/factory

        private Table(IAmazonDynamoDB ddbClient, TableConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (ddbClient == null)
                throw new ArgumentNullException("ddbClient");

#if NETSTANDARD
            DDBClient = ddbClient as AmazonDynamoDBClient;
#else
            DDBClient = ddbClient;
#endif
            Config = config;
        }


        /// <summary>
        /// Clears current table cache. Next time a Table is created, its information
        /// will be loaded from DynamoDB.
        /// </summary>
        public static void ClearTableCache()
        {
            SdkCache.Clear(TableInfoCacheIdentifier);
        }

        /// <summary>
        /// Creates a Table object with the specified configuration, using the
        /// passed-in client to load the table definition.
        /// 
        /// This method will throw an exception if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="config">Configuration to use for the table.</param>
        /// <returns>Table object representing the specified table.</returns>
        public static Table LoadTable(IAmazonDynamoDB ddbClient, TableConfig config)
        {
            Table table = new Table(ddbClient, config);
            table.LoadTableInfo();
            return table;
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        /// The returned table will use the conversion specified by
        /// AWSConfigs.DynamoDBConfig.ConversionSchema
        /// 
        /// This method will throw an exception if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Table object representing the specified table.</returns>
        public static Table LoadTable(IAmazonDynamoDB ddbClient, string tableName)
        {
            return LoadTable(ddbClient, tableName, DynamoDBEntryConversion.CurrentConversion, false);
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        ///
        /// This method will throw an exception if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <returns>Table object representing the specified table.</returns>
        public static Table LoadTable(IAmazonDynamoDB ddbClient, string tableName, DynamoDBEntryConversion conversion)
        {
            return LoadTable(ddbClient, tableName, conversion, false);
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        /// 
        /// This method will throw an exception if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled">If the property is false, empty string values will be interpreted as null values.</param>
        /// <returns>Table object representing the specified table.</returns>
        public static Table LoadTable(IAmazonDynamoDB ddbClient, string tableName, DynamoDBEntryConversion conversion, bool isEmptyStringValueEnabled)
        {
            var config = new TableConfig(tableName, conversion, DynamoDBConsumer.DocumentModel, storeAsEpoch: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);

            return LoadTable(ddbClient, config);
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        /// The returned table will use the conversion specified by
        /// AWSConfigs.DynamoDBConfig.ConversionSchema
        ///
        /// This method will return false if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="table">Loaded table.</param>
        /// <returns>
        /// True if table was successfully loaded; otherwise false.
        /// </returns>
        public static bool TryLoadTable(IAmazonDynamoDB ddbClient, string tableName, out Table table)
        {
            return TryLoadTable(ddbClient, tableName, DynamoDBEntryConversion.CurrentConversion, false, out table);
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        ///
        /// This method will return false if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="table">Loaded table.</param>
        /// <returns>
        /// True if table was successfully loaded; otherwise false.
        /// </returns>
        public static bool TryLoadTable(IAmazonDynamoDB ddbClient, string tableName, DynamoDBEntryConversion conversion, out Table table)
        {
            return TryLoadTable(ddbClient, tableName, conversion, false, out table);
        }

        /// <summary>
        /// Creates a Table object with the specified name, using the
        /// passed-in client to load the table definition.
        /// 
        /// This method will return false if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled">If the property is false, empty string values will be interpreted as null values.</param>
        /// <param name="table">Loaded table.</param>
        /// <returns>
        /// True if table was successfully loaded; otherwise false.
        /// </returns>
        public static bool TryLoadTable(IAmazonDynamoDB ddbClient, string tableName, DynamoDBEntryConversion conversion, bool isEmptyStringValueEnabled, out Table table)
        {
            var config = new TableConfig(tableName, conversion, DynamoDBConsumer.DocumentModel, storeAsEpoch: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);
            return TryLoadTable(ddbClient, config, out table);
        }

        /// <summary>
        /// Creates a Table object with the specified configuration, using the
        /// passed-in client to load the table definition.
        /// 
        /// This method will return false if the table does not exist.
        /// </summary>
        /// <param name="ddbClient">Client to use to access DynamoDB.</param>
        /// <param name="config">Configuration to use for the table.</param>
        /// <param name="table">Loaded table.</param>
        /// <returns>
        /// True if table was successfully loaded; otherwise false.
        /// </returns>
        public static bool TryLoadTable(IAmazonDynamoDB ddbClient, TableConfig config, out Table table)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            try
            {
                table = LoadTable(ddbClient, config);
                return true;
            }
            catch
            {
                table = null;
                return false;
            }
        }
        #endregion

        #region Conversion methods

        /// <summary>
        /// Creates a Document from an attribute map.
        /// </summary>
        /// <param name="data">Map of attribute names to attribute values.</param>
        /// <returns>Document representing the data.</returns>
        public Document FromAttributeMap(Dictionary<string, AttributeValue> data)
        {
            return Document.FromAttributeMap(data, this.StoreAsEpoch);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValue objects.
        /// Converts .NET types using the conversion specified in this Table.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AttributeValue> ToAttributeMap(Document doc)
        {
            return doc.ToAttributeMap(this.Conversion, this.StoreAsEpoch, this.IsEmptyStringValueEnabled);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to ExpectedAttributeValue objects.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ExpectedAttributeValue> ToExpectedAttributeMap(Document doc)
        {
            return doc.ToExpectedAttributeMap(this.Conversion, this.StoreAsEpoch, this.IsEmptyStringValueEnabled);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValueUpdate objects.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="changedAttributesOnly">If true, only attributes that have been changed will be in the map.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValueUpdate> ToAttributeUpdateMap(Document doc, bool changedAttributesOnly)
        {
            return doc.ToAttributeUpdateMap(this.Conversion, changedAttributesOnly, this.StoreAsEpoch, this.IsEmptyStringValueEnabled);
        }


        #endregion

        #region PutItem

        internal Document PutItemHelper(Document doc, PutItemOperationConfig config)
        {
            var currentConfig = config ?? new PutItemOperationConfig();

            PutItemRequest req = new PutItemRequest
            {
                TableName = TableName,
                Item = this.ToAttributeMap(doc)
            };
            this.AddRequestHandler(req, isAsync: false);

            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
                req.ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues);

            ValidateConditional(currentConfig);


            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);
            }

            var resp = DDBClient.PutItem(req);
            doc.CommitChanges();

            Document ret = null;
            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
            {
                ret = this.FromAttributeMap(resp.Attributes);
            }
            return ret;
        }

#if AWS_ASYNC_API
        internal async Task<Document> PutItemHelperAsync(Document doc, PutItemOperationConfig config, CancellationToken cancellationToken)
        {
            var currentConfig = config ?? new PutItemOperationConfig();

            PutItemRequest req = new PutItemRequest
            {
                TableName = TableName,
                Item = this.ToAttributeMap(doc)
            };
            this.AddRequestHandler(req, isAsync: true);

            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
                req.ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues);

            ValidateConditional(currentConfig);


            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);
            }

            var resp = await DDBClient.PutItemAsync(req, cancellationToken).ConfigureAwait(false);
            doc.CommitChanges();

            Document ret = null;
            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
            {
                ret = this.FromAttributeMap(resp.Attributes);
            }
            return ret;
        }
#endif

        #endregion


        #region GetItem

        internal Document GetItemHelper(Key key, GetItemOperationConfig config)
        {
            var currentConfig = config ?? new GetItemOperationConfig();
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = key,
                ConsistentRead = currentConfig.ConsistentRead
            };

            this.AddRequestHandler(request, isAsync: false);

            if (currentConfig.AttributesToGet != null)
                request.AttributesToGet = currentConfig.AttributesToGet;

            var result = DDBClient.GetItem(request);
            var attributeMap = result.Item;
            if (attributeMap == null || attributeMap.Count == 0)
                return null;
            return this.FromAttributeMap(attributeMap);
        }

#if AWS_ASYNC_API
        internal async Task<Document> GetItemHelperAsync(Key key, GetItemOperationConfig config, CancellationToken cancellationToken)
        {
            var currentConfig = config ?? new GetItemOperationConfig();
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = key,
                ConsistentRead = currentConfig.ConsistentRead
            };

            this.AddRequestHandler(request, isAsync: true);

            if (currentConfig.AttributesToGet != null)
                request.AttributesToGet = currentConfig.AttributesToGet;

            var result = await DDBClient.GetItemAsync(request, cancellationToken).ConfigureAwait(false);
            var attributeMap = result.Item;
            if (attributeMap == null || attributeMap.Count == 0)
                return null;
            return this.FromAttributeMap(attributeMap);
        }
#endif

        #endregion


        #region UpdateItem

        internal Document UpdateHelper(Document doc, Primitive hashKey, Primitive rangeKey, UpdateItemOperationConfig config)
        {
            Key key = (hashKey != null || rangeKey != null) ? MakeKey(hashKey, rangeKey) : MakeKey(doc);
            return UpdateHelper(doc, key, config);
        }

#if AWS_ASYNC_API 
        internal Task<Document> UpdateHelperAsync(Document doc, Primitive hashKey, Primitive rangeKey, UpdateItemOperationConfig config, CancellationToken cancellationToken)
        {
            Key key = (hashKey != null || rangeKey != null) ? MakeKey(hashKey, rangeKey) : MakeKey(doc);
            return UpdateHelperAsync(doc, key, config, cancellationToken);
        }
#endif

        internal Document UpdateHelper(Document doc, Key key, UpdateItemOperationConfig config)
        {
            var currentConfig = config ?? new UpdateItemOperationConfig();

            // If the keys have been changed, treat entire document as having changed
            bool haveKeysChanged = HaveKeysChanged(doc);
            bool updateChangedAttributesOnly = !haveKeysChanged;

            var attributeUpdates = this.ToAttributeUpdateMap(doc, updateChangedAttributesOnly);
            foreach (var keyName in this.KeyNames)
            {
                attributeUpdates.Remove(keyName);
            }

            UpdateItemRequest req = new UpdateItemRequest
            {
                TableName = TableName,
                Key = key,
                AttributeUpdates = attributeUpdates.Count == 0 ? null : attributeUpdates, // pass null if keys-only update
                ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues)
            };

            this.AddRequestHandler(req, isAsync: false);

            ValidateConditional(currentConfig);

            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);

                string statement;
                Dictionary<string, AttributeValue> expressionAttributeValues;
                Dictionary<string, string> expressionAttributeNames;
                Common.ConvertAttributeUpdatesToUpdateExpression(attributeUpdates, out statement, out expressionAttributeValues, out expressionAttributeNames);

                req.AttributeUpdates = null;
                req.UpdateExpression = statement;

                if (req.ExpressionAttributeValues == null)
                    req.ExpressionAttributeValues = expressionAttributeValues;
                else
                {
                    foreach (var kvp in expressionAttributeValues)
                        req.ExpressionAttributeValues.Add(kvp.Key, kvp.Value);
                }

                if (req.ExpressionAttributeNames == null)
                    req.ExpressionAttributeNames = expressionAttributeNames;
                else
                {
                    foreach (var kvp in expressionAttributeNames)
                        req.ExpressionAttributeNames.Add(kvp.Key, kvp.Value);
                }
            }

            var resp = DDBClient.UpdateItem(req);
            var returnedAttributes = resp.Attributes;
            doc.CommitChanges();

            Document ret = null;
            if (currentConfig.ReturnValues != ReturnValues.None)
            {
                ret = this.FromAttributeMap(returnedAttributes);
            }
            return ret;
        }

#if AWS_ASYNC_API 
        internal async Task<Document> UpdateHelperAsync(Document doc, Key key, UpdateItemOperationConfig config, CancellationToken cancellationToken)
        {
            var currentConfig = config ?? new UpdateItemOperationConfig();

            // If the keys have been changed, treat entire document as having changed
            bool haveKeysChanged = HaveKeysChanged(doc);
            bool updateChangedAttributesOnly = !haveKeysChanged;

            var attributeUpdates = this.ToAttributeUpdateMap(doc, updateChangedAttributesOnly);
            foreach (var keyName in this.KeyNames)
            {
                attributeUpdates.Remove(keyName);
            }

            UpdateItemRequest req = new UpdateItemRequest
            {
                TableName = TableName,
                Key = key,
                AttributeUpdates = attributeUpdates.Count == 0 ? null : attributeUpdates, // pass null if keys-only update
                ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues)
            };

            this.AddRequestHandler(req, isAsync: true);

            ValidateConditional(currentConfig);

            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);

                string statement;
                Dictionary<string, AttributeValue> expressionAttributeValues;
                Dictionary<string, string> expressionAttributeNames;
                Common.ConvertAttributeUpdatesToUpdateExpression(attributeUpdates, out statement, out expressionAttributeValues, out expressionAttributeNames);

                req.AttributeUpdates = null;
                req.UpdateExpression = statement;

                if (req.ExpressionAttributeValues == null)
                    req.ExpressionAttributeValues = expressionAttributeValues;
                else
                {
                    foreach (var kvp in expressionAttributeValues)
                        req.ExpressionAttributeValues.Add(kvp.Key, kvp.Value);
                }

                if (req.ExpressionAttributeNames == null)
                    req.ExpressionAttributeNames = expressionAttributeNames;
                else
                {
                    foreach (var kvp in expressionAttributeNames)
                        req.ExpressionAttributeNames.Add(kvp.Key, kvp.Value);
                }
            }

            var resp = await DDBClient.UpdateItemAsync(req, cancellationToken).ConfigureAwait(false);
            var returnedAttributes = resp.Attributes;
            doc.CommitChanges();

            Document ret = null;
            if (currentConfig.ReturnValues != ReturnValues.None)
            {
                ret = this.FromAttributeMap(returnedAttributes);
            }
            return ret;
        }
#endif

        // Checks if key attributes have been updated
        private bool HaveKeysChanged(Document doc)
        {
            foreach (var keyName in this.KeyNames)
            {
                if (doc.IsAttributeChanged(keyName))
                    return true;
            }
            return false;
        }

        #endregion


        #region DeleteItem

        internal Document DeleteHelper(Key key, DeleteItemOperationConfig config)
        {
            var currentConfig = config ?? new DeleteItemOperationConfig();

            var req = new DeleteItemRequest
            {
                TableName = TableName,
                Key = key
            };
            this.AddRequestHandler(req, isAsync: false);

            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
                req.ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues);

            ValidateConditional(currentConfig);

            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);
            }

            var attributes = DDBClient.DeleteItem(req).Attributes;

            Document ret = null;
            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
            {
                ret = this.FromAttributeMap(attributes);
            }
            return ret;
        }

#if AWS_ASYNC_API
        internal async Task<Document> DeleteHelperAsync(Key key, DeleteItemOperationConfig config, CancellationToken cancellationToken)
        {
            var currentConfig = config ?? new DeleteItemOperationConfig();

            var req = new DeleteItemRequest
            {
                TableName = TableName,
                Key = key
            };
            this.AddRequestHandler(req, isAsync: true);

            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
                req.ReturnValues = EnumMapper.Convert(currentConfig.ReturnValues);

            ValidateConditional(currentConfig);

            if (currentConfig.Expected != null)
            {
                req.Expected = this.ToExpectedAttributeMap(currentConfig.Expected);
            }
            else if (currentConfig.ExpectedState != null &&
                currentConfig.ExpectedState.ExpectedValues != null &&
                currentConfig.ExpectedState.ExpectedValues.Count > 0)
            {
                req.Expected = currentConfig.ExpectedState.ToExpectedAttributeMap(this);
                if (req.Expected.Count > 1)
                    req.ConditionalOperator = EnumMapper.Convert(currentConfig.ExpectedState.ConditionalOperator);
            }
            else if (currentConfig.ConditionalExpression != null && currentConfig.ConditionalExpression.IsSet)
            {
                currentConfig.ConditionalExpression.ApplyExpression(req, this);
            }

            var attributes = (await DDBClient.DeleteItemAsync(req, cancellationToken).ConfigureAwait(false)).Attributes;

            Document ret = null;
            if (currentConfig.ReturnValues == ReturnValues.AllOldAttributes)
            {
                ret = this.FromAttributeMap(attributes);
            }
            return ret;
        }
#endif

        #endregion


        #region Scan

        /// <summary>
        /// Initiates a Search object to Scan a DynamoDB table, with the
        /// specified filter.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="filter">Filter to apply to the scan.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Scan(ScanFilter filter)
        {
            return Scan(new ScanOperationConfig { Filter = filter });
        }

        /// <summary>
        /// Initiates a Search object to Scan a DynamoDB table, with the
        /// specified expression.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="filterExpression">Expression to apply to the scan.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Scan(Expression filterExpression)
        {
            ScanOperationConfig config = new ScanOperationConfig
            {
                FilterExpression = filterExpression
            };

            return Scan(config);
        }


        /// <summary>
        /// Initiates a Search object to Scan a DynamoDB table, with the
        /// specified config.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="config">Configuration to use.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Scan(ScanOperationConfig config)
        {
            var currentConfig = config ?? new ScanOperationConfig();

            Search ret = new Search(SearchType.Scan)
            {
                SourceTable = this,
                TableName = TableName,
                Limit = currentConfig.Limit,
                Filter = currentConfig.Filter,
                FilterExpression = currentConfig.FilterExpression,
                ConditionalOperator = currentConfig.ConditionalOperator,
                AttributesToGet = currentConfig.AttributesToGet,
                Select = currentConfig.Select,
                CollectResults = currentConfig.CollectResults,
                IndexName = currentConfig.IndexName,
                IsConsistentRead = currentConfig.ConsistentRead,
                PaginationToken = currentConfig.PaginationToken
            };

            if (currentConfig.TotalSegments != 0)
            {
                ret.TotalSegments = currentConfig.TotalSegments;
                ret.Segment = currentConfig.Segment;
            }

            return ret;
        }

        #endregion


        #region Query

        /// <summary>
        /// Initiates a Search object to Query a DynamoDB table, with the
        /// specified hash primary key and filter.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="hashKey">Value of the hash key for the query operation.</param>
        /// <param name="filter">Filter to use.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Query(Primitive hashKey, QueryFilter filter)
        {
            string hashKeyName = this.HashKeys[0];
            QueryFilter fullFilter = new QueryFilter(filter);
            fullFilter.AddCondition(hashKeyName, QueryOperator.Equal, hashKey);

            return Query(fullFilter);
        }


        /// <summary>
        /// Initiates a Search object to Query a DynamoDB table, with the
        /// specified hash primary key and expression.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="hashKey">Value of the hash key for the query operation.</param>
        /// <param name="filterExpression">Expression to use.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Query(Primitive hashKey, Expression filterExpression)
        {
            string hashKeyName = this.HashKeys[0];

            QueryFilter hashKeyFilter = new QueryFilter();
            hashKeyFilter.AddCondition(hashKeyName, QueryOperator.Equal, hashKey);

            QueryOperationConfig config = new QueryOperationConfig
            {
                Filter = hashKeyFilter,
                FilterExpression = filterExpression
            };

            return Query(config);
        }

        /// <summary>
        /// Initiates a Search object to Query a DynamoDB table, with the
        /// specified filter.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="filter">Filter to use.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Query(QueryFilter filter)
        {
            return Query(new QueryOperationConfig { Filter = filter });
        }

        /// <summary>
        /// Initiates a Search object to Query a DynamoDB table, with the
        /// specified config.
        /// 
        /// No calls are made until the Search object is used.
        /// </summary>
        /// <param name="config">Configuration to use.</param>
        /// <returns>Resultant Search container.</returns>
        public Search Query(QueryOperationConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            Search ret = new Search(SearchType.Query)
            {
                SourceTable = this,
                TableName = TableName,
                AttributesToGet = config.AttributesToGet,
                Filter = config.Filter,
                KeyExpression = config.KeyExpression,
                FilterExpression = config.FilterExpression,
                ConditionalOperator = config.ConditionalOperator,
                Limit = config.Limit,
                IsConsistentRead = config.ConsistentRead,
                IsBackwardSearch = config.BackwardSearch,
                IndexName = config.IndexName,
                Select = config.Select,
                CollectResults = config.CollectResults,
                PaginationToken = config.PaginationToken
            };

            return ret;
        }

        #endregion


        #region BatchGet

        /// <summary>
        /// Creates a DocumentBatchGet object for the current table, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <returns>Empty DocumentBatchGet object.</returns>
        public DocumentBatchGet CreateBatchGet()
        {
            return new DocumentBatchGet(this);
        }

        #endregion


        #region BatchWrite

        /// <summary>
        /// Creates a DocumentBatchWrite object for the current table, allowing
        /// a batch-put/delete operation against DynamoDB.
        /// </summary>
        /// <returns>Empty DocumentBatchWrite object.</returns>
        public DocumentBatchWrite CreateBatchWrite()
        {
            return new DocumentBatchWrite(this);
        }

        #endregion
    }
}
