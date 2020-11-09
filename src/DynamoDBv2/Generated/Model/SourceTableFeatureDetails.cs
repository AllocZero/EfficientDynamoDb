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
    /// Contains the details of the features enabled on the table when the backup was created.
    /// For example, LSIs, GSIs, streams, TTL.
    /// </summary>
    public partial class SourceTableFeatureDetails
    {
        private List<GlobalSecondaryIndexInfo> _globalSecondaryIndexes = new List<GlobalSecondaryIndexInfo>();
        private List<LocalSecondaryIndexInfo> _localSecondaryIndexes = new List<LocalSecondaryIndexInfo>();
        private SSEDescription _sseDescription;
        private StreamSpecification _streamDescription;
        private TimeToLiveDescription _timeToLiveDescription;

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexes. 
        /// <para>
        /// Represents the GSI properties for the table when the backup was created. It includes
        /// the IndexName, KeySchema, Projection, and ProvisionedThroughput for the GSIs on the
        /// table at the time of backup. 
        /// </para>
        /// </summary>
        public List<GlobalSecondaryIndexInfo> GlobalSecondaryIndexes
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
        /// Gets and sets the property LocalSecondaryIndexes. 
        /// <para>
        /// Represents the LSI properties for the table when the backup was created. It includes
        /// the IndexName, KeySchema and Projection for the LSIs on the table at the time of backup.
        /// 
        /// </para>
        /// </summary>
        public List<LocalSecondaryIndexInfo> LocalSecondaryIndexes
        {
            get { return this._localSecondaryIndexes; }
            set { this._localSecondaryIndexes = value; }
        }

        // Check to see if LocalSecondaryIndexes property is set
        internal bool IsSetLocalSecondaryIndexes()
        {
            return this._localSecondaryIndexes != null && this._localSecondaryIndexes.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property SSEDescription. 
        /// <para>
        /// The description of the server-side encryption status on the table when the backup
        /// was created.
        /// </para>
        /// </summary>
        public SSEDescription SSEDescription
        {
            get { return this._sseDescription; }
            set { this._sseDescription = value; }
        }

        // Check to see if SSEDescription property is set
        internal bool IsSetSSEDescription()
        {
            return this._sseDescription != null;
        }

        /// <summary>
        /// Gets and sets the property StreamDescription. 
        /// <para>
        /// Stream settings on the table when the backup was created.
        /// </para>
        /// </summary>
        public StreamSpecification StreamDescription
        {
            get { return this._streamDescription; }
            set { this._streamDescription = value; }
        }

        // Check to see if StreamDescription property is set
        internal bool IsSetStreamDescription()
        {
            return this._streamDescription != null;
        }

        /// <summary>
        /// Gets and sets the property TimeToLiveDescription. 
        /// <para>
        /// Time to Live settings on the table when the backup was created.
        /// </para>
        /// </summary>
        public TimeToLiveDescription TimeToLiveDescription
        {
            get { return this._timeToLiveDescription; }
            set { this._timeToLiveDescription = value; }
        }

        // Check to see if TimeToLiveDescription property is set
        internal bool IsSetTimeToLiveDescription()
        {
            return this._timeToLiveDescription != null;
        }

    }
}