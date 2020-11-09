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
using System.Diagnostics.CodeAnalysis;
using Amazon.Runtime.EventStreams.Internal;

namespace Amazon.Runtime.EventStreams
{
    /// <summary>
    /// Event args that contain an <see cref="IEventStreamEvent"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IEventStreamEvent"/></typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification =
        "The suffix of 'EventArgs' makes the name have 3 'Event' declarations.' Since the second 'Event' has the same syntactic meaning, adding a another is unnecessarily verbose.")]
    public class EventStreamEventReceivedArgs<T> : EventArgs where T : IEventStreamEvent
    {
        /// <summary>
        /// The EventStream Event.
        /// </summary>
        public T EventStreamEvent { get; }

        /// <summary>
        /// Constructs an EventStreamEventReceivedArgs.
        /// </summary>
        /// <param name="eventStreamEvent">The EventStream Event.</param>
        public EventStreamEventReceivedArgs(T eventStreamEvent)
        {
            EventStreamEvent = eventStreamEvent;
        }
    }
}