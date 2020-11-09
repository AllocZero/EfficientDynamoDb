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
    /// An ordered list of errors for each item in the request which caused the transaction
    /// to get cancelled. The values of the list are ordered according to the ordering of
    /// the <code>TransactWriteItems</code> request parameter. If no error occurred for the
    /// associated item an error with a Null code and Null message will be present.
    /// </summary>
    public partial class CancellationReason
    {
        private string _code;
        private Dictionary<string, AttributeValue> _item = new Dictionary<string, AttributeValue>();
        private string _message;

        /// <summary>
        /// Gets and sets the property Code. 
        /// <para>
        /// Status code for the result of the cancelled transaction.
        /// </para>
        /// </summary>
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }

        // Check to see if Code property is set
        internal bool IsSetCode()
        {
            return this._code != null;
        }

        /// <summary>
        /// Gets and sets the property Item. 
        /// <para>
        /// Item in the request which caused the transaction to get cancelled.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> Item
        {
            get { return this._item; }
            set { this._item = value; }
        }

        // Check to see if Item property is set
        internal bool IsSetItem()
        {
            return this._item != null && this._item.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property Message. 
        /// <para>
        /// Cancellation reason message description.
        /// </para>
        /// </summary>
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }

        // Check to see if Message property is set
        internal bool IsSetMessage()
        {
            return this._message != null;
        }

    }
}