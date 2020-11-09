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

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// A DynamoDBEntry that represents a DynamoDB list (L) type.
    /// </summary>
    public class DynamoDBList : DynamoDBEntry
    {
        private static DynamoDBEntryConversion conversion = CreateConversion();
        private static DynamoDBEntryConversion CreateConversion()
        {
            var conversion = DynamoDBEntryConversion.V2.Clone();
            var supportedMemberTypes = new Type[]
            {
                typeof(String),
                typeof(MemoryStream),
                typeof(byte[])
            };
            conversion.AddConverter(new DynamoDBListConverter(supportedMemberTypes));

            return conversion;
        }

        #region Constructors/static initializers

        /// <summary>
        /// Constructs an empty DynamoDBList.
        /// </summary>
        public DynamoDBList()
        {
            Entries = new List<DynamoDBEntry>();
        }

        /// <summary>
        /// Construct an instance DynamoDBList.
        /// </summary>
        /// <param name="entries"></param>
        public DynamoDBList(IEnumerable<DynamoDBEntry> entries)
            : this()
        {
            Entries.AddRange(entries);
        }

        /// <summary>
        /// Create a DynamODBList from an IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DynamoDBList Create<T>(IEnumerable<T> items)
        {
            var list = new DynamoDBList();
            foreach (var item in items)
            {
                var entry = new UnconvertedDynamoDBEntry(item);
                list.Entries.Add(entry);
            }

            return list;
        }

        #endregion

        #region Properties/Accessors

        /// <summary>
        /// Collection of DynamoDB entries
        /// </summary>
        public List<DynamoDBEntry> Entries { get; private set; }

        /// <summary>
        /// Gets or sets DynamoDB at a specific location in the list.
        /// </summary>
        /// <param name="i">Index of the DynamoDB in question.</param>
        /// <returns>DynamoDB in question.</returns>
        public DynamoDBEntry this[int i]
        {
            get
            {
                return Entries[i];
            }
            set
            {
                if (i < Entries.Count && i >= 0)
                {
                    Entries[i] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("i");
                }
            }
        }

        /// <summary>
        /// Adds a DynamoDB to the end of the list.
        /// </summary>
        /// <param name="value">DynamoDB to add.</param>
        public void Add(DynamoDBEntry value)
        {
            this.Entries.Add(value);
        }

        /// <summary>
        /// Returns a new instance of Document where all unconverted .NET types
        /// are converted to DynamoDBEntry types using a specific conversion.
        /// </summary>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public DynamoDBList ForceConversion(DynamoDBEntryConversion conversion)
        {
            DynamoDBList newList = new DynamoDBList();
            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[i];

                var unconvertedEntry = entry as UnconvertedDynamoDBEntry;
                if (unconvertedEntry != null)
                    entry = unconvertedEntry.Convert(conversion);

                var doc = entry as Document;
                if (doc != null)
                    entry = doc.ForceConversion(conversion);

                var list = entry as DynamoDBList;
                if (list != null)
                    entry = list.ForceConversion(conversion);

                newList.Add(entry);
            }

            return newList;
        }

        #endregion

        #region Overrides

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            if (Entries == null)
                return null;

            AttributeValue attribute = new AttributeValue();

            var items = new List<AttributeValue>();
            foreach (var entry in Entries)
            {
                AttributeValue entryAttributeValue;
                using (conversionConfig.CRT.Track(entry))
                {
                    entryAttributeValue = entry.ConvertToAttributeValue(conversionConfig);
                }
                items.Add(entryAttributeValue);
            }

            attribute.L = items;
            attribute.IsLSet = true;

            return attribute;
        }

        /// <summary>
        /// Implement the Clone method.s
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DynamoDBList list = new DynamoDBList();
            foreach (DynamoDBEntry entry in this.Entries)
            {
                list.Add(entry.Clone() as DynamoDBEntry);
            }

            return list;
        }

        /// <summary>
        /// Implement the Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var otherList = obj as DynamoDBList;
            if (otherList == null)
                return false;

            var entries = Entries;
            var otherEntries = otherList.Entries;
            if (entries.Count != otherEntries.Count)
                return false;

            for(int i=0;i<entries.Count;i++)
            {
                var a = entries[i];
                var b = otherEntries[i];

                if (a == null || b == null)
                    if (a != b)
                        return false;

                if (!a.Equals(b))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Implement the GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var entriesHashCode = 0;
            foreach (var entry in this.Entries)
            {
                entriesHashCode = Hashing.CombineHashes(entriesHashCode, entry.GetHashCode());
            }

            return entriesHashCode;
        }

        #endregion

        #region Explicit and Implicit conversions

        #region DynamoDBEntry-DynamoDBList conversions

        /// <summary>
        /// Explicitly convert DynamoDBList to DynamoDBEntry[]
        /// </summary>
        /// <returns>DynamoDBEntry[] value of this object</returns>
        public override DynamoDBEntry[] AsArrayOfDynamoDBEntry()
        {
            return AsListOfDynamoDBEntry().ToArray();
        }
        /// <summary>
        /// Implicitly convert DynamoDBEntry[] to DynamoDBList
        /// </summary>
        /// <param name="data">DynamoDBEntry[] data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(DynamoDBEntry[] data)
        {
            return new DynamoDBList(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to DynamoDBEntry[]
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>DynamoDBEntry[] value of DynamoDBList</returns>
        public static explicit operator DynamoDBEntry[](DynamoDBList p)
        {
            return p.AsArrayOfDynamoDBEntry();
        }

        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;DynamoDBEntry&gt;
        /// </summary>
        /// <returns>List&lt;DynamoDBEntry&gt; value of this object</returns>
        public override List<DynamoDBEntry> AsListOfDynamoDBEntry()
        {
            return Entries;
        }
        /// <summary>
        /// Implicitly convert List&lt;DynamoDBEntry&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">List&lt;DynamoDBEntry&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(List<DynamoDBEntry> data)
        {
            return new DynamoDBList(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;DynamoDBEntry&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>List&lt;DynamoDBEntry&gt; value of DynamoDBList</returns>
        public static explicit operator List<DynamoDBEntry>(DynamoDBList p)
        {
            return p.AsListOfDynamoDBEntry();
        }

        #endregion

        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;Document&gt;
        /// </summary>
        /// <returns>List&lt;Document&gt; value of this object</returns>
        public override List<Document> AsListOfDocument()
        {
            return Entries.Select(e => e.ToDocument()).ToList();
        }

        /// <summary>
        /// Implicitly convert List&lt;Document&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;Document&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBList(List<Document> data)
        {
            return new DynamoDBList(data.Cast<DynamoDBEntry>());
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;Document&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;Document&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<Document>(DynamoDBList p)
        {
            return p.AsListOfDocument();
        }


        /// <summary>
        /// Explicitly convert DynamoDBEntry to String[]
        /// </summary>
        /// <returns>String[] value of this object</returns>
        public override String[] AsArrayOfString()
        {
            return AsListOfString().ToArray();
        }
        /// <summary>
        /// Implicitly convert String[] to DynamoDBEntry
        /// </summary>
        /// <param name="data">String[] data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBList(String[] data)
        {
            return conversion.ConvertToEntry<String[]>(data).ToDynamoDBList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to String[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>String[] value of DynamoDBEntry</returns>
        public static explicit operator String[](DynamoDBList p)
        {
            return p.AsArrayOfString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public override List<String> AsListOfString()
        {
            return conversion.ConvertFromEntry<List<String>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;String&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">List&lt;String&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(List<String> data)
        {
            return conversion.ConvertToEntry<List<String>>(data).ToDynamoDBList();

        }
        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;String&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>List&lt;String&gt; value of DynamoDBList</returns>
        public static explicit operator List<String>(DynamoDBList p)
        {
            return p.AsListOfString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public override HashSet<String> AsHashSetOfString()
        {
            return conversion.ConvertFromEntry<HashSet<String>>(this);
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;String&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">HashSet&lt;String&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(HashSet<String> data)
        {
            return conversion.ConvertToEntry<HashSet<String>>(data).ToDynamoDBList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to HashSet&lt;String&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>HashSet&lt;String&gt; value of DynamoDBList</returns>
        public static explicit operator HashSet<String>(DynamoDBList p)
        {
            return p.AsHashSetOfString();
        }


        /// <summary>
        /// Explicitly convert DynamoDBList to byte[]
        /// </summary>
        /// <returns>List&lt;byte[]&gt; value of this object</returns>
        public override List<byte[]> AsListOfByteArray()
        {
            return conversion.ConvertFromEntry<List<byte[]>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;byte[]&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">List&lt;byte[]&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(List<byte[]> data)
        {
            return conversion.ConvertToEntry<List<byte[]>>(data).ToDynamoDBList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>List&lt;byte[]&gt; value of DynamoDBList</returns>
        public static explicit operator List<byte[]>(DynamoDBList p)
        {
            return p.AsListOfByteArray();
        }

        /// <summary>
        /// Explicitly convert DynamoDBList to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <returns>HashSet&lt;byte[]&gt; value of this object</returns>
        public override HashSet<byte[]> AsHashSetOfByteArray()
        {
            return conversion.ConvertFromEntry<HashSet<byte[]>>(this);
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;byte[]&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">HashSet&lt;byte[]&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(HashSet<byte[]> data)
        {
            return conversion.ConvertToEntry<HashSet<byte[]>>(data).ToDynamoDBList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>HashSet&lt;byte[]&gt; value of DynamoDBList</returns>
        public static explicit operator HashSet<byte[]>(DynamoDBList p)
        {
            return conversion.ConvertFromEntry<HashSet<byte[]>>(p);
        }



        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;MemoryStream&gt;
        /// </summary>
        /// <returns>List&lt;MemoryStream&gt; value of this object</returns>
        public override List<MemoryStream> AsListOfMemoryStream()
        {
            return conversion.ConvertFromEntry<List<MemoryStream>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;MemoryStream&gt; to DynamoDBList
        /// </summary>
        /// <param name="data">List&lt;MemoryStream&gt; data to convert</param>
        /// <returns>DynamoDBList representing the data</returns>
        public static implicit operator DynamoDBList(List<MemoryStream> data)
        {
            return conversion.ConvertToEntry<List<MemoryStream>>(data).ToDynamoDBList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBList to List&lt;MemoryStream&gt;
        /// </summary>
        /// <param name="p">DynamoDBList to convert</param>
        /// <returns>List&lt;MemoryStream&gt; value of DynamoDBList</returns>
        public static explicit operator List<MemoryStream>(DynamoDBList p)
        {
            return p.AsListOfMemoryStream();
        }


        #endregion

    }
}
