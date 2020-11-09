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
    /// Represents the output of a <code>BatchWriteItem</code> operation.
    /// </summary>
    public partial class BatchWriteItemResponse : AmazonWebServiceResponse
    {
        private List<ConsumedCapacity> _consumedCapacity = new List<ConsumedCapacity>();
        private Dictionary<string, List<ItemCollectionMetrics>> _itemCollectionMetrics = new Dictionary<string, List<ItemCollectionMetrics>>();
        private Dictionary<string, List<WriteRequest>> _unprocessedItems = new Dictionary<string, List<WriteRequest>>();

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the entire <code>BatchWriteItem</code> operation.
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
        /// Gets and sets the property ItemCollectionMetrics. 
        /// <para>
        /// A list of tables that were processed by <code>BatchWriteItem</code> and, for each
        /// table, information about any item collections that were affected by individual <code>DeleteItem</code>
        /// or <code>PutItem</code> operations.
        /// </para>
        ///  
        /// <para>
        /// Each entry consists of the following subelements:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ItemCollectionKey</code> - The partition key value of the item collection.
        /// This is the same as the partition key value of the item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>SizeEstimateRangeGB</code> - An estimate of item collection size, expressed
        /// in GB. This is a two-element array containing a lower bound and an upper bound for
        /// the estimate. The estimate includes the size of all the items in the table, plus the
        /// size of all attributes projected into all of the local secondary indexes on the table.
        /// Use this estimate to measure whether a local secondary index is approaching its size
        /// limit.
        /// </para>
        ///  
        /// <para>
        /// The estimate is subject to change over time; therefore, do not rely on the precision
        /// or accuracy of the estimate.
        /// </para>
        ///  </li> </ul>
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

        /// <summary>
        /// Gets and sets the property UnprocessedItems. 
        /// <para>
        /// A map of tables and requests against those tables that were not processed. The <code>UnprocessedItems</code>
        /// value is in the same form as <code>RequestItems</code>, so you can provide this value
        /// directly to a subsequent <code>BatchGetItem</code> operation. For more information,
        /// see <code>RequestItems</code> in the Request Parameters section.
        /// </para>
        ///  
        /// <para>
        /// Each <code>UnprocessedItems</code> entry consists of a table name and, for that table,
        /// a list of operations to perform (<code>DeleteRequest</code> or <code>PutRequest</code>).
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>DeleteRequest</code> - Perform a <code>DeleteItem</code> operation on the specified
        /// item. The item to be deleted is identified by a <code>Key</code> subelement:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Key</code> - A map of primary key attribute values that uniquely identify the
        /// item. Each entry in this map consists of an attribute name and an attribute value.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>PutRequest</code> - Perform a <code>PutItem</code> operation on the specified
        /// item. The item to be put is identified by an <code>Item</code> subelement:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Item</code> - A map of attributes and their values. Each entry in this map
        /// consists of an attribute name and an attribute value. Attribute values must not be
        /// null; string and binary type attributes must have lengths greater than zero; and set
        /// type attributes must not be empty. Requests that contain empty values will be rejected
        /// with a <code>ValidationException</code> exception.
        /// </para>
        ///  
        /// <para>
        /// If you specify any attributes that are part of an index key, then the data types for
        /// those attributes must match those of the schema in the table's attribute definition.
        /// </para>
        ///  </li> </ul> </li> </ul> 
        /// <para>
        /// If there are no unprocessed items remaining, the response contains an empty <code>UnprocessedItems</code>
        /// map.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=25)]
        public Dictionary<string, List<WriteRequest>> UnprocessedItems
        {
            get { return this._unprocessedItems; }
            set { this._unprocessedItems = value; }
        }

        // Check to see if UnprocessedItems property is set
        internal bool IsSetUnprocessedItems()
        {
            return this._unprocessedItems != null && this._unprocessedItems.Count > 0; 
        }

    }
}