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
    /// This is the response object from the DescribeTableReplicaAutoScaling operation.
    /// </summary>
    public partial class DescribeTableReplicaAutoScalingResponse : AmazonWebServiceResponse
    {
        private TableAutoScalingDescription _tableAutoScalingDescription;

        /// <summary>
        /// Gets and sets the property TableAutoScalingDescription. 
        /// <para>
        /// Represents the auto scaling properties of the table.
        /// </para>
        /// </summary>
        public TableAutoScalingDescription TableAutoScalingDescription
        {
            get { return this._tableAutoScalingDescription; }
            set { this._tableAutoScalingDescription = value; }
        }

        // Check to see if TableAutoScalingDescription property is set
        internal bool IsSetTableAutoScalingDescription()
        {
            return this._tableAutoScalingDescription != null;
        }

    }
}