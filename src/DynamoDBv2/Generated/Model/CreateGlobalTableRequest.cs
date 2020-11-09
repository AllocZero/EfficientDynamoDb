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
    /// Container for the parameters to the CreateGlobalTable operation.
    /// Creates a global table from an existing table. A global table creates a replication
    /// relationship between two or more DynamoDB tables with the same table name in the provided
    /// Regions. 
    /// 
    ///  <note> 
    /// <para>
    /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
    /// 2017.11.29</a> of global tables.
    /// </para>
    ///  </note> 
    /// <para>
    /// If you want to add a new replica table to a global table, each of the following conditions
    /// must be true:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// The table must have the same primary key as all of the other replicas.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The table must have the same name as all of the other replicas.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The table must have DynamoDB Streams enabled, with the stream containing both the
    /// new and the old images of the item.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// None of the replica tables in the global table can contain any data.
    /// </para>
    ///  </li> </ul> 
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
    ///  </li> </ul> 
    /// <para>
    ///  If local secondary indexes are specified, then the following conditions must also
    /// be met: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    ///  The local secondary indexes must have the same name. 
    /// </para>
    ///  </li> <li> 
    /// <para>
    ///  The local secondary indexes must have the same hash key and sort key (if present).
    /// 
    /// </para>
    ///  </li> </ul> <important> 
    /// <para>
    ///  Write capacity settings should be set consistently across your replica tables and
    /// secondary indexes. DynamoDB strongly recommends enabling auto scaling to manage the
    /// write capacity settings for all of your global tables replicas and indexes. 
    /// </para>
    ///  
    /// <para>
    ///  If you prefer to manage write capacity settings manually, you should provision equal
    /// replicated write capacity units to your replica tables. You should also provision
    /// equal replicated write capacity units to matching secondary indexes across your global
    /// table. 
    /// </para>
    ///  </important>
    /// </summary>
    public partial class CreateGlobalTableRequest : AmazonDynamoDBRequest
    {
        private string _globalTableName;
        private List<Replica> _replicationGroup = new List<Replica>();

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
        /// Gets and sets the property ReplicationGroup. 
        /// <para>
        /// The Regions where the global table needs to be created.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public List<Replica> ReplicationGroup
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