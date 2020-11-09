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
using System.IO;
using System.Linq;

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// A collection of attribute key-value pairs that defines
    /// an item in DynamoDB.
    /// </summary>
    public class Document : DynamoDBEntry, IDictionary<string, DynamoDBEntry>
    {
        #region Private/internal members

        private Dictionary<string, DynamoDBEntry> originalValues;
        private Dictionary<string, DynamoDBEntry> currentValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty Document.
        /// </summary>
        public Document()
        {
            originalValues = new Dictionary<string, DynamoDBEntry>();
            currentValues = new Dictionary<string, DynamoDBEntry>();
        }

        /// <summary>
        /// Constructs a Document with the passed-in values as its attribute values.
        /// </summary>
        /// <param name="values"></param>
        public Document(Dictionary<string, DynamoDBEntry> values)
        {
            originalValues = new Dictionary<string, DynamoDBEntry>();
            currentValues = new Dictionary<string, DynamoDBEntry>(values);
        }

        internal Document(Document source)
        {
            originalValues = new Dictionary<string, DynamoDBEntry>(source.originalValues);
            currentValues = new Dictionary<string, DynamoDBEntry>(source.currentValues);
        }

        #endregion

        #region Properties/accessors

        /// <summary>
        /// Attribute accessor, allows getting or setting of an individual attribute.
        /// </summary>
        /// <param name="key">Name of the attribute.</param>
        /// <returns>Current value of the attribute.</returns>
        public DynamoDBEntry this[string key]
        {
            get
            {
                return currentValues[key];
            }
            set
            {
                if (value == null)
                {
                    currentValues[key] = new Primitive();
                }
                else
                {
                    currentValues[key] = value;
                }
            }
        }

        /// <summary>
        /// Returns true if the attribute has been changed.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>True if the attribute has been changed.</returns>
        public bool IsAttributeChanged(string attributeName)
        {
            DynamoDBEntry original;
            this.originalValues.TryGetValue(attributeName, out original);

            DynamoDBEntry current;
            this.currentValues.TryGetValue(attributeName, out current);

            if ((original != null && current == null) || (original == null && current != null))
                return true;

            if (original == null && current == null)
                return false;

            return !original.Equals(current);
        }

        /// <summary>
        /// Returns true if the document contains attributes that have not been saved.
        /// </summary>
        /// <returns>True if the document contains attributes that have not been saved.</returns>
        public bool IsDirty()
        {
            Dictionary<string, string> keys = new Dictionary<string, string>();
            foreach (string key in currentValues.Keys)
            {
                keys.Add(key, key);
            }

            foreach (string key in originalValues.Keys)
            {
                if (!keys.ContainsKey(key))
                {
                    keys.Add(key, key);
                }
            }

            foreach (string key in keys.Keys)
            {
                if (this.IsAttributeChanged(key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified attribute value.
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="entry">
        /// If the specified attribute value is found, returns the value associated with the 
        /// attribute; otherwise, null.
        /// </param>
        /// <returns>True if attribute is found; false otherwise</returns>
        public bool TryGetValue(string attributeName, out DynamoDBEntry entry)
        {
            return this.currentValues.TryGetValue(attributeName, out entry);
        }

        /// <summary>
        /// Determines if a specific attribute is set on the Document.
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Returns true if the specified attribute is found; false otherwise.</returns>
        public bool Contains(string attributeName)
        {
            return this.currentValues.ContainsKey(attributeName);
        }

        /// <summary>
        /// Returns a new instance of Document where all unconverted .NET types
        /// are converted to DynamoDBEntry types using a specific conversion.
        /// </summary>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public Document ForceConversion(DynamoDBEntryConversion conversion)
        {
            Document newDocument = new Document();
            foreach(var kvp in this)
            {
                string name = kvp.Key;
                DynamoDBEntry entry = kvp.Value;

                var unconvertedEntry = entry as UnconvertedDynamoDBEntry;
                if (unconvertedEntry != null)
                    entry = unconvertedEntry.Convert(conversion);

                var doc = entry as Document;
                if (doc != null)
                    entry = doc.ForceConversion(conversion);

                var list = entry as DynamoDBList;
                if (list != null)
                    entry = list.ForceConversion(conversion);

                newDocument[name] = entry;
            }

            return newDocument;
        }

        #endregion

        #region Private/internal methods

        internal void CommitChanges()
        {
            this.originalValues.Clear();
            foreach (var kvp in currentValues)
            {
                this.originalValues[kvp.Key] = kvp.Value.Clone() as DynamoDBEntry;
            }
        }

        // Converts a Numeric Primitive value from a service attribute to a DynamoDBEntry that can
        // be converted to a DateTime.
        internal static DynamoDBEntry EpochSecondsToDateTime(DynamoDBEntry entry, string attributeName)
        {
            var primitive = entry.AsPrimitive();

            // only try to convert N types to epoch time
            if (primitive != null &&
                primitive.Type == DynamoDBEntryType.Numeric)
            {
                DateTime? dateTime = null;
                try
                {
                    var epochSeconds = primitive.AsInt();
                    dateTime = AWSSDKUtils.ConvertFromUnixEpochSeconds(epochSeconds);
                }
                catch (Exception e)
                {
                    var logger = Logger.GetLogger(typeof(Document));
                    logger.InfoFormat(
                        "Encountered error attempting to convert attribute '{0}' with value '{1}' to DateTime: {2}",
                        attributeName, entry, e);
                }

                if (dateTime.HasValue)
                {
                    entry = (Primitive)(dateTime.Value);
                }
            }

            return entry;
        }

        // Converts a user-supplied DateTime-convertible DynamoDBEntry to epoch seconds stored in a Numeric Primitive.
        internal static DynamoDBEntry DateTimeToEpochSeconds(DynamoDBEntry entry, string attributeName)
        {
            int? epochSeconds = null;
            try
            {
                var dateTime = entry.AsDateTime();
                epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(dateTime);
            }
            catch (Exception e)
            {
                var logger = Logger.GetLogger(typeof(Document));
                logger.InfoFormat(
                    "Encountered error attempting to convert '{0}' with value '{1}' to epoch seconds: {1}",
                    attributeName, entry, e);
            }

            if (epochSeconds.HasValue)
            {
                entry = (Primitive)(epochSeconds.Value);
            }

            return entry;
        }

        internal static Document FromAttributeMap(Dictionary<string, AttributeValue> data, IEnumerable<string> epochAttributes)
        {
            Document doc = new Document();

            if (data != null)
            {
                // Add Primitives and PrimitiveLists
                foreach (var attribute in data)
                {
                    string wholeKey = attribute.Key;
                    AttributeValue value = attribute.Value;

                    DynamoDBEntry convertedValue = AttributeValueToDynamoDBEntry(value);
                    if (convertedValue != null)
                        doc.currentValues[wholeKey] = convertedValue;
                }
            }

            if (epochAttributes != null)
            {
                foreach (var epochAttribute in epochAttributes)
                {
                    DynamoDBEntry epochEntry;
                    if (doc.currentValues.TryGetValue(epochAttribute, out epochEntry))
                    {
                        doc.currentValues[epochAttribute] = EpochSecondsToDateTime(epochEntry, epochAttribute);
                    }
                }
            }

            doc.CommitChanges();
            return doc;
        }

        internal Dictionary<string, AttributeValue> ToAttributeMap(DynamoDBEntryConversion conversion,
            IEnumerable<string> epochAttributes, bool isEmptyStringValueEnabled)
        {
            if (conversion == null) throw new ArgumentNullException("conversion");

            Dictionary<string, AttributeValue> ret = new Dictionary<string, AttributeValue>();

            foreach (var kvp in currentValues)
            {
                var attributeName = kvp.Key;
                var entry = kvp.Value;

                ApplyEpochRules(epochAttributes, attributeName, ref entry);

                var attributeConversionConfig = new AttributeConversionConfig(conversion, isEmptyStringValueEnabled);
                var value = entry.ConvertToAttributeValue(attributeConversionConfig);
                if (value != null)
                {
                    ret.Add(attributeName, value);
                }
            }

            return ret;
        }

        internal Dictionary<string, ExpectedAttributeValue> ToExpectedAttributeMap(DynamoDBEntryConversion conversion,
            IEnumerable<string> epochAttributes, bool isEmptyStringValueEnabled)
        {
            if (conversion == null) throw new ArgumentNullException("conversion");
            Dictionary<string, ExpectedAttributeValue> ret = new Dictionary<string, ExpectedAttributeValue>();

            foreach (var kvp in currentValues)
            {
                var attributeName = kvp.Key;
                var entry = kvp.Value;

                ApplyEpochRules(epochAttributes, attributeName, ref entry);

                var attributeConversionConfig = new AttributeConversionConfig(conversion, isEmptyStringValueEnabled);
                ret.Add(attributeName, entry.ConvertToExpectedAttributeValue(attributeConversionConfig));
            }

            return ret;
        }

        internal Dictionary<string, AttributeValueUpdate> ToAttributeUpdateMap(DynamoDBEntryConversion conversion,
            bool changedAttributesOnly, IEnumerable<string> epochAttributes, bool isEmptyStringValueEnabled)
        {
            if (conversion == null) throw new ArgumentNullException("conversion");
            Dictionary<string, AttributeValueUpdate> ret = new Dictionary<string, AttributeValueUpdate>();

            foreach (var kvp in currentValues)
            {
                string attributeName = kvp.Key;
                DynamoDBEntry entry = kvp.Value;

                ApplyEpochRules(epochAttributes, attributeName, ref entry);

                if (!changedAttributesOnly || this.IsAttributeChanged(attributeName))
                {
                    var attributeConversionConfig = new AttributeConversionConfig(conversion, isEmptyStringValueEnabled);
                    ret.Add(attributeName, entry.ConvertToAttributeUpdateValue(attributeConversionConfig));
                }
            }

            return ret;
        }

        private static void ApplyEpochRules(IEnumerable<string> epochAttributes, string attributeName, ref DynamoDBEntry entry)
        {
            if (epochAttributes != null)
            {
                foreach (var epochAttribute in epochAttributes)
                {
                    if (string.Equals(epochAttribute, attributeName, StringComparison.Ordinal))
                    {
                        entry = DateTimeToEpochSeconds(entry, attributeName);
                    }
                }
            }
        }

        #endregion

        #region DynamoDB conversion

        /// <summary>
        /// <para>
        /// Converts the current Document into a matching JSON string.
        /// </para>
        /// <para>
        /// DynamoDB types are a superset of JSON types, thus the following DynamoDB cannot
        /// be properly represented as JSON data:
        ///  PrimitiveList (SS, NS, BS types) - these sets will be converted to JSON arrays
        ///  Binary Primitive (B type) - binary data will be converted to Base64 strings
        /// </para>
        /// <para>
        /// If the resultant JSON is passed to Document.FromJson, the binary values will be
        /// treated as Base64 strings. Invoke Document.DecodeBase64Attributes to decode these
        /// strings into binary data.
        /// </para>
        /// </summary>
        /// <returns>JSON string corresponding to the current Document.</returns>
        public string ToJson()
        {
            return JsonUtils.ToJson(this, prettyPrint: false);
        }

        /// <summary>
        /// <para>
        /// Converts the current Document into a matching pretty JSON string.
        /// </para>
        /// <para>
        /// DynamoDB types are a superset of JSON types, thus the following DynamoDB cannot
        /// be properly represented as JSON data:
        ///  PrimitiveList (SS, NS, BS types) - these sets will be converted to JSON arrays
        ///  Binary Primitive (B type) - binary data will be converted to Base64 strings
        /// </para>
        /// <para>
        /// If the resultant JSON is passed to Document.FromJson, the binary values will be
        /// treated as Base64 strings. Invoke Document.DecodeBase64Attributes to decode these
        /// strings into binary data.
        /// </para>
        /// </summary>
        /// <returns>JSON string corresponding to the current Document.</returns>
        public string ToJsonPretty()
        {
            return JsonUtils.ToJson(this, prettyPrint: true);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValue objects.
        /// Converts .NET types using the conversion specified by AWSConfigs.DynamoDBConfig.ConversionSchema
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AttributeValue> ToAttributeMap()
        {
            return ToAttributeMap(DynamoDBEntryConversion.CurrentConversion);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValue objects.
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValue> ToAttributeMap(DynamoDBEntryConversion conversion)
        {
            return ToAttributeMap(conversion, epochAttributes: null, isEmptyStringValueEnabled: false);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValue objects.
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled">If the property is false, empty string values will be interpreted as null values.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValue> ToAttributeMap(DynamoDBEntryConversion conversion, bool isEmptyStringValueEnabled)
        {
            return ToAttributeMap(conversion, epochAttributes: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to ExpectedAttributeValue objects.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ExpectedAttributeValue> ToExpectedAttributeMap()
        {
            return ToExpectedAttributeMap(DynamoDBEntryConversion.CurrentConversion);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to ExpectedAttributeValue objects.
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <returns></returns>
        public Dictionary<string, ExpectedAttributeValue> ToExpectedAttributeMap(DynamoDBEntryConversion conversion)
        {
            return ToExpectedAttributeMap(conversion, false);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to ExpectedAttributeValue objects.
        /// </summary>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled">If the property is false, empty string values will be interpreted as null values.</param>
        /// <returns></returns>
        public Dictionary<string, ExpectedAttributeValue> ToExpectedAttributeMap(DynamoDBEntryConversion conversion,
            bool isEmptyStringValueEnabled)
        {
            return ToExpectedAttributeMap(conversion, epochAttributes: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValueUpdate objects.
        /// </summary>
        /// <param name="changedAttributesOnly">If true, only attributes that have been changed will be in the map.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValueUpdate> ToAttributeUpdateMap(bool changedAttributesOnly)
        {
            return ToAttributeUpdateMap(DynamoDBEntryConversion.CurrentConversion, changedAttributesOnly, false);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValueUpdate objects.
        /// </summary>
        /// <param name="changedAttributesOnly">If true, only attributes that have been changed will be in the map.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValueUpdate> ToAttributeUpdateMap(DynamoDBEntryConversion conversion, bool changedAttributesOnly)
        {
            return ToAttributeUpdateMap(conversion, changedAttributesOnly, epochAttributes: null, isEmptyStringValueEnabled: false);
        }

        /// <summary>
        /// Creates a map of attribute names mapped to AttributeValueUpdate objects.
        /// </summary>
        /// <param name="changedAttributesOnly">If true, only attributes that have been changed will be in the map.</param>
        /// <param name="conversion">Conversion to use for converting .NET values to DynamoDB values.</param>
        /// <param name="isEmptyStringValueEnabled">If the property is false, empty string values will be interpreted as null values.</param>
        /// <returns></returns>
        public Dictionary<string, AttributeValueUpdate> ToAttributeUpdateMap(DynamoDBEntryConversion conversion,
            bool changedAttributesOnly, bool isEmptyStringValueEnabled)
        {
            return ToAttributeUpdateMap(conversion, changedAttributesOnly, epochAttributes: null, isEmptyStringValueEnabled: isEmptyStringValueEnabled);
        }

        /// <summary>
        /// Returns the names of all the attributes.
        /// </summary>
        /// <returns>List of attribute names.</returns>
        public List<String> GetAttributeNames()
        {
            return new List<string>(this.currentValues.Keys);
        }

        /// <summary>
        /// <para>
        /// Decodes root-level Base64-encoded strings to their binary representations.
        /// Use this method if the Document was constructed from JSON that contains
        /// base64-encoded binary values, which result from calling ToJson on a Document
        /// with binary data.
        /// </para>
        /// <para>
        /// Individual strings become binary data.
        /// List and sets of Base64-encoded strings become lists and sets of binary data.
        /// </para>
        /// </summary>
        /// <param name="attributeNames">Names of root-level attributes to decode.</param>
        public void DecodeBase64Attributes(params string[] attributeNames)
        {
            JsonUtils.DecodeBase64Attributes(this, attributeNames);
        }

        #endregion

        #region Attribute to DynamoDBEntry conversion

        internal static DynamoDBEntry AttributeValueToDynamoDBEntry(AttributeValue attributeValue)
        {
            Primitive primitive;
            if (TryToPrimitive(attributeValue, out primitive))
                return primitive;

            PrimitiveList primitiveList;
            if (TryToPrimitiveList(attributeValue, out primitiveList))
                return primitiveList;

            DynamoDBBool ddbBool;
            if (TryToDynamoDBBool(attributeValue, out ddbBool))
                return ddbBool;

            DynamoDBNull ddbNull;
            if (TryToDynamoDBNull(attributeValue, out ddbNull))
                return ddbNull;

            DynamoDBList ddbList;
            if (TryToDynamoDBList(attributeValue, out ddbList))
                return ddbList;

            Document document;
            if (TryToDocument(attributeValue, out document))
                return document;

            return null;
        }

        private static bool TryToPrimitiveList(AttributeValue attributeValue, out PrimitiveList primitiveList)
        {
            primitiveList = null;
            Primitive primitive;

            if (attributeValue.IsSetSS())
            {
                primitiveList = new PrimitiveList(DynamoDBEntryType.String);
                foreach (string item in attributeValue.SS)
                {
                    primitive = new Primitive(item);
                    primitiveList.Add(primitive);
                }
            }
            else if (attributeValue.IsSetNS())
            {
                primitiveList = new PrimitiveList(DynamoDBEntryType.Numeric);
                foreach (string item in attributeValue.NS)
                {
                    primitive = new Primitive(item, true);
                    primitiveList.Add(primitive);
                }
            }
            else if (attributeValue.IsSetBS())
            {
                primitiveList = new PrimitiveList(DynamoDBEntryType.Binary);
                foreach (MemoryStream item in attributeValue.BS)
                {
                    primitive = new Primitive(item);
                    primitiveList.Add(primitive);
                }
            }

            return (primitiveList != null);
        }

        private static bool TryToPrimitive(AttributeValue attributeValue, out Primitive primitive)
        {
            primitive = null;
            if (attributeValue.IsSetS())
            {
                primitive = new Primitive(attributeValue.S);
            }
            else if (attributeValue.IsSetN())
            {
                primitive = new Primitive(attributeValue.N, true);
            }
            else if (attributeValue.IsSetB())
            {
                primitive = new Primitive(attributeValue.B);
            }

            return (primitive != null);
        }

        private static bool TryToDynamoDBBool(AttributeValue attributeValue, out DynamoDBBool ddbBool)
        {
            ddbBool = null;
            if (attributeValue.IsSetBOOL())
            {
                ddbBool = new DynamoDBBool(attributeValue.BOOL);
            }
            return (ddbBool != null);
        }

        private static bool TryToDynamoDBNull(AttributeValue attributeValue, out DynamoDBNull ddbNull)
        {
            ddbNull = null;
            if (attributeValue.IsSetNULL())
            {
                ddbNull = new DynamoDBNull();
            }
            return (ddbNull != null);
        }

        private static bool TryToDynamoDBList(AttributeValue attributeValue, out DynamoDBList list)
        {
            list = null;

            if (attributeValue.IsSetL())
            {
                var items = attributeValue.L;
                var entries = items.Select(AttributeValueToDynamoDBEntry);
                list = new DynamoDBList(entries);
            }

            return (list != null);
        }

        private static bool TryToDocument(AttributeValue attributeValue, out Document document)
        {
            document = null;
            
            if (attributeValue.IsSetM())
            {
                document = new Document();

                var items = attributeValue.M;
                foreach(var kvp in items)
                {
                    var name = kvp.Key;
                    var value = kvp.Value;

                    var entry = AttributeValueToDynamoDBEntry(value);
                    document[name] = entry;
                }
            }

            return (document != null);
        }

        #endregion

        #region Static methods
        
        /// <summary>
        /// Creates a Document from an attribute map.
        /// </summary>
        /// <param name="data">Map of attribute names to attribute values.</param>
        /// <returns>Document representing the data.</returns>
        public static Document FromAttributeMap(Dictionary<string, AttributeValue> data)
        {
            return FromAttributeMap(data, epochAttributes: null);
        }

        /// <summary>
        /// Creates a document from a JSON string.
        /// The conversion is as follows:
        ///  Objects are converted to DynamoDB M types.
        ///  Arrays are converted to DynamoDB L types.
        ///  Boolean types are converted to DynamoDB BOOL types.
        ///  Null values are converted to DynamoDB NULL types.
        ///  Numerics are converted to DynamoDB N types.
        ///  Strings are converted to DynamoDB S types.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Document representing the JSON data.</returns>
        public static Document FromJson(string json)
        {
            return JsonUtils.FromJson(json);
        }
        
        /// <summary>
        /// Parses JSON text to produce an array of <see cref="Document"/>.
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Document"/>.</returns>
        public static IEnumerable<Document> FromJsonArray(string jsonText)
        {
            return JsonUtils.FromJsonArray(jsonText);
        }

        #endregion

        #region IDictionary<string,DynamoDBEntry> Members

        /// <summary>
        /// Add value to Doucment.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, DynamoDBEntry value)
        {
            currentValues.Add(key, value);
        }

        /// <summary>
        /// Check to see if the value is set on the document.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return currentValues.ContainsKey(key);
        }

        /// <summary>
        /// This list of attribute keys for the document.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return currentValues.Keys; }
        }

        /// <summary>
        /// Remove the attribute from the Document.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return currentValues.Remove(key);
        }

        /// <summary>
        /// Get a list of all the values in the Document.
        /// </summary>
        public ICollection<DynamoDBEntry> Values
        {
            get { return currentValues.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<string,DynamoDBEntry>> Members

        /// <summary>
        /// Add attributes to Document.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, DynamoDBEntry> item)
        {
            currentValues.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clear attributes from document.
        /// </summary>
        public void Clear()
        {
            currentValues.Clear();
        }

        /// <summary>
        /// Check to see if the attributes are in the Document.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, DynamoDBEntry> item)
        {
            //DynamoDBEntry value;
            //if (!this.TryGetValue(item.Key, out value))
            //    return false;
            //return value.Equals(item.Value);
            var icollection = (ICollection<KeyValuePair<string, DynamoDBEntry>>)currentValues;
            return icollection.Contains(item);
        }

        /// <summary>
        /// Copies the attributes to the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, DynamoDBEntry>[] array, int arrayIndex)
        {
            var icollection = (ICollection<KeyValuePair<string, DynamoDBEntry>>)currentValues;
            icollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count of attributes.
        /// </summary>
        public int Count
        {
            get { return currentValues.Count; }
        }

        /// <summary>
        /// Returns true if the document is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the attributes from the document.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, DynamoDBEntry> item)
        {
            var icollection = (ICollection<KeyValuePair<string, DynamoDBEntry>>)currentValues;
            return icollection.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,DynamoDBEntry>> Members

        /// <summary>
        /// Gets the enumerator for the attributes.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, DynamoDBEntry>> GetEnumerator()
        {
            return currentValues.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator for the attributes.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return currentValues.GetEnumerator();
        }

        #endregion

        #region DynamoDBEntry overrides

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            var map = new Dictionary<string, AttributeValue>(StringComparer.Ordinal);
            foreach(var item in currentValues)
            {
                var key = item.Key;
                var entry = item.Value;
                AttributeValue entryAttributeValue;
                using (conversionConfig.CRT.Track(entry))
                {
                    entryAttributeValue = entry.ConvertToAttributeValue(conversionConfig);
                }

                if (entryAttributeValue != null)
                {
                    map[key] = entryAttributeValue;
                }
            }

            var attributeValue = new AttributeValue();
            attributeValue.M = map;
            attributeValue.IsMSet = true;

            return attributeValue;
        }

        /// <summary>
        /// Clones the Document
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var doc = new Document(this);
            return doc;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Compare the document to see if it is equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var otherDocument = obj as Document;
            if (otherDocument == null)
                return false;

            if (Keys.Count != otherDocument.Keys.Count)
                return false;

            foreach(var key in Keys)
            {
                if (!otherDocument.ContainsKey(key))
                    return false;

                var a = this[key];
                var b = otherDocument[key];

                if (!a.Equals(b))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Implements the GetHashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach(var kvp in this)
            {
                string key = kvp.Key;
                DynamoDBEntry entry = kvp.Value;

                hashCode = Hashing.CombineHashes(hashCode, key.GetHashCode(), entry.GetHashCode());
            }

            return hashCode;
        }

        #endregion
    }
}
