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
    /// The description of the point in time settings applied to the table.
    /// </summary>
    public partial class PointInTimeRecoveryDescription
    {
        private DateTime? _earliestRestorableDateTime;
        private DateTime? _latestRestorableDateTime;
        private PointInTimeRecoveryStatus _pointInTimeRecoveryStatus;

        /// <summary>
        /// Gets and sets the property EarliestRestorableDateTime. 
        /// <para>
        /// Specifies the earliest point in time you can restore your table to. You can restore
        /// your table to any point in time during the last 35 days. 
        /// </para>
        /// </summary>
        public DateTime EarliestRestorableDateTime
        {
            get { return this._earliestRestorableDateTime.GetValueOrDefault(); }
            set { this._earliestRestorableDateTime = value; }
        }

        // Check to see if EarliestRestorableDateTime property is set
        internal bool IsSetEarliestRestorableDateTime()
        {
            return this._earliestRestorableDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property LatestRestorableDateTime. 
        /// <para>
        ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
        /// 
        /// </para>
        /// </summary>
        public DateTime LatestRestorableDateTime
        {
            get { return this._latestRestorableDateTime.GetValueOrDefault(); }
            set { this._latestRestorableDateTime = value; }
        }

        // Check to see if LatestRestorableDateTime property is set
        internal bool IsSetLatestRestorableDateTime()
        {
            return this._latestRestorableDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property PointInTimeRecoveryStatus. 
        /// <para>
        /// The current state of point in time recovery:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ENABLING</code> - Point in time recovery is being enabled.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ENABLED</code> - Point in time recovery is enabled.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DISABLED</code> - Point in time recovery is disabled.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public PointInTimeRecoveryStatus PointInTimeRecoveryStatus
        {
            get { return this._pointInTimeRecoveryStatus; }
            set { this._pointInTimeRecoveryStatus = value; }
        }

        // Check to see if PointInTimeRecoveryStatus property is set
        internal bool IsSetPointInTimeRecoveryStatus()
        {
            return this._pointInTimeRecoveryStatus != null;
        }

    }
}