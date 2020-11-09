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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Amazon.Runtime.EventStreams.Internal
{
    /// <summary>
    /// The contract for the <see cref="EnumerableEventStream{T,TE}"/>.
    /// </summary>
    /// <typeparam name="T">An implementation of IEventStreamEvent (e.g. IS3Event).</typeparam>
    /// <typeparam name="TE">An implementation of EventStreamException (e.g. S3EventStreamException).</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "IEventStreamCollection is not descriptive.")]
    public interface IEnumerableEventStream<T, TE> : IEventStream<T, TE>, IEnumerable<T> where T : IEventStreamEvent where TE : EventStreamException, new()
    {
    }

    /// <summary>
    /// A subclass of <see cref="EventStream{T,TE}" /> that enables an enumerable interface for interacting with Events.
    /// </summary>
    /// <typeparam name="T">An implementation of IEventStreamEvent (e.g. IS3Event).</typeparam>
    /// <typeparam name="TE">An implementation of EventStreamException (e.g. S3EventStreamException).</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "EventStreamCollection is not descriptive.")]
    [SuppressMessage("Microsoft.Design", "CA1063", Justification = "IDisposable is a transient interface from IEventStream. Users need to be able to call Dispose.")]
    public abstract class EnumerableEventStream<T, TE> : EventStream<T, TE>, IEnumerableEventStream<T, TE> where T : IEventStreamEvent where TE : EventStreamException, new()
    {
        private const string MutuallyExclusiveExceptionMessage = "Stream has already begun processing. Event-driven and Enumerable traversals of the stream are mutually exclusive. " +
                                                                 "You can either use the event driven or enumerable interface, but not both.";

        /// <summary>
        /// Flag if the stream was chosen to be enumerated.
        /// </summary>
        protected bool IsEnumerated { get; set; }

        /// <summary>
        /// A Stream of Events. Events can be retrieved from this stream by either
        /// <list type="bullet">
        ///   <item><description>attaching handlers to listen events, and then call StartProcessing <i>or</i></description></item>
        ///   <item><description>enumerating over the events.</description></item>
        /// </list>
        /// <para></para>
        /// These options should be treated as mutually exclusive.
        /// </summary>
        protected EnumerableEventStream(Stream stream) : this(stream, null)
        {
        }

        /// <summary>
        /// A Stream of Events. Events can be retrieved from this stream by either
        /// <list type="bullet">
        ///   <item><description>attaching handlers to listen events, and then call StartProcessing <i>or</i></description></item>
        ///   <item><description>enumerating over the events.</description></item>
        /// </list>
        /// <para></para>
        /// These options should be treated as mutually exclusive.
        /// </summary>
        protected EnumerableEventStream(Stream stream, IEventStreamDecoder eventStreamDecoder) : base(stream, eventStreamDecoder)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (IsProcessing)
            {
                // If the queue has already begun processing, refuse to enumerate.
                throw new InvalidOperationException(MutuallyExclusiveExceptionMessage);
            }

            // There could be more than 1 message created per decoder cycle.
            var events = new Queue<T>();

            // Opting out of events - letting the enumeration handle everything.
            IsEnumerated = true;
            IsProcessing = true;

            // Enumeration is just magic over the event driven mechanism.
            EventReceived += (sender, args) => events.Enqueue(args.EventStreamEvent);
            
            var buffer = new byte[BufferSize];


            while (IsProcessing)
            {
                // If there are already events ready to be served, do not ask for more.
                if (events.Count > 0)
                {
                    var ev = events.Dequeue();
                    // Enumeration handles terminal events on behalf of the user.
                    if (ev is IEventStreamTerminalEvent)
                    {
                        IsProcessing = false;
                        Dispose();
                    }

                    yield return ev;
                }
                else
                {
                    try
                    {
                        ReadFromStream(buffer);
                    }
                    catch (Exception ex)
                    {
                        IsProcessing = false;
                        Dispose();

                        // Wrap exceptions as needed to match event-driven behavior.
                        throw WrapException(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Starts the background thread to start reading events from the network stream.
        /// </summary>
        public override void StartProcessing()
        {
            // If they are/have enumerated, the event-driven mode should be disabled
            if (IsEnumerated) throw new InvalidOperationException(MutuallyExclusiveExceptionMessage);

            base.StartProcessing();
        }
    }
}