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
    /// Represents the auto scaling settings for a global table or global secondary index.
    /// </summary>
    public partial class AutoScalingSettingsDescription
    {
        private bool? _autoScalingDisabled;
        private string _autoScalingRoleArn;
        private long? _maximumUnits;
        private long? _minimumUnits;
        private List<AutoScalingPolicyDescription> _scalingPolicies = new List<AutoScalingPolicyDescription>();

        /// <summary>
        /// Gets and sets the property AutoScalingDisabled. 
        /// <para>
        /// Disabled auto scaling for this global table or global secondary index.
        /// </para>
        /// </summary>
        public bool AutoScalingDisabled
        {
            get { return this._autoScalingDisabled.GetValueOrDefault(); }
            set { this._autoScalingDisabled = value; }
        }

        // Check to see if AutoScalingDisabled property is set
        internal bool IsSetAutoScalingDisabled()
        {
            return this._autoScalingDisabled.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property AutoScalingRoleArn. 
        /// <para>
        /// Role ARN used for configuring the auto scaling policy.
        /// </para>
        /// </summary>
        public string AutoScalingRoleArn
        {
            get { return this._autoScalingRoleArn; }
            set { this._autoScalingRoleArn = value; }
        }

        // Check to see if AutoScalingRoleArn property is set
        internal bool IsSetAutoScalingRoleArn()
        {
            return this._autoScalingRoleArn != null;
        }

        /// <summary>
        /// Gets and sets the property MaximumUnits. 
        /// <para>
        /// The maximum capacity units that a global table or global secondary index should be
        /// scaled up to.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long MaximumUnits
        {
            get { return this._maximumUnits.GetValueOrDefault(); }
            set { this._maximumUnits = value; }
        }

        // Check to see if MaximumUnits property is set
        internal bool IsSetMaximumUnits()
        {
            return this._maximumUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property MinimumUnits. 
        /// <para>
        /// The minimum capacity units that a global table or global secondary index should be
        /// scaled down to.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long MinimumUnits
        {
            get { return this._minimumUnits.GetValueOrDefault(); }
            set { this._minimumUnits = value; }
        }

        // Check to see if MinimumUnits property is set
        internal bool IsSetMinimumUnits()
        {
            return this._minimumUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ScalingPolicies. 
        /// <para>
        /// Information about the scaling policies.
        /// </para>
        /// </summary>
        public List<AutoScalingPolicyDescription> ScalingPolicies
        {
            get { return this._scalingPolicies; }
            set { this._scalingPolicies = value; }
        }

        // Check to see if ScalingPolicies property is set
        internal bool IsSetScalingPolicies()
        {
            return this._scalingPolicies != null && this._scalingPolicies.Count > 0; 
        }

    }
}