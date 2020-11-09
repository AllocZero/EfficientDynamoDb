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

namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// Paginators for the DynamoDBv2 service
    ///</summary>
    public interface IDynamoDBv2PaginatorFactory
    {

        /// <summary>
        /// Paginator for BatchGetItem operation
        ///</summary>
        IBatchGetItemPaginator BatchGetItem(BatchGetItemRequest request);

        /// <summary>
        /// Paginator for ListTables operation
        ///</summary>
        IListTablesPaginator ListTables(ListTablesRequest request);

        /// <summary>
        /// Paginator for Query operation
        ///</summary>
        IQueryPaginator Query(QueryRequest request);

        /// <summary>
        /// Paginator for Scan operation
        ///</summary>
        IScanPaginator Scan(ScanRequest request);
    }
}
#endif