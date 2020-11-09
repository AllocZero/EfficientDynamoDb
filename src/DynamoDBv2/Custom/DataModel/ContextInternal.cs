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
using System.Linq;
using System.IO;
using System.Reflection;

using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

using Amazon.Util.Internal;
using System.Globalization;

namespace Amazon.DynamoDBv2.DataModel
{
    public partial class DynamoDBContext
    {
        #region Versioning

        private static void SetNewVersion(ItemStorage storage)
        {
            if (!storage.Config.HasVersion) return;

            DynamoDBEntry versionEntry;
            Primitive version;
            string versionAttributeName = storage.Config.VersionPropertyStorage.AttributeName;

            if (storage.Document.TryGetValue(versionAttributeName, out versionEntry))
                version = versionEntry as Primitive;
            else
                version = null;

            if (version != null && version.Value != null)
            {
                if (version.Type != DynamoDBEntryType.Numeric) throw new InvalidOperationException("Version property must be numeric");
                PropertyStorage propertyStorage = storage.Config.VersionPropertyStorage;
                IncrementVersion(propertyStorage.MemberType, ref version);
            }
            else
            {
                version = new Primitive("0", true);
            }
            storage.Document[versionAttributeName] = version;
        }
        private static void IncrementVersion(Type memberType, ref Primitive version)
        {
            var memberTypeWrapper = TypeFactory.GetTypeInfo(memberType);
            if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(Byte)))) version = version.AsByte() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(SByte)))) version = version.AsSByte() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(int)))) version = version.AsInt() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(uint)))) version = version.AsUInt() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(long)))) version = version.AsLong() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(ulong)))) version = version.AsULong() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(short)))) version = version.AsShort() + 1;
            else if (memberTypeWrapper.IsAssignableFrom(TypeFactory.GetTypeInfo(typeof(ushort)))) version = version.AsUShort() + 1;
        }
        private static Document CreateExpectedDocumentForVersion(ItemStorage storage)
        {
            Document document = new Document();
            if (storage.Config.HasVersion)
            {
                string versionAttributeName = storage.Config.VersionPropertyStorage.AttributeName;
                if (storage.CurrentVersion == null)
                {
                    document[versionAttributeName] = null;
                }
                else
                {
                    document[versionAttributeName] = storage.CurrentVersion;
                }
            }
            return document;
        }

        #endregion

        #region Table methods

        // Retrieves the target table for the specified type
        private Table GetTargetTableInternal<T>(DynamoDBOperationConfig operationConfig)
        {
            Type type = typeof(T);
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig(type, flatConfig);
            Table table = GetTargetTable(storageConfig, flatConfig, Table.DynamoDBConsumer.DocumentModel);
            return table;
        }

        // Retrieves a new instance of a table configured for the given storage and flat configs.
        internal Table GetTargetTable(ItemStorageConfig storageConfig, DynamoDBFlatConfig flatConfig,
            Table.DynamoDBConsumer consumer = Table.DynamoDBConsumer.DataModel)
        {
            if (flatConfig == null)
                throw new ArgumentNullException("flatConfig");

            string tableName = GetTableName(storageConfig.TableName, flatConfig);
            var unconfiguredTable = GetUnconfiguredTable(tableName);
            ValidateConfigAgainstTable(storageConfig, unconfiguredTable);

            var tableConfig = new TableConfig(tableName, flatConfig.Conversion, consumer,
                storageConfig.AttributesToStoreAsEpoch, flatConfig.IsEmptyStringValueEnabled);
            var table = unconfiguredTable.Copy(tableConfig);
            return table;
        }

        // Retrieves Config-less Table from cache or constructs it on cache-miss
        // This Table should not be used for data operations.
        // To use for data operations, Copy with a TableConfig first.
        internal Table GetUnconfiguredTable(string tableName)
        {
            Table table;
            lock (tablesMapLock)
            {
                if (!tablesMap.TryGetValue(tableName, out table))
                {
                    var emptyConfig = new TableConfig(tableName, conversion: null, consumer: Table.DynamoDBConsumer.DataModel,
                        storeAsEpoch: null, isEmptyStringValueEnabled: false);
                    table = Table.LoadTable(Client, emptyConfig);
                    tablesMap[tableName] = table;
                }
            }
            return table;
        }

        internal static string GetTableName(string baseTableName, DynamoDBFlatConfig flatConfig)
        {
            if (flatConfig == null)
                throw new ArgumentNullException("flatConfig");

            string tableName = baseTableName;

            if (!string.IsNullOrEmpty(flatConfig.OverrideTableName))
                tableName = flatConfig.OverrideTableName;
            if (!string.IsNullOrEmpty(flatConfig.TableNamePrefix))
                tableName = flatConfig.TableNamePrefix + tableName;

            return tableName;
        }

        private static void ValidateConfigAgainstTable(ItemStorageConfig config, Table table)
        {
            CompareKeys(config, table, table.HashKeys, config.HashKeyPropertyNames, "hash");
            CompareKeys(config, table, table.RangeKeys, config.RangeKeyPropertyNames, "range");
        }
        private static void CompareKeys(ItemStorageConfig config, Table table, List<string> attributes, List<string> properties, string keyType)
        {
            if (attributes.Count != properties.Count)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, 
                    "Number of {0} keys on table {1} does not match number of hash keys on type {2}",
                    keyType, table.TableName, config.TargetTypeInfo.FullName));
            foreach (string hashProperty in properties)
            {
                PropertyStorage property = config.GetPropertyStorage(hashProperty);
                if (!attributes.Contains(property.AttributeName))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, 
                        "Key property {0} on type {1} does not correspond to a {2} key on table {3}",
                        hashProperty, config.TargetTypeInfo.FullName, keyType, table.TableName));
            }
        }

        #endregion

        #region Marshalling/unmarshalling

        // Check if DynamoDBEntry is supported
        private static bool ShouldSave(DynamoDBEntry entry, bool ignoreNullValues)
        {
            if (entry == null)
            {
                if (ignoreNullValues)
                    return false;
                else
                    return true;
            }

            var primitive = entry as Primitive;
            if (primitive != null)
                return (primitive.Value != null);

            var primitiveList = entry as PrimitiveList;
            if (primitiveList != null)
                return (primitiveList.Entries != null && primitiveList.Entries.Count > 0);

            var dynamoDBBool = entry as DynamoDBBool;
            if (dynamoDBBool != null)
                return true;

            var dynamoDBNull = entry as DynamoDBNull;
            if (dynamoDBNull != null)
                return true;

            var dynamoDBList = entry as DynamoDBList;
            if (dynamoDBList != null)
                return (dynamoDBList.Entries != null);

            var document = entry as Document;
            if (document != null)
                return true;

            throw new InvalidOperationException("Unrecognized DynamoDBEntry object");
        }

        // Deserializing DynamoDB document into an object
        private T DocumentToObject<T>(ItemStorage storage, DynamoDBFlatConfig flatConfig)
        {
            Type type = typeof(T);
            return (T)DocumentToObject(type, storage, flatConfig);
        }
        private object DocumentToObject(Type objectType, ItemStorage storage, DynamoDBFlatConfig flatConfig)
        {
            if (storage == null) throw new ArgumentNullException("storage");

            if (storage.Document == null) return null;

            object instance = Utils.InstantiateConverter(objectType, this);
            PopulateInstance(storage, instance, flatConfig);
            return instance;
        }
        private void PopulateInstance(ItemStorage storage, object instance, DynamoDBFlatConfig flatConfig)
        {
            ItemStorageConfig config = storage.Config;
            Document document = storage.Document;

            using (flatConfig.State.Track(document))
            {
                foreach (PropertyStorage propertyStorage in config.AllPropertyStorage)
                {
                    string propertyName = propertyStorage.PropertyName;
                    string attributeName = propertyStorage.AttributeName;

                    DynamoDBEntry entry;
                    if (document.TryGetValue(attributeName, out entry))
                    {
                        if (ShouldSave(entry, true))
                        {
                            object value = FromDynamoDBEntry(propertyStorage, entry, flatConfig);

                            if (!TrySetValue(instance, propertyStorage.Member, value))
                            {
                                throw new InvalidOperationException("Unable to retrieve value from " + attributeName);
                            }
                        }

                        if (propertyStorage.IsVersion)
                            storage.CurrentVersion = entry as Primitive;
                    }
                }
            }
        }

        // Serializing an object into a DynamoDB document
        private ItemStorage ObjectToItemStorage<T>(T toStore, bool keysOnly, DynamoDBFlatConfig flatConfig)
        {
            if (toStore == null) return null;

            Type objectType = typeof(T);
            return ObjectToItemStorage(toStore, objectType, keysOnly, flatConfig);
        }

        private ItemStorage ObjectToItemStorage(object toStore, Type objectType, bool keysOnly, DynamoDBFlatConfig flatConfig)
        {
            ItemStorageConfig config = StorageConfigCache.GetConfig(objectType, flatConfig);
            ItemStorage storage = ObjectToItemStorageHelper(toStore, config, flatConfig, keysOnly, flatConfig.IgnoreNullValues.Value);
            return storage;
        }
        internal ItemStorage ObjectToItemStorageHelper(object toStore, ItemStorageConfig config, DynamoDBFlatConfig flatConfig, bool keysOnly, bool ignoreNullValues)
        {
            ItemStorage storage = new ItemStorage(config);
            PopulateItemStorage(toStore, storage, flatConfig, keysOnly, ignoreNullValues);
            return storage;
        }
        private void PopulateItemStorage(object toStore, ItemStorage storage, DynamoDBFlatConfig flatConfig, bool keysOnly, bool ignoreNullValues)
        {
            ItemStorageConfig config = storage.Config;
            Document document = storage.Document;

            using (flatConfig.State.Track(toStore))
            {
                foreach (PropertyStorage propertyStorage in config.AllPropertyStorage)
                {
                    // if only keys are being serialized, skip non-key properties
                    // still include version, however, to populate the storage.CurrentVersion field
                    if (keysOnly && !propertyStorage.IsHashKey && !propertyStorage.IsRangeKey && !propertyStorage.IsVersion) continue;

                    string propertyName = propertyStorage.PropertyName;
                    string attributeName = propertyStorage.AttributeName;

                    object value;
                    if (TryGetValue(toStore, propertyStorage.Member, out value))
                    {
                        DynamoDBEntry dbe = ToDynamoDBEntry(propertyStorage, value, flatConfig);

                        if (ShouldSave(dbe, ignoreNullValues))
                        {
                            Primitive dbePrimitive = dbe as Primitive;
                            if (propertyStorage.IsHashKey || propertyStorage.IsRangeKey || propertyStorage.IsVersion || propertyStorage.IsLSIRangeKey)
                            {
                                if (dbe != null && dbePrimitive == null)
                                    throw new InvalidOperationException("Property " + propertyName + " is a hash key, range key or version property and must be Primitive");
                            }
                            document[attributeName] = dbe;

                            if (propertyStorage.IsVersion)
                                storage.CurrentVersion = dbePrimitive;
                        }
                    }
                    else
                        throw new InvalidOperationException("Unable to retrieve value from property " + propertyName);
                }
            }
        }

        // DynamoDBEntry <--> Object
        private object FromDynamoDBEntry(SimplePropertyStorage propertyStorage, DynamoDBEntry entry, DynamoDBFlatConfig flatConfig)
        {
            var converter = propertyStorage.Converter;
            if (converter != null)
                return converter.FromEntry(entry);

            var conversion = flatConfig.Conversion;
            var targetType = propertyStorage.MemberType;
            
            if (conversion.HasConverter(targetType))
                return conversion.ConvertFromEntry(targetType, entry);
            else
            {
                object output;
                Document document = entry as Document;
                if (document != null)
                {
                    if (TryFromMap(targetType, document, flatConfig, out output))
                        return output;

                    return DeserializeFromDocument(document, targetType, flatConfig);
                }

                DynamoDBList list = entry as DynamoDBList;
                if (list != null &&
                    TryFromList(targetType, list, flatConfig, out output))
                {
                    return output;
                }

                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to convert DynamoDB entry [{0}] of type {1} to property {2} of type {3}",
                    entry, entry.GetType().FullName, propertyStorage.PropertyName, propertyStorage.MemberType.FullName));
            }
        }
        private bool TryFromList(Type targetType, DynamoDBList list, DynamoDBFlatConfig flatConfig, out object output)
        {
            var targetTypeWrapper = TypeFactory.GetTypeInfo(targetType);
            return targetTypeWrapper.IsArray ?
                 TryFromListToArray(targetType, list, flatConfig, out output) : //targetType is Array
                 TryFromListToIList(targetType, list, flatConfig, out output) ; //targetType is IList or has Add method.
        }

        private bool TryFromListToIList(Type targetType, DynamoDBList list, DynamoDBFlatConfig flatConfig, out object output)
        {
            var targetTypeWrapper = TypeFactory.GetTypeInfo(targetType);
            if ((!Utils.ImplementsInterface(targetType, typeof(ICollection<>)) &&
                !Utils.ImplementsInterface(targetType, typeof(IList))) ||
                !Utils.CanInstantiate(targetType))
            {
                output = null;
                return false;
            }

            var elementType = targetTypeWrapper.GetGenericArguments()[0];
            var collection = Utils.Instantiate(targetType);
            IList ilist = collection as IList;
            bool useIListInterface = ilist != null;
            var propertyStorage = new SimplePropertyStorage(elementType);

            MethodInfo collectionAdd = null;
            if (!useIListInterface)
            {
                collectionAdd = targetTypeWrapper.GetMethod("Add");
            }

            foreach (DynamoDBEntry entry in list.Entries)
            {
                var item = FromDynamoDBEntry(propertyStorage, entry, flatConfig);

                if (useIListInterface)
                    ilist.Add(item);
                else
                    collectionAdd.Invoke(collection, new object[] { item });
            }

            output = collection;
            return true;
        }

        private bool TryFromListToArray(Type targetType, DynamoDBList list, DynamoDBFlatConfig flatConfig, out object output)
        {
            if (!Utils.CanInstantiateArray(targetType))
            {
                output = null;
                return false;
            }

            var elementType = Utils.GetElementType(targetType);
            var array = (Array)Utils.InstantiateArray(targetType,list.Entries.Count);
            var propertyStorage = new SimplePropertyStorage(elementType);


            for (int i = 0; i < list.Entries.Count; i++)
            {
                var entry = list.Entries[i];
                var item = FromDynamoDBEntry(propertyStorage, entry, flatConfig);
                array.SetValue(item,i);
            }

            output = array;
            return true;
        }

        private bool TryFromMap(Type targetType, Document map, DynamoDBFlatConfig flatConfig, out object output)
        {
            output = null;

            if (!Utils.CanInstantiate(targetType))
                return false;

            Type valueType;
            if (!IsSupportedDictionaryType(targetType, out valueType))
                return false;

            var dictionary = Utils.Instantiate(targetType);
            var idictionary = dictionary as IDictionary;
            var propertyStorage = new SimplePropertyStorage(valueType);

            foreach (var kvp in map)
            {
                var key = kvp.Key;
                var entry = kvp.Value;

                var item = FromDynamoDBEntry(propertyStorage, entry, flatConfig);
                idictionary.Add(key, item);
            }

            output = dictionary;
            return true;
        }

        private DynamoDBEntry ToDynamoDBEntry(SimplePropertyStorage propertyStorage, object value, DynamoDBFlatConfig flatConfig)
        {
            return ToDynamoDBEntry(propertyStorage, value, flatConfig, canReturnScalarInsteadOfList: false);
        }
        private DynamoDBEntry ToDynamoDBEntry(SimplePropertyStorage propertyStorage, object value, DynamoDBFlatConfig flatConfig, bool canReturnScalarInsteadOfList)
        {
            if (value == null)
                return null;

            var conversion = flatConfig.Conversion;
            var converter = propertyStorage.Converter;
            if (converter != null)
            {
                var entry = converter.ToEntry(value);
                if (entry != null)
                    entry = entry.ToConvertedEntry(conversion);
                return entry;
            }

            var type = propertyStorage.MemberType;

            if (canReturnScalarInsteadOfList)
            {
                DynamoDBEntry entry = null;
                if (TryToScalar(value, type, flatConfig, ref entry))
                    return entry;
            }

            if (conversion.HasConverter(type))
                return conversion.ConvertToEntry(type, value);
            else
            {
                Document map;
                if (TryToMap(value, type, flatConfig, out map))
                    return map;

                DynamoDBList list;
                if (TryToList(value, type, flatConfig, out list))
                    return list;

                return SerializeToDocument(value, type, flatConfig);
            }
        }
        private bool TryToMap(object value, Type type, DynamoDBFlatConfig flatConfig, out Document output)
        {
            output = null;

            ITypeInfo typeWrapper;
            Type keyType, valueType;
            if (!IsSupportedDictionaryType(type, out typeWrapper, out keyType, out valueType))
                return false;

            var idictionary = value as IDictionary;
            if (idictionary == null)
                return false;

            output = new Document();
            SimplePropertyStorage propertyStorage = new SimplePropertyStorage(valueType);

            foreach (object keyValue in idictionary.Keys)
            {
                object item = idictionary[keyValue];
                string key = keyValue as string;
                if (key == null)
                    continue;

                DynamoDBEntry entry;
                if (item == null)
                    entry = DynamoDBNull.Null;
                else
                    entry = ToDynamoDBEntry(propertyStorage, item, flatConfig);

                output[key] = entry;
            }
            return true;
        }
        private bool TryToList(object value, Type type, DynamoDBFlatConfig flatConfig, out DynamoDBList output)
        {
            if (!Utils.ImplementsInterface(type, typeof(ICollection<>)))
            {
                output = null;
                return false;
            }

            IEnumerable enumerable = value as IEnumerable;

            // Strings are collections of chars, don't treat them as collections
            if (enumerable == null || value is string)
            {
                output = null;
                return false;
            }

            Type elementType = Utils.GetElementType(type);
            SimplePropertyStorage propertyStorage = new SimplePropertyStorage(elementType);
            output = new DynamoDBList();
            foreach (var item in enumerable)
            {
                DynamoDBEntry entry;
                if (item == null)
                    entry = DynamoDBNull.Null;
                else
                    entry = ToDynamoDBEntry(propertyStorage, item, flatConfig);

                output.Add(entry);
            }
            return true;
        }
        private bool TryToScalar(object value, Type type, DynamoDBFlatConfig flatConfig, ref DynamoDBEntry entry)
        {
            var typeWrapper = TypeFactory.GetTypeInfo(type);
            var elementType = Utils.GetElementType(type);
            if (elementType != null)
            {
                IEnumerable enumerable = value as IEnumerable;

                // Strings are collections of chars, don't treat them as collections
                if (enumerable == null || value is string)
                {
                    // Only convert if value matches collection element type
                    if (TypeFactory.GetTypeInfo(value.GetType()).IsAssignableFrom(TypeFactory.GetTypeInfo(elementType)))
                    {
                        DynamoDBEntryConversion conversion = flatConfig.Conversion;
                        if (conversion.HasConverter(elementType))
                        {
                            if (conversion.TryConvertToEntry(elementType, value, out entry))
                                return true;
                        }
                        else
                        {
                            try
                            {
                                entry = SerializeToDocument(value, elementType, flatConfig);
                                return true;
                            }
                            catch { }
                        }
                    }
                }
            }

            return false;
        }

        private static bool IsSupportedDictionaryType(Type type, out Type valueType)
        {
            ITypeInfo typeWrapper;
            Type keyType;
            return IsSupportedDictionaryType(type, out typeWrapper, out keyType, out valueType);
        }
        private static bool IsSupportedDictionaryType(Type type, out ITypeInfo typeWrapper, out Type keyType, out Type valueType)
        {
            keyType = valueType = null;
            typeWrapper = null;

            // Type must implement both IDictionary<TKey,TValue> and IDictionary
            if (!(Utils.ImplementsInterface(type, typeof(IDictionary<,>)) &&
                 Utils.ImplementsInterface(type, typeof(IDictionary))))
                return false;

            typeWrapper = TypeFactory.GetTypeInfo(type);
            var genericArguments = typeWrapper.GetGenericArguments();
            if (genericArguments.Length != 2)
                return false;
            keyType = genericArguments[0];
            valueType = genericArguments[1];
            if (keyType != typeof(string) || valueType == typeof(object))
                return false;

            return true;
        }

        // Deserializes a given Document to instance of targetType
        // Use only for property conversions, not for full item conversion
        private object DeserializeFromDocument(Document document, Type targetType, DynamoDBFlatConfig flatConfig)
        {
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig(targetType, flatConfig, conversionOnly: true);
            ItemStorage storage = new ItemStorage(storageConfig);
            storage.Document = document;
            object value = DocumentToObject(targetType, storage, flatConfig);
            return value;
        }
        // Serializes a given value to Document
        // Use only for property conversions, not for full item conversion
        private Document SerializeToDocument(object value, Type type, DynamoDBFlatConfig flatConfig)
        {
            ItemStorageConfig config = StorageConfigCache.GetConfig(type, flatConfig, conversionOnly: true);
            var itemStorage = ObjectToItemStorageHelper(value, config, flatConfig, keysOnly: false, ignoreNullValues: flatConfig.IgnoreNullValues.Value);
            var doc = itemStorage.Document;
            return doc;
        }

        // Get/Set object properties
        private static bool TrySetValue(object instance, MemberInfo member, object value)
        {
            FieldInfo fieldInfo = member as FieldInfo;
            PropertyInfo propertyInfo = member as PropertyInfo;

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(instance, value);
                return true;
            }
            else if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, value, null);
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool TryGetValue(object instance, MemberInfo member, out object value)
        {
            FieldInfo fieldInfo = member as FieldInfo;
            PropertyInfo propertyInfo = member as PropertyInfo;
            
            if (fieldInfo != null)
            {
                value = fieldInfo.GetValue(instance);
                return true;
            }
            else if (propertyInfo != null)
            {
                value = propertyInfo.GetValue(instance, null);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        // Query/Scan building
        private ScanFilter ComposeScanFilter(IEnumerable<ScanCondition> conditions, ItemStorageConfig storageConfig, DynamoDBFlatConfig flatConfig)
        {
            ScanFilter filter = new ScanFilter();
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    PropertyStorage propertyStorage = storageConfig.GetPropertyStorage(condition.PropertyName);
                    List<AttributeValue> attributeValues = new List<AttributeValue>();
                    foreach (var value in condition.Values)
                    {
                        var entry = ToDynamoDBEntry(propertyStorage, value, flatConfig, canReturnScalarInsteadOfList: true);
                        if (entry == null)
                            throw new InvalidOperationException(
                                string.Format(CultureInfo.InvariantCulture, "Unable to convert value corresponding to property [{0}] to DynamoDB representation", condition.PropertyName));

                        var attributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(flatConfig.Conversion, flatConfig.IsEmptyStringValueEnabled);
                        AttributeValue nativeValue = entry.ConvertToAttributeValue(attributeConversionConfig);
                        if (nativeValue != null)
                        {
                            attributeValues.Add(nativeValue);
                        }
                    }
                    filter.AddCondition(propertyStorage.AttributeName, condition.Operator, attributeValues);
                }
            }
            return filter;
        }

        private QueryFilter ComposeQueryFilter(DynamoDBFlatConfig currentConfig, object hashKeyValue, IEnumerable<QueryCondition> conditions, ItemStorageConfig storageConfig, out List<string> indexNames)
        {
            if (hashKeyValue == null)
                throw new ArgumentNullException("hashKeyValue");

            // Set hash key property name
            // In case of index queries, if GSI, different key could be used
            string hashKeyProperty = storageConfig.HashKeyPropertyNames[0];
            hashKeyProperty = storageConfig.GetCorrectHashKeyProperty(currentConfig, hashKeyProperty);

            PropertyStorage propertyStorage = storageConfig.GetPropertyStorage(hashKeyProperty);
            string hashAttributeName = propertyStorage.AttributeName;

            DynamoDBEntry hashKeyEntry = ValueToDynamoDBEntry(propertyStorage, hashKeyValue, currentConfig);
            if (hashKeyEntry == null) throw new InvalidOperationException("Unable to convert hash key value for property " + hashKeyProperty);

            Document hashKey = new Document();
            hashKey[hashAttributeName] = hashKeyEntry;

            return ComposeQueryFilterHelper(currentConfig, hashKey, conditions, storageConfig, out indexNames);
        }

        private static string NO_INDEX = DynamoDBFlatConfig.DefaultIndexName;
        // This method composes the query filter and determines the possible indexes that the filter
        // may be used against. In the case where the condition property is also a RANGE key on the
        // table and not just on LSI/GSI, the potential index will be "" (absent).
        private QueryFilter ComposeQueryFilterHelper(
            DynamoDBFlatConfig currentConfig,
            Document hashKey,
            IEnumerable<QueryCondition> conditions,
            ItemStorageConfig storageConfig,
            out List<string> indexNames)
        {
            if (hashKey == null)
                throw new ArgumentNullException("hashKey");

            if (storageConfig.HashKeyPropertyNames.Count != 1)
                throw new InvalidOperationException("Must have one hash key defined for the table " + storageConfig.TableName);
            if (storageConfig.RangeKeyPropertyNames.Count != 1 && storageConfig.IndexNameToGSIMapping.Count == 0)
                throw new InvalidOperationException("Must have one range key or a GSI index defined for the table " + storageConfig.TableName);

            QueryFilter filter = new QueryFilter();

            // Configure hash-key equality condition
            string hashKeyProperty = storageConfig.HashKeyPropertyNames[0];
            hashKeyProperty = storageConfig.GetCorrectHashKeyProperty(currentConfig, hashKeyProperty);
            PropertyStorage propertyStorage = storageConfig.GetPropertyStorage(hashKeyProperty);
            string attributeName = propertyStorage.AttributeName;
            DynamoDBEntry hashValue = hashKey[attributeName];
            filter.AddCondition(attributeName, QueryOperator.Equal, hashValue);

            indexNames = new List<string>();
            if (conditions != null)
            {
                foreach (QueryCondition condition in conditions)
                {
                    object[] conditionValues = condition.Values;
                    PropertyStorage conditionProperty = storageConfig.GetPropertyStorage(condition.PropertyName);
                    if (conditionProperty.IsLSIRangeKey || conditionProperty.IsGSIKey)
                        indexNames.AddRange(conditionProperty.IndexNames);
                    if (conditionProperty.IsRangeKey)
                        indexNames.Add(NO_INDEX);
                    List<AttributeValue> attributeValues = ConvertConditionValues(conditionValues, conditionProperty, currentConfig);
                    filter.AddCondition(conditionProperty.AttributeName, condition.Operator, attributeValues);
                }
            }
            if (currentConfig.QueryFilter != null)
            {
                foreach (ScanCondition condition in currentConfig.QueryFilter)
                {
                    object[] conditionValues = condition.Values;
                    PropertyStorage conditionProperty = storageConfig.GetPropertyStorage(condition.PropertyName);
                    List<AttributeValue> attributeValues = ConvertConditionValues(conditionValues, conditionProperty, currentConfig, canReturnScalarInsteadOfList: true);
                    filter.AddCondition(conditionProperty.AttributeName, condition.Operator, attributeValues);
                }
            }
            return filter;
        }

        private List<AttributeValue> ConvertConditionValues(object[] conditionValues, PropertyStorage conditionProperty, DynamoDBFlatConfig flatConfig, bool canReturnScalarInsteadOfList = false)
        {
            List<AttributeValue> attributeValues = new List<AttributeValue>();
            foreach (var conditionValue in conditionValues)
            {
                DynamoDBEntry entry = ToDynamoDBEntry(conditionProperty, conditionValue, flatConfig, canReturnScalarInsteadOfList);
                var attributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(flatConfig.Conversion, flatConfig.IsEmptyStringValueEnabled);
                AttributeValue attributeValue = entry.ConvertToAttributeValue(attributeConversionConfig);
                attributeValues.Add(attributeValue);
            }
            return attributeValues;
        }

        private static string GetQueryIndexName(DynamoDBFlatConfig flatConfig, List<string> indexNames)
        {
            string specifiedIndexName = flatConfig.IndexName;

            // remove possible duplicate indexes
            indexNames = indexNames.Distinct(StringComparer.Ordinal).ToList();

            string inferredIndexName = null;
            if (string.IsNullOrEmpty(specifiedIndexName) && indexNames.Count == 1)
            {
                inferredIndexName = indexNames[0];
            }
            else if (indexNames.Contains(specifiedIndexName, StringComparer.Ordinal))
            {
                inferredIndexName = specifiedIndexName;
            }
            else if (string.IsNullOrEmpty(inferredIndexName) && indexNames.Count > 0)
                throw new InvalidOperationException("Local Secondary Index range key conditions are used but no index could be inferred from model. Specified index name = " + specifiedIndexName);

            // index is both specified and inferred
            if (!string.IsNullOrEmpty(specifiedIndexName) && !string.IsNullOrEmpty(inferredIndexName))
            {
                // check that the indexes are equal
                if (string.Equals(inferredIndexName, specifiedIndexName, StringComparison.Ordinal))
                    return inferredIndexName;
                else
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Specified index name {0} does not match with inferred index name {1}", specifiedIndexName, inferredIndexName));
            }

            if (!string.IsNullOrEmpty(inferredIndexName))
                return inferredIndexName;

            if (!string.IsNullOrEmpty(specifiedIndexName))
                return specifiedIndexName;

            return null;
        }

        private static List<QueryCondition> CreateQueryConditions(DynamoDBFlatConfig flatConfig, QueryOperator op, IEnumerable<object> values, ItemStorageConfig storageConfig)
        {
            string rangeKeyPropertyName;

            string indexName = flatConfig.IndexName;
            if (string.IsNullOrEmpty(indexName))
                rangeKeyPropertyName = storageConfig.RangeKeyPropertyNames.FirstOrDefault();
            else
                rangeKeyPropertyName = storageConfig.GetRangeKeyByIndex(indexName);

            List<QueryCondition> conditions = new List<QueryCondition>
            {
                new QueryCondition(rangeKeyPropertyName, op, values.ToArray())
            };
            return conditions;
        }

        // Key creation
        private DynamoDBEntry ValueToDynamoDBEntry(PropertyStorage propertyStorage, object value, DynamoDBFlatConfig flatConfig)
        {
            var entry = ToDynamoDBEntry(propertyStorage, value, flatConfig);
            return entry;
        }
        private static void ValidateKey(Key key, ItemStorageConfig storageConfig)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (storageConfig == null) throw new ArgumentNullException("storageConfig");
            if (key.Count == 0) throw new InvalidOperationException("Key is empty");

            foreach (string hashKey in storageConfig.HashKeyPropertyNames)
            {
                string attributeName = storageConfig.GetPropertyStorage(hashKey).AttributeName;
                if (!key.ContainsKey(attributeName))
                    throw new InvalidOperationException("Key missing hash key " + hashKey);
            }
            foreach (string rangeKey in storageConfig.RangeKeyPropertyNames)
            {
                string attributeName = storageConfig.GetPropertyStorage(rangeKey).AttributeName;
                if (!key.ContainsKey(attributeName))
                    throw new InvalidOperationException("Key missing range key " + rangeKey);
            }
        }

        internal Key MakeKey(object hashKey, object rangeKey, ItemStorageConfig storageConfig, DynamoDBFlatConfig flatConfig)
        {
            if (storageConfig.HashKeyPropertyNames.Count != 1)
                throw new InvalidOperationException("Must have one hash key defined for the table " + storageConfig.TableName);

            Key key = new Key();

            string hashKeyPropertyName = storageConfig.HashKeyPropertyNames[0];
            PropertyStorage hashKeyProperty = storageConfig.GetPropertyStorage(hashKeyPropertyName);

            DynamoDBEntry hashKeyEntry = ValueToDynamoDBEntry(hashKeyProperty, hashKey, flatConfig);
            if (hashKeyEntry == null) throw new InvalidOperationException("Unable to convert hash key value for property " + hashKeyPropertyName);
            if (storageConfig.AttributesToStoreAsEpoch.Contains(hashKeyProperty.AttributeName))
                hashKeyEntry = Document.DateTimeToEpochSeconds(hashKeyEntry, hashKeyProperty.AttributeName);
            var hashKeyEntryAttributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(flatConfig.Conversion, flatConfig.IsEmptyStringValueEnabled);
            key[hashKeyProperty.AttributeName] = hashKeyEntry.ConvertToAttributeValue(hashKeyEntryAttributeConversionConfig);

            if (storageConfig.RangeKeyPropertyNames.Count > 0)
            {
                if (storageConfig.RangeKeyPropertyNames.Count != 1)
                    throw new InvalidOperationException("Must have one range key defined for the table");

                string rangeKeyPropertyName = storageConfig.RangeKeyPropertyNames[0];
                PropertyStorage rangeKeyProperty = storageConfig.GetPropertyStorage(rangeKeyPropertyName);

                DynamoDBEntry rangeKeyEntry = ValueToDynamoDBEntry(rangeKeyProperty, rangeKey, flatConfig);
                if (rangeKeyEntry == null) throw new InvalidOperationException("Unable to convert range key value for property " + rangeKeyPropertyName);
                if (storageConfig.AttributesToStoreAsEpoch.Contains(rangeKeyProperty.AttributeName))
                    rangeKeyEntry = Document.DateTimeToEpochSeconds(rangeKeyEntry, rangeKeyProperty.AttributeName);

                var rangeKeyEntryAttributeConversionConfig = new DynamoDBEntry.AttributeConversionConfig(flatConfig.Conversion, flatConfig.IsEmptyStringValueEnabled);
                key[rangeKeyProperty.AttributeName] = rangeKeyEntry.ConvertToAttributeValue(rangeKeyEntryAttributeConversionConfig);
            }

            ValidateKey(key, storageConfig);
            return key;
        }
        internal Key MakeKey<T>(T keyObject, ItemStorageConfig storageConfig, DynamoDBFlatConfig flatConfig)
        {
            ItemStorage keyAsStorage = ObjectToItemStorageHelper(keyObject, storageConfig, flatConfig, keysOnly: true, ignoreNullValues: true);
            if (storageConfig.HasVersion) // if version field is defined, it would have been returned, so remove before making the key
                keyAsStorage.Document[storageConfig.VersionPropertyStorage.AttributeName] = null;
            Key key = new Key(keyAsStorage.Document.ToAttributeMap(flatConfig.Conversion, storageConfig.AttributesToStoreAsEpoch, flatConfig.IsEmptyStringValueEnabled));
            ValidateKey(key, storageConfig);
            return key;
        }

        // Searching
        internal class ContextSearch
        {
            public DynamoDBFlatConfig FlatConfig { get; set; }
            public Search Search { get; set; }

            public ContextSearch(Search search, DynamoDBFlatConfig flatConfig)
            {
                Search = search;
                FlatConfig = flatConfig;
            }
        }
        private IEnumerable<T> FromSearch<T>(ContextSearch cs)
        {
            if (cs == null) throw new ArgumentNullException("cs");

            // Configure search to not collect results
            cs.Search.CollectResults = false;

            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(cs.FlatConfig);
            while (!cs.Search.IsDone)
            {
                List<Document> set = cs.Search.GetNextSetHelper();
                foreach (var document in set)
                {
                    ItemStorage storage = new ItemStorage(storageConfig);
                    storage.Document = document;
                    T instance = DocumentToObject<T>(storage, cs.FlatConfig);
                    yield return instance;
                }
            }

            // Reset search to allow retrieving items more than once
            cs.Search.Reset();
        }

        #endregion

        #region Scan/Query

        private ContextSearch ConvertScan<T>(IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, this.Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            ScanFilter filter = ComposeScanFilter(conditions, storageConfig, flatConfig);

            Table table = GetTargetTable(storageConfig, flatConfig);
            var scanConfig = new ScanOperationConfig
            {
                AttributesToGet = storageConfig.AttributesToGet,
                Select = SelectValues.SpecificAttributes,
                Filter = filter,
                ConditionalOperator = flatConfig.ConditionalOperator,
                IndexName = flatConfig.IndexName,
                ConsistentRead = flatConfig.ConsistentRead.GetValueOrDefault(false)
            };
            Search scan = table.Scan(scanConfig);
            return new ContextSearch(scan, flatConfig);
        }

        private ContextSearch ConvertFromScan<T>(ScanOperationConfig scanConfig, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Table table = GetTargetTable(storageConfig, flatConfig);
            Search search = table.Scan(scanConfig);
            return new ContextSearch(search, flatConfig);
        }

        private ContextSearch ConvertFromQuery<T>(QueryOperationConfig queryConfig, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            Table table = GetTargetTable(storageConfig, flatConfig);
            Search search = table.Query(queryConfig);
            return new ContextSearch(search, flatConfig);
        }

        private ContextSearch ConvertQueryByValue<T>(object hashKeyValue, QueryOperator op, IEnumerable<object> values, DynamoDBOperationConfig operationConfig)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            ItemStorageConfig storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);
            List<QueryCondition> conditions = CreateQueryConditions(flatConfig, op, values, storageConfig);
            ContextSearch query = ConvertQueryByValue<T>(hashKeyValue, conditions, operationConfig, storageConfig);
            return query;
        }

        private ContextSearch ConvertQueryByValue<T>(object hashKeyValue, IEnumerable<QueryCondition> conditions, DynamoDBOperationConfig operationConfig, ItemStorageConfig storageConfig = null)
        {
            DynamoDBFlatConfig flatConfig = new DynamoDBFlatConfig(operationConfig, Config);
            if (storageConfig == null)
                storageConfig = StorageConfigCache.GetConfig<T>(flatConfig);

            List<string> indexNames;
            QueryFilter filter = ComposeQueryFilter(flatConfig, hashKeyValue, conditions, storageConfig, out indexNames);
            return ConvertQueryHelper<T>(flatConfig, storageConfig, filter, indexNames);
        }
        private ContextSearch ConvertQueryHelper<T>(DynamoDBFlatConfig currentConfig, ItemStorageConfig storageConfig, QueryFilter filter, List<string> indexNames)
        {
            Table table = GetTargetTable(storageConfig, currentConfig);
            string indexName = GetQueryIndexName(currentConfig, indexNames);
            var queryConfig = new QueryOperationConfig
            {
                Filter = filter,
                ConsistentRead = currentConfig.ConsistentRead.Value,
                BackwardSearch = currentConfig.BackwardQuery.Value,
                IndexName = indexName,
                ConditionalOperator = currentConfig.ConditionalOperator
            };
            if (string.IsNullOrEmpty(indexName))
            {
                queryConfig.Select = SelectValues.SpecificAttributes;
                List<string> attributesToGet = storageConfig.AttributesToGet;
                queryConfig.AttributesToGet = attributesToGet;
            }
            else
            {
                queryConfig.Select = SelectValues.AllProjectedAttributes;
            }
            Search query = table.Query(queryConfig);

            return new ContextSearch(query, currentConfig);
        }

        private AsyncSearch<T> FromSearchAsync<T>(ContextSearch contextSearch)
        {
            return new AsyncSearch<T>(this, contextSearch);
        }

        #endregion
    }
}
