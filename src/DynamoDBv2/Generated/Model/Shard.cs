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
 * Do not modify this file. This file is generated from the dynamodbstreams-2012-08-10.normal.json service model.
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
    /// A uniquely identified group of stream records within a stream.
    /// </summary>
    public partial class Shard
    {
        private string _parentShardId;
        private SequenceNumberRange _sequenceNumberRange;
        private string _shardId;

        /// <summary>
        /// Gets and sets the property ParentShardId. 
        /// <para>
        /// The shard ID of the current shard's parent.
        /// </para>
        /// </summary>
        [AWSProperty(Min=28, Max=65)]
        public string ParentShardId
        {
            get { return this._parentShardId; }
            set { this._parentShardId = value; }
        }

        // Check to see if ParentShardId property is set
        internal bool IsSetParentShardId()
        {
            return this._parentShardId != null;
        }

        /// <summary>
        /// Gets and sets the property SequenceNumberRange. 
        /// <para>
        /// The range of possible sequence numbers for the shard.
        /// </para>
        /// </summary>
        public SequenceNumberRange SequenceNumberRange
        {
            get { return this._sequenceNumberRange; }
            set { this._sequenceNumberRange = value; }
        }

        // Check to see if SequenceNumberRange property is set
        internal bool IsSetSequenceNumberRange()
        {
            return this._sequenceNumberRange != null;
        }

        /// <summary>
        /// Gets and sets the property ShardId. 
        /// <para>
        /// The system-generated identifier for this shard.
        /// </para>
        /// </summary>
        [AWSProperty(Min=28, Max=65)]
        public string ShardId
        {
            get { return this._shardId; }
            set { this._shardId = value; }
        }

        // Check to see if ShardId property is set
        internal bool IsSetShardId()
        {
            return this._shardId != null;
        }

    }
}