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
using System.Globalization;
using System.Net;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Ionic.Zlib;

namespace Amazon.Runtime.EventStreams.Internal
{
    #region streamingDecoder
    /// <summary>
    /// Event Arguments for EventStreamDecoder.MessageReceived.
    /// </summary>
    public class EventStreamMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received message.
        /// </summary>
        public EventStreamMessage Message { get; private set; }

        /// <summary>
        /// Additional object context
        /// </summary>
        public Object Context { get; private set; }

        /// <summary>
        /// Initialize this with message
        /// </summary>
        public EventStreamMessageReceivedEventArgs(EventStreamMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// Initialize this with message and object conetext
        /// </summary>
        public EventStreamMessageReceivedEventArgs(EventStreamMessage message, Object context)
        {
            Message = message;
            Context = context;
        }
    }

    /// <summary>
    /// Exception thrown when a request is made of the Decoder, but the internal state
    /// machine is an invalid state. This is usually the result of an interanl exception
    /// being thrown during parsing of the network stream.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class EventStreamDecoderIllegalStateException : Exception
    {
        public EventStreamDecoderIllegalStateException(string message) : base(message)
        {
        }

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the EventStreamDecoderIllegalStateException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected EventStreamDecoderIllegalStateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// Contract for EventStreamDecoder
    /// </summary>
    public interface IEventStreamDecoder : IDisposable
    {
        /// <summary>
        /// Invoked anytime an EventStreamMessage is found.
        /// </summary>
        event EventHandler<EventStreamMessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Processes data starting at offset up to length.
        /// Invokes MessageRecieved for each message encountered.
        /// If an exception is thrown, this object is not safe for reuse.
        /// </summary>
        /// <param name="data">buffer to read</param>
        /// <param name="offset">offset in buffer to start reading.</param>
        /// <param name="length">length of data.</param>
        void ProcessData(byte[] data, int offset, int length);
    }

    /// <summary>
    /// Streaming decoder for listening for incoming EventStreamMessage datagrams.
    /// </summary>
    public class EventStreamDecoder : IEventStreamDecoder
    {
        /// <summary>
        /// Invoked anytime an EventStreamMessage is found.
        /// </summary>
        public event EventHandler<EventStreamMessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Object data (optional) that can be passed to the event handler for MessageReceived.
        /// </summary>
        public object MessageReceivedContext { get; set; }

        private delegate int ProcessRead(byte[] data, int offset, int length);

        private enum DecoderState
        {
            Start = 0,
            ReadPrelude,
            ProcessPrelude,
            ReadMessage,
            Error
        }

        private ProcessRead[] _stateFns = null;
        private DecoderState _state;
        private int _currentMessageLength;
        private int _amountBytesRead;
        private byte[] _workingMessage;
        private byte[] _workingBuffer;
        private CrcCalculatorStream _runningChecksumStream;

        /// <summary>
        /// Default constructor. Initializes internal _state machine.
        /// </summary>
        public EventStreamDecoder()
        {
            _workingBuffer = new byte[EventStreamMessage.PreludeLen];
            _stateFns = new ProcessRead[] { Start, ReadPrelude, ProcessPrelude, ReadMessage, Error };
            _state = DecoderState.Start;
        }

        #region StreamingDecoderStateMachine
        private int Start(byte[] data, int offset, int length)
        {
            _workingMessage = null;
            _amountBytesRead = 0;

            if (_runningChecksumStream != null)
            {
                _runningChecksumStream.Dispose();
            }

            _runningChecksumStream = new CrcCalculatorStream(new NullStream());
            _currentMessageLength = 0;
            _state = DecoderState.ReadPrelude;
            return 0;
        }

        private int ReadPrelude(byte[] data, int offset, int length)
        {
            var read = 0;

            if (_amountBytesRead < EventStreamMessage.PreludeLen)
            {
                read = Math.Min(length - offset, EventStreamMessage.PreludeLen - _amountBytesRead);
                Buffer.BlockCopy(data, offset, _workingBuffer, _amountBytesRead, read);
                _amountBytesRead += read;
            }

            if (_amountBytesRead == EventStreamMessage.PreludeLen)
            {
                _state = DecoderState.ProcessPrelude;
            }

            return read;
        }

        private int ProcessPrelude(byte[] data, int offset, int length)
        {
            /* this is absolutely redundant, but since the totalLength field will result
                   in a potentially huge allocation, we want to fail fast before even attempting to continue
                   if the totalLength field has been corrupted. */
            _runningChecksumStream.Write(_workingBuffer, 0, EventStreamMessage.PreludeLen - EventStreamMessage.SizeOfInt32);
            var preludeChecksum = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_workingBuffer,
                EventStreamMessage.PreludeLen - EventStreamMessage.SizeOfInt32));

            if (preludeChecksum != _runningChecksumStream.Crc32)
            {
                _state = DecoderState.Error;
                throw new EventStreamChecksumFailureException(
                    string.Format(CultureInfo.InvariantCulture, "Message Prelude Checksum failure. Expected {0} but was {1}", preludeChecksum, _runningChecksumStream.Crc32));
            }

            _runningChecksumStream.Write(_workingBuffer, EventStreamMessage.PreludeLen - 4, EventStreamMessage.SizeOfInt32);
            _currentMessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_workingBuffer, 0));

            /* It's entirely possible to change this to not do this potentially large allocation
               but it complicates the API a bit and is most likely unnecessary. For now, just allocate
               the entire message buffer. It will be released after each message is processed. */
            _workingMessage = new byte[_currentMessageLength];
            Buffer.BlockCopy(_workingBuffer, 0, _workingMessage, 0, EventStreamMessage.PreludeLen);
            _state = DecoderState.ReadMessage;
            return 0;
        }

        private int ReadMessage(byte[] data, int offset, int length)
        {
            var read = 0;

            if (_amountBytesRead < _currentMessageLength)
            {
                read = Math.Min(length - offset, _currentMessageLength - _amountBytesRead);
                Buffer.BlockCopy(data, offset, _workingMessage, _amountBytesRead, read);
                _amountBytesRead += read;
            }

            if (_amountBytesRead == _currentMessageLength)
            {
                ProcessMessage();
            }

            return read;
        }

        private void ProcessMessage()
        {
            try
            {
                var message = EventStreamMessage.FromBuffer(_workingMessage, 0, _currentMessageLength);
                MessageReceived?.Invoke(this, new EventStreamMessageReceivedEventArgs(message, MessageReceivedContext));
                _state = DecoderState.Start;
            }
            catch(Exception)
            {
                _state = DecoderState.Error;
                throw;
            }
        }

        private int Error(byte[] data, int offset, int length)
        {
            throw new EventStreamDecoderIllegalStateException("Event stream decoder is in an error state. Create a new instance, and use a new stream to continue");
        }

        #endregion

        /// <summary>
        /// Processes data starting at offset up to length.
        /// Invokes MessageRecieved for each message encountered.
        /// If an exception is thrown, this object is not safe for reuse.
        /// </summary>
        /// <param name="data">buffer to read</param>
        /// <param name="offset">offset in buffer to start reading.</param>
        /// <param name="length">length of data.</param>
        public void ProcessData(byte[] data, int offset, int length)
        {
            var available = length - offset;

            while (offset < available)
            {
                offset += _stateFns[(int)_state](data, offset, length);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _runningChecksumStream.Dispose();
                    _workingMessage = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    #endregion
}

