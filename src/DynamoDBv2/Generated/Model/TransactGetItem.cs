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
    /// Specifies an item to be retrieved as part of the transaction.
    /// </summary>
    public partial class TransactGetItem
    {
        private Get _get;

        /// <summary>
        /// Gets and sets the property Get. 
        /// <para>
        /// Contains the primary key that identifies the item to get, together with the name of
        /// the table that contains the item, and optionally the specific attributes of the item
        /// to retrieve.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public Get Get
        {
            get { return this._get; }
            set { this._get = value; }
        }

        // Check to see if Get property is set
        internal bool IsSetGet()
        {
            return this._get != null;
        }

    }
}