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
    /// Represents the settings of a global secondary index for a global table that will be
    /// modified.
    /// </summary>
    public partial class ReplicaGlobalSecondaryIndexSettingsUpdate
    {
        private string _indexName;
        private AutoScalingSettingsUpdate _provisionedReadCapacityAutoScalingSettingsUpdate;
        private long? _provisionedReadCapacityUnits;

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
        /// Gets and sets the property ProvisionedReadCapacityAutoScalingSettingsUpdate. 
        /// <para>
        /// Auto scaling settings for managing a global secondary index replica's read capacity
        /// units.
        /// </para>
        /// </summary>
        public AutoScalingSettingsUpdate ProvisionedReadCapacityAutoScalingSettingsUpdate
        {
            get { return this._provisionedReadCapacityAutoScalingSettingsUpdate; }
            set { this._provisionedReadCapacityAutoScalingSettingsUpdate = value; }
        }

        // Check to see if ProvisionedReadCapacityAutoScalingSettingsUpdate property is set
        internal bool IsSetProvisionedReadCapacityAutoScalingSettingsUpdate()
        {
            return this._provisionedReadCapacityAutoScalingSettingsUpdate != null;
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

    }
}