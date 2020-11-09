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
    /// A list of requests that can perform update, put, delete, or check operations on multiple
    /// items in one or more tables atomically.
    /// </summary>
    public partial class TransactWriteItem
    {
        private ConditionCheck _conditionCheck;
        private Delete _delete;
        private Put _put;
        private Update _update;

        /// <summary>
        /// Gets and sets the property ConditionCheck. 
        /// <para>
        /// A request to perform a check item operation.
        /// </para>
        /// </summary>
        public ConditionCheck ConditionCheck
        {
            get { return this._conditionCheck; }
            set { this._conditionCheck = value; }
        }

        // Check to see if ConditionCheck property is set
        internal bool IsSetConditionCheck()
        {
            return this._conditionCheck != null;
        }

        /// <summary>
        /// Gets and sets the property Delete. 
        /// <para>
        /// A request to perform a <code>DeleteItem</code> operation.
        /// </para>
        /// </summary>
        public Delete Delete
        {
            get { return this._delete; }
            set { this._delete = value; }
        }

        // Check to see if Delete property is set
        internal bool IsSetDelete()
        {
            return this._delete != null;
        }

        /// <summary>
        /// Gets and sets the property Put. 
        /// <para>
        /// A request to perform a <code>PutItem</code> operation.
        /// </para>
        /// </summary>
        public Put Put
        {
            get { return this._put; }
            set { this._put = value; }
        }

        // Check to see if Put property is set
        internal bool IsSetPut()
        {
            return this._put != null;
        }

        /// <summary>
        /// Gets and sets the property Update. 
        /// <para>
        /// A request to perform an <code>UpdateItem</code> operation.
        /// </para>
        /// </summary>
        public Update Update
        {
            get { return this._update; }
            set { this._update = value; }
        }

        // Check to see if Update property is set
        internal bool IsSetUpdate()
        {
            return this._update != null;
        }

    }
}