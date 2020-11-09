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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Amazon.Runtime.Internal.Transform
{
#if !NETSTANDARD
    // This exception is marked Serializable, but its ResponseBody field is not
    // serialized/deserialized.
    [Serializable]
#endif
    public class HttpWebRequestResponseData : IWebResponseData
    {
        string[] _headerNames;
        Dictionary<string, string> _headers;
        HashSet<string> _headerNamesSet;
        [NonSerialized]
        HttpWebResponseBody _responseBody;

        public HttpWebRequestResponseData(HttpWebResponse response)
        {
            _responseBody = new HttpWebResponseBody(response);

            this.StatusCode = response.StatusCode;
            this.IsSuccessStatusCode = this.StatusCode >= HttpStatusCode.OK && this.StatusCode <= (HttpStatusCode)299;
            this.ContentType = response.ContentType;            
            this.ContentLength = response.ContentLength;
            CopyHeaderValues(response);
        }

        public HttpStatusCode StatusCode { get; private set; }

        public bool IsSuccessStatusCode { get; private set; }

        public string ContentType { get; private set; }

        public long ContentLength { get; private set; }
        
        public bool IsHeaderPresent(string headerName)
        {
            return _headerNamesSet.Contains(headerName);
        }

        public string[] GetHeaderNames()
        {
            return _headerNames;            
        }

        public string GetHeaderValue(string name)
        {
            string headerValue;
            if (_headers.TryGetValue(name, out headerValue))
                return headerValue;

            return string.Empty;
        }

        private void CopyHeaderValues(HttpWebResponse response)
        {
            var keys = response.Headers.Keys;
            _headerNames = new string[keys.Count];
            _headers = new Dictionary<string, string>(keys.Count, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var headerValue = response.GetResponseHeader(key);
                _headerNames[i] = key;
                _headers.Add(key, headerValue);
            }
            _headerNamesSet = new HashSet<string>(_headerNames, StringComparer.OrdinalIgnoreCase);
        }

        public IHttpResponseBody ResponseBody
        {
            get { return _responseBody; }
        }




        /// <summary>
        /// Constructs a new instance of the HttpWebRequestResponseData class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected HttpWebRequestResponseData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info != null)
            {
                this._headerNames = (string[])info.GetValue("_headerNames", typeof(string[]));
                this._headerNamesSet = (HashSet<string>)info.GetValue("_headerNamesSet", typeof(HashSet<string>));
                this._headers = (Dictionary<string, string>)info.GetValue("_headers", typeof(Dictionary<string, string>));
                this.StatusCode = (HttpStatusCode)info.GetValue("StatusCode", typeof(HttpStatusCode));
                this.IsSuccessStatusCode = info.GetBoolean("IsSuccessStatusCode");
                this.ContentType = info.GetString("ContentType");
                this.ContentLength = info.GetInt64("ContentLength");
            }
        }

        /// <summary>
        /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic). </exception>
        [System.Security.SecurityCritical]
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info != null)
            {
                info.AddValue("_headerNames", this._headerNames);
                info.AddValue("_headerNamesSet", this._headerNamesSet);
                info.AddValue("_headers", this._headers);
                info.AddValue("StatusCode", this.StatusCode);
                info.AddValue("IsSuccessStatusCode", this.IsSuccessStatusCode);
                info.AddValue("ContentType", this.ContentType);
                info.AddValue("ContentLength", this.ContentLength);
            }
        }

    }

    public class HttpWebResponseBody : IHttpResponseBody
    {
        HttpWebResponse _response;
        bool _disposed = false;

        public HttpWebResponseBody(HttpWebResponse response)
        {
            _response = response;
        }

        public Stream OpenResponse()
        {
            if (_disposed)
                throw new ObjectDisposedException("HttpWebResponseBody");
            
            return _response.GetResponseStream();
        }
#if AWS_ASYNC_API 
        public System.Threading.Tasks.Task<Stream> OpenResponseAsync()
        {            
            throw new NotSupportedException();
        }
#endif
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_response != null)
                    _response.Close();

                _disposed = true;
            }
        }
    }
}
