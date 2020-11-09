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
    /// Container for the parameters to the UpdateGlobalTable operation.
    /// Adds or removes replicas in the specified global table. The global table must already
    /// exist to be able to use this operation. Any replica to be added must be empty, have
    /// the same name as the global table, have the same key schema, have DynamoDB Streams
    /// enabled, and have the same provisioned and maximum write capacity units.
    /// 
    ///  <note> 
    /// <para>
    /// Although you can use <code>UpdateGlobalTable</code> to add replicas and remove replicas
    /// in a single request, for simplicity we recommend that you issue separate requests
    /// for adding or removing replicas.
    /// </para>
    ///  </note> 
    /// <para>
    ///  If global secondary indexes are specified, then the following conditions must also
    /// be met: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    ///  The global secondary indexes must have the same name. 
    /// </para>
    ///  </li> <li> 
    /// <para>
    ///  The global secondary indexes must have the same hash key and sort key (if present).
    /// 
    /// </para>
    ///  </li> <li> 
    /// <para>
    ///  The global secondary indexes must have the same provisioned and maximum write capacity
    /// units. 
    /// </para>
    ///  </li> </ul>
    /// </summary>
    public partial class UpdateGlobalTableRequest : AmazonDynamoDBRequest
    {
        private string _globalTableName;
        private List<ReplicaUpdate> _replicaUpdates = new List<ReplicaUpdate>();

        /// <summary>
        /// Gets and sets the property GlobalTableName. 
        /// <para>
        /// The global table name.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
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
        /// Gets and sets the property ReplicaUpdates. 
        /// <para>
        /// A list of Regions that should be added or removed from the global table.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public List<ReplicaUpdate> ReplicaUpdates
        {
            get { return this._replicaUpdates; }
            set { this._replicaUpdates = value; }
        }

        // Check to see if ReplicaUpdates property is set
        internal bool IsSetReplicaUpdates()
        {
            return this._replicaUpdates != null && this._replicaUpdates.Count > 0; 
        }

    }
}