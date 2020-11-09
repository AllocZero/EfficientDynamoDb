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
    /// This is the response object from the ListGlobalTables operation.
    /// </summary>
    public partial class ListGlobalTablesResponse : AmazonWebServiceResponse
    {
        private List<GlobalTable> _globalTables = new List<GlobalTable>();
        private string _lastEvaluatedGlobalTableName;

        /// <summary>
        /// Gets and sets the property GlobalTables. 
        /// <para>
        /// List of global table names.
        /// </para>
        /// </summary>
        public List<GlobalTable> GlobalTables
        {
            get { return this._globalTables; }
            set { this._globalTables = value; }
        }

        // Check to see if GlobalTables property is set
        internal bool IsSetGlobalTables()
        {
            return this._globalTables != null && this._globalTables.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property LastEvaluatedGlobalTableName. 
        /// <para>
        /// Last evaluated global table name.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string LastEvaluatedGlobalTableName
        {
            get { return this._lastEvaluatedGlobalTableName; }
            set { this._lastEvaluatedGlobalTableName = value; }
        }

        // Check to see if LastEvaluatedGlobalTableName property is set
        internal bool IsSetLastEvaluatedGlobalTableName()
        {
            return this._lastEvaluatedGlobalTableName != null;
        }

    }
}