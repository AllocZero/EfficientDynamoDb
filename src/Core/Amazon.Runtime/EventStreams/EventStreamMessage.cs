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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Ionic.Zlib;

namespace Amazon.Runtime.EventStreams
{
    #region exceptions

#if !NETSTANDARD
    [Serializable]
#endif
    public class EventStreamParseException : Exception
    {
        public EventStreamParseException(string message) : base(message)
        {
        }

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the EventStreamParseException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected EventStreamParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

#if !NETSTANDARD
    [Serializable]
#endif
    public class EventStreamChecksumFailureException : Exception
    {
        public EventStreamChecksumFailureException(string message) : base(message)
        {
        }

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the EventStreamChecksumFailureException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected EventStreamChecksumFailureException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

    #endregion

    #region messageImplementation

    public interface IEventStreamMessage
    {
        /// <summary>
        /// Headers for the message. Can be null.
        /// </summary>
        Dictionary<string, IEventStreamHeader> Headers { get; set; }

        /// <summary>
        /// Payload for the message. Can be null.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819", Justification = "This needs to be a byte[], and it makes the most sense as a property.")]
        byte[] Payload { get; set; }

        /// <summary>
        /// Converts a message into a byte buffer (usually for network transmission).
        /// </summary>
        byte[] ToByteArray();
    }

    /// <summary>
    /// Message is a single datagram. The format is as follows:
    /// [ total length (4) ] | [ headers_length (4)] | [ prelude crc(4)]
    /// [ headers (v)]       
    /// [ payload (v)}
    /// [ trailing crc ]
    /// 
    /// CRCs use the CRC32 algorithm.
    /// </summary>
    public class EventStreamMessage : IEventStreamMessage
    {
        internal const int SizeOfInt32 = 4;
        internal const int PreludeLen = SizeOfInt32 * 3;
        internal const int TrailerLen = SizeOfInt32;
        internal const int FramingSize = PreludeLen + TrailerLen;

        /// <summary>
        /// Content type for EventStreams.
        /// </summary>
        public const string ContentType = "vnd.amazon.eventstream";

        /// <summary>
        /// Headers for the message. Can be null.
        /// </summary>
        public Dictionary<string, IEventStreamHeader> Headers { get; set; }

        /// <summary>
        /// Payload for the message. Can be null.
        /// </summary>
        public byte[] Payload { get; set; }

        private EventStreamMessage() { }

        /// <summary>
        /// Initialize a message with headers and a payload.
        /// </summary>
        /// <param name="headers">list of headers for the message, can be null.</param>
        /// <param name="payload">payload for the message, can be null.</param>
        public EventStreamMessage(List<IEventStreamHeader> headers, byte[] payload)
        {
            Headers = new Dictionary<string, IEventStreamHeader>(headers.Count, StringComparer.Ordinal);
            foreach (var header in headers)
            {
                Headers.Add(header.Name, header);
            }

            Payload = payload;
        }

        /// <summary>
        /// Builds a message from buffer.
        /// </summary>
        /// <param name="buffer">buffer to read</param>
        /// <param name="offset">offset to start reading</param>
        /// <param name="length">buffer length.</param>
        /// <returns>
        /// parsed instance of EventStreamMessage. Doesn't return null,
        /// does throw if CRCs don't match.
        /// </returns>
        public static EventStreamMessage FromBuffer(byte[] buffer, int offset, int length)
        {
            var currentOffset = offset;

            //get the total length of the message
            var totalLength = BitConverter.ToInt32(buffer, currentOffset);
            //endianness conversion
            totalLength = IPAddress.NetworkToHostOrder(totalLength);
            currentOffset += SizeOfInt32;

            //get the length of the headers block.
            var headersLength = BitConverter.ToInt32(buffer, currentOffset);
            //endianness conversion
            headersLength = IPAddress.NetworkToHostOrder(headersLength);
            currentOffset += SizeOfInt32;

            //get the prelude crc
            var preludeCrc = BitConverter.ToInt32(buffer, currentOffset);
            //endianness conversion
            preludeCrc = IPAddress.NetworkToHostOrder(preludeCrc);

            var message = new EventStreamMessage();
            message.Headers = new Dictionary<string, IEventStreamHeader>(StringComparer.Ordinal);

            using (var nullStream = new NullStream())
            using (var runningChecksum = new CrcCalculatorStream(nullStream))
            {
                //write up to the prelude crc to the checksum stream
                runningChecksum.Write(buffer, offset, currentOffset - offset);
                //compare the current crc to the prelude crc and make sure they match.
                if (preludeCrc != runningChecksum.Crc32)
                {
                    throw new EventStreamChecksumFailureException(string.Format(CultureInfo.InvariantCulture, "Message Prelude Checksum failure. Expected {0} but was {1}", preludeCrc, runningChecksum.Crc32));
                }

                //if the data length passed isn't enough for the total length, that's an error condition.
                if (totalLength != length)
                {
                    throw new EventStreamChecksumFailureException(
                        string.Format(CultureInfo.InvariantCulture, "Message Total Length didn't match the passed in length. Expected {0} but was {1}", length, totalLength));
                }

                //now write the prelude crc to the checksum stream
                runningChecksum.Write(buffer, currentOffset, SizeOfInt32);
                currentOffset += SizeOfInt32;

                //prelude length is total message, minus framing and headers size.
                var payloadLength = totalLength - headersLength - FramingSize;

                //if we have headers, then loop over each one and parse them out.
                if (headersLength > 0)
                {
                    int preOpOffset = currentOffset;
                    while (currentOffset - PreludeLen < headersLength)
                    {
                        EventStreamHeader header = EventStreamHeader.FromBuffer(buffer, currentOffset, ref currentOffset);
                        message.Headers.Add(header.Name, header);
                    }

                    //after parsing the header remember to write that data to the checksum stream
                    runningChecksum.Write(buffer, preOpOffset, currentOffset - preOpOffset);
                }

                // now we're on the payload
                message.Payload = new byte[payloadLength];
                Buffer.BlockCopy(buffer, currentOffset, message.Payload, 0, message.Payload.Length);
                runningChecksum.Write(buffer, currentOffset, message.Payload.Length);
                currentOffset += message.Payload.Length;

                //after reading the payload, get the message crc and make sure it matches.
                var trailingCrc = BitConverter.ToInt32(buffer, currentOffset);
                //endianness conversion.
                trailingCrc = IPAddress.NetworkToHostOrder(trailingCrc);

                if (trailingCrc != runningChecksum.Crc32)
                {
                    throw new EventStreamChecksumFailureException(
                        string.Format(CultureInfo.InvariantCulture, "Message Checksum failure. Expected {0} but was {1}", trailingCrc, runningChecksum.Crc32));
                }
            }

            return message;
        }

        /// <summary>
        /// Converts a message into a byte buffer (usually for network transmission).
        /// </summary>
        public byte[] ToByteArray()
        {
            int headersWireLength = 0;

            //first we need to figure out how much space the headers will take up.
            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    headersWireLength += header.Value.GetWireSize();
                }
            }

            var payloadLength = Payload?.Length ?? 0;
            //total message length is the framing size + the payload size + the headers wire size.
            var totalLength = headersWireLength + payloadLength + FramingSize;

            var messageBuffer = new byte[totalLength];

            //now write the total length and the headers length to the message. make sure to handle endianness conversions
            var offset = 0;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(totalLength)), 0,
                messageBuffer, offset, SizeOfInt32);
            offset += SizeOfInt32;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(headersWireLength)), 0,
                messageBuffer, offset, SizeOfInt32);
            offset += SizeOfInt32;

            using (var nullStream = new NullStream())
            using (var runningChecksum = new CrcCalculatorStream(nullStream))
            {
                //write the total length and headers length to the checksum stream.
                runningChecksum.Write(messageBuffer, 0, offset);
                //take the current checksum and write it to the message.
                Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(runningChecksum.Crc32)), 0,
                    messageBuffer, offset, SizeOfInt32);
                //now take the current checksum and write it to the checksum stream.
                runningChecksum.Write(messageBuffer, offset, SizeOfInt32);
                offset += SizeOfInt32;

                //loop over the headers and write them out to the message.
                if (Headers != null)
                {
                    foreach (var header in Headers)
                    {
                        offset = header.Value.WriteToBuffer(messageBuffer, offset);
                    }
                    //make sure to add the header bytes to the checksum stream.
                    runningChecksum.Write(messageBuffer, PreludeLen, offset - PreludeLen);
                }

                //write the payload to the message.
                if (Payload != null)
                {
                    Buffer.BlockCopy(Payload, 0, messageBuffer, offset, Payload.Length);
                    //update the checksum
                    runningChecksum.Write(messageBuffer, offset, Payload.Length);
                    offset += Payload.Length;
                }

                //take the final checksum and add it to the end of the message.
                Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(runningChecksum.Crc32)), 0,
                    messageBuffer, offset, SizeOfInt32);
            }

            return messageBuffer;
        }
    }
    #endregion
}