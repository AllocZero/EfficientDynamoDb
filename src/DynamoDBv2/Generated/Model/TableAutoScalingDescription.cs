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
    /// Represents the auto scaling configuration for a global table.
    /// </summary>
    public partial class TableAutoScalingDescription
    {
        private List<ReplicaAutoScalingDescription> _replicas = new List<ReplicaAutoScalingDescription>();
        private string _tableName;
        private TableStatus _tableStatus;

        /// <summary>
        /// Gets and sets the property Replicas. 
        /// <para>
        /// Represents replicas of the global table.
        /// </para>
        /// </summary>
        public List<ReplicaAutoScalingDescription> Replicas
        {
            get { return this._replicas; }
            set { this._replicas = value; }
        }

        // Check to see if Replicas property is set
        internal bool IsSetReplicas()
        {
            return this._replicas != null && this._replicas.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table.
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

        /// <summary>
        /// Gets and sets the property TableStatus. 
        /// <para>
        /// The current state of the table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The table is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The table is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The table is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The table is ready for use.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public TableStatus TableStatus
        {
            get { return this._tableStatus; }
            set { this._tableStatus = value; }
        }

        // Check to see if TableStatus property is set
        internal bool IsSetTableStatus()
        {
            return this._tableStatus != null;
        }

    }
}