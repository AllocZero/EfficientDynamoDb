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
    /// Represents the auto scaling settings of the replica.
    /// </summary>
    public partial class ReplicaAutoScalingDescription
    {
        private List<ReplicaGlobalSecondaryIndexAutoScalingDescription> _globalSecondaryIndexes = new List<ReplicaGlobalSecondaryIndexAutoScalingDescription>();
        private string _regionName;
        private AutoScalingSettingsDescription _replicaProvisionedReadCapacityAutoScalingSettings;
        private AutoScalingSettingsDescription _replicaProvisionedWriteCapacityAutoScalingSettings;
        private ReplicaStatus _replicaStatus;

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexes. 
        /// <para>
        /// Replica-specific global secondary index auto scaling settings.
        /// </para>
        /// </summary>
        public List<ReplicaGlobalSecondaryIndexAutoScalingDescription> GlobalSecondaryIndexes
        {
            get { return this._globalSecondaryIndexes; }
            set { this._globalSecondaryIndexes = value; }
        }

        // Check to see if GlobalSecondaryIndexes property is set
        internal bool IsSetGlobalSecondaryIndexes()
        {
            return this._globalSecondaryIndexes != null && this._globalSecondaryIndexes.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property RegionName. 
        /// <para>
        /// The Region where the replica exists.
        /// </para>
        /// </summary>
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
        /// Gets and sets the property ReplicaProvisionedReadCapacityAutoScalingSettings.
        /// </summary>
        public AutoScalingSettingsDescription ReplicaProvisionedReadCapacityAutoScalingSettings
        {
            get { return this._replicaProvisionedReadCapacityAutoScalingSettings; }
            set { this._replicaProvisionedReadCapacityAutoScalingSettings = value; }
        }

        // Check to see if ReplicaProvisionedReadCapacityAutoScalingSettings property is set
        internal bool IsSetReplicaProvisionedReadCapacityAutoScalingSettings()
        {
            return this._replicaProvisionedReadCapacityAutoScalingSettings != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaProvisionedWriteCapacityAutoScalingSettings.
        /// </summary>
        public AutoScalingSettingsDescription ReplicaProvisionedWriteCapacityAutoScalingSettings
        {
            get { return this._replicaProvisionedWriteCapacityAutoScalingSettings; }
            set { this._replicaProvisionedWriteCapacityAutoScalingSettings = value; }
        }

        // Check to see if ReplicaProvisionedWriteCapacityAutoScalingSettings property is set
        internal bool IsSetReplicaProvisionedWriteCapacityAutoScalingSettings()
        {
            return this._replicaProvisionedWriteCapacityAutoScalingSettings != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaStatus. 
        /// <para>
        /// The current state of the replica:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The replica is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The replica is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The replica is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The replica is ready for use.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public ReplicaStatus ReplicaStatus
        {
            get { return this._replicaStatus; }
            set { this._replicaStatus = value; }
        }

        // Check to see if ReplicaStatus property is set
        internal bool IsSetReplicaStatus()
        {
            return this._replicaStatus != null;
        }

    }
}