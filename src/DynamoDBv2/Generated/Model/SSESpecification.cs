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
    /// Represents the settings used to enable server-side encryption.
    /// </summary>
    public partial class SSESpecification
    {
        private bool? _enabled;
        private string _kmsMasterKeyId;
        private SSEType _sseType;

        /// <summary>
        /// Gets and sets the property Enabled. 
        /// <para>
        /// Indicates whether server-side encryption is done using an AWS managed CMK or an AWS
        /// owned CMK. If enabled (true), server-side encryption type is set to <code>KMS</code>
        /// and an AWS managed CMK is used (AWS KMS charges apply). If disabled (false) or not
        /// specified, server-side encryption is set to AWS owned CMK.
        /// </para>
        /// </summary>
        public bool Enabled
        {
            get { return this._enabled.GetValueOrDefault(); }
            set { this._enabled = value; }
        }

        // Check to see if Enabled property is set
        internal bool IsSetEnabled()
        {
            return this._enabled.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property KMSMasterKeyId. 
        /// <para>
        /// The AWS KMS customer master key (CMK) that should be used for the AWS KMS encryption.
        /// To specify a CMK, use its key ID, Amazon Resource Name (ARN), alias name, or alias
        /// ARN. Note that you should only provide this parameter if the key is different from
        /// the default DynamoDB customer master key alias/aws/dynamodb.
        /// </para>
        /// </summary>
        public string KMSMasterKeyId
        {
            get { return this._kmsMasterKeyId; }
            set { this._kmsMasterKeyId = value; }
        }

        // Check to see if KMSMasterKeyId property is set
        internal bool IsSetKMSMasterKeyId()
        {
            return this._kmsMasterKeyId != null;
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

    }
}