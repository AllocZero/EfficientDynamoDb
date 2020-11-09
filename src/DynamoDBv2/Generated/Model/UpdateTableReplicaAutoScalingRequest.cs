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
    /// Container for the parameters to the UpdateTableReplicaAutoScaling operation.
    /// Updates auto scaling settings on your global tables at once.
    /// 
    ///  <note> 
    /// <para>
    /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
    /// 2019.11.21</a> of global tables.
    /// </para>
    ///  </note>
    /// </summary>
    public partial class UpdateTableReplicaAutoScalingRequest : AmazonDynamoDBRequest
    {
        private List<GlobalSecondaryIndexAutoScalingUpdate> _globalSecondaryIndexUpdates = new List<GlobalSecondaryIndexAutoScalingUpdate>();
        private AutoScalingSettingsUpdate _provisionedWriteCapacityAutoScalingUpdate;
        private List<ReplicaAutoScalingUpdate> _replicaUpdates = new List<ReplicaAutoScalingUpdate>();
        private string _tableName;

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexUpdates. 
        /// <para>
        /// Represents the auto scaling settings of the global secondary indexes of the replica
        /// to be updated.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public List<GlobalSecondaryIndexAutoScalingUpdate> GlobalSecondaryIndexUpdates
        {
            get { return this._globalSecondaryIndexUpdates; }
            set { this._globalSecondaryIndexUpdates = value; }
        }

        // Check to see if GlobalSecondaryIndexUpdates property is set
        internal bool IsSetGlobalSecondaryIndexUpdates()
        {
            return this._globalSecondaryIndexUpdates != null && this._globalSecondaryIndexUpdates.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ProvisionedWriteCapacityAutoScalingUpdate.
        /// </summary>
        public AutoScalingSettingsUpdate ProvisionedWriteCapacityAutoScalingUpdate
        {
            get { return this._provisionedWriteCapacityAutoScalingUpdate; }
            set { this._provisionedWriteCapacityAutoScalingUpdate = value; }
        }

        // Check to see if ProvisionedWriteCapacityAutoScalingUpdate property is set
        internal bool IsSetProvisionedWriteCapacityAutoScalingUpdate()
        {
            return this._provisionedWriteCapacityAutoScalingUpdate != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaUpdates. 
        /// <para>
        /// Represents the auto scaling settings of replicas of the table that will be modified.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public List<ReplicaAutoScalingUpdate> ReplicaUpdates
        {
            get { return this._replicaUpdates; }
            set { this._replicaUpdates = value; }
        }

        // Check to see if ReplicaUpdates property is set
        internal bool IsSetReplicaUpdates()
        {
            return this._replicaUpdates != null && this._replicaUpdates.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the global table to be updated.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string TableName
        {
            get { return this._tableName; }
            set { this._tableName = value; }
        }

        // Check to see if TableName property is set
        internal bool IsSetTableName()
        {
            return this._tableName != null;
        }

    }
}