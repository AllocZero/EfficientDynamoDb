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

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Util;
using System.Threading;

namespace Amazon.DynamoDBv2
{
    internal delegate object AsyncCall();

    internal class DynamoDBAsyncResult : IAsyncResult, IDisposable
    {
        private ManualResetEvent _waitHandle;
        private bool _disposed = false;

        public DynamoDBAsyncResult(AsyncCallback callback, object state)
        {
            this.Callback = callback;
            this.AsyncState = state;
            this._waitHandle = new ManualResetEvent(false);
        }

        #region IAsyncResult methods

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted { get; private set; }

        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return this._waitHandle; }
        }

        #endregion

        public void SignalWaitHandle()
        {
            this.IsCompleted = true;
            this._waitHandle.Set();
        }

        public AsyncCallback Callback { get; private set; }

        public Exception LastException { get; set; }

        public object Return { get; set; }

        #region Dispose Pattern Implementation

        /// <summary>
        /// Implements the Dispose pattern
        /// </summary>
        /// <param name="disposing">Whether this object is being disposed via a call to Dispose
        /// or garbage collected.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing && _waitHandle != null)
                {
                    _waitHandle.Close();
                    _waitHandle = null;
                }
                this._disposed = true;
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
    }

    internal static class DynamoDBAsyncExecutor
    {
        public static void Execute(AsyncCall call, DynamoDBAsyncResult result)
        {
            try
            {
                result.Return = call();
            }
            catch (Exception e)
            {
                result.LastException = e;
                result.Return = null;
            }
            finally
            {
                result.SignalWaitHandle();
                if (result.Callback != null)
                {
                    result.Callback(result);
                }
            }
        }

        public static IAsyncResult BeginOperation(AsyncCall call, AsyncCallback callback, object state)
        {
            DynamoDBAsyncResult result = new DynamoDBAsyncResult(callback, state);
            ThreadPool.QueueUserWorkItem(s => Execute(call, result));
            return result;
        }

        public static void EndOperation(IAsyncResult result)
        {
            EndOperation<object>(result);
        }

        public static T EndOperation<T>(IAsyncResult result)
        {
            DynamoDBAsyncResult asyncResult = result as DynamoDBAsyncResult;
            if (asyncResult == null)
                return default(T);

            using (asyncResult)
            {
                if (!asyncResult.CompletedSynchronously)
                {
                    WaitHandle.WaitAll(new WaitHandle[] { asyncResult.AsyncWaitHandle });
                }

                if (asyncResult.LastException != null)
                {
                    AWSSDKUtils.PreserveStackTrace(asyncResult.LastException);
                    throw asyncResult.LastException;
                }

                return (T)asyncResult.Return;
            }
        }
    }
}
