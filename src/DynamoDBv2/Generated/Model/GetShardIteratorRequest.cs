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
    /// Container for the parameters to the GetShardIterator operation.
    /// Returns a shard iterator. A shard iterator provides information about how to retrieve
    /// the stream records from within a shard. Use the shard iterator in a subsequent <code>GetRecords</code>
    /// request to read the stream records from the shard.
    /// 
    ///  <note> 
    /// <para>
    /// A shard iterator expires 15 minutes after it is returned to the requester.
    /// </para>
    ///  </note>
    /// </summary>
    public partial class GetShardIteratorRequest : AmazonDynamoDBStreamsRequest
    {
        private string _sequenceNumber;
        private string _shardId;
        private ShardIteratorType _shardIteratorType;
        private string _streamArn;

        /// <summary>
        /// Gets and sets the property SequenceNumber. 
        /// <para>
        /// The sequence number of a stream record in the shard from which to start reading.
        /// </para>
        /// </summary>
        [AWSProperty(Min=21, Max=40)]
        public string SequenceNumber
        {
            get { return this._sequenceNumber; }
            set { this._sequenceNumber = value; }
        }

        // Check to see if SequenceNumber property is set
        internal bool IsSetSequenceNumber()
        {
            return this._sequenceNumber != null;
        }

        /// <summary>
        /// Gets and sets the property ShardId. 
        /// <para>
        /// The identifier of the shard. The iterator will be returned for this shard ID.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=28, Max=65)]
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

        /// <summary>
        /// Gets and sets the property ShardIteratorType. 
        /// <para>
        /// Determines how the shard iterator is used to start reading stream records from the
        /// shard:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>AT_SEQUENCE_NUMBER</code> - Start reading exactly from the position denoted
        /// by a specific sequence number.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>AFTER_SEQUENCE_NUMBER</code> - Start reading right after the position denoted
        /// by a specific sequence number.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>TRIM_HORIZON</code> - Start reading at the last (untrimmed) stream record,
        /// which is the oldest record in the shard. In DynamoDB Streams, there is a 24 hour limit
        /// on data retention. Stream records whose age exceeds this limit are subject to removal
        /// (trimming) from the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>LATEST</code> - Start reading just after the most recent stream record in the
        /// shard, so that you always read the most recent data in the shard.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        [AWSProperty(Required=true)]
        public ShardIteratorType ShardIteratorType
        {
            get { return this._shardIteratorType; }
            set { this._shardIteratorType = value; }
        }

        // Check to see if ShardIteratorType property is set
        internal bool IsSetShardIteratorType()
        {
            return this._shardIteratorType != null;
        }

        /// <summary>
        /// Gets and sets the property StreamArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) for the stream.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=37, Max=1024)]
        public string StreamArn
        {
            get { return this._streamArn; }
            set { this._streamArn = value; }
        }

        // Check to see if StreamArn property is set
        internal bool IsSetStreamArn()
        {
            return this._streamArn != null;
        }

    }
}