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

namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// Paginators for the DynamoDBv2 service
    ///</summary>
    public class DynamoDBv2PaginatorFactory : IDynamoDBv2PaginatorFactory
    {
        private readonly IAmazonDynamoDB client;

        internal DynamoDBv2PaginatorFactory(IAmazonDynamoDB client) 
        {
            this.client = client;
        }

        /// <summary>
        /// Paginator for BatchGetItem operation
        ///</summary>
        public IBatchGetItemPaginator BatchGetItem(BatchGetItemRequest request) 
        {
            return new BatchGetItemPaginator(this.client, request);
        }

        /// <summary>
        /// Paginator for ListTables operation
        ///</summary>
        public IListTablesPaginator ListTables(ListTablesRequest request) 
        {
            return new ListTablesPaginator(this.client, request);
        }

        /// <summary>
        /// Paginator for Query operation
        ///</summary>
        public IQueryPaginator Query(QueryRequest request) 
        {
            return new QueryPaginator(this.client, request);
        }

        /// <summary>
        /// Paginator for Scan operation
        ///</summary>
        public IScanPaginator Scan(ScanRequest request) 
        {
            return new ScanPaginator(this.client, request);
        }
    }
}
#endif