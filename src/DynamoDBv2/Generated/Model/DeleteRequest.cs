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
    /// Represents a request to perform a <code>DeleteItem</code> operation on an item.
    /// </summary>
    public partial class DeleteRequest
    {
        private Dictionary<string, AttributeValue> _key = new Dictionary<string, AttributeValue>();

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public DeleteRequest() { }

        /// <summary>
        /// Instantiates DeleteRequest with the parameterized properties
        /// </summary>
        /// <param name="key">A map of attribute name to attribute values, representing the primary key of the item to delete. All of the table's primary key attributes must be specified, and their data types must match those of the table's key schema.</param>
        public DeleteRequest(Dictionary<string, AttributeValue> key)
        {
            _key = key;
        }

        /// <summary>
        /// Gets and sets the property Key. 
        /// <para>
        /// A map of attribute name to attribute values, representing the primary key of the item
        /// to delete. All of the table's primary key attributes must be specified, and their
        /// data types must match those of the table's key schema.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public Dictionary<string, AttributeValue> Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        // Check to see if Key property is set
        internal bool IsSetKey()
        {
            return this._key != null && this._key.Count > 0; 
        }

    }
}