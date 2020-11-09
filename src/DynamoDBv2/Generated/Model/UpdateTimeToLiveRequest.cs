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
    /// Container for the parameters to the UpdateTimeToLive operation.
    /// The <code>UpdateTimeToLive</code> method enables or disables Time to Live (TTL) for
    /// the specified table. A successful <code>UpdateTimeToLive</code> call returns the current
    /// <code>TimeToLiveSpecification</code>. It can take up to one hour for the change to
    /// fully process. Any additional <code>UpdateTimeToLive</code> calls for the same table
    /// during this one hour duration result in a <code>ValidationException</code>. 
    /// 
    ///  
    /// <para>
    /// TTL compares the current time in epoch time format to the time stored in the TTL attribute
    /// of an item. If the epoch time value stored in the attribute is less than the current
    /// time, the item is marked as expired and subsequently deleted.
    /// </para>
    ///  <note> 
    /// <para>
    ///  The epoch time format is the number of seconds elapsed since 12:00:00 AM January
    /// 1, 1970 UTC. 
    /// </para>
    ///  </note> 
    /// <para>
    /// DynamoDB deletes expired items on a best-effort basis to ensure availability of throughput
    /// for other data operations. 
    /// </para>
    ///  <important> 
    /// <para>
    /// DynamoDB typically deletes expired items within two days of expiration. The exact
    /// duration within which an item gets deleted after expiration is specific to the nature
    /// of the workload. Items that have expired and not been deleted will still show up in
    /// reads, queries, and scans.
    /// </para>
    ///  </important> 
    /// <para>
    /// As items are deleted, they are removed from any local secondary index and global secondary
    /// index immediately in the same eventually consistent way as a standard delete operation.
    /// </para>
    ///  
    /// <para>
    /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html">Time
    /// To Live</a> in the Amazon DynamoDB Developer Guide. 
    /// </para>
    /// </summary>
    public partial class UpdateTimeToLiveRequest : AmazonDynamoDBRequest
    {
        private string _tableName;
        private TimeToLiveSpecification _timeToLiveSpecification;

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table to be configured.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
        public string TableName
        {
            get { return this._tableName; }
            set { this._tableName = value; }
        }

        // Check to see if TableName property is set
        internal bool IsSetTableName()
        {
            return this._tableName != null;
        }

        /// <summary>
        /// Gets and sets the property TimeToLiveSpecification. 
        /// <para>
        /// Represents the settings used to enable or disable Time to Live for the specified table.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public TimeToLiveSpecification TimeToLiveSpecification
        {
            get { return this._timeToLiveSpecification; }
            set { this._timeToLiveSpecification = value; }
        }

        // Check to see if TimeToLiveSpecification property is set
        internal bool IsSetTimeToLiveSpecification()
        {
            return this._timeToLiveSpecification != null;
        }

    }
}