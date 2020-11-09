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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Util
{
    /// <summary>
    /// AWSStreamContent is a wrapper for StreamContent class in
    /// the System.Net.Http library.The wrapper has been added so as to 
    /// remove System.Net.Http dependencies from the Services
    /// and migrate it to a Core level dependecy only. This avoids version clashes when a service 
    /// and the Core depends on different versions of the System.Net.Http library. This is a Service 
    /// level utility method
    /// </summary>
    public class AWSStreamContent : IDisposable
    {
        private bool disposed = false;

        internal StreamContent StreamContent { get; set; }

        /// <summary>
        /// Wrapper for StreamContent constructor
        /// for stream content
        /// </summary>
        /// <param name="content"></param>
        public AWSStreamContent(Stream content)
        {
            StreamContent = new StreamContent(content);
        }

        /// <summary>
        /// Wrapper for StreamContent constructor
        /// for stream content and bufferSize
        /// </summary>
        /// <param name="content"></param>
        /// <param name="bufferSize"></param>
        public AWSStreamContent(Stream content, int bufferSize)
        {
            StreamContent = new StreamContent(content, bufferSize);
        }

        /// <summary>
        /// Wrapper to allow Services to remove StreamContent Headers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveHttpContentHeader(string name)
        {
            return StreamContent.Headers.Remove(name);
        }

        /// <summary>
        /// Wrapper to allow Services to add StreamContent Headers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void AddHttpContentHeader(string name, string value)
        {
            StreamContent.Headers.Add(name, value);
        }

        /// <summary>
        /// Wrapper for StreamContent Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                StreamContent.Dispose();
            }
            disposed = true;

        }
    }
}
