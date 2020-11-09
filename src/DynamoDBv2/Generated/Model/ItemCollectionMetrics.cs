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
    /// Information about item collections, if any, that were affected by the operation. <code>ItemCollectionMetrics</code>
    /// is only returned if the request asked for it. If the table does not have any local
    /// secondary indexes, this information is not returned in the response.
    /// </summary>
    public partial class ItemCollectionMetrics
    {
        private Dictionary<string, AttributeValue> _itemCollectionKey = new Dictionary<string, AttributeValue>();
        private List<double> _sizeEstimateRangeGB = new List<double>();

        /// <summary>
        /// Gets and sets the property ItemCollectionKey. 
        /// <para>
        /// The partition key value of the item collection. This value is the same as the partition
        /// key value of the item.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> ItemCollectionKey
        {
            get { return this._itemCollectionKey; }
            set { this._itemCollectionKey = value; }
        }

        // Check to see if ItemCollectionKey property is set
        internal bool IsSetItemCollectionKey()
        {
            return this._itemCollectionKey != null && this._itemCollectionKey.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property SizeEstimateRangeGB. 
        /// <para>
        /// An estimate of item collection size, in gigabytes. This value is a two-element array
        /// containing a lower bound and an upper bound for the estimate. The estimate includes
        /// the size of all the items in the table, plus the size of all attributes projected
        /// into all of the local secondary indexes on that table. Use this estimate to measure
        /// whether a local secondary index is approaching its size limit.
        /// </para>
        ///  
        /// <para>
        /// The estimate is subject to change over time; therefore, do not rely on the precision
        /// or accuracy of the estimate.
        /// </para>
        /// </summary>
        public List<double> SizeEstimateRangeGB
        {
            get { return this._sizeEstimateRangeGB; }
            set { this._sizeEstimateRangeGB = value; }
        }

        // Check to see if SizeEstimateRangeGB property is set
        internal bool IsSetSizeEstimateRangeGB()
        {
            return this._sizeEstimateRangeGB != null && this._sizeEstimateRangeGB.Count > 0; 
        }

    }
}