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
    /// Container for the parameters to the UpdateGlobalTableSettings operation.
    /// Updates settings for a global table.
    /// </summary>
    public partial class UpdateGlobalTableSettingsRequest : AmazonDynamoDBRequest
    {
        private BillingMode _globalTableBillingMode;
        private List<GlobalTableGlobalSecondaryIndexSettingsUpdate> _globalTableGlobalSecondaryIndexSettingsUpdate = new List<GlobalTableGlobalSecondaryIndexSettingsUpdate>();
        private string _globalTableName;
        private AutoScalingSettingsUpdate _globalTableProvisionedWriteCapacityAutoScalingSettingsUpdate;
        private long? _globalTableProvisionedWriteCapacityUnits;
        private List<ReplicaSettingsUpdate> _replicaSettingsUpdate = new List<ReplicaSettingsUpdate>();

        /// <summary>
        /// Gets and sets the property GlobalTableBillingMode. 
        /// <para>
        /// The billing mode of the global table. If <code>GlobalTableBillingMode</code> is not
        /// specified, the global table defaults to <code>PROVISIONED</code> capacity billing
        /// mode.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PROVISIONED</code> - We recommend using <code>PROVISIONED</code> for predictable
        /// workloads. <code>PROVISIONED</code> sets the billing mode to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadWriteCapacityMode.html#HowItWorks.ProvisionedThroughput.Manual">Provisioned
        /// Mode</a>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>PAY_PER_REQUEST</code> - We recommend using <code>PAY_PER_REQUEST</code> for
        /// unpredictable workloads. <code>PAY_PER_REQUEST</code> sets the billing mode to <a
        /// href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadWriteCapacityMode.html#HowItWorks.OnDemand">On-Demand
        /// Mode</a>. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public BillingMode GlobalTableBillingMode
        {
            get { return this._globalTableBillingMode; }
            set { this._globalTableBillingMode = value; }
        }

        // Check to see if GlobalTableBillingMode property is set
        internal bool IsSetGlobalTableBillingMode()
        {
            return this._globalTableBillingMode != null;
        }

        /// <summary>
        /// Gets and sets the property GlobalTableGlobalSecondaryIndexSettingsUpdate. 
        /// <para>
        /// Represents the settings of a global secondary index for a global table that will be
        /// modified.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=20)]
        public List<GlobalTableGlobalSecondaryIndexSettingsUpdate> GlobalTableGlobalSecondaryIndexSettingsUpdate
        {
            get { return this._globalTableGlobalSecondaryIndexSettingsUpdate; }
            set { this._globalTableGlobalSecondaryIndexSettingsUpdate = value; }
        }

        // Check to see if GlobalTableGlobalSecondaryIndexSettingsUpdate property is set
        internal bool IsSetGlobalTableGlobalSecondaryIndexSettingsUpdate()
        {
            return this._globalTableGlobalSecondaryIndexSettingsUpdate != null && this._globalTableGlobalSecondaryIndexSettingsUpdate.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property GlobalTableName. 
        /// <para>
        /// The name of the global table
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
        /// Gets and sets the property GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate.
        /// 
        /// <para>
        /// Auto scaling settings for managing provisioned write capacity for the global table.
        /// </para>
        /// </summary>
        public AutoScalingSettingsUpdate GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate
        {
            get { return this._globalTableProvisionedWriteCapacityAutoScalingSettingsUpdate; }
            set { this._globalTableProvisionedWriteCapacityAutoScalingSettingsUpdate = value; }
        }

        // Check to see if GlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate property is set
        internal bool IsSetGlobalTableProvisionedWriteCapacityAutoScalingSettingsUpdate()
        {
            return this._globalTableProvisionedWriteCapacityAutoScalingSettingsUpdate != null;
        }

        /// <summary>
        /// Gets and sets the property GlobalTableProvisionedWriteCapacityUnits. 
        /// <para>
        /// The maximum number of writes consumed per second before DynamoDB returns a <code>ThrottlingException.</code>
        /// 
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long GlobalTableProvisionedWriteCapacityUnits
        {
            get { return this._globalTableProvisionedWriteCapacityUnits.GetValueOrDefault(); }
            set { this._globalTableProvisionedWriteCapacityUnits = value; }
        }

        // Check to see if GlobalTableProvisionedWriteCapacityUnits property is set
        internal bool IsSetGlobalTableProvisionedWriteCapacityUnits()
        {
            return this._globalTableProvisionedWriteCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ReplicaSettingsUpdate. 
        /// <para>
        /// Represents the settings for a global table in a Region that will be modified.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=50)]
        public List<ReplicaSettingsUpdate> ReplicaSettingsUpdate
        {
            get { return this._replicaSettingsUpdate; }
            set { this._replicaSettingsUpdate = value; }
        }

        // Check to see if ReplicaSettingsUpdate property is set
        internal bool IsSetReplicaSettingsUpdate()
        {
            return this._replicaSettingsUpdate != null && this._replicaSettingsUpdate.Count > 0; 
        }

    }
}