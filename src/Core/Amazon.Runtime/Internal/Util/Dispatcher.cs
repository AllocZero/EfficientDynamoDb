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
using System.Threading;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Class to perform actions on a background thread.
    /// Uses a single background thread and performs actions
    /// on it in the order the data was sent through the instance.
    /// </summary>
    internal class BackgroundDispatcher<T> : IDisposable
    {
        #region Properties

        private bool isDisposed = false;
        private Action<T> action;
        private Queue<T> queue;
        private Thread backgroundThread;
        private AutoResetEvent resetEvent;
        private bool shouldStop;
        public bool IsRunning { get; private set; }

        #endregion


        #region Constructor/destructor

        public BackgroundDispatcher(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            queue = new Queue<T>();
            resetEvent = new AutoResetEvent(false);
            shouldStop = false;
            this.action = action;

            backgroundThread = new Thread(Run);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        ~BackgroundDispatcher()
        {
            Stop();
            this.Dispose(false);
        }

        #endregion


        #region Dispose Pattern Implementation

        /// <summary>
        /// Implements the Dispose pattern
        /// </summary>
        /// <param name="disposing">Whether this object is being disposed via a call to Dispose
        /// or garbage collected.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing && resetEvent != null)
                {
#if NETSTANDARD
                    resetEvent.Dispose();
#else
                    resetEvent.Close();
#endif
                    resetEvent = null;
                }
                this.isDisposed = true;
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

        #endregion


        #region Public Methods

        const int MAX_QUEUE_SIZE = 100;
        public void Dispatch(T data)
        {
            if (queue.Count < MAX_QUEUE_SIZE)
            {
                lock (queue)
                {
                    if (queue.Count < MAX_QUEUE_SIZE)
                        queue.Enqueue(data);
                }
            }

            resetEvent.Set();
        }

        public void Stop()
        {
            shouldStop = true;
            resetEvent.Set();
        }

        #endregion


        #region Private methods

        private void Run()
        {
            IsRunning = true;
            while (!shouldStop)
            {
                HandleInvoked();
                resetEvent.WaitOne();
            }
            HandleInvoked();
            IsRunning = false;
        }

        private void HandleInvoked()
        {
            while (true)
            {
                bool dataPresent = false;
                T data = default(T);
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        data = queue.Dequeue();
                        dataPresent = true;
                    }
                }

                if (!dataPresent)
                    break;

                try
                {
                    action(data);
                }
                catch { }
            }
        }

        #endregion
    }

    /// <summary>
    /// Class to invoke actions on a background thread.
    /// Uses a single background thread and invokes actions
    /// on it in the order they were invoked through the instance.
    /// </summary>
    internal class BackgroundInvoker : BackgroundDispatcher<Action>
    {
        public BackgroundInvoker()
            : base(action => action())
        {
        }
    }
}
