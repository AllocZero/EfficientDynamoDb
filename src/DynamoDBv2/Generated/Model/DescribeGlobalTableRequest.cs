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
    /// Container for the parameters to the DescribeGlobalTable operation.
    /// Returns information about the specified global table.
    /// 
    ///  <note> 
    /// <para>
    /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
    /// 2017.11.29</a> of global tables. If you are using global tables <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
    /// 2019.11.21</a> you can use <a href="https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_DescribeTable.html">DescribeTable</a>
    /// instead.
    /// </para>
    ///  </note>
    /// </summary>
    public partial class DescribeGlobalTableRequest : AmazonDynamoDBRequest
    {
        private string _globalTableName;

        /// <summary>
        /// Gets and sets the property GlobalTableName. 
        /// <para>
        /// The name of the global table.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string GlobalTableName
        {
            get { return this._globalTableName; }
            set { this._globalTableName = value; }
        }

        // Check to see if GlobalTableName property is set
        internal bool IsSetGlobalTableName()
        {
            return this._globalTableName != null;
        }

    }
}