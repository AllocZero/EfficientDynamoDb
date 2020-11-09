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
    /// Represents attributes that are copied (projected) from the table into an index. These
    /// are in addition to the primary key attributes and index key attributes, which are
    /// automatically projected.
    /// </summary>
    public partial class Projection
    {
        private List<string> _nonKeyAttributes = new List<string>();
        private ProjectionType _projectionType;

        /// <summary>
        /// Gets and sets the property NonKeyAttributes. 
        /// <para>
        /// Represents the non-key attribute names which will be projected into the index.
        /// </para>
        ///  
        /// <para>
        /// For local secondary indexes, the total count of <code>NonKeyAttributes</code> summed
        /// across all of the local secondary indexes, must not exceed 20. If you project the
        /// same attribute into two different indexes, this counts as two distinct attributes
        /// when determining the total.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=20)]
        public List<string> NonKeyAttributes
        {
            get { return this._nonKeyAttributes; }
            set { this._nonKeyAttributes = value; }
        }

        // Check to see if NonKeyAttributes property is set
        internal bool IsSetNonKeyAttributes()
        {
            return this._nonKeyAttributes != null && this._nonKeyAttributes.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ProjectionType. 
        /// <para>
        /// The set of attributes that are projected into the index:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KEYS_ONLY</code> - Only the index and primary keys are projected into the index.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>INCLUDE</code> - In addition to the attributes described in <code>KEYS_ONLY</code>,
        /// the secondary index will include other non-key attributes that you specify.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL</code> - All of the table attributes are projected into the index.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public ProjectionType ProjectionType
        {
            get { return this._projectionType; }
            set { this._projectionType = value; }
        }

        // Check to see if ProjectionType property is set
        internal bool IsSetProjectionType()
        {
            return this._projectionType != null;
        }

    }
}