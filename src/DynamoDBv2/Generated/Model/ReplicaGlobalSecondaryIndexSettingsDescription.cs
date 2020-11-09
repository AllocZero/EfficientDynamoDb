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
    /// Represents the properties of a global secondary index.
    /// </summary>
    public partial class ReplicaGlobalSecondaryIndexSettingsDescription
    {
        private string _indexName;
        private IndexStatus _indexStatus;
        private AutoScalingSettingsDescription _provisionedReadCapacityAutoScalingSettings;
        private long? _provisionedReadCapacityUnits;
        private AutoScalingSettingsDescription _provisionedWriteCapacityAutoScalingSettings;
        private long? _provisionedWriteCapacityUnits;

        /// <summary>
        /// Gets and sets the property IndexName. 
        /// <para>
        /// The name of the global secondary index. The name must be unique among all other indexes
        /// on this table.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string IndexName
        {
            get { return this._indexName; }
            set { this._indexName = value; }
        }

        // Check to see if IndexName property is set
        internal bool IsSetIndexName()
        {
            return this._indexName != null;
        }

        /// <summary>
        /// Gets and sets the property IndexStatus. 
        /// <para>
        ///  The current status of the global secondary index:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The global secondary index is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The global secondary index is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The global secondary index is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The global secondary index is ready for use.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public IndexStatus IndexStatus
        {
            get { return this._indexStatus; }
            set { this._indexStatus = value; }
        }

        // Check to see if IndexStatus property is set
        internal bool IsSetIndexStatus()
        {
            return this._indexStatus != null;
        }

        /// <summary>
        /// Gets and sets the property ProvisionedReadCapacityAutoScalingSettings. 
        /// <para>
        /// Auto scaling settings for a global secondary index replica's read capacity units.
        /// </para>
        /// </summary>
        public AutoScalingSettingsDescription ProvisionedReadCapacityAutoScalingSettings
        {
            get { return this._provisionedReadCapacityAutoScalingSettings; }
            set { this._provisionedReadCapacityAutoScalingSettings = value; }
        }

        // Check to see if ProvisionedReadCapacityAutoScalingSettings property is set
        internal bool IsSetProvisionedReadCapacityAutoScalingSettings()
        {
            return this._provisionedReadCapacityAutoScalingSettings != null;
        }

        /// <summary>
        /// Gets and sets the property ProvisionedReadCapacityUnits. 
        /// <para>
        /// The maximum number of strongly consistent reads consumed per second before DynamoDB
        /// returns a <code>ThrottlingException</code>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long ProvisionedReadCapacityUnits
        {
            get { return this._provisionedReadCapacityUnits.GetValueOrDefault(); }
            set { this._provisionedReadCapacityUnits = value; }
        }

        // Check to see if ProvisionedReadCapacityUnits property is set
        internal bool IsSetProvisionedReadCapacityUnits()
        {
            return this._provisionedReadCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ProvisionedWriteCapacityAutoScalingSettings. 
        /// <para>
        /// Auto scaling settings for a global secondary index replica's write capacity units.
        /// </para>
        /// </summary>
        public AutoScalingSettingsDescription ProvisionedWriteCapacityAutoScalingSettings
        {
            get { return this._provisionedWriteCapacityAutoScalingSettings; }
            set { this._provisionedWriteCapacityAutoScalingSettings = value; }
        }

        // Check to see if ProvisionedWriteCapacityAutoScalingSettings property is set
        internal bool IsSetProvisionedWriteCapacityAutoScalingSettings()
        {
            return this._provisionedWriteCapacityAutoScalingSettings != null;
        }

        /// <summary>
        /// Gets and sets the property ProvisionedWriteCapacityUnits. 
        /// <para>
        /// The maximum number of writes consumed per second before DynamoDB returns a <code>ThrottlingException</code>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long ProvisionedWriteCapacityUnits
        {
            get { return this._provisionedWriteCapacityUnits.GetValueOrDefault(); }
            set { this._provisionedWriteCapacityUnits = value; }
        }

        // Check to see if ProvisionedWriteCapacityUnits property is set
        internal bool IsSetProvisionedWriteCapacityUnits()
        {
            return this._provisionedWriteCapacityUnits.HasValue; 
        }

    }
}