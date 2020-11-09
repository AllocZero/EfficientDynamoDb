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
    /// Container for the parameters to the ListBackups operation.
    /// List backups associated with an AWS account. To list backups for a given table, specify
    /// <code>TableName</code>. <code>ListBackups</code> returns a paginated list of results
    /// with at most 1 MB worth of items in a page. You can also specify a maximum number
    /// of entries to be returned in a page. 
    /// 
    ///  
    /// <para>
    /// In the request, start time is inclusive, but end time is exclusive. Note that these
    /// boundaries are for the time at which the original backup was requested.
    /// </para>
    ///  
    /// <para>
    /// You can call <code>ListBackups</code> a maximum of five times per second.
    /// </para>
    /// </summary>
    public partial class ListBackupsRequest : AmazonDynamoDBRequest
    {
        private BackupTypeFilter _backupType;
        private string _exclusiveStartBackupArn;
        private int? _limit;
        private string _tableName;
        private DateTime? _timeRangeLowerBound;
        private DateTime? _timeRangeUpperBound;

        /// <summary>
        /// Gets and sets the property BackupType. 
        /// <para>
        /// The backups from the table specified by <code>BackupType</code> are listed.
        /// </para>
        ///  
        /// <para>
        /// Where <code>BackupType</code> can be:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>USER</code> - On-demand backup created by you.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>SYSTEM</code> - On-demand backup automatically created by DynamoDB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL</code> - All types of on-demand backups (USER and SYSTEM).
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public BackupTypeFilter BackupType
        {
            get { return this._backupType; }
            set { this._backupType = value; }
        }

        // Check to see if BackupType property is set
        internal bool IsSetBackupType()
        {
            return this._backupType != null;
        }

        /// <summary>
        /// Gets and sets the property ExclusiveStartBackupArn. 
        /// <para>
        ///  <code>LastEvaluatedBackupArn</code> is the Amazon Resource Name (ARN) of the backup
        /// last evaluated when the current page of results was returned, inclusive of the current
        /// page of results. This value may be specified as the <code>ExclusiveStartBackupArn</code>
        /// of a new <code>ListBackups</code> operation in order to fetch the next page of results.
        /// 
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string ExclusiveStartBackupArn
        {
            get { return this._exclusiveStartBackupArn; }
            set { this._exclusiveStartBackupArn = value; }
        }

        // Check to see if ExclusiveStartBackupArn property is set
        internal bool IsSetExclusiveStartBackupArn()
        {
            return this._exclusiveStartBackupArn != null;
        }

        /// <summary>
        /// Gets and sets the property Limit. 
        /// <para>
        /// Maximum number of backups to return at once.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=100)]
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
        /// Gets and sets the property TableName. 
        /// <para>
        /// The backups from the table specified by <code>TableName</code> are listed. 
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
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
        /// Gets and sets the property TimeRangeLowerBound. 
        /// <para>
        /// Only backups created after this time are listed. <code>TimeRangeLowerBound</code>
        /// is inclusive.
        /// </para>
        /// </summary>
        public DateTime TimeRangeLowerBound
        {
            get { return this._timeRangeLowerBound.GetValueOrDefault(); }
            set { this._timeRangeLowerBound = value; }
        }

        // Check to see if TimeRangeLowerBound property is set
        internal bool IsSetTimeRangeLowerBound()
        {
            return this._timeRangeLowerBound.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property TimeRangeUpperBound. 
        /// <para>
        /// Only backups created before this time are listed. <code>TimeRangeUpperBound</code>
        /// is exclusive. 
        /// </para>
        /// </summary>
        public DateTime TimeRangeUpperBound
        {
            get { return this._timeRangeUpperBound.GetValueOrDefault(); }
            set { this._timeRangeUpperBound = value; }
        }

        // Check to see if TimeRangeUpperBound property is set
        internal bool IsSetTimeRangeUpperBound()
        {
            return this._timeRangeUpperBound.HasValue; 
        }

    }
}