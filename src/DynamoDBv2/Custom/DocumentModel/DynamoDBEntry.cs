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
using System.Globalization;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

#if ADD_SUPPORT_ICLONEABLE
using Amazon.MissingTypes;
#endif

namespace Amazon.DynamoDBv2.DocumentModel
{

    /// <summary>
    /// Abstract class representing an arbitrary DynamoDB attribute value
    /// </summary>
    public abstract class DynamoDBEntry : ICloneable
    {
        internal class AttributeConversionConfig
        {
            public CircularReferenceTracking CRT { get; private set; }
            public DynamoDBEntryConversion Conversion { get; private set; }
            public bool IsEmptyStringValueEnabled { get; set; }

            public AttributeConversionConfig(DynamoDBEntryConversion conversion, bool isEmptyStringValueEnabled)
            {
                Conversion = conversion;
                CRT = new CircularReferenceTracking();
                IsEmptyStringValueEnabled = isEmptyStringValueEnabled;
            }
        }

        #region Internal conversion methods

        internal abstract AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig);
        internal ExpectedAttributeValue ConvertToExpectedAttributeValue(AttributeConversionConfig conversionConfig)
        {
            AttributeValue attributeValue = ConvertToAttributeValue(conversionConfig);

            ExpectedAttributeValue expectedAttribute = new ExpectedAttributeValue();
            if (attributeValue == null)
            {
                expectedAttribute.Exists = false;
            }
            else
            {
                expectedAttribute.Exists = true;
                expectedAttribute.Value = attributeValue;
            }

            return expectedAttribute;
        }
        internal AttributeValueUpdate ConvertToAttributeUpdateValue(AttributeConversionConfig conversionConfig)
        {
            AttributeValue attributeValue = ConvertToAttributeValue(conversionConfig);

            AttributeValueUpdate attributeUpdate = new AttributeValueUpdate();
            if (attributeValue == null)
            {
                attributeUpdate.Action = "DELETE";
            }
            else
            {
                attributeUpdate.Action = "PUT";
                attributeUpdate.Value = attributeValue;
            }

            return attributeUpdate;
        }
        
        #endregion

        #region Subclass conversions

        /// <summary>
        /// Convert DynamoDBEntry to Primitive
        /// </summary>
        /// <returns>
        /// Primitive if DynamoDBEntry is of Primitive type; otherwise null
        /// </returns>
        public Primitive AsPrimitive()
        {
            return (this as Primitive);
        }

        /// <summary>
        /// Convert DynamoDBEntry to PrimitiveList
        /// </summary>
        /// <returns>
        /// PrimitiveList if DynamoDBEntry is of PrimitiveList type; otherwise null
        /// </returns>
        public PrimitiveList AsPrimitiveList()
        {
            return (this as PrimitiveList);
        }

        /// <summary>
        /// Convert DynamoDBEntry to DynamoDBList
        /// </summary>
        /// <returns>
        /// DynamoDBList if DynamoDBEntry is of DynamoDBList type; otherwise null
        /// </returns>
        public DynamoDBList AsDynamoDBList()
        {
            return (this as DynamoDBList);
        }

        /// <summary>
        /// Convert DynamoDBEntry to DynamoDBBool
        /// </summary>
        /// <returns>
        /// DynamoDBBool if DynamoDBEntry is of DynamoDBBool type; otherwise null
        /// </returns>
        public DynamoDBBool AsDynamoDBBool()
        {
            return (this as DynamoDBBool);
        }

        /// <summary>
        /// Convert DynamoDBEntry to DynamoDBNull
        /// </summary>
        /// <returns>
        /// DynamoDBNull if DynamoDBEntry is of DynamoDBNull type; otherwise null
        /// </returns>
        public DynamoDBNull AsDynamoDBNull()
        {
            return (this as DynamoDBNull);
        }

        /// <summary>
        /// Convert DynamoDBEntry to Document
        /// </summary>
        /// <returns>
        /// Document if DynamoDBEntry is of Document type; otherwise null
        /// </returns>
        public Document AsDocument()
        {
            return (this as Document);
        }

        #endregion

        #region Subclass conversion and validation

        internal DynamoDBEntry ToConvertedEntry(DynamoDBEntryConversion conversion)
        {
            var unconverted = this as UnconvertedDynamoDBEntry;
            if (unconverted == null)
                return this;
            else
                return unconverted.Convert(conversion);
        }

        internal Primitive ToPrimitive()
        {
            return Validate(AsPrimitive());
        }

        internal PrimitiveList ToPrimitiveList()
        {
            return Validate(AsPrimitiveList());
        }

        internal DynamoDBList ToDynamoDBList()
        {
            return Validate(AsDynamoDBList());
        }

        internal DynamoDBBool ToDynamoDBBool()
        {
            return Validate(AsDynamoDBBool());
        }

        internal DynamoDBNull ToDynamoDBNull()
        {
            return Validate(AsDynamoDBNull());
        }

        internal Document ToDocument()
        {
            return Validate(AsDocument());
        }

        private T Validate<T>(T item)
        {
            if (item == null)
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to cast {0} as {1}", GetType().FullName, typeof(T).FullName));
            return item;
        }

        #endregion

        #region Explicit and Implicit conversions, base types

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Boolean
        /// </summary>
        /// <returns>Boolean value of this object</returns>
        public virtual Boolean AsBoolean()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Boolean to DynamoDBEntry
        /// </summary>
        /// <param name="data">Boolean data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Boolean data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Boolean
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Boolean value of DynamoDBEntry</returns>
        public static explicit operator Boolean(DynamoDBEntry p)
        {
            return p.AsBoolean();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Byte
        /// </summary>
        /// <returns>Byte value of this object</returns>
        public virtual Byte AsByte()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Byte to DynamoDBEntry
        /// </summary>
        /// <param name="data">Byte data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Byte data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Byte
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Byte value of DynamoDBEntry</returns>
        public static explicit operator Byte(DynamoDBEntry p)
        {
            return p.AsByte();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to SByte
        /// </summary>
        /// <returns>SByte value of this object</returns>
        [CLSCompliant(false)]
        public virtual SByte AsSByte()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert SByte to DynamoDBEntry
        /// </summary>
        /// <param name="data">SByte data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator DynamoDBEntry(SByte data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to SByte
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>SByte value of DynamoDBEntry</returns>
        [CLSCompliant(false)]
        public static explicit operator SByte(DynamoDBEntry p)
        {
            return p.AsSByte();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt16
        /// </summary>
        /// <returns>UInt16 value of this object</returns>
        [CLSCompliant(false)]
        public virtual UInt16 AsUShort()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert UInt16 to DynamoDBEntry
        /// </summary>
        /// <param name="data">UInt16 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator DynamoDBEntry(UInt16 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt16
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>UInt16 value of DynamoDBEntry</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt16(DynamoDBEntry p)
        {
            return p.AsUShort();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int16
        /// </summary>
        /// <returns>Int16 value of this object</returns>
        public virtual Int16 AsShort()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Int16 to DynamoDBEntry
        /// </summary>
        /// <param name="data">Int16 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Int16 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int16
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Int16 value of DynamoDBEntry</returns>
        public static explicit operator Int16(DynamoDBEntry p)
        {
            return p.AsShort();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt32
        /// </summary>
        /// <returns>UInt32 value of this object</returns>
        [CLSCompliant(false)]
        public virtual UInt32 AsUInt()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert UInt32 to DynamoDBEntry
        /// </summary>
        /// <param name="data">UInt32 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator DynamoDBEntry(UInt32 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt32
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>UInt32 value of DynamoDBEntry</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt32(DynamoDBEntry p)
        {
            return p.AsUInt();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int32
        /// </summary>
        /// <returns>Int32 value of this object</returns>
        public virtual Int32 AsInt()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Int32 to DynamoDBEntry
        /// </summary>
        /// <param name="data">Int32 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Int32 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int32
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Int32 value of DynamoDBEntry</returns>
        public static explicit operator Int32(DynamoDBEntry p)
        {
            return p.AsInt();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt64
        /// </summary>
        /// <returns>UInt64 value of this object</returns>
        [CLSCompliant(false)]
        public virtual UInt64 AsULong()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert UInt64 to DynamoDBEntry
        /// </summary>
        /// <param name="data">UInt64 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator DynamoDBEntry(UInt64 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to UInt64
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>UInt64 value of DynamoDBEntry</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt64(DynamoDBEntry p)
        {
            return p.AsULong();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int64
        /// </summary>
        /// <returns>Int64 value of this object</returns>
        public virtual Int64 AsLong()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Int64 to DynamoDBEntry
        /// </summary>
        /// <param name="data">Int64 data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Int64 data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Int64
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Int64 value of DynamoDBEntry</returns>
        public static explicit operator Int64(DynamoDBEntry p)
        {
            return p.AsLong();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Single
        /// </summary>
        /// <returns>Single value of this object</returns>
        public virtual Single AsSingle()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Single to DynamoDBEntry
        /// </summary>
        /// <param name="data">Single data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Single data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Single
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Single value of DynamoDBEntry</returns>
        public static explicit operator Single(DynamoDBEntry p)
        {
            return p.AsSingle();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Double
        /// </summary>
        /// <returns>Double value of this object</returns>
        public virtual Double AsDouble()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Double to DynamoDBEntry
        /// </summary>
        /// <param name="data">Double data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Double data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Double
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Double value of DynamoDBEntry</returns>
        public static explicit operator Double(DynamoDBEntry p)
        {
            return p.AsDouble();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Decimal
        /// </summary>
        /// <returns>Decimal value of this object</returns>
        public virtual Decimal AsDecimal()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Decimal to DynamoDBEntry
        /// </summary>
        /// <param name="data">Decimal data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Decimal data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Decimal
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Decimal value of DynamoDBEntry</returns>
        public static explicit operator Decimal(DynamoDBEntry p)
        {
            return p.AsDecimal();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Char
        /// </summary>
        /// <returns>Char value of this object</returns>
        public virtual Char AsChar()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Char to DynamoDBEntry
        /// </summary>
        /// <param name="data">Char data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Char data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Char
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Char value of DynamoDBEntry</returns>
        public static explicit operator Char(DynamoDBEntry p)
        {
            return p.AsChar();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to String
        /// </summary>
        /// <returns>String value of this object</returns>
        public virtual String AsString()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert String to DynamoDBEntry
        /// </summary>
        /// <param name="data">String data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(String data)
        {
            if(data == null)
                return new Primitive();
            return new UnconvertedDynamoDBEntry(data);
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to String
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>String value of DynamoDBEntry</returns>
        public static implicit operator String(DynamoDBEntry p)
        {
            return p.AsString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to DateTime
        /// </summary>
        /// <returns>DateTime value of this object</returns>
        public virtual DateTime AsDateTime()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert DateTime to DynamoDBEntry
        /// </summary>
        /// <param name="data">DateTime data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(DateTime data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to DateTime
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>DateTime value of DynamoDBEntry</returns>
        public static explicit operator DateTime(DynamoDBEntry p)
        {
            return p.AsDateTime();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Guid
        /// </summary>
        /// <returns>Guid value of this object</returns>
        public virtual Guid AsGuid()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Guid to DynamoDBEntry
        /// </summary>
        /// <param name="data">Guid data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Guid data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Guid
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Guid value of DynamoDBEntry</returns>
        public static explicit operator Guid(DynamoDBEntry p)
        {
            return p.AsGuid();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to byte[]
        /// </summary>
        /// <returns>byte[] value of this object</returns>
        public virtual byte[] AsByteArray()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert byte[] to DynamoDBEntry
        /// </summary>
        /// <param name="data">byte[] data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(byte[] data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to byte[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>byte[] value of DynamoDBEntry</returns>
        public static explicit operator byte[](DynamoDBEntry p)
        {
            return p.AsByteArray();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to MemoryStream
        /// </summary>
        /// <returns>MemoryStream value of this object</returns>
        public virtual MemoryStream AsMemoryStream()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert MemoryStream to DynamoDBEntry
        /// </summary>
        /// <param name="data">MemoryStream data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(MemoryStream data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to MemoryStream
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>MemoryStream value of DynamoDBEntry</returns>
        public static explicit operator MemoryStream(DynamoDBEntry p)
        {
            return p.AsMemoryStream();
        }

        #endregion

        #region Explicit and Implicit conversions, lists and sets

        /// <summary>
        /// Explicitly convert DynamoDBEntry to DynamoDBEntry[]
        /// </summary>
        /// <returns>DynamoDBEntry[] value of this object</returns>
        public virtual DynamoDBEntry[] AsArrayOfDynamoDBEntry()
        {
            return AsListOfDynamoDBEntry().ToArray();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;DynamoDBEntry&gt;
        /// </summary>
        /// <returns>List&lt;DynamoDBEntry&gt; value of this object</returns>
        public virtual List<DynamoDBEntry> AsListOfDynamoDBEntry()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;DynamoDBEntry&gt;
        /// </summary>
        /// <returns>HashSet&lt;DynamoDBEntry&gt; value of this object</returns>
        public virtual HashSet<DynamoDBEntry> AsHashSetOfDynamoDBEntry()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to Primitive[]
        /// </summary>
        /// <returns>Primitive[] value of this object</returns>
        public virtual Primitive[] AsArrayOfPrimitive()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert Primitive[] to DynamoDBEntry
        /// </summary>
        /// <param name="data">Primitive[] data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(Primitive[] data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to Primitive[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>Primitive[] value of DynamoDBEntry</returns>
        public static explicit operator Primitive[](DynamoDBEntry p)
        {
            return p.AsArrayOfPrimitive();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;Primitive&gt;
        /// </summary>
        /// <returns>List&lt;Primitive&gt; value of this object</returns>
        public virtual List<Primitive> AsListOfPrimitive()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert List&lt;Primitive&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;Primitive&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(List<Primitive> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;Primitive&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;Primitive&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<Primitive>(DynamoDBEntry p)
        {
            return p.AsListOfPrimitive();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;Primitive&gt;
        /// </summary>
        /// <returns>HashSet&lt;Primitive&gt; value of this object</returns>
        public virtual HashSet<Primitive> AsHashSetOfPrimitive()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;Primitive&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">HashSet&lt;Primitive&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(HashSet<Primitive> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;Primitive&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>HashSet&lt;Primitive&gt; value of DynamoDBEntry</returns>
        public static explicit operator HashSet<Primitive>(DynamoDBEntry p)
        {
            return p.AsHashSetOfPrimitive();
        }


        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;Document&gt;
        /// </summary>
        /// <returns>List&lt;Document&gt; value of this object</returns>
        public virtual List<Document> AsListOfDocument()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert List&lt;Document&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;Document&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(List<Document> data)
        {
            return new DynamoDBList(data.Cast<DynamoDBEntry>());
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;Document&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;Document&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<Document>(DynamoDBEntry p)
        {
            return p.AsListOfDocument();
        }


        /// <summary>
        /// Explicitly convert DynamoDBEntry to String[]
        /// </summary>
        /// <returns>String[] value of this object</returns>
        public virtual String[] AsArrayOfString()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert String[] to DynamoDBEntry
        /// </summary>
        /// <param name="data">String[] data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(String[] data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to String[]
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>String[] value of DynamoDBEntry</returns>
        public static explicit operator String[](DynamoDBEntry p)
        {
            return p.AsArrayOfString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public virtual List<String> AsListOfString()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert List&lt;String&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;String&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(List<String> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;String&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;String&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<String>(DynamoDBEntry p)
        {
            return p.AsListOfString();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;String&gt;
        /// </summary>
        /// <returns>List&lt;String&gt; value of this object</returns>
        public virtual HashSet<String> AsHashSetOfString()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;String&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">HashSet&lt;String&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(HashSet<String> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;String&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>HashSet&lt;String&gt; value of DynamoDBEntry</returns>
        public static explicit operator HashSet<String>(DynamoDBEntry p)
        {
            return p.AsHashSetOfString();
        }


        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;byte[]&gt;
        /// </summary>
        /// <returns>List&lt;byte[]&gt; value of this object</returns>
        public virtual List<byte[]> AsListOfByteArray()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert List&lt;byte[]&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;byte[]&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(List<byte[]> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;byte[]&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<byte[]>(DynamoDBEntry p)
        {
            return p.AsListOfByteArray();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <returns>HashSet&lt;byte[]&gt; value of this object</returns>
        public virtual HashSet<byte[]> AsHashSetOfByteArray()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;byte[]&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">HashSet&lt;byte[]&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(HashSet<byte[]> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;byte[]&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>HashSet&lt;byte[]&gt; value of DynamoDBEntry</returns>
        public static explicit operator HashSet<byte[]>(DynamoDBEntry p)
        {
            return p.AsHashSetOfByteArray();
        }


        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;MemoryStream&gt;
        /// </summary>
        /// <returns>List&lt;MemoryStream&gt; value of this object</returns>
        public virtual List<MemoryStream> AsListOfMemoryStream()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert List&lt;MemoryStream&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">List&lt;MemoryStream&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(List<MemoryStream> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to List&lt;MemoryStream&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>List&lt;MemoryStream&gt; value of DynamoDBEntry</returns>
        public static explicit operator List<MemoryStream>(DynamoDBEntry p)
        {
            return p.AsListOfMemoryStream();
        }

        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;MemoryStream&gt;
        /// </summary>
        /// <returns>HashSet&lt;MemoryStream&gt; value of this object</returns>
        public virtual HashSet<MemoryStream> AsHashSetOfMemoryStream()
        {
            throw new InvalidCastException();
        }
        /// <summary>
        /// Implicitly convert HashSet&lt;MemoryStream&gt; to DynamoDBEntry
        /// </summary>
        /// <param name="data">HashSet&lt;MemoryStream&gt; data to convert</param>
        /// <returns>DynamoDBEntry representing the data</returns>
        public static implicit operator DynamoDBEntry(HashSet<MemoryStream> data)
        {
            return new UnconvertedDynamoDBEntry(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBEntry to HashSet&lt;MemoryStream&gt;
        /// </summary>
        /// <param name="p">DynamoDBEntry to convert</param>
        /// <returns>HashSet&lt;MemoryStream&gt; value of DynamoDBEntry</returns>
        public static explicit operator HashSet<MemoryStream>(DynamoDBEntry p)
        {
            return p.AsHashSetOfMemoryStream();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Declare the Clone method must be implemented.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        #endregion
    }

    /// <summary>
    /// A DynamoDBEntry holding an unconverted object.
    /// The entry is converted to a converted DynamoDBEntry either by the
    /// consuming Document or Table.
    /// </summary>
    internal class UnconvertedDynamoDBEntry : DynamoDBEntry
    {
        private object Value;
        private Type ValueType;

        /// <summary>
        /// Construct instance of UnconvertedDynamoDBEntry
        /// </summary>
        /// <param name="value"></param>
        public UnconvertedDynamoDBEntry(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Value = value;
            ValueType = value.GetType();
        }

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            using (conversionConfig.CRT.Track(Value))
            {
                var convertedEntry = Convert(conversionConfig.Conversion);
                return convertedEntry.ConvertToAttributeValue(conversionConfig);
            }
        }

        /// <summary>
        /// Perform conversion with the given converter.
        /// </summary>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public DynamoDBEntry Convert(DynamoDBEntryConversion conversion)
        {
            var convertedEntry = conversion.ConvertToEntry(ValueType, Value);
            return convertedEntry;
        }

        /// <summary>
        /// Implememnt the Clone method.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new UnconvertedDynamoDBEntry(Value);
        }

        #region Conversion overrides - Convert.ChangeType

        /// <summary>
        /// Return the value as a boolean.
        /// </summary>
        /// <returns></returns>
        public override bool AsBoolean()
        {
            return (bool)System.Convert.ChangeType(Value, typeof(bool), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a byte.
        /// </summary>
        /// <returns></returns>
        public override byte AsByte()
        {
            return (byte)System.Convert.ChangeType(Value, typeof(byte), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a string.
        /// </summary>
        /// <returns></returns>
        public override string AsString()
        {
            return System.Convert.ChangeType(Value, typeof(string), CultureInfo.InvariantCulture) as string;
        }

        /// <summary>
        /// Return the value as a char.
        /// </summary>
        /// <returns></returns>
        public override char AsChar()
        {
            return (char)System.Convert.ChangeType(Value, typeof(char), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a DateTime.
        /// </summary>
        /// <returns></returns>
        public override DateTime AsDateTime()
        {
            return (DateTime)System.Convert.ChangeType(Value, typeof(DateTime), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a Decimal.
        /// </summary>
        /// <returns></returns>
        public override decimal AsDecimal()
        {
            return (decimal)System.Convert.ChangeType(Value, typeof(decimal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a double.
        /// </summary>
        /// <returns></returns>
        public override double AsDouble()
        {
            return (double)System.Convert.ChangeType(Value, typeof(double), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as int.
        /// </summary>
        /// <returns></returns>
        public override int AsInt()
        {
            return (int)System.Convert.ChangeType(Value, typeof(int), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a long.
        /// </summary>
        /// <returns></returns>
        public override long AsLong()
        {
            return (long)System.Convert.ChangeType(Value, typeof(long), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a sbyte.
        /// </summary>
        /// <returns></returns>
        public override sbyte AsSByte()
        {
            return (sbyte)System.Convert.ChangeType(Value, typeof(sbyte), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as a short.
        /// </summary>
        /// <returns></returns>
        public override short AsShort()
        {
            return (short)System.Convert.ChangeType(Value, typeof(short), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as single.
        /// </summary>
        /// <returns></returns>
        public override float AsSingle()
        {
            return (float)System.Convert.ChangeType(Value, typeof(float), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as uint.
        /// </summary>
        /// <returns></returns>
        public override uint AsUInt()
        {
            return (uint)System.Convert.ChangeType(Value, typeof(uint), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as ulong.
        /// </summary>
        /// <returns></returns>
        public override ulong AsULong()
        {
            return (ulong)System.Convert.ChangeType(Value, typeof(ulong), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as ushort.
        /// </summary>
        /// <returns></returns>
        public override ushort AsUShort()
        {
            return (ushort)System.Convert.ChangeType(Value, typeof(ushort), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the value as array of strings.
        /// </summary>
        /// <returns></returns>
        public override string[] AsArrayOfString()
        {
            return base.AsArrayOfString();
        }

        #endregion

        #region Conversion overrides - Cast

        /// <summary>
        /// Return value as byte[].
        /// </summary>
        /// <returns></returns>
        public override byte[] AsByteArray()
        {
            return (byte[])Value;
        }

        /// <summary>
        /// Return value as Guid.
        /// </summary>
        /// <returns></returns>
        public override Guid AsGuid()
        {
            return (Guid)Value;
        }

        /// <summary>
        /// Return value as MemoryStream.
        /// </summary>
        /// <returns></returns>
        public override MemoryStream AsMemoryStream()
        {
            return (MemoryStream)Value;
        }

        /// <summary>
        /// Return value as List of strings.
        /// </summary>
        /// <returns></returns>
        public override List<string> AsListOfString()
        {
            return (List<string>)Value;
        }

        /// <summary>
        /// Return value as HashSet of byte[].
        /// </summary>
        /// <returns></returns>
        public override HashSet<byte[]> AsHashSetOfByteArray()
        {
            return (HashSet<byte[]>)Value;
        }

        /// <summary>
        /// Return value as HashSet of MemoryStream.
        /// </summary>
        /// <returns></returns>
        public override HashSet<MemoryStream> AsHashSetOfMemoryStream()
        {
            return (HashSet<MemoryStream>)Value;
        }

        /// <summary>
        /// Return value as HashSet of string.
        /// </summary>
        /// <returns></returns>
        public override HashSet<string> AsHashSetOfString()
        {
            return (HashSet<string>)Value;
        }

        /// <summary>
        /// Return value as List of byte[].
        /// </summary>
        /// <returns></returns>
        public override List<byte[]> AsListOfByteArray()
        {
            return (List<byte[]>)Value;
        }

        /// <summary>
        /// Return value as List of MemoryStream.
        /// </summary>
        /// <returns></returns>
        public override List<MemoryStream> AsListOfMemoryStream()
        {
            return (List<MemoryStream>)Value;
        } 

        #endregion

        #region Public overrides

        public override int GetHashCode()
        {
            return Hashing.Hash(ValueType, Value);
        }
        public override bool Equals(object obj)
        {
            var uddbe = obj as UnconvertedDynamoDBEntry;
            if (uddbe == null)
                return false;

            return (ValueType.Equals(uddbe.ValueType) &&
                Value.Equals(uddbe.Value));
        }

        #endregion
    }
}
