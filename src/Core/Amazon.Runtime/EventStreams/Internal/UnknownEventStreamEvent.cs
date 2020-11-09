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

using Amazon.Runtime.EventStreams;

namespace Amazon.Runtime.EventStreams.Internal
{
    /// <summary>
    /// This Event is returned if an event is retrieved from the event stream, but a generator function
    /// for the event is not defined. This can occur if the SDK is not updated after a new event type is introduced.
    /// </summary>
    public class UnknownEventStreamEvent : IEventStreamEvent
    {
        /// <summary>
        /// The Message recieved from the event stream before conversion.
        /// </summary>
        public IEventStreamMessage ReceivedMessage { get; set; }
        /// <summary>
        /// The event type.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Constructs an UnknownEventStreamEvent.
        /// </summary>
        public UnknownEventStreamEvent()
        {
        }

        /// <summary>
        /// Constructs an UnknownEventStreamEvent.
        /// </summary>
        /// <param name="receivedMessage">The Message recieved from the event stream before conversion.</param>
        /// <param name="eventType">The event type.</param>
        public UnknownEventStreamEvent(IEventStreamMessage receivedMessage, string eventType)
        {
            ReceivedMessage = receivedMessage;
            EventType = eventType;
        }
    }
}