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
    /// This is the response object from the UpdateGlobalTableSettings operation.
    /// </summary>
    public partial class UpdateGlobalTableSettingsResponse : AmazonWebServiceResponse
    {
        private string _globalTableName;
        private List<ReplicaSettingsDescription> _replicaSettings = new List<ReplicaSettingsDescription>();

        /// <summary>
        /// Gets and sets the property GlobalTableName. 
        /// <para>
        /// The name of the global table.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
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
        /// Gets and sets the property ReplicaSettings. 
        /// <para>
        /// The Region-specific settings for the global table.
        /// </para>
        /// </summary>
        public List<ReplicaSettingsDescription> ReplicaSettings
        {
            get { return this._replicaSettings; }
            set { this._replicaSettings = value; }
        }

        // Check to see if ReplicaSettings property is set
        internal bool IsSetReplicaSettings()
        {
            return this._replicaSettings != null && this._replicaSettings.Count > 0; 
        }

    }
}