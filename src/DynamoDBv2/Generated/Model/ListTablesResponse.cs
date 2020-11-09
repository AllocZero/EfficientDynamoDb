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
    /// Represents the output of a <code>ListTables</code> operation.
    /// </summary>
    public partial class ListTablesResponse : AmazonWebServiceResponse
    {
        private string _lastEvaluatedTableName;
        private List<string> _tableNames = new List<string>();

        /// <summary>
        /// Gets and sets the property LastEvaluatedTableName. 
        /// <para>
        /// The name of the last table in the current page of results. Use this value as the <code>ExclusiveStartTableName</code>
        /// in a new request to obtain the next page of results, until all the table names are
        /// returned.
        /// </para>
        ///  
        /// <para>
        /// If you do not receive a <code>LastEvaluatedTableName</code> value in the response,
        /// this means that there are no more table names to be retrieved.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string LastEvaluatedTableName
        {
            get { return this._lastEvaluatedTableName; }
            set { this._lastEvaluatedTableName = value; }
        }

        // Check to see if LastEvaluatedTableName property is set
        internal bool IsSetLastEvaluatedTableName()
        {
            return this._lastEvaluatedTableName != null;
        }

        /// <summary>
        /// Gets and sets the property TableNames. 
        /// <para>
        /// The names of the tables associated with the current account at the current endpoint.
        /// The maximum size of this array is 100.
        /// </para>
        ///  
        /// <para>
        /// If <code>LastEvaluatedTableName</code> also appears in the output, you can use this
        /// value as the <code>ExclusiveStartTableName</code> parameter in a subsequent <code>ListTables</code>
        /// request and obtain the next page of results.
        /// </para>
        /// </summary>
        public List<string> TableNames
        {
            get { return this._tableNames; }
            set { this._tableNames = value; }
        }

        // Check to see if TableNames property is set
        internal bool IsSetTableNames()
        {
            return this._tableNames != null && this._tableNames.Count > 0; 
        }

    }
}