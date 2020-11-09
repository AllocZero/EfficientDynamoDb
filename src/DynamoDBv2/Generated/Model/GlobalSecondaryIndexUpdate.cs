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
    /// Represents one of the following:
    /// 
    ///  <ul> <li> 
    /// <para>
    /// A new global secondary index to be added to an existing table.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// New provisioned throughput parameters for an existing global secondary index.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// An existing global secondary index to be removed from an existing table.
    /// </para>
    ///  </li> </ul>
    /// </summary>
    public partial class GlobalSecondaryIndexUpdate
    {
        private CreateGlobalSecondaryIndexAction _create;
        private DeleteGlobalSecondaryIndexAction _delete;
        private UpdateGlobalSecondaryIndexAction _update;

        /// <summary>
        /// Gets and sets the property Create. 
        /// <para>
        /// The parameters required for creating a global secondary index on an existing table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>IndexName </code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>KeySchema </code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>AttributeDefinitions </code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Projection </code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ProvisionedThroughput </code> 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public CreateGlobalSecondaryIndexAction Create
        {
            get { return this._create; }
            set { this._create = value; }
        }

        // Check to see if Create property is set
        internal bool IsSetCreate()
        {
            return this._create != null;
        }

        /// <summary>
        /// Gets and sets the property Delete. 
        /// <para>
        /// The name of an existing global secondary index to be removed.
        /// </para>
        /// </summary>
        public DeleteGlobalSecondaryIndexAction Delete
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
        /// Gets and sets the property Update. 
        /// <para>
        /// The name of an existing global secondary index, along with new provisioned throughput
        /// settings to be applied to that index.
        /// </para>
        /// </summary>
        public UpdateGlobalSecondaryIndexAction Update
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