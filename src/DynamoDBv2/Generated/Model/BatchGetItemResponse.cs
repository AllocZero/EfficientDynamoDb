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
    /// Represents the output of a <code>BatchGetItem</code> operation.
    /// </summary>
    public partial class BatchGetItemResponse : AmazonWebServiceResponse
    {
        private List<ConsumedCapacity> _consumedCapacity = new List<ConsumedCapacity>();
        private Dictionary<string, List<Dictionary<string, AttributeValue>>> _responses = new Dictionary<string, List<Dictionary<string, AttributeValue>>>();
        private Dictionary<string, KeysAndAttributes> _unprocessedKeys = new Dictionary<string, KeysAndAttributes>();

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The read capacity units consumed by the entire <code>BatchGetItem</code> operation.
        /// </para>
        ///  
        /// <para>
        /// Each element consists of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>TableName</code> - The table that consumed the provisioned throughput.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>CapacityUnits</code> - The total number of capacity units consumed.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public List<ConsumedCapacity> ConsumedCapacity
        {
            get { return this._consumedCapacity; }
            set { this._consumedCapacity = value; }
        }

        // Check to see if ConsumedCapacity property is set
        internal bool IsSetConsumedCapacity()
        {
            return this._consumedCapacity != null && this._consumedCapacity.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property Responses. 
        /// <para>
        /// A map of table name to a list of items. Each object in <code>Responses</code> consists
        /// of a table name, along with a map of attribute data consisting of the data type and
        /// attribute value.
        /// </para>
        /// </summary>
        public Dictionary<string, List<Dictionary<string, AttributeValue>>> Responses
        {
            get { return this._responses; }
            set { this._responses = value; }
        }

        // Check to see if Responses property is set
        internal bool IsSetResponses()
        {
            return this._responses != null && this._responses.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property UnprocessedKeys. 
        /// <para>
        /// A map of tables and their respective keys that were not processed with the current
        /// response. The <code>UnprocessedKeys</code> value is in the same form as <code>RequestItems</code>,
        /// so the value can be provided directly to a subsequent <code>BatchGetItem</code> operation.
        /// For more information, see <code>RequestItems</code> in the Request Parameters section.
        /// </para>
        ///  
        /// <para>
        /// Each element consists of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Keys</code> - An array of primary key attribute values that define specific
        /// items in the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ProjectionExpression</code> - One or more attributes to be retrieved from the
        /// table or index. By default, all attributes are returned. If a requested attribute
        /// is not found, it does not appear in the result.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ConsistentRead</code> - The consistency of a read operation. If set to <code>true</code>,
        /// then a strongly consistent read is used; otherwise, an eventually consistent read
        /// is used.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// If there are no unprocessed keys remaining, the response contains an empty <code>UnprocessedKeys</code>
        /// map.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=100)]
        public Dictionary<string, KeysAndAttributes> UnprocessedKeys
        {
            get { return this._unprocessedKeys; }
            set { this._unprocessedKeys = value; }
        }

        // Check to see if UnprocessedKeys property is set
        internal bool IsSetUnprocessedKeys()
        {
            return this._unprocessedKeys != null && this._unprocessedKeys.Count > 0; 
        }

    }
}