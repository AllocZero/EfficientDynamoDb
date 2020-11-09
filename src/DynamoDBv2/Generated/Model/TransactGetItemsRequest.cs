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
    /// Container for the parameters to the TransactGetItems operation.
    /// <code>TransactGetItems</code> is a synchronous operation that atomically retrieves
    /// multiple items from one or more tables (but not from indexes) in a single account
    /// and Region. A <code>TransactGetItems</code> call can contain up to 25 <code>TransactGetItem</code>
    /// objects, each of which contains a <code>Get</code> structure that specifies an item
    /// to retrieve from a table in the account and Region. A call to <code>TransactGetItems</code>
    /// cannot retrieve items from tables in more than one AWS account or Region. The aggregate
    /// size of the items in the transaction cannot exceed 4 MB.
    /// 
    ///  
    /// <para>
    /// DynamoDB rejects the entire <code>TransactGetItems</code> request if any of the following
    /// is true:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// A conflicting operation is in the process of updating an item to be read.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is insufficient provisioned capacity for the transaction to be completed.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is a user error, such as an invalid data format.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The aggregate size of the items in the transaction cannot exceed 4 MB.
    /// </para>
    ///  </li> </ul>
    /// </summary>
    public partial class TransactGetItemsRequest : AmazonDynamoDBRequest
    {
        private ReturnConsumedCapacity _returnConsumedCapacity;
        private List<TransactGetItem> _transactItems = new List<TransactGetItem>();

        /// <summary>
        /// Gets and sets the property ReturnConsumedCapacity. 
        /// <para>
        /// A value of <code>TOTAL</code> causes consumed capacity information to be returned,
        /// and a value of <code>NONE</code> prevents that information from being returned. No
        /// other value is valid.
        /// </para>
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity
        {
            get { return this._returnConsumedCapacity; }
            set { this._returnConsumedCapacity = value; }
        }

        // Check to see if ReturnConsumedCapacity property is set
        internal bool IsSetReturnConsumedCapacity()
        {
            return this._returnConsumedCapacity != null;
        }

        /// <summary>
        /// Gets and sets the property TransactItems. 
        /// <para>
        /// An ordered array of up to 25 <code>TransactGetItem</code> objects, each of which contains
        /// a <code>Get</code> structure.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=1, Max=25)]
        public List<TransactGetItem> TransactItems
        {
            get { return this._transactItems; }
            set { this._transactItems = value; }
        }

        // Check to see if TransactItems property is set
        internal bool IsSetTransactItems()
        {
            return this._transactItems != null && this._transactItems.Count > 0; 
        }

    }
}