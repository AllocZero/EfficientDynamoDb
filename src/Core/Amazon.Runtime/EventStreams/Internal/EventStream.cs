// /*******************************************************************************
//  *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
//  *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
//  *  this file except in compliance with the License. A copy of the License is located at
//  *
//  *  http://aws.amazon.com/apache2.0
//  *
//  *  or in the "license" file accompanying this file.
//  *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
//  *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
//  *  specific language governing permissions and limitations under the License.
//  * *****************************************************************************
//  *    __  _    _  ___
//  *   (  )( \/\/ )/ __)
//  *   /__\ \    / \__ \
//  *  (_)(_) \/\/  (___/
//  *
//  *  AWS SDK for .NET
//  *
//  */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#else
using System.Threading;
#endif

namespace Amazon.Runtime.EventStreams.Internal
{
    /// <summary>
    /// The contract for the <see cref="EventStream{T,TE}"/>.
    /// </summary>
    /// <typeparam name="T">An implementation of IEventStreamEvent (e.g. IS3Event).</typeparam>
    /// <typeparam name="TE">An implementation of EventStreamException (e.g. S3EventStreamException).</typeparam>
    public interface IEventStream<T, TE> : IDisposable where T : IEventStreamEvent where TE : EventStreamException, new()
    {
        /// <summary>
        /// The size of the buffer for reading from the network stream.
        /// </summary>
        int BufferSize { get; set; }
        /// <summary>
        /// Fires when an event is received.
        /// </summary>
        event EventHandler<EventStreamEventReceivedArgs<T>> EventReceived;
        /// <summary>
        /// Fired when an exception or error is raised.
        /// </summary>
        event EventHandler<EventStreamExceptionReceivedArgs<TE>> ExceptionReceived;
        /// <summary>
        /// Starts the background thread to start reading events from the network stream.
        /// </summary>
        void StartProcessing();
    }

    /// <summary>
    /// The superclass for all EventStreams. It contains the common processing logic needed to retreive events from a network Stream. It
    /// also contains the mechanisms needed to have a background loop raise events.
    /// </summary>
    /// <typeparam name="T">An implementation of IEventStreamEvent (e.g. IS3Event).</typeparam>
    /// <typeparam name="TE">An implementation of EventStreamException (e.g. S3EventStreamException).</typeparam>
    public abstract class EventStream<T, TE> : IEventStream<T, TE> where T : IEventStreamEvent where TE : EventStreamException, new()
    {
        /// <summary>
        /// "Unique" key for unknown event lookup.
        /// </summary>
        protected const string UnknownEventKey = "===UNKNOWN===";

        /// <summary>
        /// Header key for message type.
        /// </summary>
        private const string HeaderMessageType = ":message-type";
        /// <summary>
        /// Header key for event type.
        /// </summary>
        private const string HeaderEventType = ":event-type";
        /// <summary>
        /// Header key for exception type.
        /// </summary>
        private const string HeaderExceptionType = ":exception-type";
        /// <summary>
        /// Header key for error code.
        /// </summary>
        private const string HeaderErrorCode = ":error-code";
        /// <summary>
        /// Header key for error message.
        /// </summary>
        private const string HeaderErrorMessage = ":error-message";

        /// <summary>
        /// Value of <see cref="HeaderMessageType"/> when the message is an event.
        /// </summary>
        private const string EventHeaderMessageTypeValue = "event";
        /// <summary>
        /// Value of <see cref="HeaderMessageType"/> when the message is an exception.
        /// </summary>
        private const string ExceptionHeaderMessageTypeValue = "exception";
        /// <summary>
        /// Value of <see cref="HeaderMessageType"/> when the message is an error.
        /// </summary>
        private const string ErrorHeaderMessageTypeValue = "error";

        private const string WrappedErrorMessage = "Error.";

        /// <summary>
        /// The size of the buffer for reading from the network stream.
        /// Default is 8192.
        /// </summary>
        public int BufferSize { get; set; } = 8192;
        /// <summary>
        /// The underlying stream to read events from.
        /// </summary>
        protected Stream NetworkStream { get; }
        /// <summary>
        /// Responsible for decoding events from sequences of bytes.
        /// </summary>
        protected IEventStreamDecoder Decoder { get; }

        /// <summary>
        /// Fires when an event is recieved.
        /// </summary>
        public virtual event EventHandler<EventStreamEventReceivedArgs<T>> EventReceived;

        /// <summary>
        /// Fired when an exception or error is raised.
        /// </summary>
        public virtual event EventHandler<EventStreamExceptionReceivedArgs<TE>> ExceptionReceived;

        /// <summary>
        /// The mapping of event message to a generator function to construct the matching Event Stream event.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006", 
            Justification = "Mapping of string to generic generator function is clear to the reader. This property is not exposed to the end user.")]
        protected abstract IDictionary<string, Func<IEventStreamMessage, T>> EventMapping { get; }

        /// <summary>
        /// The mapping of event message to a generator function to construct the matching Event Stream exception.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006",
            Justification = "Mapping of string to generic generator function is clear to the reader. This property is not exposed to the end user.")]
        protected abstract IDictionary<string, Func<IEventStreamMessage, TE>> ExceptionMapping { get; }

        /// <summary>
        /// Whether the Stream is currently being processed.
        /// </summary>
        // This is true is StartProcessing is called, or if enumeration has started.
        protected abstract bool IsProcessing { get; set; }

        /// <summary>
        /// A Stream of Events. Events can be retrieved from this stream by attaching handlers to listen events, and then calling StartProcessing.
        /// </summary>
        protected EventStream(Stream stream) : this(stream, null)
        {
        }

        /// <summary>
        /// A Stream of Events. Events can be retrieved from this stream by attaching handlers to listen events, and then calling StartProcessing.
        /// </summary>
        protected EventStream(Stream stream, IEventStreamDecoder eventStreamDecoder)
        {
            NetworkStream = stream;
            Decoder = eventStreamDecoder ?? new EventStreamDecoder();
        }

        /// <summary>
        /// Converts an EventStreamMessage to an event.
        /// </summary>
        /// <param name="eventStreamMessage">The event stream message to be converted.</param>
        /// <returns>The event</returns>
        protected T ConvertMessageToEvent(EventStreamMessage eventStreamMessage)
        {
            var eventStreamMessageHeaders = eventStreamMessage.Headers;
            string eventStreamMessageType;
            try
            {
                // Message type can be an event, an exception, or an error. This information is stored in the :message-type header.
                eventStreamMessageType = eventStreamMessageHeaders[HeaderMessageType].AsString();
            }
            catch (KeyNotFoundException ex)
            {
                throw new EventStreamValidationException("Message type missing from event stream message.", ex);
            }

            switch (eventStreamMessageType)
            {
                case EventHeaderMessageTypeValue:
                    string eventTypeKey;
                    try
                    {
                        eventTypeKey = eventStreamMessageHeaders[HeaderEventType].AsString();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new EventStreamValidationException("Event Type not defined for event.", ex);
                    }

                    try
                    {
                        return EventMapping[eventTypeKey](eventStreamMessage);
                    }
                    catch (KeyNotFoundException)
                    {
                        return EventMapping[UnknownEventKey](eventStreamMessage);
                    }

                case ExceptionHeaderMessageTypeValue:
                    string exceptionTypeKey;
                    try
                    {
                        exceptionTypeKey = eventStreamMessageHeaders[HeaderExceptionType].AsString();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new EventStreamValidationException("Exception Type not defined for exception.", ex);
                    }

                    try
                    {
                        throw ExceptionMapping[exceptionTypeKey](eventStreamMessage);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new UnknownEventStreamException(exceptionTypeKey);
                    }

                case ErrorHeaderMessageTypeValue:
                    int errorCode;
                    try
                    {
                        errorCode = eventStreamMessageHeaders[HeaderErrorCode].AsInt32();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new EventStreamValidationException("Error Code not defined for error.", ex);
                    }
                    // Error message is not required for errors. Errors do not have payloads.
                    IEventStreamHeader errorMessage = null;
                    var hasErrorMessage = eventStreamMessageHeaders.TryGetValue(HeaderErrorMessage, out errorMessage);
                    throw new EventStreamErrorCodeException(errorCode, hasErrorMessage ? errorMessage.AsString() : string.Empty);
                default:
                    // Unknown message type. Swallow the message to enable future message types without breaking existing clients.
                    throw new UnknownEventStreamMessageTypeException();
            }
        }

        /// <summary>
        /// Abstraction for cross-framework initiation of the background thread.
        /// </summary>
        protected void Process()
        {
#if AWS_ASYNC_API
            // Task only exists in framework 4.5 and up, and Standard.
            Task.Run(() => ProcessLoop());
#else
            // ThreadPool only exists in 3.5 and below. These implementations do not have the Task library.
            ThreadPool.QueueUserWorkItem(ProcessLoop);
#endif
        }

        /// <summary>
        /// The background thread main loop. It will constantly read from the network stream until IsProcessing is false, or an error occurs.
        /// <para></para>
        /// This stub exists due to FXCop.
        /// </summary>
        private void ProcessLoop()
        {
            ProcessLoop(null);
        }

        /// <summary>
        /// The background thread main loop. It will constantly read from the network stream until IsProcessing is false, or an error occurs.
        /// </summary>
        /// <param name="state">Needed for 3.5 support. Not used.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Needed for .NET 3.5 (ThreadPool.QueueUserWorkItem)")]
        private void ProcessLoop(object state)
        {
            var buffer = new byte[BufferSize];

            try
            {
                while (IsProcessing)
                {
                    ReadFromStream(buffer);
                }
            }
            // These exceptions are raised on the background thread. They are fired as events for visibility.
            catch (Exception ex)
            {
                IsProcessing = false;

                // surfaceException means what is surfaced to the user. For example, in S3Select, that would be a S3EventStreamException.
                var surfaceException = WrapException(ex);
                
                // Raise the exception as an event.
                ExceptionReceived?.Invoke(this,
                    new EventStreamExceptionReceivedArgs<TE>(surfaceException));
            }
        }

        /// <summary>
        /// Reads from the stream into the buffer. It then passes the buffer to the decoder, which raises an event for
        /// each message it decodes.
        /// </summary>
        /// <param name="buffer">The buffer to store the read bytes from the stream.</param>
        protected void ReadFromStream(byte[] buffer)
        {
            var bytesRead = NetworkStream.Read(buffer, 0, buffer.Length);
            if (NetworkStream.CanRead)
            {
                if (bytesRead > 0)
                {
                    // Decoder raises MessageReceived for every message it encounters.
                    Decoder.ProcessData(buffer, 0, bytesRead);
                }
            }
            else
            {
                IsProcessing = false;
            }
        }

        /// <summary>
        /// Wraps exceptions in an outer exception so they can be passed to event handlers. If the Exception is already of a compatable type,
        /// the method returns what it was given.
        /// </summary>
        /// <param name="ex">The exception to wrap.</param>
        /// <returns>An exception of type TE</returns>
        protected TE WrapException(Exception ex)
        {
            var teEx = ex as TE;
            if (teEx != null)
            {
                return teEx;
            }

            // Types of exception that would not already be of type TE would be DecoderExceptions, EventStreamValidationExceptions,
            // and EventStreamErrorCodeExceptions.

            // We want to wrap the exception in the generic type so we can give it to the exception event handler.

            // Only one exception should fire, since the background thread dies on the exception. Therefore, the reflection
            // used here is not a preformance concern, and lets us abstract this method to the superclass. 
            var exArgs = new object[] {WrappedErrorMessage, ex};
            return (TE) Activator.CreateInstance(typeof(TE), exArgs);
        }

        /// <summary>
        /// Starts the background thread to start reading events from the network stream.
        /// </summary>
        public virtual void StartProcessing()
        {
            if (IsProcessing) return;

            IsProcessing = true;
            Process();
        }

        #region Dispose Pattern
        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources of this stream.
        /// </summary>
        /// <param name="disposing">Should dispose of unmanged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                IsProcessing = false;

                NetworkStream?.Dispose();
                Decoder?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}