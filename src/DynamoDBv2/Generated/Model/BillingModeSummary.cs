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
    /// Contains the details for the read/write capacity mode.
    /// </summary>
    public partial class BillingModeSummary
    {
        private BillingMode _billingMode;
        private DateTime? _lastUpdateToPayPerRequestDateTime;

        /// <summary>
        /// Gets and sets the property BillingMode. 
        /// <para>
        /// Controls how you are charged for read and write throughput and how you manage capacity.
        /// This setting can be changed later.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PROVISIONED</code> - Sets the read/write capacity mode to <code>PROVISIONED</code>.
        /// We recommend using <code>PROVISIONED</code> for predictable workloads.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>PAY_PER_REQUEST</code> - Sets the read/write capacity mode to <code>PAY_PER_REQUEST</code>.
        /// We recommend using <code>PAY_PER_REQUEST</code> for unpredictable workloads. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public BillingMode BillingMode
        {
            get { return this._billingMode; }
            set { this._billingMode = value; }
        }

        // Check to see if BillingMode property is set
        internal bool IsSetBillingMode()
        {
            return this._billingMode != null;
        }

        /// <summary>
        /// Gets and sets the property LastUpdateToPayPerRequestDateTime. 
        /// <para>
        /// Represents the time when <code>PAY_PER_REQUEST</code> was last set as the read/write
        /// capacity mode.
        /// </para>
        /// </summary>
        public DateTime LastUpdateToPayPerRequestDateTime
        {
            get { return this._lastUpdateToPayPerRequestDateTime.GetValueOrDefault(); }
            set { this._lastUpdateToPayPerRequestDateTime = value; }
        }

        // Check to see if LastUpdateToPayPerRequestDateTime property is set
        internal bool IsSetLastUpdateToPayPerRequestDateTime()
        {
            return this._lastUpdateToPayPerRequestDateTime.HasValue; 
        }

    }
}