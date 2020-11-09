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
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Class for putting and/or deleting a batch of items in a single DynamoDB table.
    /// </summary>
    public partial class DocumentBatchWrite
    {
        #region Public methods

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteHelperAsync(cancellationToken);
        }

        #endregion
    }

    /// <summary>
    /// Class for putting and/or deleting a batch of items in multiple DynamoDB tables.
    /// </summary>
    public partial class MultiTableDocumentBatchWrite
    {
        #region Public methods

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.MultiTableDocumentBatchWrite.Execute"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteHelperAsync(cancellationToken);
        }

        #endregion
    }
}
