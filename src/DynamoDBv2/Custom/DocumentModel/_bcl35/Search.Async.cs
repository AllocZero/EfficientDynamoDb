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

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Search response object
    /// </summary>
    public partial class Search
    {

        #region Async public 

        /// <summary>
        /// Initiates the asynchronous execution of the GetNextSet operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet"/>
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetNextSet
        ///         operation.</returns>
        public IAsyncResult BeginGetNextSet(AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetNextSetHelper(), callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the Execute operation.
        /// 
        /// If there are more items in the Scan/Query, PaginationToken will be
        /// set and can be consumed in a new Scan/Query operation to resume
        /// retrieving items from this point.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetNextSet.</param>
        /// <returns>Next set of Documents matching the search parameters</returns>
        public List<Document> EndGetNextSet(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<List<Document>>(asyncResult);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetRemaining operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetRemaining"/>
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetRemaining
        ///         operation.</returns>
        public IAsyncResult BeginGetRemaining(AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetRemainingHelper(), callback, state);
        }
        /// <summary>
        /// Finishes the asynchronous execution of the GetRemaining operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetRemaining"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetRemaining.</param>
        /// <returns>List of Documents matching the search parameters</returns>
        public List<Document> EndGetRemaining(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<List<Document>>(asyncResult);
        }

        #endregion
    }
}
