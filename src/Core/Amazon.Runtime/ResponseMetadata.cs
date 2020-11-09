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
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Runtime
{
    /// <summary>
    /// Information about the request.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class ResponseMetadata
    {
        private string requestIdField;
        private IDictionary<string, string> _metadata;

        /// <summary>
        /// Gets and sets the RequestId property.
        /// ID that uniquely identifies a request. Amazon keeps track of request IDs. If you have a question about a request, include the request ID in your correspondence.
        /// </summary>
        public string RequestId
        {
            get { return this.requestIdField; }
            set { this.requestIdField = value; }
        }

        public IDictionary<string, string> Metadata
        {
            get
            {
                if (this._metadata == null)
                    this._metadata = new Dictionary<string, string>();

                return this._metadata;
            }
        }
    }
}
