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
    /// Container for the parameters to the RestoreTableToPointInTime operation.
    /// Restores the specified table to the specified point in time within <code>EarliestRestorableDateTime</code>
    /// and <code>LatestRestorableDateTime</code>. You can restore your table to any point
    /// in time during the last 35 days. Any number of users can execute up to 4 concurrent
    /// restores (any type of restore) in a given account. 
    /// 
    ///  
    /// <para>
    ///  When you restore using point in time recovery, DynamoDB restores your table data
    /// to the state based on the selected date and time (day:hour:minute:second) to a new
    /// table. 
    /// </para>
    ///  
    /// <para>
    ///  Along with data, the following are also included on the new restored table using
    /// point in time recovery: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Global secondary indexes (GSIs)
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Local secondary indexes (LSIs)
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Provisioned read and write capacity
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Encryption settings
    /// </para>
    ///  <important> 
    /// <para>
    ///  All these settings come from the current settings of the source table at the time
    /// of restore. 
    /// </para>
    ///  </important> </li> </ul> 
    /// <para>
    /// You must manually set up the following on the restored table:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Auto scaling policies
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// IAM policies
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Amazon CloudWatch metrics and alarms
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Tags
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Stream settings
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Time to Live (TTL) settings
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Point in time recovery settings
    /// </para>
    ///  </li> </ul>
    /// </summary>
    public partial class RestoreTableToPointInTimeRequest : AmazonDynamoDBRequest
    {
        private BillingMode _billingModeOverride;
        private List<GlobalSecondaryIndex> _globalSecondaryIndexOverride = new List<GlobalSecondaryIndex>();
        private List<LocalSecondaryIndex> _localSecondaryIndexOverride = new List<LocalSecondaryIndex>();
        private ProvisionedThroughput _provisionedThroughputOverride;
        private DateTime? _restoreDateTime;
        private string _sourceTableArn;
        private string _sourceTableName;
        private SSESpecification _sseSpecificationOverride;
        private string _targetTableName;
        private bool? _useLatestRestorableTime;

        /// <summary>
        /// Gets and sets the property BillingModeOverride. 
        /// <para>
        /// The billing mode of the restored table.
        /// </para>
        /// </summary>
        public BillingMode BillingModeOverride
        {
            get { return this._billingModeOverride; }
            set { this._billingModeOverride = value; }
        }

        // Check to see if BillingModeOverride property is set
        internal bool IsSetBillingModeOverride()
        {
            return this._billingModeOverride != null;
        }

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexOverride. 
        /// <para>
        /// List of global secondary indexes for the restored table. The indexes provided should
        /// match existing secondary indexes. You can choose to exclude some or all of the indexes
        /// at the time of restore.
        /// </para>
        /// </summary>
        public List<GlobalSecondaryIndex> GlobalSecondaryIndexOverride
        {
            get { return this._globalSecondaryIndexOverride; }
            set { this._globalSecondaryIndexOverride = value; }
        }

        // Check to see if GlobalSecondaryIndexOverride property is set
        internal bool IsSetGlobalSecondaryIndexOverride()
        {
            return this._globalSecondaryIndexOverride != null && this._globalSecondaryIndexOverride.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property LocalSecondaryIndexOverride. 
        /// <para>
        /// List of local secondary indexes for the restored table. The indexes provided should
        /// match existing secondary indexes. You can choose to exclude some or all of the indexes
        /// at the time of restore.
        /// </para>
        /// </summary>
        public List<LocalSecondaryIndex> LocalSecondaryIndexOverride
        {
            get { return this._localSecondaryIndexOverride; }
            set { this._localSecondaryIndexOverride = value; }
        }

        // Check to see if LocalSecondaryIndexOverride property is set
        internal bool IsSetLocalSecondaryIndexOverride()
        {
            return this._localSecondaryIndexOverride != null && this._localSecondaryIndexOverride.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ProvisionedThroughputOverride. 
        /// <para>
        /// Provisioned throughput settings for the restored table.
        /// </para>
        /// </summary>
        public ProvisionedThroughput ProvisionedThroughputOverride
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
        /// Gets and sets the property RestoreDateTime. 
        /// <para>
        /// Time in the past to restore the table to.
        /// </para>
        /// </summary>
        public DateTime RestoreDateTime
        {
            get { return this._restoreDateTime.GetValueOrDefault(); }
            set { this._restoreDateTime = value; }
        }

        // Check to see if RestoreDateTime property is set
        internal bool IsSetRestoreDateTime()
        {
            return this._restoreDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property SourceTableArn. 
        /// <para>
        /// The DynamoDB table that will be restored. This value is an Amazon Resource Name (ARN).
        /// </para>
        /// </summary>
        public string SourceTableArn
        {
            get { return this._sourceTableArn; }
            set { this._sourceTableArn = value; }
        }

        // Check to see if SourceTableArn property is set
        internal bool IsSetSourceTableArn()
        {
            return this._sourceTableArn != null;
        }

        /// <summary>
        /// Gets and sets the property SourceTableName. 
        /// <para>
        /// Name of the source table that is being restored.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string SourceTableName
        {
            get { return this._sourceTableName; }
            set { this._sourceTableName = value; }
        }

        // Check to see if SourceTableName property is set
        internal bool IsSetSourceTableName()
        {
            return this._sourceTableName != null;
        }

        /// <summary>
        /// Gets and sets the property SSESpecificationOverride. 
        /// <para>
        /// The new server-side encryption settings for the restored table.
        /// </para>
        /// </summary>
        public SSESpecification SSESpecificationOverride
        {
            get { return this._sseSpecificationOverride; }
            set { this._sseSpecificationOverride = value; }
        }

        // Check to see if SSESpecificationOverride property is set
        internal bool IsSetSSESpecificationOverride()
        {
            return this._sseSpecificationOverride != null;
        }

        /// <summary>
        /// Gets and sets the property TargetTableName. 
        /// <para>
        /// The name of the new table to which it must be restored to.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string TargetTableName
        {
            get { return this._targetTableName; }
            set { this._targetTableName = value; }
        }

        // Check to see if TargetTableName property is set
        internal bool IsSetTargetTableName()
        {
            return this._targetTableName != null;
        }

        /// <summary>
        /// Gets and sets the property UseLatestRestorableTime. 
        /// <para>
        /// Restore the table to the latest possible time. <code>LatestRestorableDateTime</code>
        /// is typically 5 minutes before the current time. 
        /// </para>
        /// </summary>
        public bool UseLatestRestorableTime
        {
            get { return this._useLatestRestorableTime.GetValueOrDefault(); }
            set { this._useLatestRestorableTime = value; }
        }

        // Check to see if UseLatestRestorableTime property is set
        internal bool IsSetUseLatestRestorableTime()
        {
            return this._useLatestRestorableTime.HasValue; 
        }

    }
}