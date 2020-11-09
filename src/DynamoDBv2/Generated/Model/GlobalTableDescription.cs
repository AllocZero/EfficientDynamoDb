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
    /// Contains details about the global table.
    /// </summary>
    public partial class GlobalTableDescription
    {
        private DateTime? _creationDateTime;
        private string _globalTableArn;
        private string _globalTableName;
        private GlobalTableStatus _globalTableStatus;
        private List<ReplicaDescription> _replicationGroup = new List<ReplicaDescription>();

        /// <summary>
        /// Gets and sets the property CreationDateTime. 
        /// <para>
        /// The creation time of the global table.
        /// </para>
        /// </summary>
        public DateTime CreationDateTime
        {
            get { return this._creationDateTime.GetValueOrDefault(); }
            set { this._creationDateTime = value; }
        }

        // Check to see if CreationDateTime property is set
        internal bool IsSetCreationDateTime()
        {
            return this._creationDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property GlobalTableArn. 
        /// <para>
        /// The unique identifier of the global table.
        /// </para>
        /// </summary>
        public string GlobalTableArn
        {
            get { return this._globalTableArn; }
            set { this._globalTableArn = value; }
        }

        // Check to see if GlobalTableArn property is set
        internal bool IsSetGlobalTableArn()
        {
            return this._globalTableArn != null;
        }

        /// <summary>
        /// Gets and sets the property GlobalTableName. 
        /// <para>
        /// The global table name.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string GlobalTableName
        {
            get { return this._globalTableName; }
            set { this._globalTableName = value; }
        }

        // Check to see if GlobalTableName property is set
        internal bool IsSetGlobalTableName()
        {
            return this._globalTableName != null;
        }

        /// <summary>
        /// Gets and sets the property GlobalTableStatus. 
        /// <para>
        /// The current state of the global table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The global table is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The global table is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The global table is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The global table is ready for use.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public GlobalTableStatus GlobalTableStatus
        {
            get { return this._globalTableStatus; }
            set { this._globalTableStatus = value; }
        }

        // Check to see if GlobalTableStatus property is set
        internal bool IsSetGlobalTableStatus()
        {
            return this._globalTableStatus != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicationGroup. 
        /// <para>
        /// The Regions where the global table has replicas.
        /// </para>
        /// </summary>
        public List<ReplicaDescription> ReplicationGroup
        {
            get { return this._replicationGroup; }
            set { this._replicationGroup = value; }
        }

        // Check to see if ReplicationGroup property is set
        internal bool IsSetReplicationGroup()
        {
            return this._replicationGroup != null && this._replicationGroup.Count > 0; 
        }

    }
}