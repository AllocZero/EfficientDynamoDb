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
    /// Represents the output of a <code>ListStreams</code> operation.
    /// </summary>
    public partial class ListStreamsResponse : AmazonWebServiceResponse
    {
        private string _lastEvaluatedStreamArn;
        private List<StreamSummary> _streams = new List<StreamSummary>();

        /// <summary>
        /// Gets and sets the property LastEvaluatedStreamArn. 
        /// <para>
        /// The stream ARN of the item where the operation stopped, inclusive of the previous
        /// result set. Use this value to start a new operation, excluding this value in the new
        /// request.
        /// </para>
        ///  
        /// <para>
        /// If <code>LastEvaluatedStreamArn</code> is empty, then the "last page" of results has
        /// been processed and there is no more data to be retrieved.
        /// </para>
        ///  
        /// <para>
        /// If <code>LastEvaluatedStreamArn</code> is not empty, it does not necessarily mean
        /// that there is more data in the result set. The only way to know when you have reached
        /// the end of the result set is when <code>LastEvaluatedStreamArn</code> is empty.
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string LastEvaluatedStreamArn
        {
            get { return this._lastEvaluatedStreamArn; }
            set { this._lastEvaluatedStreamArn = value; }
        }

        // Check to see if LastEvaluatedStreamArn property is set
        internal bool IsSetLastEvaluatedStreamArn()
        {
            return this._lastEvaluatedStreamArn != null;
        }

        /// <summary>
        /// Gets and sets the property Streams. 
        /// <para>
        /// A list of stream descriptors associated with the current account and endpoint.
        /// </para>
        /// </summary>
        public List<StreamSummary> Streams
        {
            get { return this._streams; }
            set { this._streams = value; }
        }

        // Check to see if Streams property is set
        internal bool IsSetStreams()
        {
            return this._streams != null && this._streams.Count > 0; 
        }

    }
}