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
    /// Container for the parameters to the Scan operation.
    /// The <code>Scan</code> operation returns one or more items and item attributes by accessing
    /// every item in a table or a secondary index. To have DynamoDB return fewer items, you
    /// can provide a <code>FilterExpression</code> operation.
    /// 
    ///  
    /// <para>
    /// If the total number of scanned items exceeds the maximum dataset size limit of 1 MB,
    /// the scan stops and results are returned to the user as a <code>LastEvaluatedKey</code>
    /// value to continue the scan in a subsequent operation. The results also include the
    /// number of items exceeding the limit. A scan can result in no table data meeting the
    /// filter criteria. 
    /// </para>
    ///  
    /// <para>
    /// A single <code>Scan</code> operation reads up to the maximum number of items set (if
    /// using the <code>Limit</code> parameter) or a maximum of 1 MB of data and then apply
    /// any filtering to the results using <code>FilterExpression</code>. If <code>LastEvaluatedKey</code>
    /// is present in the response, you need to paginate the result set. For more information,
    /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html#Scan.Pagination">Paginating
    /// the Results</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
    /// </para>
    ///  
    /// <para>
    ///  <code>Scan</code> operations proceed sequentially; however, for faster performance
    /// on a large table or secondary index, applications can request a parallel <code>Scan</code>
    /// operation by providing the <code>Segment</code> and <code>TotalSegments</code> parameters.
    /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Scan.html#Scan.ParallelScan">Parallel
    /// Scan</a> in the <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///  
    /// <para>
    ///  <code>Scan</code> uses eventually consistent reads when accessing the data in a table;
    /// therefore, the result set might not include the changes to data in the table immediately
    /// before the operation began. If you need a consistent copy of the data, as of the time
    /// that the <code>Scan</code> begins, you can set the <code>ConsistentRead</code> parameter
    /// to <code>true</code>.
    /// </para>
    /// </summary>
    public partial class ScanRequest : AmazonDynamoDBRequest
    {
        private List<string> _attributesToGet = new List<string>();
        private ConditionalOperator _conditionalOperator;
        private bool? _consistentRead;
        private Dictionary<string, AttributeValue> _exclusiveStartKey = new Dictionary<string, AttributeValue>();
        private Dictionary<string, string> _expressionAttributeNames = new Dictionary<string, string>();
        private Dictionary<string, AttributeValue> _expressionAttributeValues = new Dictionary<string, AttributeValue>();
        private string _filterExpression;
        private string _indexName;
        private int? _limit;
        private string _projectionExpression;
        private ReturnConsumedCapacity _returnConsumedCapacity;
        private Dictionary<string, Condition> _scanFilter = new Dictionary<string, Condition>();
        private int? _segment;
        private Select _select;
        private string _tableName;
        private int? _totalSegments;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public ScanRequest() { }

        /// <summary>
        /// Instantiates ScanRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        public ScanRequest(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Gets and sets the property AttributesToGet. 
        /// <para>
        /// This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more
        /// information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public List<string> AttributesToGet
        {
            get { return this._attributesToGet; }
            set { this._attributesToGet = value; }
        }

        // Check to see if AttributesToGet property is set
        internal bool IsSetAttributesToGet()
        {
            return this._attributesToGet != null && this._attributesToGet.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ConditionalOperator. 
        /// <para>
        /// This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ConditionalOperator.html">ConditionalOperator</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public ConditionalOperator ConditionalOperator
        {
            get { return this._conditionalOperator; }
            set { this._conditionalOperator = value; }
        }

        // Check to see if ConditionalOperator property is set
        internal bool IsSetConditionalOperator()
        {
            return this._conditionalOperator != null;
        }

        /// <summary>
        /// Gets and sets the property ConsistentRead. 
        /// <para>
        /// A Boolean value that determines the read consistency model during the scan:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// If <code>ConsistentRead</code> is <code>false</code>, then the data returned from
        /// <code>Scan</code> might not contain the results from other recently completed write
        /// operations (<code>PutItem</code>, <code>UpdateItem</code>, or <code>DeleteItem</code>).
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// If <code>ConsistentRead</code> is <code>true</code>, then all of the write operations
        /// that completed before the <code>Scan</code> began are guaranteed to be contained in
        /// the <code>Scan</code> response.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// The default setting for <code>ConsistentRead</code> is <code>false</code>.
        /// </para>
        ///  
        /// <para>
        /// The <code>ConsistentRead</code> parameter is not supported on global secondary indexes.
        /// If you scan a global secondary index with <code>ConsistentRead</code> set to true,
        /// you will receive a <code>ValidationException</code>.
        /// </para>
        /// </summary>
        public bool ConsistentRead
        {
            get { return this._consistentRead.GetValueOrDefault(); }
            set { this._consistentRead = value; }
        }

        // Check to see if ConsistentRead property is set
        internal bool IsSetConsistentRead()
        {
            return this._consistentRead.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ExclusiveStartKey. 
        /// <para>
        /// The primary key of the first item that this operation will evaluate. Use the value
        /// that was returned for <code>LastEvaluatedKey</code> in the previous operation.
        /// </para>
        ///  
        /// <para>
        /// The data type for <code>ExclusiveStartKey</code> must be String, Number or Binary.
        /// No set data types are allowed.
        /// </para>
        ///  
        /// <para>
        /// In a parallel scan, a <code>Scan</code> request that includes <code>ExclusiveStartKey</code>
        /// must specify the same segment whose previous <code>Scan</code> returned the corresponding
        /// value of <code>LastEvaluatedKey</code>.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> ExclusiveStartKey
        {
            get { return this._exclusiveStartKey; }
            set { this._exclusiveStartKey = value; }
        }

        // Check to see if ExclusiveStartKey property is set
        internal bool IsSetExclusiveStartKey()
        {
            return this._exclusiveStartKey != null && this._exclusiveStartKey.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ExpressionAttributeNames. 
        /// <para>
        /// One or more substitution tokens for attribute names in an expression. The following
        /// are some use cases for using <code>ExpressionAttributeNames</code>:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// To access an attribute whose name conflicts with a DynamoDB reserved word.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// To create a placeholder for repeating occurrences of an attribute name in an expression.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// To prevent special characters in an attribute name from being misinterpreted in an
        /// expression.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// Use the <b>#</b> character in an expression to dereference an attribute name. For
        /// example, consider the following attribute name:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Percentile</code> 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// The name of this attribute conflicts with a reserved word, so it cannot be used directly
        /// in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved
        /// Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you
        /// could specify the following for <code>ExpressionAttributeNames</code>:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>{"#P":"Percentile"}</code> 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// You could then use this substitution in an expression, as in this example:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>#P = :val</code> 
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>,
        /// which are placeholders for the actual value at runtime.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information on expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Specifying
        /// Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public Dictionary<string, string> ExpressionAttributeNames
        {
            get { return this._expressionAttributeNames; }
            set { this._expressionAttributeNames = value; }
        }

        // Check to see if ExpressionAttributeNames property is set
        internal bool IsSetExpressionAttributeNames()
        {
            return this._expressionAttributeNames != null && this._expressionAttributeNames.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ExpressionAttributeValues. 
        /// <para>
        /// One or more values that can be substituted in an expression.
        /// </para>
        ///  
        /// <para>
        /// Use the <b>:</b> (colon) character in an expression to dereference an attribute value.
        /// For example, suppose that you wanted to check whether the value of the <code>ProductStatus</code>
        /// attribute was one of the following: 
        /// </para>
        ///  
        /// <para>
        ///  <code>Available | Backordered | Discontinued</code> 
        /// </para>
        ///  
        /// <para>
        /// You would first need to specify <code>ExpressionAttributeValues</code> as follows:
        /// </para>
        ///  
        /// <para>
        ///  <code>{ ":avail":{"S":"Available"}, ":back":{"S":"Backordered"}, ":disc":{"S":"Discontinued"}
        /// }</code> 
        /// </para>
        ///  
        /// <para>
        /// You could then use these values in an expression, such as this:
        /// </para>
        ///  
        /// <para>
        ///  <code>ProductStatus IN (:avail, :back, :disc)</code> 
        /// </para>
        ///  
        /// <para>
        /// For more information on expression attribute values, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.SpecifyingConditions.html">Condition
        /// Expressions</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValue> ExpressionAttributeValues
        {
            get { return this._expressionAttributeValues; }
            set { this._expressionAttributeValues = value; }
        }

        // Check to see if ExpressionAttributeValues property is set
        internal bool IsSetExpressionAttributeValues()
        {
            return this._expressionAttributeValues != null && this._expressionAttributeValues.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property FilterExpression. 
        /// <para>
        /// A string that contains conditions that DynamoDB applies after the <code>Scan</code>
        /// operation, but before the data is returned to you. Items that do not satisfy the <code>FilterExpression</code>
        /// criteria are not returned.
        /// </para>
        ///  <note> 
        /// <para>
        /// A <code>FilterExpression</code> is applied after the items have already been read;
        /// the process of filtering does not consume any additional read capacity units.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html#FilteringResults">Filter
        /// Expressions</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public string FilterExpression
        {
            get { return this._filterExpression; }
            set { this._filterExpression = value; }
        }

        // Check to see if FilterExpression property is set
        internal bool IsSetFilterExpression()
        {
            return this._filterExpression != null;
        }

        /// <summary>
        /// Gets and sets the property IndexName. 
        /// <para>
        /// The name of a secondary index to scan. This index can be any local secondary index
        /// or global secondary index. Note that if you use the <code>IndexName</code> parameter,
        /// you must also provide <code>TableName</code>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string IndexName
        {
            get { return this._indexName; }
            set { this._indexName = value; }
        }

        // Check to see if IndexName property is set
        internal bool IsSetIndexName()
        {
            return this._indexName != null;
        }

        /// <summary>
        /// Gets and sets the property Limit. 
        /// <para>
        /// The maximum number of items to evaluate (not necessarily the number of matching items).
        /// If DynamoDB processes the number of items up to the limit while processing the results,
        /// it stops the operation and returns the matching values up to that point, and a key
        /// in <code>LastEvaluatedKey</code> to apply in a subsequent operation, so that you can
        /// pick up where you left off. Also, if the processed dataset size exceeds 1 MB before
        /// DynamoDB reaches this limit, it stops the operation and returns the matching values
        /// up to the limit, and a key in <code>LastEvaluatedKey</code> to apply in a subsequent
        /// operation to continue the operation. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/QueryAndScan.html">Working
        /// with Queries</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1)]
        public int Limit
        {
            get { return this._limit.GetValueOrDefault(); }
            set { this._limit = value; }
        }

        // Check to see if Limit property is set
        internal bool IsSetLimit()
        {
            return this._limit.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property ProjectionExpression. 
        /// <para>
        /// A string that identifies one or more attributes to retrieve from the specified table
        /// or index. These attributes can include scalars, sets, or elements of a JSON document.
        /// The attributes in the expression must be separated by commas.
        /// </para>
        ///  
        /// <para>
        /// If no attribute names are specified, then all attributes will be returned. If any
        /// of the requested attributes are not found, they will not appear in the result.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Specifying
        /// Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public string ProjectionExpression
        {
            get { return this._projectionExpression; }
            set { this._projectionExpression = value; }
        }

        // Check to see if ProjectionExpression property is set
        internal bool IsSetProjectionExpression()
        {
            return this._projectionExpression != null;
        }

        /// <summary>
        /// Gets and sets the property ReturnConsumedCapacity.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity
        {
            get { return this._returnConsumedCapacity; }
            set { this._returnConsumedCapacity = value; }
        }

        // Check to see if ReturnConsumedCapacity property is set
        internal bool IsSetReturnConsumedCapacity()
        {
            return this._returnConsumedCapacity != null;
        }

        /// <summary>
        /// Gets and sets the property ScanFilter. 
        /// <para>
        /// This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ScanFilter.html">ScanFilter</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public Dictionary<string, Condition> ScanFilter
        {
            get { return this._scanFilter; }
            set { this._scanFilter = value; }
        }

        // Check to see if ScanFilter property is set
        internal bool IsSetScanFilter()
        {
            return this._scanFilter != null && this._scanFilter.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property Segment. 
        /// <para>
        /// For a parallel <code>Scan</code> request, <code>Segment</code> identifies an individual
        /// segment to be scanned by an application worker.
        /// </para>
        ///  
        /// <para>
        /// Segment IDs are zero-based, so the first segment is always 0. For example, if you
        /// want to use four application threads to scan a table or an index, then the first thread
        /// specifies a <code>Segment</code> value of 0, the second thread specifies 1, and so
        /// on.
        /// </para>
        ///  
        /// <para>
        /// The value of <code>LastEvaluatedKey</code> returned from a parallel <code>Scan</code>
        /// request must be used as <code>ExclusiveStartKey</code> with the same segment ID in
        /// a subsequent <code>Scan</code> operation.
        /// </para>
        ///  
        /// <para>
        /// The value for <code>Segment</code> must be greater than or equal to 0, and less than
        /// the value provided for <code>TotalSegments</code>.
        /// </para>
        ///  
        /// <para>
        /// If you provide <code>Segment</code>, you must also provide <code>TotalSegments</code>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=0, Max=999999)]
        public int Segment
        {
            get { return this._segment.GetValueOrDefault(); }
            set { this._segment = value; }
        }

        // Check to see if Segment property is set
        internal bool IsSetSegment()
        {
            return this._segment.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property Select. 
        /// <para>
        /// The attributes to be returned in the result. You can retrieve all item attributes,
        /// specific item attributes, the count of matching items, or in the case of an index,
        /// some or all of the attributes projected into the index.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ALL_ATTRIBUTES</code> - Returns all of the item attributes from the specified
        /// table or index. If you query a local secondary index, then for each matching item
        /// in the index, DynamoDB fetches the entire item from the parent table. If the index
        /// is configured to project all item attributes, then all of the data can be obtained
        /// from the local secondary index, and no fetching is required.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL_PROJECTED_ATTRIBUTES</code> - Allowed only when querying an index. Retrieves
        /// all attributes that have been projected into the index. If the index is configured
        /// to project all attributes, this return value is equivalent to specifying <code>ALL_ATTRIBUTES</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>COUNT</code> - Returns the number of matching items, rather than the matching
        /// items themselves.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>SPECIFIC_ATTRIBUTES</code> - Returns only the attributes listed in <code>AttributesToGet</code>.
        /// This return value is equivalent to specifying <code>AttributesToGet</code> without
        /// specifying any value for <code>Select</code>.
        /// </para>
        ///  
        /// <para>
        /// If you query or scan a local secondary index and request only attributes that are
        /// projected into that index, the operation reads only the index and not the table. If
        /// any of the requested attributes are not projected into the local secondary index,
        /// DynamoDB fetches each of these attributes from the parent table. This extra fetching
        /// incurs additional throughput cost and latency.
        /// </para>
        ///  
        /// <para>
        /// If you query or scan a global secondary index, you can only request attributes that
        /// are projected into the index. Global secondary index queries cannot fetch attributes
        /// from the parent table.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// If neither <code>Select</code> nor <code>AttributesToGet</code> are specified, DynamoDB
        /// defaults to <code>ALL_ATTRIBUTES</code> when accessing a table, and <code>ALL_PROJECTED_ATTRIBUTES</code>
        /// when accessing an index. You cannot use both <code>Select</code> and <code>AttributesToGet</code>
        /// together in a single request, unless the value for <code>Select</code> is <code>SPECIFIC_ATTRIBUTES</code>.
        /// (This usage is equivalent to specifying <code>AttributesToGet</code> without any value
        /// for <code>Select</code>.)
        /// </para>
        ///  <note> 
        /// <para>
        /// If you use the <code>ProjectionExpression</code> parameter, then the value for <code>Select</code>
        /// can only be <code>SPECIFIC_ATTRIBUTES</code>. Any other value for <code>Select</code>
        /// will return an error.
        /// </para>
        ///  </note>
        /// </summary>
        public Select Select
        {
            get { return this._select; }
            set { this._select = value; }
        }

        // Check to see if Select property is set
        internal bool IsSetSelect()
        {
            return this._select != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table containing the requested items; or, if you provide <code>IndexName</code>,
        /// the name of the table to which that index belongs.
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
        /// Gets and sets the property TotalSegments. 
        /// <para>
        /// For a parallel <code>Scan</code> request, <code>TotalSegments</code> represents the
        /// total number of segments into which the <code>Scan</code> operation will be divided.
        /// The value of <code>TotalSegments</code> corresponds to the number of application workers
        /// that will perform the parallel scan. For example, if you want to use four application
        /// threads to scan a table or an index, specify a <code>TotalSegments</code> value of
        /// 4.
        /// </para>
        ///  
        /// <para>
        /// The value for <code>TotalSegments</code> must be greater than or equal to 1, and less
        /// than or equal to 1000000. If you specify a <code>TotalSegments</code> value of 1,
        /// the <code>Scan</code> operation will be sequential rather than parallel.
        /// </para>
        ///  
        /// <para>
        /// If you specify <code>TotalSegments</code>, you must also specify <code>Segment</code>.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=1000000)]
        public int TotalSegments
        {
            get { return this._totalSegments.GetValueOrDefault(); }
            set { this._totalSegments = value; }
        }

        // Check to see if TotalSegments property is set
        internal bool IsSetTotalSegments()
        {
            return this._totalSegments.HasValue; 
        }

    }
}