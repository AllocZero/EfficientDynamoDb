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
    /// Represents the DynamoDB Streams configuration for a table in DynamoDB.
    /// </summary>
    public partial class StreamSpecification
    {
        private bool? _streamEnabled;
        private StreamViewType _streamViewType;

        /// <summary>
        /// Gets and sets the property StreamEnabled. 
        /// <para>
        /// Indicates whether DynamoDB Streams is enabled (true) or disabled (false) on the table.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public bool StreamEnabled
        {
            get { return this._streamEnabled.GetValueOrDefault(); }
            set { this._streamEnabled = value; }
        }

        // Check to see if StreamEnabled property is set
        internal bool IsSetStreamEnabled()
        {
            return this._streamEnabled.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property StreamViewType. 
        /// <para>
        ///  When an item in the table is modified, <code>StreamViewType</code> determines what
        /// information is written to the stream for this table. Valid values for <code>StreamViewType</code>
        /// are:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KEYS_ONLY</code> - Only the key attributes of the modified item are written
        /// to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>NEW_IMAGE</code> - The entire item, as it appears after it was modified, is
        /// written to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>OLD_IMAGE</code> - The entire item, as it appeared before it was modified,
        /// is written to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>NEW_AND_OLD_IMAGES</code> - Both the new and the old item images of the item
        /// are written to the stream.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public StreamViewType StreamViewType
        {
            get { return this._streamViewType; }
            set { this._streamViewType = value; }
        }

        // Check to see if StreamViewType property is set
        internal bool IsSetStreamViewType()
        {
            return this._streamViewType != null;
        }

    }
}