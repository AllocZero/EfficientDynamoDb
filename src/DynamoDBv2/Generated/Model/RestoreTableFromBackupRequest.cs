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
    /// Container for the parameters to the RestoreTableFromBackup operation.
    /// Creates a new table from an existing backup. Any number of users can execute up to
    /// 4 concurrent restores (any type of restore) in a given account. 
    /// 
    ///  
    /// <para>
    /// You can call <code>RestoreTableFromBackup</code> at a maximum rate of 10 times per
    /// second.
    /// </para>
    ///  
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
    ///  </li> </ul>
    /// </summary>
    public partial class RestoreTableFromBackupRequest : AmazonDynamoDBRequest
    {
        private string _backupArn;
        private BillingMode _billingModeOverride;
        private List<GlobalSecondaryIndex> _globalSecondaryIndexOverride = new List<GlobalSecondaryIndex>();
        private List<LocalSecondaryIndex> _localSecondaryIndexOverride = new List<LocalSecondaryIndex>();
        private ProvisionedThroughput _provisionedThroughputOverride;
        private SSESpecification _sseSpecificationOverride;
        private string _targetTableName;

        /// <summary>
        /// Gets and sets the property BackupArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) associated with the backup.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=37, Max=1024)]
        public string BackupArn
        {
            get { return this._backupArn; }
            set { this._backupArn = value; }
        }

        // Check to see if BackupArn property is set
        internal bool IsSetBackupArn()
        {
            return this._backupArn != null;
        }

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
        /// The name of the new table to which the backup must be restored.
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

    }
}