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
    /// Represents the properties of a replica.
    /// </summary>
    public partial class ReplicaSettingsDescription
    {
        private string _regionName;
        private BillingModeSummary _replicaBillingModeSummary;
        private List<ReplicaGlobalSecondaryIndexSettingsDescription> _replicaGlobalSecondaryIndexSettings = new List<ReplicaGlobalSecondaryIndexSettingsDescription>();
        private AutoScalingSettingsDescription _replicaProvisionedReadCapacityAutoScalingSettings;
        private long? _replicaProvisionedReadCapacityUnits;
        private AutoScalingSettingsDescription _replicaProvisionedWriteCapacityAutoScalingSettings;
        private long? _replicaProvisionedWriteCapacityUnits;
        private ReplicaStatus _replicaStatus;

        /// <summary>
        /// Gets and sets the property RegionName. 
        /// <para>
        /// The Region name of the replica.
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
        /// Gets and sets the property ReplicaBillingModeSummary. 
        /// <para>
        /// The read/write capacity mode of the replica.
        /// </para>
        /// </summary>
        public BillingModeSummary ReplicaBillingModeSummary
        {
            get { return this._replicaBillingModeSummary; }
            set { this._replicaBillingModeSummary = value; }
        }

        // Check to see if ReplicaBillingModeSummary property is set
        internal bool IsSetReplicaBillingModeSummary()
        {
            return this._replicaBillingModeSummary != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaGlobalSecondaryIndexSettings. 
        /// <para>
        /// Replica global secondary index settings for the global table.
        /// </para>
        /// </summary>
        public List<ReplicaGlobalSecondaryIndexSettingsDescription> ReplicaGlobalSecondaryIndexSettings
        {
            get { return this._replicaGlobalSecondaryIndexSettings; }
            set { this._replicaGlobalSecondaryIndexSettings = value; }
        }

        // Check to see if ReplicaGlobalSecondaryIndexSettings property is set
        internal bool IsSetReplicaGlobalSecondaryIndexSettings()
        {
            return this._replicaGlobalSecondaryIndexSettings != null && this._replicaGlobalSecondaryIndexSettings.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ReplicaProvisionedReadCapacityAutoScalingSettings. 
        /// <para>
        /// Auto scaling settings for a global table replica's read capacity units.
        /// </para>
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
        /// Gets and sets the property ReplicaProvisionedReadCapacityUnits. 
        /// <para>
        /// The maximum number of strongly consistent reads consumed per second before DynamoDB
        /// returns a <code>ThrottlingException</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#ProvisionedThroughput">Specifying
        /// Read and Write Requirements</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        /// </summary>
        [AWSProperty(Min=0)]
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

        /// <summary>
        /// Gets and sets the property ReplicaProvisionedWriteCapacityAutoScalingSettings. 
        /// <para>
        /// Auto scaling settings for a global table replica's write capacity units.
        /// </para>
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
        /// Gets and sets the property ReplicaProvisionedWriteCapacityUnits. 
        /// <para>
        /// The maximum number of writes consumed per second before DynamoDB returns a <code>ThrottlingException</code>.
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#ProvisionedThroughput">Specifying
        /// Read and Write Requirements</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=0)]
        public long ReplicaProvisionedWriteCapacityUnits
        {
            get { return this._replicaProvisionedWriteCapacityUnits.GetValueOrDefault(); }
            set { this._replicaProvisionedWriteCapacityUnits = value; }
        }

        // Check to see if ReplicaProvisionedWriteCapacityUnits property is set
        internal bool IsSetReplicaProvisionedWriteCapacityUnits()
        {
            return this._replicaProvisionedWriteCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ReplicaStatus. 
        /// <para>
        /// The current state of the Region:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The Region is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The Region is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The Region is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The Region is ready for use.
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