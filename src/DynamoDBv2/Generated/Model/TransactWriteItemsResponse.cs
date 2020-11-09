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
    /// This is the response object from the TransactWriteItems operation.
    /// </summary>
    public partial class TransactWriteItemsResponse : AmazonWebServiceResponse
    {
        private List<ConsumedCapacity> _consumedCapacity = new List<ConsumedCapacity>();
        private Dictionary<string, List<ItemCollectionMetrics>> _itemCollectionMetrics = new Dictionary<string, List<ItemCollectionMetrics>>();

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire <code>TransactWriteItems</code> operation.
        /// The values of the list are ordered according to the ordering of the <code>TransactItems</code>
        /// request parameter. 
        /// </para>
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
        /// Gets and sets the property ItemCollectionMetrics. 
        /// <para>
        /// A list of tables that were processed by <code>TransactWriteItems</code> and, for each
        /// table, information about any item collections that were affected by individual <code>UpdateItem</code>,
        /// <code>PutItem</code>, or <code>DeleteItem</code> operations. 
        /// </para>
        /// </summary>
        public Dictionary<string, List<ItemCollectionMetrics>> ItemCollectionMetrics
        {
            get { return this._itemCollectionMetrics; }
            set { this._itemCollectionMetrics = value; }
        }

        // Check to see if ItemCollectionMetrics property is set
        internal bool IsSetItemCollectionMetrics()
        {
            return this._itemCollectionMetrics != null && this._itemCollectionMetrics.Count > 0; 
        }

    }
}