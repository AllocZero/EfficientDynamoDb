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
    /// Contains the details of the replica.
    /// </summary>
    public partial class ReplicaDescription
    {
        private List<ReplicaGlobalSecondaryIndexDescription> _globalSecondaryIndexes = new List<ReplicaGlobalSecondaryIndexDescription>();
        private string _kmsMasterKeyId;
        private ProvisionedThroughputOverride _provisionedThroughputOverride;
        private string _regionName;
        private DateTime? _replicaInaccessibleDateTime;
        private ReplicaStatus _replicaStatus;
        private string _replicaStatusDescription;
        private string _replicaStatusPercentProgress;

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexes. 
        /// <para>
        /// Replica-specific global secondary index settings.
        /// </para>
        /// </summary>
        public List<ReplicaGlobalSecondaryIndexDescription> GlobalSecondaryIndexes
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
        /// Gets and sets the property KMSMasterKeyId. 
        /// <para>
        /// The AWS KMS customer master key (CMK) of the replica that will be used for AWS KMS
        /// encryption.
        /// </para>
        /// </summary>
        public string KMSMasterKeyId
        {
            get { return this._kmsMasterKeyId; }
            set { this._kmsMasterKeyId = value; }
        }

        // Check to see if KMSMasterKeyId property is set
        internal bool IsSetKMSMasterKeyId()
        {
            return this._kmsMasterKeyId != null;
        }

        /// <summary>
        /// Gets and sets the property ProvisionedThroughputOverride. 
        /// <para>
        /// Replica-specific provisioned throughput. If not described, uses the source table's
        /// provisioned throughput settings.
        /// </para>
        /// </summary>
        public ProvisionedThroughputOverride ProvisionedThroughputOverride
        {
            get { return this._provisionedThroughputOverride; }
            set { this._provisionedThroughputOverride = value; }
        }

        // Check to see if ProvisionedThroughputOverride property is set
        internal bool IsSetProvisionedThroughputOverride()
        {
            return this._provisionedThroughputOverride != null;
        }

        /// <summary>
        /// Gets and sets the property RegionName. 
        /// <para>
        /// The name of the Region.
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
        /// Gets and sets the property ReplicaInaccessibleDateTime. 
        /// <para>
        /// The time at which the replica was first detected as inaccessible. To determine cause
        /// of inaccessibility check the <code>ReplicaStatus</code> property.
        /// </para>
        /// </summary>
        public DateTime ReplicaInaccessibleDateTime
        {
            get { return this._replicaInaccessibleDateTime.GetValueOrDefault(); }
            set { this._replicaInaccessibleDateTime = value; }
        }

        // Check to see if ReplicaInaccessibleDateTime property is set
        internal bool IsSetReplicaInaccessibleDateTime()
        {
            return this._replicaInaccessibleDateTime.HasValue; 
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
        ///  </li> <li> 
        /// <para>
        ///  <code>REGION_DISABLED</code> - The replica is inaccessible because the AWS Region
        /// has been disabled.
        /// </para>
        ///  <note> 
        /// <para>
        /// If the AWS Region remains inaccessible for more than 20 hours, DynamoDB will remove
        /// this replica from the replication group. The replica will not be deleted and replication
        /// will stop from and to this region.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        ///  <code>INACCESSIBLE_ENCRYPTION_CREDENTIALS </code> - The AWS KMS key used to encrypt
        /// the table is inaccessible.
        /// </para>
        ///  <note> 
        /// <para>
        /// If the AWS KMS key remains inaccessible for more than 20 hours, DynamoDB will remove
        /// this replica from the replication group. The replica will not be deleted and replication
        /// will stop from and to this region.
        /// </para>
        ///  </note> </li> </ul>
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

        /// <summary>
        /// Gets and sets the property ReplicaStatusDescription. 
        /// <para>
        /// Detailed information about the replica status.
        /// </para>
        /// </summary>
        public string ReplicaStatusDescription
        {
            get { return this._replicaStatusDescription; }
            set { this._replicaStatusDescription = value; }
        }

        // Check to see if ReplicaStatusDescription property is set
        internal bool IsSetReplicaStatusDescription()
        {
            return this._replicaStatusDescription != null;
        }

        /// <summary>
        /// Gets and sets the property ReplicaStatusPercentProgress. 
        /// <para>
        /// Specifies the progress of a Create, Update, or Delete action on the replica as a percentage.
        /// </para>
        /// </summary>
        public string ReplicaStatusPercentProgress
        {
            get { return this._replicaStatusPercentProgress; }
            set { this._replicaStatusPercentProgress = value; }
        }

        // Check to see if ReplicaStatusPercentProgress property is set
        internal bool IsSetReplicaStatusPercentProgress()
        {
            return this._replicaStatusPercentProgress != null;
        }

    }
}