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
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// Container for the parameters to the ListTables operation.
    /// Returns an array of table names associated with the current account and endpoint.
    /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
    /// of 100 table names.
    /// </summary>
    public partial class ListTablesRequest : AmazonDynamoDBRequest
    {
        private string _exclusiveStartTableName;
        private int? _limit;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public ListTablesRequest() { }

        /// <summary>
        /// Instantiates ListTablesRequest with the parameterized properties
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        public ListTablesRequest(string exclusiveStartTableName)
        {
            _exclusiveStartTableName = exclusiveStartTableName;
        }

        /// <summary>
        /// Instantiates ListTablesRequest with the parameterized properties
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        /// <param name="limit">A maximum number of table names to return. If this parameter is not specified, the limit is 100.</param>
        public ListTablesRequest(string exclusiveStartTableName, int limit)
        {
            _exclusiveStartTableName = exclusiveStartTableName;
            _limit = limit;
        }

        /// <summary>
        /// Gets and sets the property ExclusiveStartTableName. 
        /// <para>
        /// The first table name that this operation will evaluate. Use the value that was returned
        /// for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain
        /// the next page of results.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string ExclusiveStartTableName
        {
            get { return this._exclusiveStartTableName; }
            set { this._exclusiveStartTableName = value; }
        }

        // Check to see if ExclusiveStartTableName property is set
        internal bool IsSetExclusiveStartTableName()
        {
            return this._exclusiveStartTableName != null;
        }

        /// <summary>
        /// Gets and sets the property Limit. 
        /// <para>
        /// A maximum number of table names to return. If this parameter is not specified, the
        /// limit is 100.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=100)]
        public int Limit
        {
            get { return this._limit.GetValueOrDefault(); }
            set { this._limit = value; }
        }

        // Check to see if Limit property is set
        internal bool IsSetLimit()
        {
            return this._limit.HasValue; 
        }

    }
}