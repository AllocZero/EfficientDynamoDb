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
    /// Container for the parameters to the CreateTable operation.
    /// The <code>CreateTable</code> operation adds a new table to your account. In an AWS
    /// account, table names must be unique within each Region. That is, you can have two
    /// tables with same name if you create the tables in different Regions.
    /// 
    ///  
    /// <para>
    ///  <code>CreateTable</code> is an asynchronous operation. Upon receiving a <code>CreateTable</code>
    /// request, DynamoDB immediately returns a response with a <code>TableStatus</code> of
    /// <code>CREATING</code>. After the table is created, DynamoDB sets the <code>TableStatus</code>
    /// to <code>ACTIVE</code>. You can perform read and write operations only on an <code>ACTIVE</code>
    /// table. 
    /// </para>
    ///  
    /// <para>
    /// You can optionally define secondary indexes on the new table, as part of the <code>CreateTable</code>
    /// operation. If you want to create multiple tables with secondary indexes on them, you
    /// must create the tables sequentially. Only one table with secondary indexes can be
    /// in the <code>CREATING</code> state at any given time.
    /// </para>
    ///  
    /// <para>
    /// You can use the <code>DescribeTable</code> action to check the table status.
    /// </para>
    /// </summary>
    public partial class CreateTableRequest : AmazonDynamoDBRequest
    {
        private List<AttributeDefinition> _attributeDefinitions = new List<AttributeDefinition>();
        private BillingMode _billingMode;
        private List<GlobalSecondaryIndex> _globalSecondaryIndexes = new List<GlobalSecondaryIndex>();
        private List<KeySchemaElement> _keySchema = new List<KeySchemaElement>();
        private List<LocalSecondaryIndex> _localSecondaryIndexes = new List<LocalSecondaryIndex>();
        private ProvisionedThroughput _provisionedThroughput;
        private SSESpecification _sseSpecification;
        private StreamSpecification _streamSpecification;
        private string _tableName;
        private List<Tag> _tags = new List<Tag>();

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public CreateTableRequest() { }

        /// <summary>
        /// Instantiates CreateTableRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="keySchema">Specifies the attributes that make up the primary key for a table or an index. The attributes in <code>KeySchema</code> must also be defined in the <code>AttributeDefinitions</code> array. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data Model</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each <code>KeySchemaElement</code> in the array is composed of: <ul> <li>  <code>AttributeName</code> - The name of this key attribute. </li> <li>  <code>KeyType</code> - The role that the key attribute will assume: <ul> <li>  <code>HASH</code> - partition key </li> <li>  <code>RANGE</code> - sort key </li> </ul> </li> </ul> <note> The partition key of an item is also known as its <i>hash attribute</i>. The term "hash attribute" derives from the DynamoDB usage of an internal hash function to evenly distribute data items across partitions, based on their partition key values. The sort key of an item is also known as its <i>range attribute</i>. The term "range attribute" derives from the way DynamoDB stores items with the same partition key physically close together, in sorted order by the sort key value. </note> For a simple primary key (partition key), you must provide exactly one element with a <code>KeyType</code> of <code>HASH</code>. For a composite primary key (partition key and sort key), you must provide exactly two elements, in this order: The first element must have a <code>KeyType</code> of <code>HASH</code>, and the second element must have a <code>KeyType</code> of <code>RANGE</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Working with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        public CreateTableRequest(string tableName, List<KeySchemaElement> keySchema)
        {
            _tableName = tableName;
            _keySchema = keySchema;
        }

        /// <summary>
        /// Instantiates CreateTableRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="keySchema">Specifies the attributes that make up the primary key for a table or an index. The attributes in <code>KeySchema</code> must also be defined in the <code>AttributeDefinitions</code> array. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data Model</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each <code>KeySchemaElement</code> in the array is composed of: <ul> <li>  <code>AttributeName</code> - The name of this key attribute. </li> <li>  <code>KeyType</code> - The role that the key attribute will assume: <ul> <li>  <code>HASH</code> - partition key </li> <li>  <code>RANGE</code> - sort key </li> </ul> </li> </ul> <note> The partition key of an item is also known as its <i>hash attribute</i>. The term "hash attribute" derives from the DynamoDB usage of an internal hash function to evenly distribute data items across partitions, based on their partition key values. The sort key of an item is also known as its <i>range attribute</i>. The term "range attribute" derives from the way DynamoDB stores items with the same partition key physically close together, in sorted order by the sort key value. </note> For a simple primary key (partition key), you must provide exactly one element with a <code>KeyType</code> of <code>HASH</code>. For a composite primary key (partition key and sort key), you must provide exactly two elements, in this order: The first element must have a <code>KeyType</code> of <code>HASH</code>, and the second element must have a <code>KeyType</code> of <code>RANGE</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Working with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="attributeDefinitions">An array of attributes that describe the key schema for the table and indexes.</param>
        /// <param name="provisionedThroughput">Represents the provisioned throughput settings for a specified table or index. The settings can be modified using the <code>UpdateTable</code> operation.  If you set BillingMode as <code>PROVISIONED</code>, you must specify this property. If you set BillingMode as <code>PAY_PER_REQUEST</code>, you cannot specify this property. For current minimum and maximum provisioned throughput values, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service, Account, and Table Quotas</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        public CreateTableRequest(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput)
        {
            _tableName = tableName;
            _keySchema = keySchema;
            _attributeDefinitions = attributeDefinitions;
            _provisionedThroughput = provisionedThroughput;
        }

        /// <summary>
        /// Gets and sets the property AttributeDefinitions. 
        /// <para>
        /// An array of attributes that describe the key schema for the table and indexes.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
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
        /// Gets and sets the property BillingMode. 
        /// <para>
        /// Controls how you are charged for read and write throughput and how you manage capacity.
        /// This setting can be changed later.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PROVISIONED</code> - We recommend using <code>PROVISIONED</code> for predictable
        /// workloads. <code>PROVISIONED</code> sets the billing mode to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadWriteCapacityMode.html#HowItWorks.ProvisionedThroughput.Manual">Provisioned
        /// Mode</a>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>PAY_PER_REQUEST</code> - We recommend using <code>PAY_PER_REQUEST</code> for
        /// unpredictable workloads. <code>PAY_PER_REQUEST</code> sets the billing mode to <a
        /// href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadWriteCapacityMode.html#HowItWorks.OnDemand">On-Demand
        /// Mode</a>. 
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
        /// Gets and sets the property GlobalSecondaryIndexes. 
        /// <para>
        /// One or more global secondary indexes (the maximum is 20) to be created on the table.
        /// Each global secondary index in the array includes the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>IndexName</code> - The name of the global secondary index. Must be unique only
        /// for this table.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>KeySchema</code> - Specifies the key schema for the global secondary index.
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
        /// exceed 100. If you project the same attribute into two different indexes, this counts
        /// as two distinct attributes when determining the total.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        ///  <code>ProvisionedThroughput</code> - The provisioned throughput settings for the
        /// global secondary index, consisting of read and write capacity units.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public List<GlobalSecondaryIndex> GlobalSecondaryIndexes
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
        /// Gets and sets the property KeySchema. 
        /// <para>
        /// Specifies the attributes that make up the primary key for a table or an index. The
        /// attributes in <code>KeySchema</code> must also be defined in the <code>AttributeDefinitions</code>
        /// array. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data
        /// Model</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  
        /// <para>
        /// Each <code>KeySchemaElement</code> in the array is composed of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>AttributeName</code> - The name of this key attribute.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>KeyType</code> - The role that the key attribute will assume:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>HASH</code> - partition key
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>RANGE</code> - sort key
        /// </para>
        ///  </li> </ul> </li> </ul> <note> 
        /// <para>
        /// The partition key of an item is also known as its <i>hash attribute</i>. The term
        /// "hash attribute" derives from the DynamoDB usage of an internal hash function to evenly
        /// distribute data items across partitions, based on their partition key values.
        /// </para>
        ///  
        /// <para>
        /// The sort key of an item is also known as its <i>range attribute</i>. The term "range
        /// attribute" derives from the way DynamoDB stores items with the same partition key
        /// physically close together, in sorted order by the sort key value.
        /// </para>
        ///  </note> 
        /// <para>
        /// For a simple primary key (partition key), you must provide exactly one element with
        /// a <code>KeyType</code> of <code>HASH</code>.
        /// </para>
        ///  
        /// <para>
        /// For a composite primary key (partition key and sort key), you must provide exactly
        /// two elements, in this order: The first element must have a <code>KeyType</code> of
        /// <code>HASH</code>, and the second element must have a <code>KeyType</code> of <code>RANGE</code>.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Working
        /// with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.
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
        /// Gets and sets the property LocalSecondaryIndexes. 
        /// <para>
        /// One or more local secondary indexes (the maximum is 5) to be created on the table.
        /// Each index is scoped to a given partition key value. There is a 10 GB size limit per
        /// partition key value; otherwise, the size of a local secondary index is unconstrained.
        /// </para>
        ///  
        /// <para>
        /// Each local secondary index in the array includes the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>IndexName</code> - The name of the local secondary index. Must be unique only
        /// for this table.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>KeySchema</code> - Specifies the key schema for the local secondary index.
        /// The key schema must begin with the same partition key as the table.
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
        /// exceed 100. If you project the same attribute into two different indexes, this counts
        /// as two distinct attributes when determining the total.
        /// </para>
        ///  </li> </ul> </li> </ul>
        /// </summary>
        public List<LocalSecondaryIndex> LocalSecondaryIndexes
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
        /// Represents the provisioned throughput settings for a specified table or index. The
        /// settings can be modified using the <code>UpdateTable</code> operation.
        /// </para>
        ///  
        /// <para>
        ///  If you set BillingMode as <code>PROVISIONED</code>, you must specify this property.
        /// If you set BillingMode as <code>PAY_PER_REQUEST</code>, you cannot specify this property.
        /// </para>
        ///  
        /// <para>
        /// For current minimum and maximum provisioned throughput values, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service,
        /// Account, and Table Quotas</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
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
        /// Gets and sets the property SSESpecification. 
        /// <para>
        /// Represents the settings used to enable server-side encryption.
        /// </para>
        /// </summary>
        public SSESpecification SSESpecification
        {
            get { return this._sseSpecification; }
            set { this._sseSpecification = value; }
        }

        // Check to see if SSESpecification property is set
        internal bool IsSetSSESpecification()
        {
            return this._sseSpecification != null;
        }

        /// <summary>
        /// Gets and sets the property StreamSpecification. 
        /// <para>
        /// The settings for DynamoDB Streams on the table. These settings consist of:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>StreamEnabled</code> - Indicates whether DynamoDB Streams is to be enabled
        /// (true) or disabled (false).
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>StreamViewType</code> - When an item in the table is modified, <code>StreamViewType</code>
        /// determines what information is written to the table's stream. Valid values for <code>StreamViewType</code>
        /// are:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>KEYS_ONLY</code> - Only the key attributes of the modified item are written
        /// to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>NEW_IMAGE</code> - The entire item, as it appears after it was modified, is
        /// written to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>OLD_IMAGE</code> - The entire item, as it appeared before it was modified,
        /// is written to the stream.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>NEW_AND_OLD_IMAGES</code> - Both the new and the old item images of the item
        /// are written to the stream.
        /// </para>
        ///  </li> </ul> </li> </ul>
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
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table to create.
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
        /// Gets and sets the property Tags. 
        /// <para>
        /// A list of key-value pairs to label the table. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a>.
        /// </para>
        /// </summary>
        public List<Tag> Tags
        {
            get { return this._tags; }
            set { this._tags = value; }
        }

        // Check to see if Tags property is set
        internal bool IsSetTags()
        {
            return this._tags != null && this._tags.Count > 0; 
        }

    }
}