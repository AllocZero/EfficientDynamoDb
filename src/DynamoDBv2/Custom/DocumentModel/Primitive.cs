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
using System.Globalization;
using System.IO;

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System.Collections;
using System.Collections.Generic;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Enumerator describing type of DynamoDB data in a Primitive or PrimitiveList
    /// </summary>
    public enum DynamoDBEntryType { 
        /// <summary>
        /// DynamoDB String type.
        /// </summary>
        String, 
        /// <summary>
        /// DynamoDB Numeric type.
        /// </summary>
        Numeric, 
        /// <summary>
        /// DynamoDB Binary type.
        /// </summary>
        Binary 
    }

    /// <summary>
    /// A DynamoDBEntry that represents a scalar DynamoDB type
    /// </summary>
    public class Primitive : DynamoDBEntry, IEquatable<Primitive>
    {
        #region Private members

        private static DynamoDBEntryConversion V1Conversion = DynamoDBEntryConversion.V1;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty Primitive
        /// </summary>
        public Primitive()
            : this(null, DynamoDBEntryType.String)
        {
        }

        /// <summary>
        /// Constructs a Primitive with the specified value.
        /// Value is stored as a string, not numeric.
        /// </summary>
        /// <param name="value">Value of the Primitive</param>
        public Primitive(string value)
            : this(value, DynamoDBEntryType.String)
        {
        }

        /// <summary>
        /// Constructs a Primitive with the specified value
        /// and specifies whether it should be stored as a number or not.
        /// </summary>
        /// <param name="value">Value of the Primitive</param>
        /// <param name="saveAsNumeric">
        /// Flag, set to true if value should be treated as a number instead of a string
        /// </param>
        public Primitive(string value, bool saveAsNumeric)
            : this(value, saveAsNumeric ? DynamoDBEntryType.Numeric : DynamoDBEntryType.String)
        {
        }

        /// <summary>
        /// Constructs a Binary Primitive with the specified MemoryStream value.
        /// Note: Primitive's Value is set to the stream's ToArray() response.
        /// </summary>
        /// <param name="value">Value of the Primitive</param>
        public Primitive(MemoryStream value)
            : this(value.ToArray()) { }

        /// <summary>
        /// Constructs a Binary Primitive with the specified byte[] value.
        /// </summary>
        /// <param name="value">Value of the Primitive</param>
        public Primitive(byte[] value)
            : this(value, DynamoDBEntryType.Binary) { }

        internal Primitive(object value, DynamoDBEntryType type)
        {
            Value = value;
            Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value of the Primitive.
        /// If Type is String or Numeric, this property is a string.
        /// If Type is Binary, this property is a byte array.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Type of this primitive object
        /// </summary>
        public DynamoDBEntryType Type { get; set; }

        #endregion

        #region Internal conversion methods

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            if (this.Value == null)
                return null;

            if (conversionConfig.IsEmptyStringValueEnabled)
            {
                if (this.Type != DynamoDBEntryType.Binary && StringValue == null)
                {
                    return null;
                }
            }
            else
            {
                if (this.Type != DynamoDBEntryType.Binary && string.IsNullOrEmpty(this.StringValue))
                {
                    return null;
                }
            }

            AttributeValue attribute = new AttributeValue();
            switch (this.Type)
            {
                case DynamoDBEntryType.Numeric:
                    attribute.N = StringValue;
                    break;
                case DynamoDBEntryType.String:
                    attribute.S = StringValue;
                    break;
                case DynamoDBEntryType.Binary:
                    byte[] bytes = (byte[])Value;
                    attribute.B = new MemoryStream(bytes);
                    break;
            }
            return attribute;
        }

        internal string StringValue
        {
            get
            {
                if (this.Type == DynamoDBEntryType.Numeric || this.Type == DynamoDBEntryType.String)
                    return this.Value as string;
                return null;
            }
        }

        #endregion

        #region Explicit and Implicit conversions

        /// <summary>
        /// Explicitly convert Primitive to Boolean
        /// </summary>
        /// <returns>Boolean value of this object</returns>
        public override Boolean AsBoolean()
        {
            return V1Conversion.ConvertFromEntry<Boolean>(this);
        }
        /// <summary>
        /// Implicitly convert Boolean to Primitive
        /// </summary>
        /// <param name="data">Boolean data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Boolean data)
        {
            return V1Conversion.ConvertToEntry<Boolean>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Boolean
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Boolean value of Primitive</returns>
        public static explicit operator Boolean(Primitive p)
        {
            return p.AsBoolean();
        }



        /// <summary>
        /// Explicitly convert Primitive to Byte
        /// </summary>
        /// <returns>Byte value of this object</returns>
        public override Byte AsByte()
        {
            return V1Conversion.ConvertFromEntry<Byte>(this);
        }
        /// <summary>
        /// Implicitly convert Byte to Primitive
        /// </summary>
        /// <param name="data">Byte data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Byte data)
        {
            return V1Conversion.ConvertToEntry<Byte>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Byte
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Byte value of Primitive</returns>
        public static explicit operator Byte(Primitive p)
        {
            return p.AsByte();
        }



        /// <summary>
        /// Explicitly convert Primitive to SByte
        /// </summary>
        /// <returns>SByte value of this object</returns>
        [CLSCompliant(false)]
        public override SByte AsSByte()
        {
            return V1Conversion.ConvertFromEntry<SByte>(this);
        }
        /// <summary>
        /// Implicitly convert SByte to Primitive
        /// </summary>
        /// <param name="data">SByte data to convert</param>
        /// <returns>Primitive representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator Primitive(SByte data)
        {
            return V1Conversion.ConvertToEntry<SByte>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to SByte
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>SByte value of Primitive</returns>
        [CLSCompliant(false)]
        public static explicit operator SByte(Primitive p)
        {
            return p.AsSByte();
        }



        /// <summary>
        /// Explicitly convert Primitive to UInt16
        /// </summary>
        /// <returns>UInt16 value of this object</returns>
        [CLSCompliant(false)]
        public override UInt16 AsUShort()
        {
            return V1Conversion.ConvertFromEntry<UInt16>(this);
        }
        /// <summary>
        /// Implicitly convert UInt16 to Primitive
        /// </summary>
        /// <param name="data">UInt16 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator Primitive(UInt16 data)
        {
            return V1Conversion.ConvertToEntry<UInt16>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to UInt16
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>UInt16 value of Primitive</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt16(Primitive p)
        {
            return p.AsUShort();
        }



        /// <summary>
        /// Explicitly convert Primitive to Int16
        /// </summary>
        /// <returns>Int16 value of this object</returns>
        public override Int16 AsShort()
        {
            return V1Conversion.ConvertFromEntry<Int16>(this);
        }
        /// <summary>
        /// Implicitly convert Int16 to Primitive
        /// </summary>
        /// <param name="data">Int16 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Int16 data)
        {
            return V1Conversion.ConvertToEntry<Int16>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Int16
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Int16 value of Primitive</returns>
        public static explicit operator Int16(Primitive p)
        {
            return p.AsShort();
        }



        /// <summary>
        /// Explicitly convert Primitive to UInt32
        /// </summary>
        /// <returns>UInt32 value of this object</returns>
        [CLSCompliant(false)]
        public override UInt32 AsUInt()
        {
            return V1Conversion.ConvertFromEntry<UInt32>(this);
        }
        /// <summary>
        /// Implicitly convert UInt32 to Primitive
        /// </summary>
        /// <param name="data">UInt32 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator Primitive(UInt32 data)
        {
            return V1Conversion.ConvertToEntry<UInt32>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to UInt32
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>UInt32 value of Primitive</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt32(Primitive p)
        {
            return p.AsUInt();
        }



        /// <summary>
        /// Explicitly convert Primitive to Int32
        /// </summary>
        /// <returns>Int32 value of this object</returns>
        public override Int32 AsInt()
        {
            return V1Conversion.ConvertFromEntry<Int32>(this);
        }
        /// <summary>
        /// Implicitly convert Int32 to Primitive
        /// </summary>
        /// <param name="data">Int32 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Int32 data)
        {
            return V1Conversion.ConvertToEntry<Int32>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Int32
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Int32 value of Primitive</returns>
        public static explicit operator Int32(Primitive p)
        {
            return p.AsInt();
        }



        /// <summary>
        /// Explicitly convert Primitive to UInt64
        /// </summary>
        /// <returns>UInt64 value of this object</returns>
        [CLSCompliant(false)]
        public override UInt64 AsULong()
        {
            return V1Conversion.ConvertFromEntry<UInt64>(this);
        }
        /// <summary>
        /// Implicitly convert UInt64 to Primitive
        /// </summary>
        /// <param name="data">UInt64 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        [CLSCompliant(false)]
        public static implicit operator Primitive(UInt64 data)
        {
            return V1Conversion.ConvertToEntry<UInt64>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to UInt64
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>UInt64 value of Primitive</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt64(Primitive p)
        {
            return p.AsULong();
        }



        /// <summary>
        /// Explicitly convert Primitive to Int64
        /// </summary>
        /// <returns>Int64 value of this object</returns>
        public override Int64 AsLong()
        {
            return V1Conversion.ConvertFromEntry<Int64>(this);
        }
        /// <summary>
        /// Implicitly convert Int64 to Primitive
        /// </summary>
        /// <param name="data">Int64 data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Int64 data)
        {
            return V1Conversion.ConvertToEntry<Int64>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Int64
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Int64 value of Primitive</returns>
        public static explicit operator Int64(Primitive p)
        {
            return p.AsLong();
        }



        /// <summary>
        /// Explicitly convert Primitive to Single
        /// </summary>
        /// <returns>Single value of this object</returns>
        public override Single AsSingle()
        {
            return V1Conversion.ConvertFromEntry<Single>(this);
        }
        /// <summary>
        /// Implicitly convert Single to Primitive
        /// </summary>
        /// <param name="data">Single data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Single data)
        {
            return V1Conversion.ConvertToEntry<Single>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Single
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Single value of Primitive</returns>
        public static explicit operator Single(Primitive p)
        {
            return p.AsSingle();
        }



        /// <summary>
        /// Explicitly convert Primitive to Double
        /// </summary>
        /// <returns>Double value of this object</returns>
        public override Double AsDouble()
        {
            return V1Conversion.ConvertFromEntry<Double>(this);
        }
        /// <summary>
        /// Implicitly convert Double to Primitive
        /// </summary>
        /// <param name="data">Double data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Double data)
        {
            return V1Conversion.ConvertToEntry<Double>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Double
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Double value of Primitive</returns>
        public static explicit operator Double(Primitive p)
        {
            return p.AsDouble();
        }



        /// <summary>
        /// Explicitly convert Primitive to Decimal
        /// </summary>
        /// <returns>Decimal value of this object</returns>
        public override Decimal AsDecimal()
        {
            return V1Conversion.ConvertFromEntry<Decimal>(this);
        }
        /// <summary>
        /// Implicitly convert Decimal to Primitive
        /// </summary>
        /// <param name="data">Decimal data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Decimal data)
        {
            return V1Conversion.ConvertToEntry<Decimal>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Decimal
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Decimal value of Primitive</returns>
        public static explicit operator Decimal(Primitive p)
        {
            return p.AsDecimal();
        }



        /// <summary>
        /// Explicitly convert Primitive to Char
        /// </summary>
        /// <returns>Char value of this object</returns>
        public override Char AsChar()
        {
            return V1Conversion.ConvertFromEntry<Char>(this);
        }
        /// <summary>
        /// Implicitly convert Char to Primitive
        /// </summary>
        /// <param name="data">Char data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Char data)
        {
            return V1Conversion.ConvertToEntry<Char>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Char
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Char value of Primitive</returns>
        public static explicit operator Char(Primitive p)
        {
            return p.AsChar();
        }



        /// <summary>
        /// Explicitly convert Primitive to String
        /// </summary>
        /// <returns>String value of this object</returns>
        public override String AsString()
        {
            return V1Conversion.ConvertFromEntry<String>(this);
        }
        /// <summary>
        /// Implicitly convert String to Primitive
        /// </summary>
        /// <param name="data">String data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(String data)
        {
            return V1Conversion.ConvertToEntry<String>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to String
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>String value of Primitive</returns>
        public static implicit operator String(Primitive p)
        {
            return p.AsString();
        }



        /// <summary>
        /// Explicitly convert Primitive to DateTime
        /// </summary>
        /// <returns>DateTime value of this object</returns>
        public override DateTime AsDateTime()
        {
            return V1Conversion.ConvertFromEntry<DateTime>(this);
        }
        /// <summary>
        /// Implicitly convert DateTime to Primitive
        /// </summary>
        /// <param name="data">DateTime data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(DateTime data)
        {
            return V1Conversion.ConvertToEntry<DateTime>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to DateTime
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>DateTime value of Primitive</returns>
        public static explicit operator DateTime(Primitive p)
        {
            return p.AsDateTime();
        }



        /// <summary>
        /// Explicitly convert Primitive to Guid
        /// </summary>
        /// <returns>Guid value of this object</returns>
        public override Guid AsGuid()
        {
            return V1Conversion.ConvertFromEntry<Guid>(this);
        }
        /// <summary>
        /// Implicitly convert Guid to Primitive
        /// </summary>
        /// <param name="data">Guid data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(Guid data)
        {
            return V1Conversion.ConvertToEntry<Guid>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to Guid
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>Guid value of Primitive</returns>
        public static explicit operator Guid(Primitive p)
        {
            return p.AsGuid();
        }



        /// <summary>
        /// Explicitly convert Primitive to byte[]
        /// </summary>
        /// <returns>byte[] value of this object</returns>
        public override byte[] AsByteArray()
        {
            return V1Conversion.ConvertFromEntry<byte[]>(this);
        }
        /// <summary>
        /// Implicitly convert byte[] to Primitive
        /// </summary>
        /// <param name="data">byte[] data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(byte[] data)
        {
            return V1Conversion.ConvertToEntry<byte[]>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to byte[]
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>byte[] value of Primitive</returns>
        public static explicit operator byte[](Primitive p)
        {
            return p.AsByteArray();
        }



        /// <summary>
        /// Explicitly convert Primitive to MemoryStream
        /// </summary>
        /// <returns>MemoryStream value of this object</returns>
        public override MemoryStream AsMemoryStream()
        {
            return V1Conversion.ConvertFromEntry<MemoryStream>(this);
        }
        /// <summary>
        /// Implicitly convert MemoryStream to Primitive
        /// </summary>
        /// <param name="data">MemoryStream data to convert</param>
        /// <returns>Primitive representing the data</returns>
        public static implicit operator Primitive(MemoryStream data)
        {
            return V1Conversion.ConvertToEntry<MemoryStream>(data).AsPrimitive();
        }
        /// <summary>
        /// Explicitly convert Primitive to MemoryStream
        /// </summary>
        /// <param name="p">Primitive to convert</param>
        /// <returns>MemoryStream value of Primitive</returns>
        public static explicit operator MemoryStream(Primitive p)
        {
            return p.AsMemoryStream();
        }

        #endregion

        #region Public overrides

        /// <summary>
        /// Implement the ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Value == null)
                return string.Empty;

            return this.Value.ToString();
        }

        /// <summary>
        /// Implement the Clone method.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Primitive(this.Value, this.Type);
        }

        /// <summary>
        /// Implement the GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var typeHashCode = this.Type.GetHashCode();
            var valueHashCode = 0;

            if (this.Value != null)
            {
                if (this.Type == DynamoDBEntryType.Numeric || this.Type == DynamoDBEntryType.String)
                    valueHashCode = this.Value.GetHashCode();
                else if (this.Type == DynamoDBEntryType.Binary)
                {
                    var bytes = this.Value as byte[];
                    if (bytes != null)
                    {
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            byte b = bytes[i];
                            valueHashCode = Hashing.CombineHashes(valueHashCode, b.GetHashCode());
                        }
                    }
                }
            }

            return Hashing.CombineHashes(typeHashCode, valueHashCode);
        }

        /// <summary>
        /// Implement the Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Primitive entryOther = obj as Primitive;
            if (entryOther == null)
                return false;
            if (this.Type != entryOther.Type)
                return false;

            if (this.Type == DynamoDBEntryType.Numeric || this.Type == DynamoDBEntryType.String)
            {
                return (string.Equals(this.StringValue, entryOther.StringValue));
            }
            else if (this.Type == DynamoDBEntryType.Binary)
            {
                byte[] thisByteArray = this.Value as byte[];
                byte[] otherByteArray = entryOther.Value as byte[];

                if (thisByteArray.Length != otherByteArray.Length)
                    return false;

                for (int i = 0; i < thisByteArray.Length; i++)
                {
                    if (thisByteArray[i] != otherByteArray[i])
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEquatable<Primitive> Members

        /// <summary>
        /// Implement the Equals method from the IEquatable interface.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Primitive other)
        {
            return this.Equals((object)other);
        }

        #endregion
    }

    internal class PrimitiveEqualityComparer : IEqualityComparer<Primitive>
    {
        public static readonly PrimitiveEqualityComparer Default = new PrimitiveEqualityComparer();

        public bool Equals(Primitive x, Primitive y)
        {
            if (x == null || y == null)
                return (x == y);

            return x.Equals(y);
        }

        public int GetHashCode(Primitive obj)
        {
            if (obj == null)
                return 0;

            return obj.GetHashCode();
        }
    }

    internal class PrimitiveComparer : IComparer<Primitive>
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

        public static PrimitiveComparer Default = new PrimitiveComparer();
    }
}
