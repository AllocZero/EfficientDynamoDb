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
    /// Contains details for the restore.
    /// </summary>
    public partial class RestoreSummary
    {
        private DateTime? _restoreDateTime;
        private bool? _restoreInProgress;
        private string _sourceBackupArn;
        private string _sourceTableArn;

        /// <summary>
        /// Gets and sets the property RestoreDateTime. 
        /// <para>
        /// Point in time or source backup time.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public DateTime RestoreDateTime
        {
            get { return this._restoreDateTime.GetValueOrDefault(); }
            set { this._restoreDateTime = value; }
        }

        // Check to see if RestoreDateTime property is set
        internal bool IsSetRestoreDateTime()
        {
            return this._restoreDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property RestoreInProgress. 
        /// <para>
        /// Indicates if a restore is in progress or not.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public bool RestoreInProgress
        {
            get { return this._restoreInProgress.GetValueOrDefault(); }
            set { this._restoreInProgress = value; }
        }

        // Check to see if RestoreInProgress property is set
        internal bool IsSetRestoreInProgress()
        {
            return this._restoreInProgress.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property SourceBackupArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) of the backup from which the table was restored.
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string SourceBackupArn
        {
            get { return this._sourceBackupArn; }
            set { this._sourceBackupArn = value; }
        }

        // Check to see if SourceBackupArn property is set
        internal bool IsSetSourceBackupArn()
        {
            return this._sourceBackupArn != null;
        }

        /// <summary>
        /// Gets and sets the property SourceTableArn. 
        /// <para>
        /// The ARN of the source table of the backup that is being restored.
        /// </para>
        /// </summary>
        public string SourceTableArn
        {
            get { return this._sourceTableArn; }
            set { this._sourceTableArn = value; }
        }

        // Check to see if SourceTableArn property is set
        internal bool IsSetSourceTableArn()
        {
            return this._sourceTableArn != null;
        }

    }
}