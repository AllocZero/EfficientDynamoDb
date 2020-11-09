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

using Amazon.DynamoDBv2.Model;
using System.IO;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// A DynamoDBEntry that represents a primitive list DynamoDB type
    /// </summary>
    public class PrimitiveList : DynamoDBEntry, IEquatable<PrimitiveList>
    {
        private static DynamoDBEntryConversion V1Conversion = DynamoDBEntryConversion.V1;

        #region Constructors

        /// <summary>
        /// Constructs an empty PrimitiveList.
        /// Values are configured to be saved as strings.
        /// </summary>
        public PrimitiveList()
            : this(DynamoDBEntryType.String)
        {
        }

        /// <summary>
        /// Constructs an empty PrimitiveList and specifies
        /// the type of its elements.
        /// </summary>
        /// <param name="type"></param>
        public PrimitiveList(DynamoDBEntryType type)
        {
            Entries = new List<Primitive>();
            Type = type;
        }

        internal PrimitiveList(IEnumerable<Primitive> primitives)
            : this()
        {
            DynamoDBEntryType? listType = null;
            foreach (var primitive in primitives)
            {
                listType = primitive.Type;
                Entries.Add(primitive);
            }
            Type = listType.GetValueOrDefault(DynamoDBEntryType.String);
        }

        #endregion


        #region Properties/Accessors

        /// <summary>
        /// Collection of Primitive entries
        /// </summary>
        public List<Primitive> Entries { get; set; }

        /// <summary>
        /// Type of Primitive items in the list
        /// </summary>
        public DynamoDBEntryType Type { get; set; }

        /// <summary>
        /// Gets or sets Primitive at a specific location in the list.
        /// </summary>
        /// <param name="i">Index of the Primitive in question.</param>
        /// <returns>Primitive in question.</returns>
        public Primitive this[int i]
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
        /// Adds a Primitive to the end of the list.
        /// </summary>
        /// <param name="value">Primitive to add.</param>
        public void Add(Primitive value)
        {
            this.Entries.Add(value);
        }

        #endregion


        #region Internal conversion methods

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            if (Entries == null || Entries.Count == 0)
                return null;

            AttributeValue attribute = new AttributeValue();
            if (Type == DynamoDBEntryType.Numeric || Type == DynamoDBEntryType.String)
            {
                List<string> values = new List<string>();
                foreach (var entry in Entries)
                {
                    values.Add(entry.StringValue);
                }

                if (Type == DynamoDBEntryType.Numeric)
                {
                    attribute.NS = values;
                }
                else
                {
                    attribute.SS = values;
                }
            }
            else if (Type == DynamoDBEntryType.Binary)
            {
                List<MemoryStream> values = new List<MemoryStream>();
                foreach (var entry in Entries)
                {
                    MemoryStream stream = new MemoryStream(entry.AsByteArray());
                    values.Add(stream);
                }

                attribute.BS = values;
            }
            else
            {
                throw new InvalidOperationException("Unsupported Type");
            }

            return attribute;
        }

        internal List<Primitive> GetSortedEntries()
        {
            var sortedEntries = new List<Primitive>(Entries);
            sortedEntries.Sort(PrimitiveComparer.Comparer);

            return sortedEntries;
        }

        private class PrimitiveComparer : IComparer<Primitive>
            {
            public int Compare(Primitive x, Primitive y)
            {
                if (x.Type != y.Type)
                    return x.Type.CompareTo(y.Type);

                if (x.Type == DynamoDBEntryType.Numeric || x.Type == DynamoDBEntryType.String)
                {
                    return (string.Compare(x.StringValue, y.StringValue, StringComparison.Ordinal));
                }
                else if (x.Type == DynamoDBEntryType.Binary)
                {
                    byte[] xByteArray = x.Value as byte[];
                    byte[] yByteArray = y.Value as byte[];

                    if (xByteArray.Length != yByteArray.Length)
                        return xByteArray.Length.CompareTo(yByteArray.Length);

                    for (int i = 0; i < xByteArray.Length; i++)
                    {
                        byte xb = xByteArray[i];
                        byte yb = yByteArray[i];
                        int byteCompare = xb.CompareTo(yb);
                        if (byteCompare != 0)
                            return byteCompare;
            }

                    return 0;
        }
                else
        {
                    throw new InvalidOperationException("Unknown type of Primitive: " + x.Type);
                }
            }

            public static PrimitiveComparer Comparer = new PrimitiveComparer();
        }

        #endregion

        #region Explicit and Implicit conversions

        #region Primitive-PrimitiveList conversions

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Primitive[]
        /// </summary>
        /// <returns>Primitive[] value of this object</returns>
        public override Primitive[] AsArrayOfPrimitive()
        {
            return AsListOfPrimitive().ToArray();
        }
        /// <summary>
        /// Implicitly convert Primitive[] to DynamoDBEntry
        /// </summary>
        /// <param name="data">Primitive[] data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator PrimitiveList(Primitive[] data)
        {
            return (PrimitiveList)(data.ToList());
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Primitive[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Primitive[] value of DynamoDBEntry</returns>
        public static explicit operator Primitive[](PrimitiveList p)
        {
            return p.AsArrayOfPrimitive();
        }

        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;Primitive&gt;
        /// </summary>
        /// <returns>List&lt;Primitive&gt; value of this object</returns>
        public override List<Primitive> AsListOfPrimitive()
        {
            return Entries;
        }
        /// <summary>
        /// Implicitly convert List&lt;Primitive&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">List&lt;Primitive&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(List<Primitive> data)
        {
            return new PrimitiveList(data);
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;Primitive&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>List&lt;Primitive&gt; value of PrimitiveList</returns>
        public static explicit operator List<Primitive>(PrimitiveList p)
        {
            return p.AsListOfPrimitive();
        }

        /// <summary>
        /// Explicitly convert PrimitiveList to HashSet&lt;Primitive&gt;
        /// </summary>
        /// <returns>HashSet&lt;Primitive&gt; value of this object</returns>
        public override HashSet<Primitive> AsHashSetOfPrimitive()
        {
            return new HashSet<Primitive>(Entries, PrimitiveEqualityComparer.Default);
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;Primitive&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">HashSet&lt;Primitive&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(HashSet<Primitive> data)
        {
            return new PrimitiveList(data);
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to HashSet&lt;Primitive&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>HashSet&lt;Primitive&gt; value of PrimitiveList</returns>
        public static explicit operator HashSet<Primitive>(PrimitiveList p)
        {
            return p.AsHashSetOfPrimitive();
        }

        #endregion


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
        public static implicit operator PrimitiveList(String[] data)
        {
            return V1Conversion.ConvertToEntry<String[]>(data).ToPrimitiveList();
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to String[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>String[] value of DynamoDBEntry</returns>
        public static explicit operator String[](PrimitiveList p)
        {
            return p.AsArrayOfString();
        }

        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public override List<String> AsListOfString()
        {
            return V1Conversion.ConvertFromEntry<List<String>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;String&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">List&lt;String&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(List<String> data)
        {
            return V1Conversion.ConvertToEntry<List<String>>(data).ToPrimitiveList();

        }
        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;String&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>List&lt;String&gt; value of PrimitiveList</returns>
        public static explicit operator List<String>(PrimitiveList p)
        {
            return p.AsListOfString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public override HashSet<String> AsHashSetOfString()
        {
            return V1Conversion.ConvertFromEntry<HashSet<String>>(this);
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;String&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">HashSet&lt;String&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(HashSet<String> data)
        {
            return V1Conversion.ConvertToEntry<HashSet<String>>(data).ToPrimitiveList();
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to HashSet&lt;String&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>HashSet&lt;String&gt; value of PrimitiveList</returns>
        public static explicit operator HashSet<String>(PrimitiveList p)
        {
            return p.AsHashSetOfString();
        }


        /// <summary>
        /// Explicitly convert PrimitiveList to byte[]
        /// </summary>
        /// <returns>List&lt;byte[]&gt; value of this object</returns>
        public override List<byte[]> AsListOfByteArray()
        {
            return V1Conversion.ConvertFromEntry<List<byte[]>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;byte[]&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">List&lt;byte[]&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(List<byte[]> data)
        {
            return V1Conversion.ConvertToEntry<List<byte[]>>(data).ToPrimitiveList();
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>List&lt;byte[]&gt; value of PrimitiveList</returns>
        public static explicit operator List<byte[]>(PrimitiveList p)
        {
            return p.AsListOfByteArray();
        }

        /// <summary>
        /// Explicitly convert PrimitiveList to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <returns>HashSet&lt;byte[]&gt; value of this object</returns>
        public override HashSet<byte[]> AsHashSetOfByteArray()
        {
            return V1Conversion.ConvertFromEntry<HashSet<byte[]>>(this);
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;byte[]&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">HashSet&lt;byte[]&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(HashSet<byte[]> data)
        {
            return V1Conversion.ConvertToEntry<HashSet<byte[]>>(data).ToPrimitiveList();
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>HashSet&lt;byte[]&gt; value of PrimitiveList</returns>
        public static explicit operator HashSet<byte[]>(PrimitiveList p)
        {
            return V1Conversion.ConvertFromEntry<HashSet<byte[]>>(p);
        }


        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;MemoryStream&gt;
        /// </summary>
        /// <returns>List&lt;MemoryStream&gt; value of this object</returns>
        public override List<MemoryStream> AsListOfMemoryStream()
        {
            return V1Conversion.ConvertFromEntry<List<MemoryStream>>(this);
        }
        /// <summary>
        /// Implicitly convert List&lt;MemoryStream&gt; to PrimitiveList
        /// </summary>
        /// <param name="data">List&lt;MemoryStream&gt; data to convert</param>
        /// <returns>PrimitiveList representing the data</returns>
        public static implicit operator PrimitiveList(List<MemoryStream> data)
        {
            return V1Conversion.ConvertToEntry<List<MemoryStream>>(data).ToPrimitiveList();
        }
        /// <summary>
        /// Explicitly convert PrimitiveList to List&lt;MemoryStream&gt;
        /// </summary>
        /// <param name="p">PrimitiveList to convert</param>
        /// <returns>List&lt;MemoryStream&gt; value of PrimitiveList</returns>
        public static explicit operator List<MemoryStream>(PrimitiveList p)
        {
            return p.AsListOfMemoryStream();
        }


        #endregion


        #region Public overrides

        /// <summary>
        /// Implement the Clone method.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            PrimitiveList list = new PrimitiveList(this.Type);
            foreach (Primitive entry in this.Entries)
            {
                list.Add(entry.Clone() as Primitive);
            }

            return list;
        }

        /// <summary>
        /// Implement the GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var typeHashCode = this.Type.GetHashCode();
            var entriesHashCode = 0;
            foreach(var entry in this.Entries)
            {
                // Hash entries in such a way that order doesn't matter
                entriesHashCode = entriesHashCode ^ entry.GetHashCode();
            }

            return Hashing.CombineHashes(typeHashCode, entriesHashCode);
        }

        /// <summary>
        /// Implement the Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PrimitiveList entryOther = obj as PrimitiveList;
            if (entryOther == null || this.Type != entryOther.Type)
                return false;

            if (entryOther.Entries.Count != this.Entries.Count)
                return false;

            var thisSortedEntries = this.GetSortedEntries();
            var otherSortedEntries = entryOther.GetSortedEntries();
            for (int i = 0; i < this.Entries.Count; i++)
            {
                Primitive thisPrim = thisSortedEntries[i];
                Primitive otherPrim = otherSortedEntries[i];

                if (thisPrim == null && otherPrim == null)
                    continue;
                else if (thisPrim == null || otherPrim == null)
                    return false;
                else if (!thisPrim.Equals(otherPrim))
                    return false;
            }

            return true;
        }
        
        #endregion

        #region IEquatable<PrimitiveList> Members

        /// <summary>
        /// Implement the Equals method from IEquatable
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PrimitiveList other)
        {
            return this.Equals((object)other);
        }

        #endregion
    }
}
