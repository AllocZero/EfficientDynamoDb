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
    /// Represents the properties of a table.
    /// </summary>
    public partial class TableDescription
    {
        private ArchivalSummary _archivalSummary;
        private List<AttributeDefinition> _attributeDefinitions = new List<AttributeDefinition>();
        private BillingModeSummary _billingModeSummary;
        private DateTime? _creationDateTime;
        private List<GlobalSecondaryIndexDescription> _globalSecondaryIndexes = new List<GlobalSecondaryIndexDescription>();
        private string _globalTableVersion;
        private long? _itemCount;
        private List<KeySchemaElement> _keySchema = new List<KeySchemaElement>();
        private string _latestStreamArn;
        private string _latestStreamLabel;
        private List<LocalSecondaryIndexDescription> _localSecondaryIndexes = new List<LocalSecondaryIndexDescription>();
        private ProvisionedThroughputDescription _provisionedThroughput;
        private List<ReplicaDescription> _replicas = new List<ReplicaDescription>();
        private RestoreSummary _restoreSummary;
        private SSEDescription _sseDescription;
        private StreamSpecification _streamSpecification;
        private string _tableArn;
        private string _tableId;
        private string _tableName;
        private long? _tableSizeBytes;
        private TableStatus _tableStatus;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public TableDescription() { }

        /// <summary>
        /// Gets and sets the property ArchivalSummary. 
        /// <para>
        /// Contains information about the table archive.
        /// </para>
        /// </summary>
        public ArchivalSummary ArchivalSummary
        {
            get { return this._archivalSummary; }
            set { this._archivalSummary = value; }
        }

        // Check to see if ArchivalSummary property is set
        internal bool IsSetArchivalSummary()
        {
            return this._archivalSummary != null;
        }

        /// <summary>
        /// Gets and sets the property AttributeDefinitions. 
        /// <para>
        /// An array of <code>AttributeDefinition</code> objects. Each of these objects describes
        /// one attribute in the table and index key schema.
        /// </para>
        ///  
        /// <para>
        /// Each <code>AttributeDefinition</code> object in this array is composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>AttributeName</code> - The name of the attribute.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>AttributeType</code> - The data type for the attribute.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public List<AttributeDefinition> AttributeDefinitions
        {
            get { return this._attributeDefinitions; }
            set { this._attributeDefinitions = value; }
        }

        // Check to see if AttributeDefinitions property is set
        internal bool IsSetAttributeDefinitions()
        {
            return this._attributeDefinitions != null && this._attributeDefinitions.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property BillingModeSummary. 
        /// <para>
        /// Contains the details for the read/write capacity mode.
        /// </para>
        /// </summary>
        public BillingModeSummary BillingModeSummary
        {
            get { return this._billingModeSummary; }
            set { this._billingModeSummary = value; }
        }

        // Check to see if BillingModeSummary property is set
        internal bool IsSetBillingModeSummary()
        {
            return this._billingModeSummary != null;
        }

        /// <summary>
        /// Gets and sets the property CreationDateTime. 
        /// <para>
        /// The date and time when the table was created, in <a href="http://www.epochconverter.com/">UNIX
        /// epoch time</a> format.
        /// </para>
        /// </summary>
        public DateTime CreationDateTime
        {
            get { return this._creationDateTime.GetValueOrDefault(); }
            set { this._creationDateTime = value; }
        }

        // Check to see if CreationDateTime property is set
        internal bool IsSetCreationDateTime()
        {
            return this._creationDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property GlobalSecondaryIndexes. 
        /// <para>
        /// The global secondary indexes, if any, on the table. Each index is scoped to a given
        /// partition key value. Each element is composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Backfilling</code> - If true, then the index is currently in the backfilling
        /// phase. Backfilling occurs only when a new global secondary index is added to the table.
        /// It is the process by which DynamoDB populates the new index with data from the table.
        /// (This attribute does not appear for indexes that were created during a <code>CreateTable</code>
        /// operation.) 
        /// </para>
        ///  
        /// <para>
        ///  You can delete an index that is being created during the <code>Backfilling</code>
        /// phase when <code>IndexStatus</code> is set to CREATING and <code>Backfilling</code>
        /// is true. You can't delete the index that is being created when <code>IndexStatus</code>
        /// is set to CREATING and <code>Backfilling</code> is false. (This attribute does not
        /// appear for indexes that were created during a <code>CreateTable</code> operation.)
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>IndexName</code> - The name of the global secondary index.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>IndexSizeBytes</code> - The total size of the global secondary index, in bytes.
        /// DynamoDB updates this value approximately every six hours. Recent changes might not
        /// be reflected in this value. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>IndexStatus</code> - The current status of the global secondary index:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>CREATING</code> - The index is being created.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATING</code> - The index is being updated.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETING</code> - The index is being deleted.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ACTIVE</code> - The index is ready for use.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>ItemCount</code> - The number of items in the global secondary index. DynamoDB
        /// updates this value approximately every six hours. Recent changes might not be reflected
        /// in this value. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>KeySchema</code> - Specifies the complete index key schema. The attribute names
        /// in the key schema must be between 1 and 255 characters (inclusive). The key schema
        /// must begin with the same partition key as the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Projection</code> - Specifies attributes that are copied (projected) from the
        /// table into the index. These are in addition to the primary key attributes and index
        /// key attributes, which are automatically projected. Each attribute specification is
        /// composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ProjectionType</code> - One of the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KEYS_ONLY</code> - Only the index and primary keys are projected into the index.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>INCLUDE</code> - In addition to the attributes described in <code>KEYS_ONLY</code>,
        /// the secondary index will include other non-key attributes that you specify.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL</code> - All of the table attributes are projected into the index.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>NonKeyAttributes</code> - A list of one or more non-key attribute names that
        /// are projected into the secondary index. The total count of attributes provided in
        /// <code>NonKeyAttributes</code>, summed across all of the secondary indexes, must not
        /// exceed 20. If you project the same attribute into two different indexes, this counts
        /// as two distinct attributes when determining the total.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>ProvisionedThroughput</code> - The provisioned throughput settings for the
        /// global secondary index, consisting of read and write capacity units, along with data
        /// about increases and decreases. 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// If the table is in the <code>DELETING</code> state, no information about indexes will
        /// be returned.
        /// </para>
        /// </summary>
        public List<GlobalSecondaryIndexDescription> GlobalSecondaryIndexes
        {
            get { return this._globalSecondaryIndexes; }
            set { this._globalSecondaryIndexes = value; }
        }

        // Check to see if GlobalSecondaryIndexes property is set
        internal bool IsSetGlobalSecondaryIndexes()
        {
            return this._globalSecondaryIndexes != null && this._globalSecondaryIndexes.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property GlobalTableVersion. 
        /// <para>
        /// Represents the version of <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GlobalTables.html">global
        /// tables</a> in use, if the table is replicated across AWS Regions.
        /// </para>
        /// </summary>
        public string GlobalTableVersion
        {
            get { return this._globalTableVersion; }
            set { this._globalTableVersion = value; }
        }

        // Check to see if GlobalTableVersion property is set
        internal bool IsSetGlobalTableVersion()
        {
            return this._globalTableVersion != null;
        }

        /// <summary>
        /// Gets and sets the property ItemCount. 
        /// <para>
        /// The number of items in the specified table. DynamoDB updates this value approximately
        /// every six hours. Recent changes might not be reflected in this value.
        /// </para>
        /// </summary>
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
        /// The primary key structure for the table. Each <code>KeySchemaElement</code> consists
        /// of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>AttributeName</code> - The name of the attribute.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>KeyType</code> - The role of the attribute:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>HASH</code> - partition key
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>RANGE</code> - sort key
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// The partition key of an item is also known as its <i>hash attribute</i>. The term
        /// "hash attribute" derives from DynamoDB's usage of an internal hash function to evenly
        /// distribute data items across partitions, based on their partition key values.
        /// </para>
        ///  
        /// <para>
        /// The sort key of an item is also known as its <i>range attribute</i>. The term "range
        /// attribute" derives from the way DynamoDB stores items with the same partition key
        /// physically close together, in sorted order by the sort key value.
        /// </para>
        ///  </note> </li> </ul> 
        /// <para>
        /// For more information about primary keys, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html#DataModelPrimaryKey">Primary
        /// Key</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=2)]
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
        /// Gets and sets the property LatestStreamArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) that uniquely identifies the latest stream for this
        /// table.
        /// </para>
        /// </summary>
        [AWSProperty(Min=37, Max=1024)]
        public string LatestStreamArn
        {
            get { return this._latestStreamArn; }
            set { this._latestStreamArn = value; }
        }

        // Check to see if LatestStreamArn property is set
        internal bool IsSetLatestStreamArn()
        {
            return this._latestStreamArn != null;
        }

        /// <summary>
        /// Gets and sets the property LatestStreamLabel. 
        /// <para>
        /// A timestamp, in ISO 8601 format, for this stream.
        /// </para>
        ///  
        /// <para>
        /// Note that <code>LatestStreamLabel</code> is not a unique identifier for the stream,
        /// because it is possible that a stream from another table might have the same timestamp.
        /// However, the combination of the following three elements is guaranteed to be unique:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// AWS customer ID
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Table name
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>StreamLabel</code> 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public string LatestStreamLabel
        {
            get { return this._latestStreamLabel; }
            set { this._latestStreamLabel = value; }
        }

        // Check to see if LatestStreamLabel property is set
        internal bool IsSetLatestStreamLabel()
        {
            return this._latestStreamLabel != null;
        }

        /// <summary>
        /// Gets and sets the property LocalSecondaryIndexes. 
        /// <para>
        /// Represents one or more local secondary indexes on the table. Each index is scoped
        /// to a given partition key value. Tables with one or more local secondary indexes are
        /// subject to an item collection size limit, where the amount of data within a given
        /// item collection cannot exceed 10 GB. Each element is composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>IndexName</code> - The name of the local secondary index.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>KeySchema</code> - Specifies the complete index key schema. The attribute names
        /// in the key schema must be between 1 and 255 characters (inclusive). The key schema
        /// must begin with the same partition key as the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Projection</code> - Specifies attributes that are copied (projected) from the
        /// table into the index. These are in addition to the primary key attributes and index
        /// key attributes, which are automatically projected. Each attribute specification is
        /// composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ProjectionType</code> - One of the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KEYS_ONLY</code> - Only the index and primary keys are projected into the index.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>INCLUDE</code> - Only the specified table attributes are projected into the
        /// index. The list of projected attributes is in <code>NonKeyAttributes</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL</code> - All of the table attributes are projected into the index.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>NonKeyAttributes</code> - A list of one or more non-key attribute names that
        /// are projected into the secondary index. The total count of attributes provided in
        /// <code>NonKeyAttributes</code>, summed across all of the secondary indexes, must not
        /// exceed 20. If you project the same attribute into two different indexes, this counts
        /// as two distinct attributes when determining the total.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>IndexSizeBytes</code> - Represents the total size of the index, in bytes. DynamoDB
        /// updates this value approximately every six hours. Recent changes might not be reflected
        /// in this value.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ItemCount</code> - Represents the number of items in the index. DynamoDB updates
        /// this value approximately every six hours. Recent changes might not be reflected in
        /// this value.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// If the table is in the <code>DELETING</code> state, no information about indexes will
        /// be returned.
        /// </para>
        /// </summary>
        public List<LocalSecondaryIndexDescription> LocalSecondaryIndexes
        {
            get { return this._localSecondaryIndexes; }
            set { this._localSecondaryIndexes = value; }
        }

        // Check to see if LocalSecondaryIndexes property is set
        internal bool IsSetLocalSecondaryIndexes()
        {
            return this._localSecondaryIndexes != null && this._localSecondaryIndexes.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ProvisionedThroughput. 
        /// <para>
        /// The provisioned throughput settings for the table, consisting of read and write capacity
        /// units, along with data about increases and decreases.
        /// </para>
        /// </summary>
        public ProvisionedThroughputDescription ProvisionedThroughput
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
        /// Gets and sets the property Replicas. 
        /// <para>
        /// Represents replicas of the table.
        /// </para>
        /// </summary>
        public List<ReplicaDescription> Replicas
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
        /// Gets and sets the property RestoreSummary. 
        /// <para>
        /// Contains details for the restore.
        /// </para>
        /// </summary>
        public RestoreSummary RestoreSummary
        {
            get { return this._restoreSummary; }
            set { this._restoreSummary = value; }
        }

        // Check to see if RestoreSummary property is set
        internal bool IsSetRestoreSummary()
        {
            return this._restoreSummary != null;
        }

        /// <summary>
        /// Gets and sets the property SSEDescription. 
        /// <para>
        /// The description of the server-side encryption status on the specified table.
        /// </para>
        /// </summary>
        public SSEDescription SSEDescription
        {
            get { return this._sseDescription; }
            set { this._sseDescription = value; }
        }

        // Check to see if SSEDescription property is set
        internal bool IsSetSSEDescription()
        {
            return this._sseDescription != null;
        }

        /// <summary>
        /// Gets and sets the property StreamSpecification. 
        /// <para>
        /// The current DynamoDB Streams configuration for the table.
        /// </para>
        /// </summary>
        public StreamSpecification StreamSpecification
        {
            get { return this._streamSpecification; }
            set { this._streamSpecification = value; }
        }

        // Check to see if StreamSpecification property is set
        internal bool IsSetStreamSpecification()
        {
            return this._streamSpecification != null;
        }

        /// <summary>
        /// Gets and sets the property TableArn. 
        /// <para>
        /// The Amazon Resource Name (ARN) that uniquely identifies the table.
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
        /// Gets and sets the property TableId. 
        /// <para>
        /// Unique identifier for the table for which the backup was created. 
        /// </para>
        /// </summary>
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
        /// Gets and sets the property TableSizeBytes. 
        /// <para>
        /// The total size of the specified table, in bytes. DynamoDB updates this value approximately
        /// every six hours. Recent changes might not be reflected in this value.
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
        ///  </li> <li> 
        /// <para>
        ///  <code>INACCESSIBLE_ENCRYPTION_CREDENTIALS</code> - The AWS KMS key used to encrypt
        /// the table in inaccessible. Table operations may fail due to failure to use the AWS
        /// KMS key. DynamoDB will initiate the table archival process when a table's AWS KMS
        /// key remains inaccessible for more than seven days. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ARCHIVING</code> - The table is being archived. Operations are not allowed
        /// until archival is complete. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ARCHIVED</code> - The table has been archived. See the ArchivalReason for more
        /// information. 
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