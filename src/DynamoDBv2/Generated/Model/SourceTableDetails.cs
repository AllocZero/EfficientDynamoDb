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
    /// Contains the details of the table when the backup was created.
    /// </summary>
    public partial class SourceTableDetails
    {
        private BillingMode _billingMode;
        private long? _itemCount;
        private List<KeySchemaElement> _keySchema = new List<KeySchemaElement>();
        private ProvisionedThroughput _provisionedThroughput;
        private string _tableArn;
        private DateTime? _tableCreationDateTime;
        private string _tableId;
        private string _tableName;
        private long? _tableSizeBytes;

        /// <summary>
        /// Gets and sets the property BillingMode. 
        /// <para>
        /// Controls how you are charged for read and write throughput and how you manage capacity.
        /// This setting can be changed later.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PROVISIONED</code> - Sets the read/write capacity mode to <code>PROVISIONED</code>.
        /// We recommend using <code>PROVISIONED</code> for predictable workloads.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>PAY_PER_REQUEST</code> - Sets the read/write capacity mode to <code>PAY_PER_REQUEST</code>.
        /// We recommend using <code>PAY_PER_REQUEST</code> for unpredictable workloads. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public BillingMode BillingMode
        {
            get { return this._billingMode; }
            set { this._billingMode = value; }
        }

        // Check to see if BillingMode property is set
        internal bool IsSetBillingMode()
        {
            return this._billingMode != null;
        }

        /// <summary>
        /// Gets and sets the property ItemCount. 
        /// <para>
        /// Number of items in the table. Note that this is an approximate value. 
        /// </para>
        /// </summary>
        [AWSProperty(Min=0)]
        public long ItemCount
        {
            get { return this._itemCount.GetValueOrDefault(); }
            set { this._itemCount = value; }
        }

        // Check to see if ItemCount property is set
        internal bool IsSetItemCount()
        {
            return this._itemCount.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property KeySchema. 
        /// <para>
        /// Schema of the table. 
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=1, Max=2)]
        public List<KeySchemaElement> KeySchema
        {
            get { return this._keySchema; }
            set { this._keySchema = value; }
        }

        // Check to see if KeySchema property is set
        internal bool IsSetKeySchema()
        {
            return this._keySchema != null && this._keySchema.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ProvisionedThroughput. 
        /// <para>
        /// Read IOPs and Write IOPS on the table when the backup was created.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public ProvisionedThroughput ProvisionedThroughput
        {
            get { return this._provisionedThroughput; }
            set { this._provisionedThroughput = value; }
        }

        // Check to see if ProvisionedThroughput property is set
        internal bool IsSetProvisionedThroughput()
        {
            return this._provisionedThroughput != null;
        }

        /// <summary>
        /// Gets and sets the property TableArn. 
        /// <para>
        /// ARN of the table for which backup was created. 
        /// </para>
        /// </summary>
        public string TableArn
        {
            get { return this._tableArn; }
            set { this._tableArn = value; }
        }

        // Check to see if TableArn property is set
        internal bool IsSetTableArn()
        {
            return this._tableArn != null;
        }

        /// <summary>
        /// Gets and sets the property TableCreationDateTime. 
        /// <para>
        /// Time when the source table was created. 
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public DateTime TableCreationDateTime
        {
            get { return this._tableCreationDateTime.GetValueOrDefault(); }
            set { this._tableCreationDateTime = value; }
        }

        // Check to see if TableCreationDateTime property is set
        internal bool IsSetTableCreationDateTime()
        {
            return this._tableCreationDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property TableId. 
        /// <para>
        /// Unique identifier for the table for which the backup was created. 
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public string TableId
        {
            get { return this._tableId; }
            set { this._tableId = value; }
        }

        // Check to see if TableId property is set
        internal bool IsSetTableId()
        {
            return this._tableId != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table for which the backup was created. 
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=3, Max=255)]
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
        /// Gets and sets the property TableSizeBytes. 
        /// <para>
        /// Size of the table in bytes. Note that this is an approximate value.
        /// </para>
        /// </summary>
        public long TableSizeBytes
        {
            get { return this._tableSizeBytes.GetValueOrDefault(); }
            set { this._tableSizeBytes = value; }
        }

        // Check to see if TableSizeBytes property is set
        internal bool IsSetTableSizeBytes()
        {
            return this._tableSizeBytes.HasValue; 
        }

    }
}