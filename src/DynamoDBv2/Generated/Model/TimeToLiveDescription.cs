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
    /// The description of the Time to Live (TTL) status on the specified table.
    /// </summary>
    public partial class TimeToLiveDescription
    {
        private string _attributeName;
        private TimeToLiveStatus _timeToLiveStatus;

        /// <summary>
        /// Gets and sets the property AttributeName. 
        /// <para>
        ///  The name of the TTL attribute for items in the table.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=255)]
        public string AttributeName
        {
            get { return this._attributeName; }
            set { this._attributeName = value; }
        }

        // Check to see if AttributeName property is set
        internal bool IsSetAttributeName()
        {
            return this._attributeName != null;
        }

        /// <summary>
        /// Gets and sets the property TimeToLiveStatus. 
        /// <para>
        ///  The TTL status for the table.
        /// </para>
        /// </summary>
        public TimeToLiveStatus TimeToLiveStatus
        {
            get { return this._timeToLiveStatus; }
            set { this._timeToLiveStatus = value; }
        }

        // Check to see if TimeToLiveStatus property is set
        internal bool IsSetTimeToLiveStatus()
        {
            return this._timeToLiveStatus != null;
        }

    }
}