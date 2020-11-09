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
    /// This is the response object from the TransactGetItems operation.
    /// </summary>
    public partial class TransactGetItemsResponse : AmazonWebServiceResponse
    {
        private List<ConsumedCapacity> _consumedCapacity = new List<ConsumedCapacity>();
        private List<ItemResponse> _responses = new List<ItemResponse>();

        /// <summary>
        /// Gets and sets the property ConsumedCapacity. 
        /// <para>
        /// If the <i>ReturnConsumedCapacity</i> value was <code>TOTAL</code>, this is an array
        /// of <code>ConsumedCapacity</code> objects, one for each table addressed by <code>TransactGetItem</code>
        /// objects in the <i>TransactItems</i> parameter. These <code>ConsumedCapacity</code>
        /// objects report the read-capacity units consumed by the <code>TransactGetItems</code>
        /// call in that table.
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
        /// Gets and sets the property Responses. 
        /// <para>
        /// An ordered array of up to 25 <code>ItemResponse</code> objects, each of which corresponds
        /// to the <code>TransactGetItem</code> object in the same position in the <i>TransactItems</i>
        /// array. Each <code>ItemResponse</code> object contains a Map of the name-value pairs
        /// that are the projected attributes of the requested item.
        /// </para>
        ///  
        /// <para>
        /// If a requested item could not be retrieved, the corresponding <code>ItemResponse</code>
        /// object is Null, or if the requested item has no projected attributes, the corresponding
        /// <code>ItemResponse</code> object is an empty Map. 
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=25)]
        public List<ItemResponse> Responses
        {
            get { return this._responses; }
            set { this._responses = value; }
        }

        // Check to see if Responses property is set
        internal bool IsSetResponses()
        {
            return this._responses != null && this._responses.Count > 0; 
        }

    }
}