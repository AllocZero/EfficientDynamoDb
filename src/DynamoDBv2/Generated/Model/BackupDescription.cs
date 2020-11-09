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
    /// Contains the description of the backup created for the table.
    /// </summary>
    public partial class BackupDescription
    {
        private BackupDetails _backupDetails;
        private SourceTableDetails _sourceTableDetails;
        private SourceTableFeatureDetails _sourceTableFeatureDetails;

        /// <summary>
        /// Gets and sets the property BackupDetails. 
        /// <para>
        /// Contains the details of the backup created for the table. 
        /// </para>
        /// </summary>
        public BackupDetails BackupDetails
        {
            get { return this._backupDetails; }
            set { this._backupDetails = value; }
        }

        // Check to see if BackupDetails property is set
        internal bool IsSetBackupDetails()
        {
            return this._backupDetails != null;
        }

        /// <summary>
        /// Gets and sets the property SourceTableDetails. 
        /// <para>
        /// Contains the details of the table when the backup was created. 
        /// </para>
        /// </summary>
        public SourceTableDetails SourceTableDetails
        {
            get { return this._sourceTableDetails; }
            set { this._sourceTableDetails = value; }
        }

        // Check to see if SourceTableDetails property is set
        internal bool IsSetSourceTableDetails()
        {
            return this._sourceTableDetails != null;
        }

        /// <summary>
        /// Gets and sets the property SourceTableFeatureDetails. 
        /// <para>
        /// Contains the details of the features enabled on the table when the backup was created.
        /// For example, LSIs, GSIs, streams, TTL.
        /// </para>
        /// </summary>
        public SourceTableFeatureDetails SourceTableFeatureDetails
        {
            get { return this._sourceTableFeatureDetails; }
            set { this._sourceTableFeatureDetails = value; }
        }

        // Check to see if SourceTableFeatureDetails property is set
        internal bool IsSetSourceTableFeatureDetails()
        {
            return this._sourceTableFeatureDetails != null;
        }

    }
}