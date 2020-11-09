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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Represents a non-generic object for retrieving a batch of items
    /// from a single DynamoDB table
    /// </summary>
    public abstract partial class BatchGet
    {
        #region Public methods

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.BatchGet.Execute"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// 
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteHelperAsync(cancellationToken);
        }

        #endregion
    }

    /// <summary>
    /// Class for retrieving a batch of items from multiple DynamoDB tables,
    /// using multiple strongly-typed BatchGet objects
    /// </summary>
    public partial class MultiTableBatchGet
    {
        #region Public methods

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation.
        /// <seealso cref="Amazon.DynamoDBv2.DataModel.MultiTableBatchGet.Execute"/>
        /// </summary>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
        /// 
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteHelperAsync(cancellationToken);
        }

        #endregion
    }
}
