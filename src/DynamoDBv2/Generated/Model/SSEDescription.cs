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
    /// The description of the server-side encryption status on the specified table.
    /// </summary>
    public partial class SSEDescription
    {
        private DateTime? _inaccessibleEncryptionDateTime;
        private string _kmsMasterKeyArn;
        private SSEType _sseType;
        private SSEStatus _status;

        /// <summary>
        /// Gets and sets the property InaccessibleEncryptionDateTime. 
        /// <para>
        /// Indicates the time, in UNIX epoch date format, when DynamoDB detected that the table's
        /// AWS KMS key was inaccessible. This attribute will automatically be cleared when DynamoDB
        /// detects that the table's AWS KMS key is accessible again. DynamoDB will initiate the
        /// table archival process when table's AWS KMS key remains inaccessible for more than
        /// seven days from this date.
        /// </para>
        /// </summary>
        public DateTime InaccessibleEncryptionDateTime
        {
            get { return this._inaccessibleEncryptionDateTime.GetValueOrDefault(); }
            set { this._inaccessibleEncryptionDateTime = value; }
        }

        // Check to see if InaccessibleEncryptionDateTime property is set
        internal bool IsSetInaccessibleEncryptionDateTime()
        {
            return this._inaccessibleEncryptionDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property KMSMasterKeyArn. 
        /// <para>
        /// The AWS KMS customer master key (CMK) ARN used for the AWS KMS encryption.
        /// </para>
        /// </summary>
        public string KMSMasterKeyArn
        {
            get { return this._kmsMasterKeyArn; }
            set { this._kmsMasterKeyArn = value; }
        }

        // Check to see if KMSMasterKeyArn property is set
        internal bool IsSetKMSMasterKeyArn()
        {
            return this._kmsMasterKeyArn != null;
        }

        /// <summary>
        /// Gets and sets the property SSEType. 
        /// <para>
        /// Server-side encryption type. The only supported value is:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KMS</code> - Server-side encryption that uses AWS Key Management Service. The
        /// key is stored in your account and is managed by AWS KMS (AWS KMS charges apply).
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public SSEType SSEType
        {
            get { return this._sseType; }
            set { this._sseType = value; }
        }

        // Check to see if SSEType property is set
        internal bool IsSetSSEType()
        {
            return this._sseType != null;
        }

        /// <summary>
        /// Gets and sets the property Status. 
        /// <para>
        /// Represents the current state of server-side encryption. The only supported values
        /// are:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ENABLED</code> - Server-side encryption is enabled.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - Server-side encryption is being updated.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public SSEStatus Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        // Check to see if Status property is set
        internal bool IsSetStatus()
        {
            return this._status != null;
        }

    }
}