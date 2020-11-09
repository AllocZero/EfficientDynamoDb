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
    /// Represents all of the data describing a particular stream.
    /// </summary>
    public partial class StreamSummary
    {
        private string _streamArn;
        private string _streamLabel;
        private string _tableName;

        /// <summary>
        /// Gets and sets the property StreamArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) for the stream.
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string StreamArn
        {
            get { return this._streamArn; }
            set { this._streamArn = value; }
        }

        // Check to see if StreamArn property is set
        internal bool IsSetStreamArn()
        {
            return this._streamArn != null;
        }

        /// <summary>
        /// Gets and sets the property StreamLabel. 
        /// <para>
        /// A timestamp, in ISO 8601 format, for this stream.
        /// </para>
        ///  
        /// <para>
        /// Note that <code>LatestStreamLabel</code> is not a unique identifier for the stream,
        /// because it is possible that a stream from another table might have the same timestamp.
        /// However, the combination of the following three elements is guaranteed to be unique:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// the AWS customer ID.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// the table name
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// the <code>StreamLabel</code> 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public string StreamLabel
        {
            get { return this._streamLabel; }
            set { this._streamLabel = value; }
        }

        // Check to see if StreamLabel property is set
        internal bool IsSetStreamLabel()
        {
            return this._streamLabel != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The DynamoDB table with which the stream is associated.
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

    }
}