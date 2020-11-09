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
    /// Represents the output of a <code>Scan</code> operation.
    /// </summary>
    public partial class ScanResponse : AmazonWebServiceResponse
    {
        private ConsumedCapacity _consumedCapacity;
        private int? _count;
        private List<Dictionary<string, AttributeValue>> _items = new List<Dictionary<string, AttributeValue>>();
        private Dictionary<string, AttributeValue> _lastEvaluatedKey = new Dictionary<string, AttributeValue>();
        private int? _scannedCount;

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// The capacity units consumed by the <code>Scan</code> operation. The data returned
        /// includes the total provisioned throughput consumed, along with statistics for the
        /// table and any indexes involved in the operation. <code>ConsumedCapacity</code> is
        /// only returned if the <code>ReturnConsumedCapacity</code> parameter was specified.
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ProvisionedThroughputIntro.html">Provisioned
        /// Throughput</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public ConsumedCapacity ConsumedCapacity
        {
            get { return this._consumedCapacity; }
            set { this._consumedCapacity = value; }
        }

        // Check to see if ConsumedCapacity property is set
        internal bool IsSetConsumedCapacity()
        {
            return this._consumedCapacity != null;
        }

        /// <summary>
        /// Gets and sets the property Count. 
        /// <para>
        /// The number of items in the response.
        /// </para>
        ///  
        /// <para>
        /// If you set <code>ScanFilter</code> in the request, then <code>Count</code> is the
        /// number of items returned after the filter was applied, and <code>ScannedCount</code>
        /// is the number of matching items before the filter was applied.
        /// </para>
        ///  
        /// <para>
        /// If you did not use a filter in the request, then <code>Count</code> is the same as
        /// <code>ScannedCount</code>.
        /// </para>
        /// </summary>
        public int Count
        {
            get { return this._count.GetValueOrDefault(); }
            set { this._count = value; }
        }

        // Check to see if Count property is set
        internal bool IsSetCount()
        {
            return this._count.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property Items. 
        /// <para>
        /// An array of item attributes that match the scan criteria. Each element in this array
        /// consists of an attribute name and the value for that attribute.
        /// </para>
        /// </summary>
        public List<Dictionary<string, AttributeValue>> Items
        {
            get { return this._items; }
            set { this._items = value; }
        }

        // Check to see if Items property is set
        internal bool IsSetItems()
        {
            return this._items != null && this._items.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property LastEvaluatedKey. 
        /// <para>
        /// The primary key of the item where the operation stopped, inclusive of the previous
        /// result set. Use this value to start a new operation, excluding this value in the new
        /// request.
        /// </para>
        ///  
        /// <para>
        /// If <code>LastEvaluatedKey</code> is empty, then the "last page" of results has been
        /// processed and there is no more data to be retrieved.
        /// </para>
        ///  
        /// <para>
        /// If <code>LastEvaluatedKey</code> is not empty, it does not necessarily mean that there
        /// is more data in the result set. The only way to know when you have reached the end
        /// of the result set is when <code>LastEvaluatedKey</code> is empty.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> LastEvaluatedKey
        {
            get { return this._lastEvaluatedKey; }
            set { this._lastEvaluatedKey = value; }
        }

        // Check to see if LastEvaluatedKey property is set
        internal bool IsSetLastEvaluatedKey()
        {
            return this._lastEvaluatedKey != null && this._lastEvaluatedKey.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ScannedCount. 
        /// <para>
        /// The number of items evaluated, before any <code>ScanFilter</code> is applied. A high
        /// <code>ScannedCount</code> value with few, or no, <code>Count</code> results indicates
        /// an inefficient <code>Scan</code> operation. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html#Count">Count
        /// and ScannedCount</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  
        /// <para>
        /// If you did not use a filter in the request, then <code>ScannedCount</code> is the
        /// same as <code>Count</code>.
        /// </para>
        /// </summary>
        public int ScannedCount
        {
            get { return this._scannedCount.GetValueOrDefault(); }
            set { this._scannedCount = value; }
        }

        // Check to see if ScannedCount property is set
        internal bool IsSetScannedCount()
        {
            return this._scannedCount.HasValue; 
        }

    }
}