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
    /// This is the response object from the DescribeContinuousBackups operation.
    /// </summary>
    public partial class DescribeContinuousBackupsResponse : AmazonWebServiceResponse
    {
        private ContinuousBackupsDescription _continuousBackupsDescription;

        /// <summary>
        /// Gets and sets the property ContinuousBackupsDescription. 
        /// <para>
        /// Represents the continuous backups and point in time recovery settings on the table.
        /// </para>
        /// </summary>
        public ContinuousBackupsDescription ContinuousBackupsDescription
        {
            get { return this._continuousBackupsDescription; }
            set { this._continuousBackupsDescription = value; }
        }

        // Check to see if ContinuousBackupsDescription property is set
        internal bool IsSetContinuousBackupsDescription()
        {
            return this._continuousBackupsDescription != null;
        }

    }
}