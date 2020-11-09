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
    /// Represents the settings for a global table in a Region that will be modified.
    /// </summary>
    public partial class ReplicaSettingsUpdate
    {
        private string _regionName;
        private List<ReplicaGlobalSecondaryIndexSettingsUpdate> _replicaGlobalSecondaryIndexSettingsUpdate = new List<ReplicaGlobalSecondaryIndexSettingsUpdate>();
        private AutoScalingSettingsUpdate _replicaProvisionedReadCapacityAutoScalingSettingsUpdate;
        private long? _replicaProvisionedReadCapacityUnits;

        /// <summary>
        /// Gets and sets the property RegionName. 
        /// <para>
        /// The Region of the replica to be added.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public string RegionName
        {
            get { return this._regionName; }
            set { this._regionName = value; }
        }

        // Check to see if RegionName property is set
        internal bool IsSetRegionName()
        {
            return this._regionName != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaGlobalSecondaryIndexSettingsUpdate. 
        /// <para>
        /// Represents the settings of a global secondary index for a global table that will be
        /// modified.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=20)]
        public List<ReplicaGlobalSecondaryIndexSettingsUpdate> ReplicaGlobalSecondaryIndexSettingsUpdate
        {
            get { return this._replicaGlobalSecondaryIndexSettingsUpdate; }
            set { this._replicaGlobalSecondaryIndexSettingsUpdate = value; }
        }

        // Check to see if ReplicaGlobalSecondaryIndexSettingsUpdate property is set
        internal bool IsSetReplicaGlobalSecondaryIndexSettingsUpdate()
        {
            return this._replicaGlobalSecondaryIndexSettingsUpdate != null && this._replicaGlobalSecondaryIndexSettingsUpdate.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ReplicaProvisionedReadCapacityAutoScalingSettingsUpdate.
        /// 
        /// <para>
        /// Auto scaling settings for managing a global table replica's read capacity units.
        /// </para>
        /// </summary>
        public AutoScalingSettingsUpdate ReplicaProvisionedReadCapacityAutoScalingSettingsUpdate
        {
            get { return this._replicaProvisionedReadCapacityAutoScalingSettingsUpdate; }
            set { this._replicaProvisionedReadCapacityAutoScalingSettingsUpdate = value; }
        }

        // Check to see if ReplicaProvisionedReadCapacityAutoScalingSettingsUpdate property is set
        internal bool IsSetReplicaProvisionedReadCapacityAutoScalingSettingsUpdate()
        {
            return this._replicaProvisionedReadCapacityAutoScalingSettingsUpdate != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaProvisionedReadCapacityUnits. 
        /// <para>
        /// The maximum number of strongly consistent reads consumed per second before DynamoDB
        /// returns a <code>ThrottlingException</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#ProvisionedThroughput">Specifying
        /// Read and Write Requirements</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long ReplicaProvisionedReadCapacityUnits
        {
            get { return this._replicaProvisionedReadCapacityUnits.GetValueOrDefault(); }
            set { this._replicaProvisionedReadCapacityUnits = value; }
        }

        // Check to see if ReplicaProvisionedReadCapacityUnits property is set
        internal bool IsSetReplicaProvisionedReadCapacityUnits()
        {
            return this._replicaProvisionedReadCapacityUnits.HasValue; 
        }

    }
}