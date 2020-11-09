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
    /// Contains details of a table archival operation.
    /// </summary>
    public partial class ArchivalSummary
    {
        private string _archivalBackupArn;
        private DateTime? _archivalDateTime;
        private string _archivalReason;

        /// <summary>
        /// Gets and sets the property ArchivalBackupArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) of the backup the table was archived to, when applicable
        /// in the archival reason. If you wish to restore this backup to the same table name,
        /// you will need to delete the original table.
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string ArchivalBackupArn
        {
            get { return this._archivalBackupArn; }
            set { this._archivalBackupArn = value; }
        }

        // Check to see if ArchivalBackupArn property is set
        internal bool IsSetArchivalBackupArn()
        {
            return this._archivalBackupArn != null;
        }

        /// <summary>
        /// Gets and sets the property ArchivalDateTime. 
        /// <para>
        /// The date and time when table archival was initiated by DynamoDB, in UNIX epoch time
        /// format.
        /// </para>
        /// </summary>
        public DateTime ArchivalDateTime
        {
            get { return this._archivalDateTime.GetValueOrDefault(); }
            set { this._archivalDateTime = value; }
        }

        // Check to see if ArchivalDateTime property is set
        internal bool IsSetArchivalDateTime()
        {
            return this._archivalDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ArchivalReason. 
        /// <para>
        /// The reason DynamoDB archived the table. Currently, the only possible value is:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>INACCESSIBLE_ENCRYPTION_CREDENTIALS</code> - The table was archived due to
        /// the table's AWS KMS key being inaccessible for more than seven days. An On-Demand
        /// backup was created at the archival time.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public string ArchivalReason
        {
            get { return this._archivalReason; }
            set { this._archivalReason = value; }
        }

        // Check to see if ArchivalReason property is set
        internal bool IsSetArchivalReason()
        {
            return this._archivalReason != null;
        }

    }
}