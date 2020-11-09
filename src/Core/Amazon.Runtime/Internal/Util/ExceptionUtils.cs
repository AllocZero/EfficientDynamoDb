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
using System.Net;

namespace AWSSDK.Runtime.Internal.Util
{
    internal static class ExceptionUtils
    {
        internal static HttpStatusCode? DetermineHttpStatusCode(Exception e)
        {
            var response = (e as WebException)?.Response as HttpWebResponse;
            if (response != null)
            {
                return response.StatusCode;
            }

#if !BCL35
            var requestException = e as System.Net.Http.HttpRequestException;            
            if (requestException?.Data?.Contains("StatusCode") == true)
            {
                return (HttpStatusCode)requestException.Data["StatusCode"];
            }
#endif

            if (e?.InnerException != null)
            {
                return DetermineHttpStatusCode(e.InnerException);
            }

            return null;
        }

        internal static bool IsInnerException<T>(Exception exception)
            where T : Exception
        {
            T innerException;
            return IsInnerException<T>(exception, out innerException);
        }

        internal static bool IsInnerException<T>(Exception exception, out T inner)
            where T : Exception
        {
            inner = null;
            Queue<Exception> queue = new Queue<Exception>();

            //Modified BFS for the specific exception type
            var currentException = exception;
            do
            {
                //When queue.Count == 0  this is the initial parent queue item
                //which will not be checked for the exception of type T.
                if(queue.Count > 0)
                {
                    currentException = queue.Dequeue();
                    inner = currentException as T;
                    if (inner != null)
                    {
                        return true;
                    }
                }                

#if !BCL35
                var aggregateException = currentException as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var e in aggregateException.InnerExceptions)
                    {
                        queue.Enqueue(e);
                    }

                    continue;
                }
#endif
                if (currentException.InnerException != null)
                {
                    queue.Enqueue(currentException.InnerException);
                }                                
            }
            while (queue.Count > 0);                        

            return false;
        }
    }
}
