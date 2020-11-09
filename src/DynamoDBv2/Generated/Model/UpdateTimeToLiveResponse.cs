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
    /// This is the response object from the UpdateTimeToLive operation.
    /// </summary>
    public partial class UpdateTimeToLiveResponse : AmazonWebServiceResponse
    {
        private TimeToLiveSpecification _timeToLiveSpecification;

        /// <summary>
        /// Gets and sets the property TimeToLiveSpecification. 
        /// <para>
        /// Represents the output of an <code>UpdateTimeToLive</code> operation.
        /// </para>
        /// </summary>
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