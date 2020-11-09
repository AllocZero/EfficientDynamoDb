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
#pragma warning disable 1574

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

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
        /// 
        /// If there are more items in the Scan/Query, PaginationToken will be
        /// set and can be consumed in a new Scan/Query operation to resume
        /// retrieving items from this point.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<List<Document>> GetNextSetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetNextSetHelperAsync(cancellationToken);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetRemaining operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Search.GetRemaining"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task<List<Document>> GetRemainingAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetRemainingHelperAsync(cancellationToken);
        }

        #endregion
    }
}
