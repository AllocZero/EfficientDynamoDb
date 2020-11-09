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
    /// Container for the parameters to the DescribeTable operation.
    /// Returns information about the table, including the current status of the table, when
    /// it was created, the primary key schema, and any indexes on the table.
    /// 
    ///  <note> 
    /// <para>
    /// If you issue a <code>DescribeTable</code> request immediately after a <code>CreateTable</code>
    /// request, DynamoDB might return a <code>ResourceNotFoundException</code>. This is because
    /// <code>DescribeTable</code> uses an eventually consistent query, and the metadata for
    /// your table might not be available at that moment. Wait for a few seconds, and then
    /// try the <code>DescribeTable</code> request again.
    /// </para>
    ///  </note>
    /// </summary>
    public partial class DescribeTableRequest : AmazonDynamoDBRequest
    {
        private string _tableName;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public DescribeTableRequest() { }

        /// <summary>
        /// Instantiates DescribeTableRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table to describe.</param>
        public DescribeTableRequest(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table to describe.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string TableName
        {
            get { return this._tableName; }
            set { this._tableName = value; }
        }

        // Check to see if TableName property is set
        internal bool IsSetTableName()
        {
            return this._tableName != null;
        }

    }
}