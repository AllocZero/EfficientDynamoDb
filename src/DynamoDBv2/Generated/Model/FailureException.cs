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
    /// Represents a failure a contributor insights operation.
    /// </summary>
    public partial class FailureException
    {
        private string _exceptionDescription;
        private string _exceptionName;

        /// <summary>
        /// Gets and sets the property ExceptionDescription. 
        /// <para>
        /// Description of the failure.
        /// </para>
        /// </summary>
        public string ExceptionDescription
        {
            get { return this._exceptionDescription; }
            set { this._exceptionDescription = value; }
        }

        // Check to see if ExceptionDescription property is set
        internal bool IsSetExceptionDescription()
        {
            return this._exceptionDescription != null;
        }

        /// <summary>
        /// Gets and sets the property ExceptionName. 
        /// <para>
        /// Exception name.
        /// </para>
        /// </summary>
        public string ExceptionName
        {
            get { return this._exceptionName; }
            set { this._exceptionName = value; }
        }

        // Check to see if ExceptionName property is set
        internal bool IsSetExceptionName()
        {
            return this._exceptionName != null;
        }

    }
}