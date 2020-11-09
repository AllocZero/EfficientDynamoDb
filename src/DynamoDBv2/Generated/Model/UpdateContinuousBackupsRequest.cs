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
    /// Container for the parameters to the UpdateContinuousBackups operation.
    /// <code>UpdateContinuousBackups</code> enables or disables point in time recovery for
    /// the specified table. A successful <code>UpdateContinuousBackups</code> call returns
    /// the current <code>ContinuousBackupsDescription</code>. Continuous backups are <code>ENABLED</code>
    /// on all tables at table creation. If point in time recovery is enabled, <code>PointInTimeRecoveryStatus</code>
    /// will be set to ENABLED.
    /// 
    ///  
    /// <para>
    ///  Once continuous backups and point in time recovery are enabled, you can restore to
    /// any point in time within <code>EarliestRestorableDateTime</code> and <code>LatestRestorableDateTime</code>.
    /// 
    /// </para>
    ///  
    /// <para>
    ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
    /// You can restore your table to any point in time during the last 35 days. 
    /// </para>
    /// </summary>
    public partial class UpdateContinuousBackupsRequest : AmazonDynamoDBRequest
    {
        private PointInTimeRecoverySpecification _pointInTimeRecoverySpecification;
        private string _tableName;

        /// <summary>
        /// Gets and sets the property PointInTimeRecoverySpecification. 
        /// <para>
        /// Represents the settings used to enable point in time recovery.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public PointInTimeRecoverySpecification PointInTimeRecoverySpecification
        {
            get { return this._pointInTimeRecoverySpecification; }
            set { this._pointInTimeRecoverySpecification = value; }
        }

        // Check to see if PointInTimeRecoverySpecification property is set
        internal bool IsSetPointInTimeRecoverySpecification()
        {
            return this._pointInTimeRecoverySpecification != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table.
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

    }
}