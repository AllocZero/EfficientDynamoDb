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
    /// Represents an operation to perform - either <code>DeleteItem</code> or <code>PutItem</code>.
    /// You can only request one of these operations, not both, in a single <code>WriteRequest</code>.
    /// If you do need to perform both of these operations, you need to provide two separate
    /// <code>WriteRequest</code> objects.
    /// </summary>
    public partial class WriteRequest
    {
        private DeleteRequest _deleteRequest;
        private PutRequest _putRequest;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public WriteRequest() { }

        /// <summary>
        /// Instantiates WriteRequest with the parameterized properties
        /// </summary>
        /// <param name="putRequest">A request to perform a <code>PutItem</code> operation.</param>
        public WriteRequest(PutRequest putRequest)
        {
            _putRequest = putRequest;
        }

        /// <summary>
        /// Instantiates WriteRequest with the parameterized properties
        /// </summary>
        /// <param name="deleteRequest">A request to perform a <code>DeleteItem</code> operation.</param>
        public WriteRequest(DeleteRequest deleteRequest)
        {
            _deleteRequest = deleteRequest;
        }

        /// <summary>
        /// Gets and sets the property DeleteRequest. 
        /// <para>
        /// A request to perform a <code>DeleteItem</code> operation.
        /// </para>
        /// </summary>
        public DeleteRequest DeleteRequest
        {
            get { return this._deleteRequest; }
            set { this._deleteRequest = value; }
        }

        // Check to see if DeleteRequest property is set
        internal bool IsSetDeleteRequest()
        {
            return this._deleteRequest != null;
        }

        /// <summary>
        /// Gets and sets the property PutRequest. 
        /// <para>
        /// A request to perform a <code>PutItem</code> operation.
        /// </para>
        /// </summary>
        public PutRequest PutRequest
        {
            get { return this._putRequest; }
            set { this._putRequest = value; }
        }

        // Check to see if PutRequest property is set
        internal bool IsSetPutRequest()
        {
            return this._putRequest != null;
        }

    }
}