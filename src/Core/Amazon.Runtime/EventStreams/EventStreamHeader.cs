/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *
 */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Text;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.EventStreams
{
    #region headerParser
    /// <summary>
    ///  Supported Header types for vnd.amazon.event-stream
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028", Justification = "Low-level memory usage")]
    public enum EventStreamHeaderType : byte
    {
        BoolTrue = 0,
        BoolFalse,
        Byte,
        Int16,
        Int32,
        Int64,
        ByteBuf,
        String,
        Timestamp,
        UUID
    }

    public interface IEventStreamHeader
    {
        /// <summary>
        /// Name for the Header. Maximum of 255 bytes.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Header type id
        /// </summary>
        EventStreamHeaderType HeaderType { get; }

        /// <summary>
        /// Computes the amount of memory neccesary to serialize this Header.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024", Justification = "Method can throw an exception.")]
        int GetWireSize();

        /// <summary>
        /// Writes this Header to buffer starting at offset
        /// Keep in mind, this API assumes buffer is large enough
        /// for the operation.
        /// </summary>
        /// <param name="buffer">buffer to serialize this Header to</param>
        /// <param name="offset">offset to begin writing at.</param>
        /// <returns>
        /// the new offset.
        /// </returns>
        int WriteToBuffer(byte[] buffer, int offset);

        /// <summary>
        /// Returns the current value as a bool
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720", Justification = "Current name is most descriptive.")]
        bool AsBool();

        /// <summary>
        /// Sets the current value
        /// </summary>
        void SetBool(bool value);

        /// <summary>
        /// Returns the current value as a byte
        /// </summary>
        Byte AsByte();

        /// <summary>
        /// Sets the current value
        /// </summary>
        void SetByte(Byte value);

        /// <summary>
        /// Gets the current value as a 16 bit integer. (Host Order).
        /// </summary>
        /// <returns></returns>
        Int16 AsInt16();

        /// <summary>
        /// Sets the current value. (Host Order)
        /// </summary>
        void SetInt16(Int16 value);

        /// <summary>
        /// Returns the current value as a 32 bit integer. (Host Order)
        /// </summary>
        Int32 AsInt32();

        /// <summary>
        /// Sets the current value
        /// </summary>
        void SetInt32(Int32 value);

        /// <summary>
        /// returns the current value as a 64-bit integer. (Host Order)
        /// </summary>
        Int64 AsInt64();

        /// <summary>
        /// Sets the current value. (Host Order)
        /// </summary>
        void SetInt64(Int64 value);

        /// <summary>
        /// Returns the current value as a byte buffer.
        /// </summary>
        /// <returns></returns>
        byte[] AsByteBuf();

        /// <summary>
        /// Sets the current value. Max length is 2^15 - 1
        /// </summary>
        void SetByteBuf(byte[] value);

        /// <summary>
        /// Returns the current value as a utf-8 string.
        /// </summary>
        string AsString();

        /// <summary>
        /// Sets the current value. Utf-8 encoded. Max byte size is 2^16 - 1
        /// </summary>
        void SetString(string value);

        /// <summary>
        /// Gets the current value as a DateTime.
        /// Note: You do not need to compensate for unix epoch on this API.
        /// </summary>
        /// <returns></returns>
        DateTime AsTimestamp();

        /// <summary>
        /// Sets the current value.
        /// Note: You do not need to compensate for unix epoch on this API.
        /// </summary>
        void SetTimestamp(DateTime value);

        /// <summary>
        /// Returns the current value as a Guid (UUID)
        /// </summary>
        Guid AsUUID();

        /// <summary>
        /// Sets the current value
        /// </summary>
        void SetUUID(Guid value);
    }

    /// <summary>
    /// Header Data for an EventStreamMessage
    /// 
    /// Each header is of format:
    /// [name len] (1) | [utf-8 name] (v)
    /// [type (1)]
    /// [value (v)]
    /// </summary>
    public class EventStreamHeader : IEventStreamHeader
    {
        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const int _sizeOfByte = 1;
        private const int _sizeOfInt16 = 2;
        private const int _sizeOfInt32 = 4;
        private const int _sizeOfInt64 = 8;
        private const int _sizeOfGuid = 16;

        /// <summary>
        /// Name for the Header. Maximum of 255 bytes.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Header type id
        /// </summary>
        public EventStreamHeaderType HeaderType { get; set; }

        private Object HeaderValue { get; set; }

        /// <summary>
        /// Initializes this Header with name.
        /// </summary>
        /// <param name="name">utf-8 string, max length is 255</param>
        public EventStreamHeader(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Deserializes the header in buffer.
        /// </summary>
        /// <param buffer="buffer">buffer constaining the header</param>
        /// <param offset="offset">index to start from in the buffer.</param>
        /// <param offset="newOffset">updated offset with new value for offset</param>
        [SuppressMessage("Microsoft.Design", "CA1045", Justification = "Performance and Cross-Runtime compatability.")]
        public static EventStreamHeader FromBuffer(byte[] buffer, int offset, ref int newOffset)
        {
            newOffset = offset;
            byte nameLength = buffer[newOffset++];
            var header = new EventStreamHeader(Encoding.UTF8.GetString(buffer, newOffset, nameLength));
            newOffset += nameLength;

            header.HeaderType = (EventStreamHeaderType)buffer[newOffset++];

            Int16 valueLength = 0;
            switch (header.HeaderType)
            {
                case EventStreamHeaderType.BoolTrue:
                    header.HeaderValue = true;
                    break;
                case EventStreamHeaderType.BoolFalse:
                    header.HeaderValue = false;
                    break;
                case EventStreamHeaderType.Byte:
                    header.HeaderValue = buffer[newOffset];
                    newOffset += _sizeOfByte;
                    break;
                case EventStreamHeaderType.Int16:
                    header.HeaderValue = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, newOffset));
                    newOffset += _sizeOfInt16;
                    break;
                case EventStreamHeaderType.Int32:
                    header.HeaderValue = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, newOffset));
                    newOffset += _sizeOfInt32;
                    break;
                case EventStreamHeaderType.Int64:
                    header.HeaderValue = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer, newOffset));
                    newOffset += _sizeOfInt64;
                    break;
                case EventStreamHeaderType.ByteBuf:
                    valueLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, newOffset));
                    newOffset += _sizeOfInt16;
                    header.HeaderValue = new byte[valueLength];
                    Buffer.BlockCopy(buffer, newOffset, header.HeaderValue as byte[], 0, valueLength);
                    newOffset += valueLength;
                    break;
                case EventStreamHeaderType.String:
                    valueLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, newOffset));
                    newOffset += _sizeOfInt16;
                    header.HeaderValue = Encoding.UTF8.GetString(buffer, newOffset, valueLength);
                    newOffset += valueLength;
                    break;
                case EventStreamHeaderType.Timestamp:
                    Int64 tempValue = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer, newOffset));
                    newOffset += _sizeOfInt64;
                    //keep in mind on the windows APIs (and hence NetStandard as well) the epoch is 1/1/1900,
                    //and we're using unix epoch. So we compensate here.
                    header.HeaderValue = _unixEpoch.AddMilliseconds(tempValue);
                    break;
                case EventStreamHeaderType.UUID:
                    var guidCpy = new byte[16];
                    valueLength = _sizeOfGuid;
                    Buffer.BlockCopy(buffer, newOffset, guidCpy, 0, valueLength);
                    newOffset += valueLength;
                    header.HeaderValue = new Guid(guidCpy);
                    break;
                default:
                    throw new EventStreamParseException(string.Format(CultureInfo.InvariantCulture, "Header Type: {0} is an unknown type.", header.HeaderType));
            }

            return header;
        }

        /// <summary>
        /// Writes this Header to buffer starting at offset
        /// Keep in mind, this API assumes buffer is large enough
        /// for the operation.
        /// </summary>
        /// <param name="buffer">buffer to serialize this Header to</param>
        /// <param name="offset">offset to begin writing at.</param>
        /// <returns>
        /// the new offset.
        /// </returns>
        public int WriteToBuffer(byte[] buffer, int offset)
        {
            var newOffset = offset;
            buffer[newOffset++] = (byte)Name.Length;
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            Buffer.BlockCopy(nameBytes, 0, buffer, newOffset, Name.Length);
            newOffset += Name.Length;

            buffer[newOffset++] = (byte)HeaderType;
            byte[] serializedBytes = null;
            int valueLength = 0;

            switch (HeaderType)
            {
                case EventStreamHeaderType.BoolTrue:
                case EventStreamHeaderType.BoolFalse:
                    break;
                case EventStreamHeaderType.Byte:
                    buffer[newOffset++] = (byte)HeaderValue;
                    break;
                case EventStreamHeaderType.Int16:
                    serializedBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)HeaderValue));
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, 2);
                    newOffset += _sizeOfInt16;
                    break;
                case EventStreamHeaderType.Int32:
                    serializedBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int32)HeaderValue));
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, 4);
                    newOffset += _sizeOfInt32;
                    break;
                case EventStreamHeaderType.Int64:
                    serializedBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int64)HeaderValue));
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, 8);
                    newOffset += _sizeOfInt64;
                    break;
                case EventStreamHeaderType.ByteBuf:
                    serializedBytes = HeaderValue as byte[];
                    valueLength = serializedBytes.Length;
                    Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)valueLength)), 0,
                        buffer, newOffset, 2);
                    newOffset += _sizeOfInt16;
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, valueLength);
                    newOffset += valueLength;
                    break;
                case EventStreamHeaderType.String:
                    serializedBytes = Encoding.UTF8.GetBytes(HeaderValue as string);
                    valueLength = serializedBytes.Length;
                    Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)valueLength)), 0,
                        buffer, newOffset, 2);
                    newOffset += _sizeOfInt16;
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, valueLength);
                    newOffset += valueLength;
                    break;
                case EventStreamHeaderType.Timestamp:
                    var tempValue = (Int64)((DateTime)HeaderValue).Subtract(_unixEpoch).TotalMilliseconds;
                    serializedBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(tempValue));
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, 8);
                    newOffset += _sizeOfInt64;
                    break;
                case EventStreamHeaderType.UUID:
                    serializedBytes = ((Guid)HeaderValue).ToByteArray();
                    Buffer.BlockCopy(serializedBytes, 0, buffer, newOffset, serializedBytes.Length);
                    newOffset += serializedBytes.Length;
                    break;
                default:
                    throw new EventStreamParseException(string.Format(CultureInfo.InvariantCulture, "Header Type: {0} is an unknown type.", HeaderType));
            }

            return newOffset;
        }

        /// <summary>
        /// Computes the amount of memory neccesary to serialize this Header.
        /// </summary>
        public int GetWireSize()
        {
            //each header is of format:
            // [name len] (1) | [utf-8 name] (v)
            // [type (1)]
            // [value (v)]
            var len = _sizeOfByte + Name.Length + _sizeOfByte;
            switch (HeaderType)
            {
                case EventStreamHeaderType.BoolTrue:
                case EventStreamHeaderType.BoolFalse:
                    break;
                case EventStreamHeaderType.Byte:
                    len += _sizeOfByte;
                    break;
                case EventStreamHeaderType.Int16:
                    len += _sizeOfInt16;
                    break;
                case EventStreamHeaderType.Int32:
                    len += _sizeOfInt32;
                    break;
                case EventStreamHeaderType.Int64:
                    len += _sizeOfInt64;
                    break;
                case EventStreamHeaderType.ByteBuf:
                    var bbuf = HeaderValue as byte[];
                    len += _sizeOfInt16 + bbuf.Length; //2 byte len | buffer
                    break;
                case EventStreamHeaderType.String:
                    var strLen = Encoding.UTF8.GetBytes(HeaderValue as string).Length;
                    len += _sizeOfInt16 + strLen; //2 byte len | utf-8 buffer
                    break;
                case EventStreamHeaderType.Timestamp:
                    len += _sizeOfInt64;
                    break;
                case EventStreamHeaderType.UUID:
                    len += _sizeOfGuid;
                    break;
                default:
                    throw new EventStreamParseException(string.Format(CultureInfo.InvariantCulture, "Header Type: {0} is an unknown type.", HeaderType));
            }

            return len;
        }

        /// <summary>
        /// Returns the current value as a bool
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720", Justification = "Current name is most descriptive.")]
        public bool AsBool()
        {
            return HeaderType == EventStreamHeaderType.BoolTrue;
        }

        /// <summary>
        /// Sets the current value
        /// </summary>
        public void SetBool(bool value)
        {
            HeaderValue = value;
            HeaderType = value ? EventStreamHeaderType.BoolTrue : EventStreamHeaderType.BoolFalse;
        }

        /// <summary>
        /// Returns the current value as a byte
        /// </summary>
        public Byte AsByte()
        {
            return (Byte)HeaderValue;
        }

        /// <summary>
        /// Sets the current value
        /// </summary>
        public void SetByte(Byte value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.Byte;
        }

        /// <summary>
        /// Gets the current value as a 16 bit integer. (Host Order).
        /// </summary>
        /// <returns></returns>
        public Int16 AsInt16()
        {
            return (Int16)HeaderValue;
        }

        /// <summary>
        /// Sets the current value. (Host Order)
        /// </summary>
        public void SetInt16(Int16 value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.Int16;
        }

        /// <summary>
        /// Returns the current value as a 32 bit integer. (Host Order)
        /// </summary>
        public Int32 AsInt32()
        {
            return (Int32)HeaderValue;
        }

        /// <summary>
        /// Sets the current value
        /// </summary>
        public void SetInt32(Int32 value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.Int32;
        }

        /// <summary>
        /// returns the current value as a 64-bit integer. (Host Order)
        /// </summary>
        public Int64 AsInt64()
        {
            return (Int64)HeaderValue;
        }

        /// <summary>
        /// Sets the current value. (Host Order)
        /// </summary>
        public void SetInt64(Int64 value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.Int64;
        }

        /// <summary>
        /// Returns the current value as a byte buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] AsByteBuf()
        {
            return HeaderValue as byte[];
        }

        /// <summary>
        /// Sets the current value. Max length is 2^15 - 1
        /// </summary>
        public void SetByteBuf(byte[] value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.ByteBuf;
        }

        /// <summary>
        /// Returns the current value as a utf-8 string.
        /// </summary>
        public string AsString()
        {
            return HeaderValue as string;
        }

        /// <summary>
        /// Sets the current value. Utf-8 encoded. Max byte size is 2^16 - 1
        /// </summary>
        public void SetString(string value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.String;
        }

        /// <summary>
        /// Gets the current value as a DateTime.
        /// Note: You do not need to compensate for unix epoch on this API.
        /// </summary>
        /// <returns></returns>
        public DateTime AsTimestamp()
        {
            return (DateTime)HeaderValue;
        }

        /// <summary>
        /// Sets the current value.
        /// Note: You do not need to compensate for unix epoch on this API.
        /// </summary>
        public void SetTimestamp(DateTime value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.Timestamp;
        }

        /// <summary>
        /// Returns the current value as a Guid (UUID)
        /// </summary>
        public Guid AsUUID()
        {
            return (Guid)HeaderValue;
        }

        /// <summary>
        /// Sets the current value
        /// </summary>
        public void SetUUID(Guid value)
        {
            HeaderValue = value;
            HeaderType = EventStreamHeaderType.UUID;
        }
    }
    #endregion
}
