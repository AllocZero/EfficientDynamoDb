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
    /// Container for the parameters to the GetRecords operation.
    /// Retrieves the stream records from a given shard.
    /// 
    ///  
    /// <para>
    /// Specify a shard iterator using the <code>ShardIterator</code> parameter. The shard
    /// iterator specifies the position in the shard from which you want to start reading
    /// stream records sequentially. If there are no stream records available in the portion
    /// of the shard that the iterator points to, <code>GetRecords</code> returns an empty
    /// list. Note that it might take multiple calls to get to a portion of the shard that
    /// contains stream records.
    /// </para>
    ///  <note> 
    /// <para>
    ///  <code>GetRecords</code> can retrieve a maximum of 1 MB of data or 1000 stream records,
    /// whichever comes first.
    /// </para>
    ///  </note>
    /// </summary>
    public partial class GetRecordsRequest : AmazonDynamoDBStreamsRequest
    {
        private int? _limit;
        private string _shardIterator;

        /// <summary>
        /// Gets and sets the property Limit. 
        /// <para>
        /// The maximum number of records to return from the shard. The upper limit is 1000.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public int Limit
        {
            get { return this._limit.GetValueOrDefault(); }
            set { this._limit = value; }
        }

        // Check to see if Limit property is set
        internal bool IsSetLimit()
        {
            return this._limit.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ShardIterator. 
        /// <para>
        /// A shard iterator that was retrieved from a previous GetShardIterator operation. This
        /// iterator can be used to access the stream records in this shard.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=1, Max=2048)]
        public string ShardIterator
        {
            get { return this._shardIterator; }
            set { this._shardIterator = value; }
        }

        // Check to see if ShardIterator property is set
        internal bool IsSetShardIterator()
        {
            return this._shardIterator != null;
        }

    }
}