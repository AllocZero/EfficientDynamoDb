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
 */
using System;
using System.Collections.Generic;
using System.Globalization;


namespace Amazon.Util
{
    /// <summary>
    /// Object to track circular references in nested types.
    /// At each level of nesting, make a call to Track to retrieve Tracker,
    /// a tracking object implementing the IDisposable interface.
    /// Dispose of this tracker when leaving the context of the tracked object.
    /// </summary>
    public class CircularReferenceTracking
    {
        private object referenceTrackersLock = new object();
        private Stack<Tracker> referenceTrackers = new Stack<Tracker>();

        /// <summary>
        /// Tracker. Must be disposed.
        /// </summary>
        private class Tracker : IDisposable
        {
            public object Target { get; private set; }
            private CircularReferenceTracking State { get; set; }
            private bool disposed;

            public Tracker(CircularReferenceTracking state, object target)
            {
                State = state;
                Target = target;
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "Tracking {0}", Target);
            }

            #region Dispose Pattern Implementation

            /// <summary>
            /// Implements the Dispose pattern
            /// </summary>
            /// <param name="disposing">Whether this object is being disposed via a call to Dispose
            /// or garbage collected.</param>
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        State.PopTracker(this);
                    }
                    this.disposed = true;
                }
            }

            /// <summary>
            /// Disposes of all managed and unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~Tracker()
            {
                this.Dispose(false);
            }

            #endregion
        }

        /// <summary>
        /// Adds the current target to a reference list and returns a tracker.
        /// The tracker removes the target from the reference list when the
        /// tracker is disposed.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public IDisposable Track(object target)
        {
            if (target == null) throw new ArgumentNullException("target");

            lock (referenceTrackersLock)
            {
                if (TrackerExists(target))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "Circular reference detected with object [{0}] of type {1}", target, target.GetType().FullName));

                var tracker = new Tracker(this, target);
                referenceTrackers.Push(tracker);
                return tracker;
            }
        }

        private void PopTracker(Tracker tracker)
        {
            lock (referenceTrackersLock)
            {
                if (referenceTrackers.Peek() != tracker)
                    throw new InvalidOperationException("Tracker being released is not the latest one. Make sure to release child trackers before releasing parent.");

                referenceTrackers.Pop();
            }
        }
        private bool TrackerExists(object target)
        {
            foreach (var tracker in referenceTrackers)
                if (object.ReferenceEquals(tracker.Target, target))
                    return true;
            return false;
        }
    }
}
