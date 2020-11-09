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
    /// Represents the output of a <code>DescribeLimits</code> operation.
    /// </summary>
    public partial class DescribeLimitsResponse : AmazonWebServiceResponse
    {
        private long? _accountMaxReadCapacityUnits;
        private long? _accountMaxWriteCapacityUnits;
        private long? _tableMaxReadCapacityUnits;
        private long? _tableMaxWriteCapacityUnits;

        /// <summary>
        /// Gets and sets the property AccountMaxReadCapacityUnits. 
        /// <para>
        /// The maximum total read capacity units that your account allows you to provision across
        /// all of your tables in this Region.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long AccountMaxReadCapacityUnits
        {
            get { return this._accountMaxReadCapacityUnits.GetValueOrDefault(); }
            set { this._accountMaxReadCapacityUnits = value; }
        }

        // Check to see if AccountMaxReadCapacityUnits property is set
        internal bool IsSetAccountMaxReadCapacityUnits()
        {
            return this._accountMaxReadCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property AccountMaxWriteCapacityUnits. 
        /// <para>
        /// The maximum total write capacity units that your account allows you to provision across
        /// all of your tables in this Region.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long AccountMaxWriteCapacityUnits
        {
            get { return this._accountMaxWriteCapacityUnits.GetValueOrDefault(); }
            set { this._accountMaxWriteCapacityUnits = value; }
        }

        // Check to see if AccountMaxWriteCapacityUnits property is set
        internal bool IsSetAccountMaxWriteCapacityUnits()
        {
            return this._accountMaxWriteCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property TableMaxReadCapacityUnits. 
        /// <para>
        /// The maximum read capacity units that your account allows you to provision for a new
        /// table that you are creating in this Region, including the read capacity units provisioned
        /// for its global secondary indexes (GSIs).
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long TableMaxReadCapacityUnits
        {
            get { return this._tableMaxReadCapacityUnits.GetValueOrDefault(); }
            set { this._tableMaxReadCapacityUnits = value; }
        }

        // Check to see if TableMaxReadCapacityUnits property is set
        internal bool IsSetTableMaxReadCapacityUnits()
        {
            return this._tableMaxReadCapacityUnits.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property TableMaxWriteCapacityUnits. 
        /// <para>
        /// The maximum write capacity units that your account allows you to provision for a new
        /// table that you are creating in this Region, including the write capacity units provisioned
        /// for its global secondary indexes (GSIs).
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public long TableMaxWriteCapacityUnits
        {
            get { return this._tableMaxWriteCapacityUnits.GetValueOrDefault(); }
            set { this._tableMaxWriteCapacityUnits = value; }
        }

        // Check to see if TableMaxWriteCapacityUnits property is set
        internal bool IsSetTableMaxWriteCapacityUnits()
        {
            return this._tableMaxWriteCapacityUnits.HasValue; 
        }

    }
}