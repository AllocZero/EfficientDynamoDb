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
using System.Globalization;
using System.Text;

using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using ThirdParty.Json.LitJson;
using System.IO;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Utility methods to handle conversion from/to JSON
    /// </summary>
    internal static class JsonUtils
    {
        /// <summary>
        /// Parses JSON text to produce Document.
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static Document FromJson(string jsonText)
        {
            var json = JsonMapper.ToObject(jsonText);
            if (!json.IsObject)
                throw new InvalidOperationException("Expected object at JSON root.");

            var document = ToEntry(json, DynamoDBEntryConversion.V2) as Document;
            if (document == null)
                throw new InvalidOperationException();

            return document;
        }

        /// <summary>
        /// Parses JSON text to produce an <see cref="IEnumerable{T}"/> of type <see cref="Document"/>.
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Document"/></returns>
        public static IEnumerable<Document> FromJsonArray(string jsonText)
        {
            var json = JsonMapper.ToObject(jsonText);
            if (!json.IsArray)
                throw new InvalidOperationException("Expected array at JSON root.");
            
            var array = new List<Document>();
            for(int i=0;i<json.Count;i++)
            {
                var item = json[i];
                var entry = ToEntry(item, DynamoDBEntryConversion.V2) as Document;
                if (entry == null)
                    throw new InvalidOperationException();
                array.Add(entry);
            }

            return array;
        }

        /// <summary>
        /// Creates JSON text for a given Document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        public static string ToJson(Document document, bool prettyPrint)
        {
            var sb = new StringBuilder();
            var writer = new JsonWriter(sb);
            writer.PrettyPrint = prettyPrint;

            WriteJson(document, writer, DynamoDBEntryConversion.V2);

            // Trim everything before the first '{' character
            var jsonIndex = FirstIndex(sb, '{');
            if (jsonIndex > 0)
                sb.Remove(0, jsonIndex);

            var jsonText = sb.ToString();
            return jsonText;
        }


        /// <summary>
        /// Decodes specific attributes from base64 to their binary representation.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="attributeNames"></param>
        public static void DecodeBase64Attributes(Document document, params string[] attributeNames)
        {
            if (attributeNames == null || attributeNames.Length == 0)
                return;

            var decodedValues = new Dictionary<string, DynamoDBEntry>(StringComparer.Ordinal);

            // Convert, but don't alter the original yet
            foreach(var attributeName in attributeNames)
            {
                DynamoDBEntry entry;
                // If an attribute is not present, do nothing
                if (!document.TryGetValue(attributeName, out entry))
                    continue;

                DynamoDBEntry decodedEntry;
                if (!TryDecodeBase64(entry, out decodedEntry))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Unable to decode attribute {0} of type {1} in document", attributeName, entry.GetType().FullName));

                decodedValues[attributeName] = decodedEntry;
            }

            // Update document with decoded attribute values
            foreach(var kvp in decodedValues)
            {
                var attributeName = kvp.Key;
                var decodedEntry = kvp.Value;

                document[attributeName] = decodedEntry;
            }
        }


        // Attempts to decode a particular DynamoDBEntry.
        // May throw exceptions, in particular if the data is not base64 encoded
        private static bool TryDecodeBase64(DynamoDBEntry entry, out DynamoDBEntry decodedEntry)
        {
            decodedEntry = null;

            // Convert string primitive (S) to binary primitive (B)
            var primitive = entry as Primitive;
            if (primitive != null && primitive.Type == DynamoDBEntryType.String)
            {
                // Decode the contents
                var base64 = primitive.Value as string;
                byte[] bytes;
                if (!TryDecodeBase64(base64, out bytes))
                    return false;

                // Store as binary primitive (B)
                decodedEntry = new Primitive(bytes);
                return true;
            }

            // Convert string set (SS) to binary set (BS)
            var primitiveList = entry as PrimitiveList;
            if (primitiveList != null && primitiveList.Type == DynamoDBEntryType.String)
            {
                var decodedList = new PrimitiveList(DynamoDBEntryType.Binary);
                foreach(var item in primitiveList.Entries)
                {
                    // Attempt to decode
                    DynamoDBEntry decodedItem;
                    if (!TryDecodeBase64(item, out decodedItem))
                        return false;

                    // The decoded item must be a Primitive
                    Primitive decodedPrimitive = decodedItem as Primitive;
                    if (decodedPrimitive == null)
                        return false;

                    decodedList.Add(decodedPrimitive);
                }

                decodedEntry = decodedList;
                return true;
            }

            // In a given list (L), convert every string primitive (S) to binary primitive (B)
            // Non-strings and strings that cannot be converted will be left as-is
            var dynamoDBList = entry as DynamoDBList;
            if (dynamoDBList != null)
            {
                var decodedList = new DynamoDBList();
                foreach(var item in dynamoDBList.Entries)
                {
                    DynamoDBEntry decodedItem;
                    if (!TryDecodeBase64(item, out decodedItem))
                    {
                        // if decoding could not succeed, store same item
                        decodedItem = item;
                    }

                    decodedList.Add(decodedItem);
                }

                decodedEntry = decodedList;
                return true;
            }

            return false;
        }

        // Attempt to decode base64 string to bytes
        private static bool TryDecodeBase64(string base64Data, out byte[] bytes)
        {
            bytes = null;

            if (base64Data == null)
                return false;

            try
            {
                bytes = Convert.FromBase64String(base64Data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Returns a DynamoDB entry for the given JSON data
        private static DynamoDBEntry ToEntry(JsonData data, DynamoDBEntryConversion conversion)
        {
            if (data == null)
                return new DynamoDBNull();

            if (data.IsObject)
            {
                var document = new Document();
                foreach (var propertyName in data.PropertyNames)
                {
                    var nestedData = data[propertyName];
                    var entry = ToEntry(nestedData, conversion);
                    document[propertyName] = entry;
                }
                return document;
            }

            if (data.IsArray)
            {
                var list = new DynamoDBList();
                for(int i=0;i<data.Count;i++)
                {
                    var item = data[i];
                    var entry = ToEntry(item, conversion);
                    list.Add(entry);
                }
                return list;
            }

            if (data.IsBoolean)
                return new UnconvertedDynamoDBEntry((bool)data).Convert(conversion);

            if (data.IsDouble)
                return new UnconvertedDynamoDBEntry((double)data).Convert(conversion);

            if (data.IsInt)
                return new UnconvertedDynamoDBEntry((int)data).Convert(conversion);

            if (data.IsUInt)
                return new UnconvertedDynamoDBEntry((uint)data).Convert(conversion);

            if (data.IsLong)
                return new UnconvertedDynamoDBEntry((long)data).Convert(conversion);

            if (data.IsULong)
                return new UnconvertedDynamoDBEntry((ulong)data).Convert(conversion);

            if (data.IsString)
                return new UnconvertedDynamoDBEntry((string)data).Convert(conversion);

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                "Unable to convert JSON data of type {0} to DynamoDB type.", data.GetJsonType()));
        }

        // Writes a JSON representation of the given DynamoDBEntry
        internal static void WriteJson(DynamoDBEntry entry, JsonWriter writer, DynamoDBEntryConversion conversion)
        {
            entry = entry.ToConvertedEntry(conversion);

            var document = entry as Document;
            if (document != null)
            {
                writer.WriteObjectStart();

                // Both item attributes and entries in M type are unordered, so sorting by key
                // http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html#DataModel.DataTypes
                foreach (var kvp in document)
                {
                    var name = kvp.Key;
                    var value = kvp.Value;

                    writer.WritePropertyName(name);
                    WriteJson(value, writer, conversion);
                }
                writer.WriteObjectEnd();
                return;
            }

            var primitive = entry as Primitive;
            if (primitive != null)
            {
                var type = primitive.Type;
                var value = primitive.Value;
                WritePrimitive(writer, type, value);
                return;
            }

            var primitiveList = entry as PrimitiveList;
            if (primitiveList != null)
            {
                var itemType = primitiveList.Type;

                writer.WriteArrayStart();
                foreach (var item in primitiveList.Entries)
                {
                    var itemValue = item.Value;
                    WritePrimitive(writer, itemType, itemValue);
                }
                writer.WriteArrayEnd();
                return;
            }

            var ddbList = entry as DynamoDBList;
            if (ddbList != null)
            {
                writer.WriteArrayStart();
                foreach(var item in ddbList.Entries)
                {
                    WriteJson(item, writer, conversion);
                }
                writer.WriteArrayEnd();
                return;
            }

            var ddbBool = entry as DynamoDBBool;
            if (ddbBool != null)
            {
                writer.Write(ddbBool.Value);
                return;
            }

            var ddbNull = entry as DynamoDBNull;
            if (ddbNull != null)
            {
                writer.Write(null);
                return;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                "Unable to convert entry of type {0} to JSON", entry.GetType().FullName));
        }

        // Write the contents of a Primitive object as JSON data
        private static void WritePrimitive(JsonWriter writer, DynamoDBEntryType type, object value)
        {
            var stringValue = value as string;

            switch (type)
            {
                case DynamoDBEntryType.Numeric:
                    writer.WriteRaw(stringValue);
                    break;
                case DynamoDBEntryType.String:
                    writer.Write(stringValue);
                    break;
                case DynamoDBEntryType.Binary:
                    var bytes = value as byte[];
                    var base64 = Convert.ToBase64String(bytes);
                    writer.Write(base64);
                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Unsupport DynamoDBEntryType: {0}", type));
            }
        }

        // Finds first instance of a character
        private static int FirstIndex(StringBuilder sb, char toMatch)
        {
            for(int i=0;i<sb.Length;i++)
            {
                var c = sb[i];
                if (c.Equals(toMatch))
                    return i;
            }

            return -1;
        }
    }
}
