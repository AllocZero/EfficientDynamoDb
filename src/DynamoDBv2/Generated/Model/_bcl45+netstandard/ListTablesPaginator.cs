#if !NETSTANDARD13
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

/*
 * Do not modify this file. This file is generated from the dynamodb-2012-08-10.normal.json service model.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
 
namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// Base class for ListTables paginators.
    /// </summary>
    internal sealed partial class ListTablesPaginator : IPaginator<ListTablesResponse>, IListTablesPaginator
    {
        private readonly IAmazonDynamoDB _client;
        private readonly ListTablesRequest _request;
        private int _isPaginatorInUse = 0;
        
        /// <summary>
        /// Enumerable containing all full responses for the operation
        /// </summary>
        public IPaginatedEnumerable<ListTablesResponse> Responses => new PaginatedResponse<ListTablesResponse>(this);

        /// <summary>
        /// Enumerable containing all of the TableNames
        /// </summary>
        public IPaginatedEnumerable<string> TableNames => 
            new PaginatedResultKeyResponse<ListTablesResponse, string>(this, (i) => i.TableNames);

        internal ListTablesPaginator(IAmazonDynamoDB client, ListTablesRequest request)
        {
            this._client = client;
            this._request = request;
        }
#if BCL
        IEnumerable<ListTablesResponse> IPaginator<ListTablesResponse>.Paginate()
        {
            if (Interlocked.Exchange(ref _isPaginatorInUse, 1) != 0)
            {
                throw new System.InvalidOperationException("Paginator has already been consumed and cannot be reused. Please create a new instance.");
            }
            PaginatorUtils.SetUserAgentAdditionOnRequest(_request);
            var exclusiveStartTableName = _request.ExclusiveStartTableName;
            ListTablesResponse response;
            do
            {
                _request.ExclusiveStartTableName = exclusiveStartTableName;
                response = _client.ListTables(_request);
                exclusiveStartTableName = response.LastEvaluatedTableName;
                yield return response;
            }
            while (exclusiveStartTableName != null);
        }
#endif
#if AWS_ASYNC_ENUMERABLES_API
        async IAsyncEnumerable<ListTablesResponse> IPaginator<ListTablesResponse>.PaginateAsync(CancellationToken cancellationToken = default)
        {
            if (Interlocked.Exchange(ref _isPaginatorInUse, 1) != 0)
            {
                throw new System.InvalidOperationException("Paginator has already been consumed and cannot be reused. Please create a new instance.");
            }
            PaginatorUtils.SetUserAgentAdditionOnRequest(_request);
            var exclusiveStartTableName = _request.ExclusiveStartTableName;
            ListTablesResponse response;
            do
            {
                _request.ExclusiveStartTableName = exclusiveStartTableName;
                response = await _client.ListTablesAsync(_request, cancellationToken).ConfigureAwait(false);
                exclusiveStartTableName = response.LastEvaluatedTableName;
                cancellationToken.ThrowIfCancellationRequested();
                yield return response;
            }
            while (exclusiveStartTableName != null);
        }
#endif
    }
}
#endif