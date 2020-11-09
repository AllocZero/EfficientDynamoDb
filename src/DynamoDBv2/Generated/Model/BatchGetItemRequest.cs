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
    /// Container for the parameters to the BatchGetItem operation.
    /// The <code>BatchGetItem</code> operation returns the attributes of one or more items
    /// from one or more tables. You identify requested items by primary key.
    /// 
    ///  
    /// <para>
    /// A single operation can retrieve up to 16 MB of data, which can contain as many as
    /// 100 items. <code>BatchGetItem</code> returns a partial result if the response size
    /// limit is exceeded, the table's provisioned throughput is exceeded, or an internal
    /// processing failure occurs. If a partial result is returned, the operation returns
    /// a value for <code>UnprocessedKeys</code>. You can use this value to retry the operation
    /// starting with the next item to get.
    /// </para>
    ///  <important> 
    /// <para>
    /// If you request more than 100 items, <code>BatchGetItem</code> returns a <code>ValidationException</code>
    /// with the message "Too many items requested for the BatchGetItem call."
    /// </para>
    ///  </important> 
    /// <para>
    /// For example, if you ask to retrieve 100 items, but each individual item is 300 KB
    /// in size, the system returns 52 items (so as not to exceed the 16 MB limit). It also
    /// returns an appropriate <code>UnprocessedKeys</code> value so you can get the next
    /// page of results. If desired, your application can include its own logic to assemble
    /// the pages of results into one dataset.
    /// </para>
    ///  
    /// <para>
    /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
    /// on all of the tables in the request, then <code>BatchGetItem</code> returns a <code>ProvisionedThroughputExceededException</code>.
    /// If <i>at least one</i> of the items is successfully processed, then <code>BatchGetItem</code>
    /// completes successfully, while returning the keys of the unread items in <code>UnprocessedKeys</code>.
    /// </para>
    ///  <important> 
    /// <para>
    /// If DynamoDB returns any unprocessed items, you should retry the batch operation on
    /// those items. However, <i>we strongly recommend that you use an exponential backoff
    /// algorithm</i>. If you retry the batch operation immediately, the underlying read or
    /// write requests can still fail due to throttling on the individual tables. If you delay
    /// the batch operation using exponential backoff, the individual requests in the batch
    /// are much more likely to succeed.
    /// </para>
    ///  
    /// <para>
    /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#BatchOperations">Batch
    /// Operations and Error Handling</a> in the <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///  </important> 
    /// <para>
    /// By default, <code>BatchGetItem</code> performs eventually consistent reads on every
    /// table in the request. If you want strongly consistent reads instead, you can set <code>ConsistentRead</code>
    /// to <code>true</code> for any or all tables.
    /// </para>
    ///  
    /// <para>
    /// In order to minimize response latency, <code>BatchGetItem</code> retrieves items in
    /// parallel.
    /// </para>
    ///  
    /// <para>
    /// When designing your application, keep in mind that DynamoDB does not return items
    /// in any particular order. To help parse the response by item, include the primary key
    /// values for the items in your request in the <code>ProjectionExpression</code> parameter.
    /// </para>
    ///  
    /// <para>
    /// If a requested item does not exist, it is not returned in the result. Requests for
    /// nonexistent items consume the minimum read capacity units according to the type of
    /// read. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#CapacityUnitCalculations">Working
    /// with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    /// </summary>
    public partial class BatchGetItemRequest : AmazonDynamoDBRequest
    {
        private Dictionary<string, KeysAndAttributes> _requestItems = new Dictionary<string, KeysAndAttributes>();
        private ReturnConsumedCapacity _returnConsumedCapacity;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public BatchGetItemRequest() { }

        /// <summary>
        /// Instantiates BatchGetItemRequest with the parameterized properties
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        public BatchGetItemRequest(Dictionary<string, KeysAndAttributes> requestItems)
        {
            _requestItems = requestItems;
        }

        /// <summary>
        /// Instantiates BatchGetItemRequest with the parameterized properties
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        /// <param name="returnConsumedCapacity">Sets the BatchGetItemRequest ReturnConsumedCapacity property</param>
        public BatchGetItemRequest(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity)
        {
            _requestItems = requestItems;
            _returnConsumedCapacity = returnConsumedCapacity;
        }

        /// <summary>
        /// Gets and sets the property RequestItems. 
        /// <para>
        /// A map of one or more table names and, for each table, a map that describes one or
        /// more items to retrieve from that table. Each table name can be used only once per
        /// <code>BatchGetItem</code> request.
        /// </para>
        ///  
        /// <para>
        /// Each element in the map of items to retrieve consists of the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is
        /// used; if <code>false</code> (the default), an eventually consistent read is used.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute
        /// names in the <code>ProjectionExpression</code> parameter. The following are some use
        /// cases for using <code>ExpressionAttributeNames</code>:
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
        /// For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing
        /// Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Keys</code> - An array of primary key attribute values that define specific
        /// items in the table. For each primary key, you must provide <i>all</i> of the key attributes.
        /// For example, with a simple primary key, you only need to provide the partition key
        /// value. For a composite key, you must provide <i>both</i> the partition key value and
        /// the sort key value.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ProjectionExpression</code> - A string that identifies one or more attributes
        /// to retrieve from the table. These attributes can include scalars, sets, or elements
        /// of a JSON document. The attributes in the expression must be separated by commas.
        /// </para>
        ///  
        /// <para>
        /// If no attribute names are specified, then all attributes are returned. If any of the
        /// requested attributes are not found, they do not appear in the result.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing
        /// Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code>
        /// instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        [AWSProperty(Required=true, Min=1, Max=100)]
        public Dictionary<string, KeysAndAttributes> RequestItems
        {
            get { return this._requestItems; }
            set { this._requestItems = value; }
        }

        // Check to see if RequestItems property is set
        internal bool IsSetRequestItems()
        {
            return this._requestItems != null && this._requestItems.Count > 0; 
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

    }
}