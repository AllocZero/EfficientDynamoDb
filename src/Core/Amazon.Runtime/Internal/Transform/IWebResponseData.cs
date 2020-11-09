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
    public interface IWebResponseData
    {        
        long ContentLength { get; }
        string ContentType { get; }
        HttpStatusCode StatusCode { get; }
        bool IsSuccessStatusCode { get; }
        string[] GetHeaderNames();
        bool IsHeaderPresent(string headerName);
        string GetHeaderValue(string headerName);

        IHttpResponseBody ResponseBody { get; }
    }

    public interface IHttpResponseBody : IDisposable
    {
        Stream OpenResponse();

#if AWS_ASYNC_API
        System.Threading.Tasks.Task<Stream> OpenResponseAsync();
#endif
    }
}
