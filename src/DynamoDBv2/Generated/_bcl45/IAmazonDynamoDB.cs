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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Amazon.Runtime;
using Amazon.DynamoDBv2.Model;

namespace Amazon.DynamoDBv2
{
    /// <summary>
    /// Interface for accessing DynamoDB
    ///
    /// Amazon DynamoDB 
    /// <para>
    /// Amazon DynamoDB is a fully managed NoSQL database service that provides fast and predictable
    /// performance with seamless scalability. DynamoDB lets you offload the administrative
    /// burdens of operating and scaling a distributed database, so that you don't have to
    /// worry about hardware provisioning, setup and configuration, replication, software
    /// patching, or cluster scaling.
    /// </para>
    ///  
    /// <para>
    /// With DynamoDB, you can create database tables that can store and retrieve any amount
    /// of data, and serve any level of request traffic. You can scale up or scale down your
    /// tables' throughput capacity without downtime or performance degradation, and use the
    /// AWS Management Console to monitor resource utilization and performance metrics.
    /// </para>
    ///  
    /// <para>
    /// DynamoDB automatically spreads the data and traffic for your tables over a sufficient
    /// number of servers to handle your throughput and storage requirements, while maintaining
    /// consistent and fast performance. All of your data is stored on solid state disks (SSDs)
    /// and automatically replicated across multiple Availability Zones in an AWS region,
    /// providing built-in high availability and data durability. 
    /// </para>
    /// </summary>
    public partial interface IAmazonDynamoDB : IAmazonService, IDisposable
    {

        /// <summary>
        /// Paginators for the service
        /// </summary>
        IDynamoDBv2PaginatorFactory Paginators { get; }

        
        #region  BatchGetItem


        /// <summary>
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
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        /// <param name="returnConsumedCapacity">A property of BatchGetItemRequest used to execute the BatchGetItem service method.</param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        BatchGetItemResponse BatchGetItem(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity);

        /// <summary>
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
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        BatchGetItemResponse BatchGetItem(Dictionary<string, KeysAndAttributes> requestItems);

        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the BatchGetItem service method.</param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        BatchGetItemResponse BatchGetItem(BatchGetItemRequest request);


        /// <summary>
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
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        /// <param name="returnConsumedCapacity">A property of BatchGetItemRequest used to execute the BatchGetItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
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
        /// <param name="requestItems">A map of one or more table names and, for each table, a map that describes one or more items to retrieve from that table. Each table name can be used only once per <code>BatchGetItem</code> request. Each element in the map of items to retrieve consists of the following: <ul> <li>  <code>ConsistentRead</code> - If <code>true</code>, a strongly consistent read is used; if <code>false</code> (the default), an eventually consistent read is used. </li> <li>  <code>ExpressionAttributeNames</code> - One or more substitution tokens for attribute names in the <code>ProjectionExpression</code> parameter. The following are some use cases for using <code>ExpressionAttributeNames</code>: <ul> <li> To access an attribute whose name conflicts with a DynamoDB reserved word. </li> <li> To create a placeholder for repeating occurrences of an attribute name in an expression. </li> <li> To prevent special characters in an attribute name from being misinterpreted in an expression. </li> </ul> Use the <b>#</b> character in an expression to dereference an attribute name. For example, consider the following attribute name: <ul> <li>  <code>Percentile</code>  </li> </ul> The name of this attribute conflicts with a reserved word, so it cannot be used directly in an expression. (For the complete list of reserved words, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ReservedWords.html">Reserved Words</a> in the <i>Amazon DynamoDB Developer Guide</i>). To work around this, you could specify the following for <code>ExpressionAttributeNames</code>: <ul> <li>  <code>{"#P":"Percentile"}</code>  </li> </ul> You could then use this substitution in an expression, as in this example: <ul> <li>  <code>#P = :val</code>  </li> </ul> <note> Tokens that begin with the <b>:</b> character are <i>expression attribute values</i>, which are placeholders for the actual value at runtime. </note> For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>Keys</code> - An array of primary key attribute values that define specific items in the table. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide the partition key value. For a composite key, you must provide <i>both</i> the partition key value and the sort key value. </li> <li>  <code>ProjectionExpression</code> - A string that identifies one or more attributes to retrieve from the table. These attributes can include scalars, sets, or elements of a JSON document. The attributes in the expression must be separated by commas. If no attribute names are specified, then all attributes are returned. If any of the requested attributes are not found, they do not appear in the result. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Accessing Item Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>. </li> <li>  <code>AttributesToGet</code> - This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.  </li> </ul></param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the BatchGetItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the BatchGetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchGetItem">REST API Reference for BatchGetItem Operation</seealso>
        Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  BatchWriteItem


        /// <summary>
        /// The <code>BatchWriteItem</code> operation puts or deletes multiple items in one or
        /// more tables. A single call to <code>BatchWriteItem</code> can write up to 16 MB of
        /// data, which can comprise as many as 25 put or delete requests. Individual items to
        /// be written can be as large as 400 KB.
        /// 
        ///  <note> 
        /// <para>
        ///  <code>BatchWriteItem</code> cannot update items. To update items, use the <code>UpdateItem</code>
        /// action.
        /// </para>
        ///  </note> 
        /// <para>
        /// The individual <code>PutItem</code> and <code>DeleteItem</code> operations specified
        /// in <code>BatchWriteItem</code> are atomic; however <code>BatchWriteItem</code> as
        /// a whole is not. If any requested operations fail because the table's provisioned throughput
        /// is exceeded or an internal processing failure occurs, the failed operations are returned
        /// in the <code>UnprocessedItems</code> response parameter. You can investigate and optionally
        /// resend the requests. Typically, you would call <code>BatchWriteItem</code> in a loop.
        /// Each iteration would check for unprocessed items and submit a new <code>BatchWriteItem</code>
        /// request with those unprocessed items until all items have been processed.
        /// </para>
        ///  
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on all of the tables in the request, then <code>BatchWriteItem</code> returns a <code>ProvisionedThroughputExceededException</code>.
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
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#Programming.Errors.BatchOperations">Batch
        /// Operations and Error Handling</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </important> 
        /// <para>
        /// With <code>BatchWriteItem</code>, you can efficiently write or delete large amounts
        /// of data, such as from Amazon EMR, or copy data from another database into DynamoDB.
        /// In order to improve performance with these large-scale operations, <code>BatchWriteItem</code>
        /// does not behave in the same way as individual <code>PutItem</code> and <code>DeleteItem</code>
        /// calls would. For example, you cannot specify conditions on individual put and delete
        /// requests, and <code>BatchWriteItem</code> does not return deleted items in the response.
        /// </para>
        ///  
        /// <para>
        /// If you use a programming language that supports concurrency, you can use threads to
        /// write items in parallel. Your application must include the necessary logic to manage
        /// the threads. With languages that don't support threading, you must update or delete
        /// the specified items one at a time. In both situations, <code>BatchWriteItem</code>
        /// performs the specified put and delete operations in parallel, giving you the power
        /// of the thread pool approach without having to introduce complexity into your application.
        /// </para>
        ///  
        /// <para>
        /// Parallel processing reduces latency, but each specified put and delete request consumes
        /// the same number of write capacity units whether it is processed in parallel or not.
        /// Delete operations on nonexistent items consume one write capacity unit.
        /// </para>
        ///  
        /// <para>
        /// If one or more of the following is true, DynamoDB rejects the entire batch write operation:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more tables specified in the <code>BatchWriteItem</code> request does not exist.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Primary key attributes specified on an item in the request do not match those in the
        /// corresponding table's primary key schema.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// You try to perform multiple operations on the same item in the same <code>BatchWriteItem</code>
        /// request. For example, you cannot put and delete the same item in the same <code>BatchWriteItem</code>
        /// request. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  Your request contains at least two items with identical hash and range keys (which
        /// essentially is two put operations). 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There are more than 25 requests in the batch.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Any individual item in a batch exceeds 400 KB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The total request size exceeds 16 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, a list of operations to be performed (<code>DeleteRequest</code> or <code>PutRequest</code>). Each element in the map consists of the following: <ul> <li>  <code>DeleteRequest</code> - Perform a <code>DeleteItem</code> operation on the specified item. The item to be deleted is identified by a <code>Key</code> subelement: <ul> <li>  <code>Key</code> - A map of primary key attribute values that uniquely identify the item. Each entry in this map consists of an attribute name and an attribute value. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for <i>both</i> the partition key and the sort key. </li> </ul> </li> <li>  <code>PutRequest</code> - Perform a <code>PutItem</code> operation on the specified item. The item to be put is identified by an <code>Item</code> subelement: <ul> <li>  <code>Item</code> - A map of attributes and their values. Each entry in this map consists of an attribute name and an attribute value. Attribute values must not be null; string and binary type attributes must have lengths greater than zero; and set type attributes must not be empty. Requests that contain empty values are rejected with a <code>ValidationException</code> exception. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. </li> </ul> </li> </ul></param>
        /// 
        /// <returns>The response from the BatchWriteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchWriteItem">REST API Reference for BatchWriteItem Operation</seealso>
        BatchWriteItemResponse BatchWriteItem(Dictionary<string, List<WriteRequest>> requestItems);

        /// <summary>
        /// The <code>BatchWriteItem</code> operation puts or deletes multiple items in one or
        /// more tables. A single call to <code>BatchWriteItem</code> can write up to 16 MB of
        /// data, which can comprise as many as 25 put or delete requests. Individual items to
        /// be written can be as large as 400 KB.
        /// 
        ///  <note> 
        /// <para>
        ///  <code>BatchWriteItem</code> cannot update items. To update items, use the <code>UpdateItem</code>
        /// action.
        /// </para>
        ///  </note> 
        /// <para>
        /// The individual <code>PutItem</code> and <code>DeleteItem</code> operations specified
        /// in <code>BatchWriteItem</code> are atomic; however <code>BatchWriteItem</code> as
        /// a whole is not. If any requested operations fail because the table's provisioned throughput
        /// is exceeded or an internal processing failure occurs, the failed operations are returned
        /// in the <code>UnprocessedItems</code> response parameter. You can investigate and optionally
        /// resend the requests. Typically, you would call <code>BatchWriteItem</code> in a loop.
        /// Each iteration would check for unprocessed items and submit a new <code>BatchWriteItem</code>
        /// request with those unprocessed items until all items have been processed.
        /// </para>
        ///  
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on all of the tables in the request, then <code>BatchWriteItem</code> returns a <code>ProvisionedThroughputExceededException</code>.
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
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#Programming.Errors.BatchOperations">Batch
        /// Operations and Error Handling</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </important> 
        /// <para>
        /// With <code>BatchWriteItem</code>, you can efficiently write or delete large amounts
        /// of data, such as from Amazon EMR, or copy data from another database into DynamoDB.
        /// In order to improve performance with these large-scale operations, <code>BatchWriteItem</code>
        /// does not behave in the same way as individual <code>PutItem</code> and <code>DeleteItem</code>
        /// calls would. For example, you cannot specify conditions on individual put and delete
        /// requests, and <code>BatchWriteItem</code> does not return deleted items in the response.
        /// </para>
        ///  
        /// <para>
        /// If you use a programming language that supports concurrency, you can use threads to
        /// write items in parallel. Your application must include the necessary logic to manage
        /// the threads. With languages that don't support threading, you must update or delete
        /// the specified items one at a time. In both situations, <code>BatchWriteItem</code>
        /// performs the specified put and delete operations in parallel, giving you the power
        /// of the thread pool approach without having to introduce complexity into your application.
        /// </para>
        ///  
        /// <para>
        /// Parallel processing reduces latency, but each specified put and delete request consumes
        /// the same number of write capacity units whether it is processed in parallel or not.
        /// Delete operations on nonexistent items consume one write capacity unit.
        /// </para>
        ///  
        /// <para>
        /// If one or more of the following is true, DynamoDB rejects the entire batch write operation:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more tables specified in the <code>BatchWriteItem</code> request does not exist.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Primary key attributes specified on an item in the request do not match those in the
        /// corresponding table's primary key schema.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// You try to perform multiple operations on the same item in the same <code>BatchWriteItem</code>
        /// request. For example, you cannot put and delete the same item in the same <code>BatchWriteItem</code>
        /// request. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  Your request contains at least two items with identical hash and range keys (which
        /// essentially is two put operations). 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There are more than 25 requests in the batch.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Any individual item in a batch exceeds 400 KB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The total request size exceeds 16 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the BatchWriteItem service method.</param>
        /// 
        /// <returns>The response from the BatchWriteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchWriteItem">REST API Reference for BatchWriteItem Operation</seealso>
        BatchWriteItemResponse BatchWriteItem(BatchWriteItemRequest request);


        /// <summary>
        /// The <code>BatchWriteItem</code> operation puts or deletes multiple items in one or
        /// more tables. A single call to <code>BatchWriteItem</code> can write up to 16 MB of
        /// data, which can comprise as many as 25 put or delete requests. Individual items to
        /// be written can be as large as 400 KB.
        /// 
        ///  <note> 
        /// <para>
        ///  <code>BatchWriteItem</code> cannot update items. To update items, use the <code>UpdateItem</code>
        /// action.
        /// </para>
        ///  </note> 
        /// <para>
        /// The individual <code>PutItem</code> and <code>DeleteItem</code> operations specified
        /// in <code>BatchWriteItem</code> are atomic; however <code>BatchWriteItem</code> as
        /// a whole is not. If any requested operations fail because the table's provisioned throughput
        /// is exceeded or an internal processing failure occurs, the failed operations are returned
        /// in the <code>UnprocessedItems</code> response parameter. You can investigate and optionally
        /// resend the requests. Typically, you would call <code>BatchWriteItem</code> in a loop.
        /// Each iteration would check for unprocessed items and submit a new <code>BatchWriteItem</code>
        /// request with those unprocessed items until all items have been processed.
        /// </para>
        ///  
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on all of the tables in the request, then <code>BatchWriteItem</code> returns a <code>ProvisionedThroughputExceededException</code>.
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
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#Programming.Errors.BatchOperations">Batch
        /// Operations and Error Handling</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </important> 
        /// <para>
        /// With <code>BatchWriteItem</code>, you can efficiently write or delete large amounts
        /// of data, such as from Amazon EMR, or copy data from another database into DynamoDB.
        /// In order to improve performance with these large-scale operations, <code>BatchWriteItem</code>
        /// does not behave in the same way as individual <code>PutItem</code> and <code>DeleteItem</code>
        /// calls would. For example, you cannot specify conditions on individual put and delete
        /// requests, and <code>BatchWriteItem</code> does not return deleted items in the response.
        /// </para>
        ///  
        /// <para>
        /// If you use a programming language that supports concurrency, you can use threads to
        /// write items in parallel. Your application must include the necessary logic to manage
        /// the threads. With languages that don't support threading, you must update or delete
        /// the specified items one at a time. In both situations, <code>BatchWriteItem</code>
        /// performs the specified put and delete operations in parallel, giving you the power
        /// of the thread pool approach without having to introduce complexity into your application.
        /// </para>
        ///  
        /// <para>
        /// Parallel processing reduces latency, but each specified put and delete request consumes
        /// the same number of write capacity units whether it is processed in parallel or not.
        /// Delete operations on nonexistent items consume one write capacity unit.
        /// </para>
        ///  
        /// <para>
        /// If one or more of the following is true, DynamoDB rejects the entire batch write operation:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more tables specified in the <code>BatchWriteItem</code> request does not exist.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Primary key attributes specified on an item in the request do not match those in the
        /// corresponding table's primary key schema.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// You try to perform multiple operations on the same item in the same <code>BatchWriteItem</code>
        /// request. For example, you cannot put and delete the same item in the same <code>BatchWriteItem</code>
        /// request. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  Your request contains at least two items with identical hash and range keys (which
        /// essentially is two put operations). 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There are more than 25 requests in the batch.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Any individual item in a batch exceeds 400 KB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The total request size exceeds 16 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="requestItems">A map of one or more table names and, for each table, a list of operations to be performed (<code>DeleteRequest</code> or <code>PutRequest</code>). Each element in the map consists of the following: <ul> <li>  <code>DeleteRequest</code> - Perform a <code>DeleteItem</code> operation on the specified item. The item to be deleted is identified by a <code>Key</code> subelement: <ul> <li>  <code>Key</code> - A map of primary key attribute values that uniquely identify the item. Each entry in this map consists of an attribute name and an attribute value. For each primary key, you must provide <i>all</i> of the key attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for <i>both</i> the partition key and the sort key. </li> </ul> </li> <li>  <code>PutRequest</code> - Perform a <code>PutItem</code> operation on the specified item. The item to be put is identified by an <code>Item</code> subelement: <ul> <li>  <code>Item</code> - A map of attributes and their values. Each entry in this map consists of an attribute name and an attribute value. Attribute values must not be null; string and binary type attributes must have lengths greater than zero; and set type attributes must not be empty. Requests that contain empty values are rejected with a <code>ValidationException</code> exception. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. </li> </ul> </li> </ul></param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the BatchWriteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchWriteItem">REST API Reference for BatchWriteItem Operation</seealso>
        Task<BatchWriteItemResponse> BatchWriteItemAsync(Dictionary<string, List<WriteRequest>> requestItems, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// The <code>BatchWriteItem</code> operation puts or deletes multiple items in one or
        /// more tables. A single call to <code>BatchWriteItem</code> can write up to 16 MB of
        /// data, which can comprise as many as 25 put or delete requests. Individual items to
        /// be written can be as large as 400 KB.
        /// 
        ///  <note> 
        /// <para>
        ///  <code>BatchWriteItem</code> cannot update items. To update items, use the <code>UpdateItem</code>
        /// action.
        /// </para>
        ///  </note> 
        /// <para>
        /// The individual <code>PutItem</code> and <code>DeleteItem</code> operations specified
        /// in <code>BatchWriteItem</code> are atomic; however <code>BatchWriteItem</code> as
        /// a whole is not. If any requested operations fail because the table's provisioned throughput
        /// is exceeded or an internal processing failure occurs, the failed operations are returned
        /// in the <code>UnprocessedItems</code> response parameter. You can investigate and optionally
        /// resend the requests. Typically, you would call <code>BatchWriteItem</code> in a loop.
        /// Each iteration would check for unprocessed items and submit a new <code>BatchWriteItem</code>
        /// request with those unprocessed items until all items have been processed.
        /// </para>
        ///  
        /// <para>
        /// If <i>none</i> of the items can be processed due to insufficient provisioned throughput
        /// on all of the tables in the request, then <code>BatchWriteItem</code> returns a <code>ProvisionedThroughputExceededException</code>.
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
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#Programming.Errors.BatchOperations">Batch
        /// Operations and Error Handling</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  </important> 
        /// <para>
        /// With <code>BatchWriteItem</code>, you can efficiently write or delete large amounts
        /// of data, such as from Amazon EMR, or copy data from another database into DynamoDB.
        /// In order to improve performance with these large-scale operations, <code>BatchWriteItem</code>
        /// does not behave in the same way as individual <code>PutItem</code> and <code>DeleteItem</code>
        /// calls would. For example, you cannot specify conditions on individual put and delete
        /// requests, and <code>BatchWriteItem</code> does not return deleted items in the response.
        /// </para>
        ///  
        /// <para>
        /// If you use a programming language that supports concurrency, you can use threads to
        /// write items in parallel. Your application must include the necessary logic to manage
        /// the threads. With languages that don't support threading, you must update or delete
        /// the specified items one at a time. In both situations, <code>BatchWriteItem</code>
        /// performs the specified put and delete operations in parallel, giving you the power
        /// of the thread pool approach without having to introduce complexity into your application.
        /// </para>
        ///  
        /// <para>
        /// Parallel processing reduces latency, but each specified put and delete request consumes
        /// the same number of write capacity units whether it is processed in parallel or not.
        /// Delete operations on nonexistent items consume one write capacity unit.
        /// </para>
        ///  
        /// <para>
        /// If one or more of the following is true, DynamoDB rejects the entire batch write operation:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more tables specified in the <code>BatchWriteItem</code> request does not exist.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Primary key attributes specified on an item in the request do not match those in the
        /// corresponding table's primary key schema.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// You try to perform multiple operations on the same item in the same <code>BatchWriteItem</code>
        /// request. For example, you cannot put and delete the same item in the same <code>BatchWriteItem</code>
        /// request. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  Your request contains at least two items with identical hash and range keys (which
        /// essentially is two put operations). 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There are more than 25 requests in the batch.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Any individual item in a batch exceeds 400 KB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The total request size exceeds 16 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the BatchWriteItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the BatchWriteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/BatchWriteItem">REST API Reference for BatchWriteItem Operation</seealso>
        Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  CreateBackup


        /// <summary>
        /// Creates a backup for an existing table.
        /// 
        ///  
        /// <para>
        ///  Each time you create an on-demand backup, the entire table data is backed up. There
        /// is no limit to the number of on-demand backups that can be taken. 
        /// </para>
        ///  
        /// <para>
        ///  When you create an on-demand backup, a time marker of the request is cataloged, and
        /// the backup is created asynchronously, by applying all changes until the time of the
        /// request to the last full table snapshot. Backup requests are processed instantaneously
        /// and become available for restore within minutes. 
        /// </para>
        ///  
        /// <para>
        /// You can call <code>CreateBackup</code> at a maximum rate of 50 times per second.
        /// </para>
        ///  
        /// <para>
        /// All backups in DynamoDB work without consuming any provisioned throughput on the table.
        /// </para>
        ///  
        /// <para>
        ///  If you submit a backup request on 2018-12-14 at 14:25:00, the backup is guaranteed
        /// to contain all data committed to the table up to 14:24:00, and data committed after
        /// 14:26:00 will not be. The backup might contain data modifications made between 14:24:00
        /// and 14:26:00. On-demand backup does not support causal consistency. 
        /// </para>
        ///  
        /// <para>
        ///  Along with data, the following are also included on the backups: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Global secondary indexes (GSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Local secondary indexes (LSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Streams
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Provisioned read and write capacity
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the CreateBackup service method.</param>
        /// 
        /// <returns>The response from the CreateBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ContinuousBackupsUnavailableException">
        /// Backups have not yet been enabled for this table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateBackup">REST API Reference for CreateBackup Operation</seealso>
        CreateBackupResponse CreateBackup(CreateBackupRequest request);



        /// <summary>
        /// Creates a backup for an existing table.
        /// 
        ///  
        /// <para>
        ///  Each time you create an on-demand backup, the entire table data is backed up. There
        /// is no limit to the number of on-demand backups that can be taken. 
        /// </para>
        ///  
        /// <para>
        ///  When you create an on-demand backup, a time marker of the request is cataloged, and
        /// the backup is created asynchronously, by applying all changes until the time of the
        /// request to the last full table snapshot. Backup requests are processed instantaneously
        /// and become available for restore within minutes. 
        /// </para>
        ///  
        /// <para>
        /// You can call <code>CreateBackup</code> at a maximum rate of 50 times per second.
        /// </para>
        ///  
        /// <para>
        /// All backups in DynamoDB work without consuming any provisioned throughput on the table.
        /// </para>
        ///  
        /// <para>
        ///  If you submit a backup request on 2018-12-14 at 14:25:00, the backup is guaranteed
        /// to contain all data committed to the table up to 14:24:00, and data committed after
        /// 14:26:00 will not be. The backup might contain data modifications made between 14:24:00
        /// and 14:26:00. On-demand backup does not support causal consistency. 
        /// </para>
        ///  
        /// <para>
        ///  Along with data, the following are also included on the backups: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Global secondary indexes (GSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Local secondary indexes (LSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Streams
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Provisioned read and write capacity
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the CreateBackup service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the CreateBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ContinuousBackupsUnavailableException">
        /// Backups have not yet been enabled for this table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateBackup">REST API Reference for CreateBackup Operation</seealso>
        Task<CreateBackupResponse> CreateBackupAsync(CreateBackupRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  CreateGlobalTable


        /// <summary>
        /// Creates a global table from an existing table. A global table creates a replication
        /// relationship between two or more DynamoDB tables with the same table name in the provided
        /// Regions. 
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note> 
        /// <para>
        /// If you want to add a new replica table to a global table, each of the following conditions
        /// must be true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The table must have the same primary key as all of the other replicas.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The table must have the same name as all of the other replicas.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The table must have DynamoDB Streams enabled, with the stream containing both the
        /// new and the old images of the item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// None of the replica tables in the global table can contain any data.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  If global secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The global secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  If local secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The local secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The local secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> </ul> <important> 
        /// <para>
        ///  Write capacity settings should be set consistently across your replica tables and
        /// secondary indexes. DynamoDB strongly recommends enabling auto scaling to manage the
        /// write capacity settings for all of your global tables replicas and indexes. 
        /// </para>
        ///  
        /// <para>
        ///  If you prefer to manage write capacity settings manually, you should provision equal
        /// replicated write capacity units to your replica tables. You should also provision
        /// equal replicated write capacity units to matching secondary indexes across your global
        /// table. 
        /// </para>
        ///  </important>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the CreateGlobalTable service method.</param>
        /// 
        /// <returns>The response from the CreateGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableAlreadyExistsException">
        /// The specified global table already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateGlobalTable">REST API Reference for CreateGlobalTable Operation</seealso>
        CreateGlobalTableResponse CreateGlobalTable(CreateGlobalTableRequest request);



        /// <summary>
        /// Creates a global table from an existing table. A global table creates a replication
        /// relationship between two or more DynamoDB tables with the same table name in the provided
        /// Regions. 
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note> 
        /// <para>
        /// If you want to add a new replica table to a global table, each of the following conditions
        /// must be true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The table must have the same primary key as all of the other replicas.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The table must have the same name as all of the other replicas.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The table must have DynamoDB Streams enabled, with the stream containing both the
        /// new and the old images of the item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// None of the replica tables in the global table can contain any data.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  If global secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The global secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  If local secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The local secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The local secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> </ul> <important> 
        /// <para>
        ///  Write capacity settings should be set consistently across your replica tables and
        /// secondary indexes. DynamoDB strongly recommends enabling auto scaling to manage the
        /// write capacity settings for all of your global tables replicas and indexes. 
        /// </para>
        ///  
        /// <para>
        ///  If you prefer to manage write capacity settings manually, you should provision equal
        /// replicated write capacity units to your replica tables. You should also provision
        /// equal replicated write capacity units to matching secondary indexes across your global
        /// table. 
        /// </para>
        ///  </important>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the CreateGlobalTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the CreateGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableAlreadyExistsException">
        /// The specified global table already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateGlobalTable">REST API Reference for CreateGlobalTable Operation</seealso>
        Task<CreateGlobalTableResponse> CreateGlobalTableAsync(CreateGlobalTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  CreateTable


        /// <summary>
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
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="keySchema">Specifies the attributes that make up the primary key for a table or an index. The attributes in <code>KeySchema</code> must also be defined in the <code>AttributeDefinitions</code> array. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data Model</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each <code>KeySchemaElement</code> in the array is composed of: <ul> <li>  <code>AttributeName</code> - The name of this key attribute. </li> <li>  <code>KeyType</code> - The role that the key attribute will assume: <ul> <li>  <code>HASH</code> - partition key </li> <li>  <code>RANGE</code> - sort key </li> </ul> </li> </ul> <note> The partition key of an item is also known as its <i>hash attribute</i>. The term "hash attribute" derives from the DynamoDB usage of an internal hash function to evenly distribute data items across partitions, based on their partition key values. The sort key of an item is also known as its <i>range attribute</i>. The term "range attribute" derives from the way DynamoDB stores items with the same partition key physically close together, in sorted order by the sort key value. </note> For a simple primary key (partition key), you must provide exactly one element with a <code>KeyType</code> of <code>HASH</code>. For a composite primary key (partition key and sort key), you must provide exactly two elements, in this order: The first element must have a <code>KeyType</code> of <code>HASH</code>, and the second element must have a <code>KeyType</code> of <code>RANGE</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Working with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="attributeDefinitions">An array of attributes that describe the key schema for the table and indexes.</param>
        /// <param name="provisionedThroughput">Represents the provisioned throughput settings for a specified table or index. The settings can be modified using the <code>UpdateTable</code> operation.  If you set BillingMode as <code>PROVISIONED</code>, you must specify this property. If you set BillingMode as <code>PAY_PER_REQUEST</code>, you cannot specify this property. For current minimum and maximum provisioned throughput values, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service, Account, and Table Quotas</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// 
        /// <returns>The response from the CreateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateTable">REST API Reference for CreateTable Operation</seealso>
        CreateTableResponse CreateTable(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput);

        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the CreateTable service method.</param>
        /// 
        /// <returns>The response from the CreateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateTable">REST API Reference for CreateTable Operation</seealso>
        CreateTableResponse CreateTable(CreateTableRequest request);


        /// <summary>
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
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="keySchema">Specifies the attributes that make up the primary key for a table or an index. The attributes in <code>KeySchema</code> must also be defined in the <code>AttributeDefinitions</code> array. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DataModel.html">Data Model</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each <code>KeySchemaElement</code> in the array is composed of: <ul> <li>  <code>AttributeName</code> - The name of this key attribute. </li> <li>  <code>KeyType</code> - The role that the key attribute will assume: <ul> <li>  <code>HASH</code> - partition key </li> <li>  <code>RANGE</code> - sort key </li> </ul> </li> </ul> <note> The partition key of an item is also known as its <i>hash attribute</i>. The term "hash attribute" derives from the DynamoDB usage of an internal hash function to evenly distribute data items across partitions, based on their partition key values. The sort key of an item is also known as its <i>range attribute</i>. The term "range attribute" derives from the way DynamoDB stores items with the same partition key physically close together, in sorted order by the sort key value. </note> For a simple primary key (partition key), you must provide exactly one element with a <code>KeyType</code> of <code>HASH</code>. For a composite primary key (partition key and sort key), you must provide exactly two elements, in this order: The first element must have a <code>KeyType</code> of <code>HASH</code>, and the second element must have a <code>KeyType</code> of <code>RANGE</code>. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithTables.html#WorkingWithTables.primary.key">Working with Tables</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="attributeDefinitions">An array of attributes that describe the key schema for the table and indexes.</param>
        /// <param name="provisionedThroughput">Represents the provisioned throughput settings for a specified table or index. The settings can be modified using the <code>UpdateTable</code> operation.  If you set BillingMode as <code>PROVISIONED</code>, you must specify this property. If you set BillingMode as <code>PAY_PER_REQUEST</code>, you cannot specify this property. For current minimum and maximum provisioned throughput values, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service, Account, and Table Quotas</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the CreateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateTable">REST API Reference for CreateTable Operation</seealso>
        Task<CreateTableResponse> CreateTableAsync(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the CreateTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the CreateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/CreateTable">REST API Reference for CreateTable Operation</seealso>
        Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DeleteBackup


        /// <summary>
        /// Deletes an existing backup of a table.
        /// 
        ///  
        /// <para>
        /// You can call <code>DeleteBackup</code> at a maximum rate of 10 times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteBackup service method.</param>
        /// 
        /// <returns>The response from the DeleteBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteBackup">REST API Reference for DeleteBackup Operation</seealso>
        DeleteBackupResponse DeleteBackup(DeleteBackupRequest request);



        /// <summary>
        /// Deletes an existing backup of a table.
        /// 
        ///  
        /// <para>
        /// You can call <code>DeleteBackup</code> at a maximum rate of 10 times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteBackup service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteBackup">REST API Reference for DeleteBackup Operation</seealso>
        Task<DeleteBackupResponse> DeleteBackupAsync(DeleteBackupRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DeleteItem


        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to delete. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        DeleteItemResponse DeleteItem(string tableName, Dictionary<string, AttributeValue> key);

        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to delete. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appeared before they were deleted. For <code>DeleteItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - The content of the old item is returned. </li> </ul> <note> The <code>ReturnValues</code> parameter is used by several DynamoDB operations; however, <code>DeleteItem</code> does not recognize any values other than <code>NONE</code> or <code>ALL_OLD</code>. </note></param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        DeleteItemResponse DeleteItem(string tableName, Dictionary<string, AttributeValue> key, ReturnValue returnValues);

        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteItem service method.</param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        DeleteItemResponse DeleteItem(DeleteItemRequest request);


        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to delete. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table from which to delete the item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to delete. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appeared before they were deleted. For <code>DeleteItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - The content of the old item is returned. </li> </ul> <note> The <code>ReturnValues</code> parameter is used by several DynamoDB operations; however, <code>DeleteItem</code> does not recognize any values other than <code>NONE</code> or <code>ALL_OLD</code>. </note></param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, ReturnValue returnValues, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Deletes a single item in a table by primary key. You can perform a conditional delete
        /// operation that deletes the item if it exists, or if it has an expected attribute value.
        /// 
        ///  
        /// <para>
        /// In addition to deleting an item, you can also return the item's attribute values in
        /// the same operation, using the <code>ReturnValues</code> parameter.
        /// </para>
        ///  
        /// <para>
        /// Unless you specify conditions, the <code>DeleteItem</code> is an idempotent operation;
        /// running it multiple times on the same item or attribute does <i>not</i> result in
        /// an error response.
        /// </para>
        ///  
        /// <para>
        /// Conditional deletes are useful for deleting items only if specific conditions are
        /// met. If those conditions are met, DynamoDB performs the delete. Otherwise, the item
        /// is not deleted.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteItem">REST API Reference for DeleteItem Operation</seealso>
        Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DeleteTable


        /// <summary>
        /// The <code>DeleteTable</code> operation deletes a table and all of its items. After
        /// a <code>DeleteTable</code> request, the specified table is in the <code>DELETING</code>
        /// state until DynamoDB completes the deletion. If the table is in the <code>ACTIVE</code>
        /// state, you can delete it. If a table is in <code>CREATING</code> or <code>UPDATING</code>
        /// states, then DynamoDB returns a <code>ResourceInUseException</code>. If the specified
        /// table does not exist, DynamoDB returns a <code>ResourceNotFoundException</code>. If
        /// table is already in the <code>DELETING</code> state, no error is returned. 
        /// 
        ///  <note> 
        /// <para>
        /// DynamoDB might continue to accept data read and write operations, such as <code>GetItem</code>
        /// and <code>PutItem</code>, on a table in the <code>DELETING</code> state until the
        /// table deletion is complete.
        /// </para>
        ///  </note> 
        /// <para>
        /// When you delete a table, any indexes on that table are also deleted.
        /// </para>
        ///  
        /// <para>
        /// If you have DynamoDB Streams enabled on the table, then the corresponding stream on
        /// that table goes into the <code>DISABLED</code> state, and the stream is automatically
        /// deleted after 24 hours.
        /// </para>
        ///  
        /// <para>
        /// Use the <code>DescribeTable</code> action to check the status of the table. 
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to delete.</param>
        /// 
        /// <returns>The response from the DeleteTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteTable">REST API Reference for DeleteTable Operation</seealso>
        DeleteTableResponse DeleteTable(string tableName);

        /// <summary>
        /// The <code>DeleteTable</code> operation deletes a table and all of its items. After
        /// a <code>DeleteTable</code> request, the specified table is in the <code>DELETING</code>
        /// state until DynamoDB completes the deletion. If the table is in the <code>ACTIVE</code>
        /// state, you can delete it. If a table is in <code>CREATING</code> or <code>UPDATING</code>
        /// states, then DynamoDB returns a <code>ResourceInUseException</code>. If the specified
        /// table does not exist, DynamoDB returns a <code>ResourceNotFoundException</code>. If
        /// table is already in the <code>DELETING</code> state, no error is returned. 
        /// 
        ///  <note> 
        /// <para>
        /// DynamoDB might continue to accept data read and write operations, such as <code>GetItem</code>
        /// and <code>PutItem</code>, on a table in the <code>DELETING</code> state until the
        /// table deletion is complete.
        /// </para>
        ///  </note> 
        /// <para>
        /// When you delete a table, any indexes on that table are also deleted.
        /// </para>
        ///  
        /// <para>
        /// If you have DynamoDB Streams enabled on the table, then the corresponding stream on
        /// that table goes into the <code>DISABLED</code> state, and the stream is automatically
        /// deleted after 24 hours.
        /// </para>
        ///  
        /// <para>
        /// Use the <code>DescribeTable</code> action to check the status of the table. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteTable service method.</param>
        /// 
        /// <returns>The response from the DeleteTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteTable">REST API Reference for DeleteTable Operation</seealso>
        DeleteTableResponse DeleteTable(DeleteTableRequest request);


        /// <summary>
        /// The <code>DeleteTable</code> operation deletes a table and all of its items. After
        /// a <code>DeleteTable</code> request, the specified table is in the <code>DELETING</code>
        /// state until DynamoDB completes the deletion. If the table is in the <code>ACTIVE</code>
        /// state, you can delete it. If a table is in <code>CREATING</code> or <code>UPDATING</code>
        /// states, then DynamoDB returns a <code>ResourceInUseException</code>. If the specified
        /// table does not exist, DynamoDB returns a <code>ResourceNotFoundException</code>. If
        /// table is already in the <code>DELETING</code> state, no error is returned. 
        /// 
        ///  <note> 
        /// <para>
        /// DynamoDB might continue to accept data read and write operations, such as <code>GetItem</code>
        /// and <code>PutItem</code>, on a table in the <code>DELETING</code> state until the
        /// table deletion is complete.
        /// </para>
        ///  </note> 
        /// <para>
        /// When you delete a table, any indexes on that table are also deleted.
        /// </para>
        ///  
        /// <para>
        /// If you have DynamoDB Streams enabled on the table, then the corresponding stream on
        /// that table goes into the <code>DISABLED</code> state, and the stream is automatically
        /// deleted after 24 hours.
        /// </para>
        ///  
        /// <para>
        /// Use the <code>DescribeTable</code> action to check the status of the table. 
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to delete.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteTable">REST API Reference for DeleteTable Operation</seealso>
        Task<DeleteTableResponse> DeleteTableAsync(string tableName, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// The <code>DeleteTable</code> operation deletes a table and all of its items. After
        /// a <code>DeleteTable</code> request, the specified table is in the <code>DELETING</code>
        /// state until DynamoDB completes the deletion. If the table is in the <code>ACTIVE</code>
        /// state, you can delete it. If a table is in <code>CREATING</code> or <code>UPDATING</code>
        /// states, then DynamoDB returns a <code>ResourceInUseException</code>. If the specified
        /// table does not exist, DynamoDB returns a <code>ResourceNotFoundException</code>. If
        /// table is already in the <code>DELETING</code> state, no error is returned. 
        /// 
        ///  <note> 
        /// <para>
        /// DynamoDB might continue to accept data read and write operations, such as <code>GetItem</code>
        /// and <code>PutItem</code>, on a table in the <code>DELETING</code> state until the
        /// table deletion is complete.
        /// </para>
        ///  </note> 
        /// <para>
        /// When you delete a table, any indexes on that table are also deleted.
        /// </para>
        ///  
        /// <para>
        /// If you have DynamoDB Streams enabled on the table, then the corresponding stream on
        /// that table goes into the <code>DISABLED</code> state, and the stream is automatically
        /// deleted after 24 hours.
        /// </para>
        ///  
        /// <para>
        /// Use the <code>DescribeTable</code> action to check the status of the table. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DeleteTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DeleteTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DeleteTable">REST API Reference for DeleteTable Operation</seealso>
        Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeBackup


        /// <summary>
        /// Describes an existing backup of a table.
        /// 
        ///  
        /// <para>
        /// You can call <code>DescribeBackup</code> at a maximum rate of 10 times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeBackup service method.</param>
        /// 
        /// <returns>The response from the DescribeBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeBackup">REST API Reference for DescribeBackup Operation</seealso>
        DescribeBackupResponse DescribeBackup(DescribeBackupRequest request);



        /// <summary>
        /// Describes an existing backup of a table.
        /// 
        ///  
        /// <para>
        /// You can call <code>DescribeBackup</code> at a maximum rate of 10 times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeBackup service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeBackup">REST API Reference for DescribeBackup Operation</seealso>
        Task<DescribeBackupResponse> DescribeBackupAsync(DescribeBackupRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeContinuousBackups


        /// <summary>
        /// Checks the status of continuous backups and point in time recovery on the specified
        /// table. Continuous backups are <code>ENABLED</code> on all tables at table creation.
        /// If point in time recovery is enabled, <code>PointInTimeRecoveryStatus</code> will
        /// be set to ENABLED.
        /// 
        ///  
        /// <para>
        ///  After continuous backups and point in time recovery are enabled, you can restore
        /// to any point in time within <code>EarliestRestorableDateTime</code> and <code>LatestRestorableDateTime</code>.
        /// 
        /// </para>
        ///  
        /// <para>
        ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
        /// You can restore your table to any point in time during the last 35 days. 
        /// </para>
        ///  
        /// <para>
        /// You can call <code>DescribeContinuousBackups</code> at a maximum rate of 10 times
        /// per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeContinuousBackups service method.</param>
        /// 
        /// <returns>The response from the DescribeContinuousBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeContinuousBackups">REST API Reference for DescribeContinuousBackups Operation</seealso>
        DescribeContinuousBackupsResponse DescribeContinuousBackups(DescribeContinuousBackupsRequest request);



        /// <summary>
        /// Checks the status of continuous backups and point in time recovery on the specified
        /// table. Continuous backups are <code>ENABLED</code> on all tables at table creation.
        /// If point in time recovery is enabled, <code>PointInTimeRecoveryStatus</code> will
        /// be set to ENABLED.
        /// 
        ///  
        /// <para>
        ///  After continuous backups and point in time recovery are enabled, you can restore
        /// to any point in time within <code>EarliestRestorableDateTime</code> and <code>LatestRestorableDateTime</code>.
        /// 
        /// </para>
        ///  
        /// <para>
        ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
        /// You can restore your table to any point in time during the last 35 days. 
        /// </para>
        ///  
        /// <para>
        /// You can call <code>DescribeContinuousBackups</code> at a maximum rate of 10 times
        /// per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeContinuousBackups service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeContinuousBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeContinuousBackups">REST API Reference for DescribeContinuousBackups Operation</seealso>
        Task<DescribeContinuousBackupsResponse> DescribeContinuousBackupsAsync(DescribeContinuousBackupsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeContributorInsights


        /// <summary>
        /// Returns information about contributor insights, for a given table or global secondary
        /// index.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeContributorInsights service method.</param>
        /// 
        /// <returns>The response from the DescribeContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeContributorInsights">REST API Reference for DescribeContributorInsights Operation</seealso>
        DescribeContributorInsightsResponse DescribeContributorInsights(DescribeContributorInsightsRequest request);



        /// <summary>
        /// Returns information about contributor insights, for a given table or global secondary
        /// index.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeContributorInsights service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeContributorInsights">REST API Reference for DescribeContributorInsights Operation</seealso>
        Task<DescribeContributorInsightsResponse> DescribeContributorInsightsAsync(DescribeContributorInsightsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeEndpoints


        /// <summary>
        /// Returns the regional endpoint information.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeEndpoints service method.</param>
        /// 
        /// <returns>The response from the DescribeEndpoints service method, as returned by DynamoDB.</returns>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeEndpoints">REST API Reference for DescribeEndpoints Operation</seealso>
        DescribeEndpointsResponse DescribeEndpoints(DescribeEndpointsRequest request);



        /// <summary>
        /// Returns the regional endpoint information.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeEndpoints service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeEndpoints service method, as returned by DynamoDB.</returns>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeEndpoints">REST API Reference for DescribeEndpoints Operation</seealso>
        Task<DescribeEndpointsResponse> DescribeEndpointsAsync(DescribeEndpointsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeGlobalTable


        /// <summary>
        /// Returns information about the specified global table.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables. If you are using global tables <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> you can use <a href="https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_DescribeTable.html">DescribeTable</a>
        /// instead.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeGlobalTable service method.</param>
        /// 
        /// <returns>The response from the DescribeGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeGlobalTable">REST API Reference for DescribeGlobalTable Operation</seealso>
        DescribeGlobalTableResponse DescribeGlobalTable(DescribeGlobalTableRequest request);



        /// <summary>
        /// Returns information about the specified global table.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables. If you are using global tables <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> you can use <a href="https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_DescribeTable.html">DescribeTable</a>
        /// instead.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeGlobalTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeGlobalTable">REST API Reference for DescribeGlobalTable Operation</seealso>
        Task<DescribeGlobalTableResponse> DescribeGlobalTableAsync(DescribeGlobalTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeGlobalTableSettings


        /// <summary>
        /// Describes Region-specific settings for a global table.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeGlobalTableSettings service method.</param>
        /// 
        /// <returns>The response from the DescribeGlobalTableSettings service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeGlobalTableSettings">REST API Reference for DescribeGlobalTableSettings Operation</seealso>
        DescribeGlobalTableSettingsResponse DescribeGlobalTableSettings(DescribeGlobalTableSettingsRequest request);



        /// <summary>
        /// Describes Region-specific settings for a global table.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeGlobalTableSettings service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeGlobalTableSettings service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeGlobalTableSettings">REST API Reference for DescribeGlobalTableSettings Operation</seealso>
        Task<DescribeGlobalTableSettingsResponse> DescribeGlobalTableSettingsAsync(DescribeGlobalTableSettingsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeLimits


        /// <summary>
        /// Returns the current provisioned-capacity quotas for your AWS account in a Region,
        /// both for the Region as a whole and for any one DynamoDB table that you create there.
        /// 
        ///  
        /// <para>
        /// When you establish an AWS account, the account has initial quotas on the maximum read
        /// capacity units and write capacity units that you can provision across all of your
        /// DynamoDB tables in a given Region. Also, there are per-table quotas that apply when
        /// you create a table there. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service,
        /// Account, and Table Quotas</a> page in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  
        /// <para>
        /// Although you can increase these quotas by filing a case at <a href="https://console.aws.amazon.com/support/home#/">AWS
        /// Support Center</a>, obtaining the increase is not instantaneous. The <code>DescribeLimits</code>
        /// action lets you write code to compare the capacity you are currently using to those
        /// quotas imposed by your account so that you have enough time to apply for an increase
        /// before you hit a quota.
        /// </para>
        ///  
        /// <para>
        /// For example, you could use one of the AWS SDKs to do the following:
        /// </para>
        ///  <ol> <li> 
        /// <para>
        /// Call <code>DescribeLimits</code> for a particular Region to obtain your current account
        /// quotas on provisioned capacity there.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a variable to hold the aggregate read capacity units provisioned for all your
        /// tables in that Region, and one to hold the aggregate write capacity units. Zero them
        /// both.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Call <code>ListTables</code> to obtain a list of all your DynamoDB tables.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// For each table name listed by <code>ListTables</code>, do the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Call <code>DescribeTable</code> with the table name.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Use the data returned by <code>DescribeTable</code> to add the read capacity units
        /// and write capacity units provisioned for the table itself to your variables.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// If the table has one or more global secondary indexes (GSIs), loop over these GSIs
        /// and add their provisioned capacity values to your variables as well.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Report the account quotas for that Region returned by <code>DescribeLimits</code>,
        /// along with the total current provisioned capacity levels you have calculated.
        /// </para>
        ///  </li> </ol> 
        /// <para>
        /// This will let you see whether you are getting close to your account-level quotas.
        /// </para>
        ///  
        /// <para>
        /// The per-table quotas apply only when you are creating a new table. They restrict the
        /// sum of the provisioned capacity of the new table itself and all its global secondary
        /// indexes.
        /// </para>
        ///  
        /// <para>
        /// For existing tables and their GSIs, DynamoDB doesn't let you increase provisioned
        /// capacity extremely rapidly, but the only quota that applies is that the aggregate
        /// provisioned capacity over all your tables and GSIs cannot exceed either of the per-account
        /// quotas.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>DescribeLimits</code> should only be called periodically. You can expect throttling
        /// errors if you call it more than once in a minute.
        /// </para>
        ///  </note> 
        /// <para>
        /// The <code>DescribeLimits</code> Request element has no content.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeLimits service method.</param>
        /// 
        /// <returns>The response from the DescribeLimits service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeLimits">REST API Reference for DescribeLimits Operation</seealso>
        DescribeLimitsResponse DescribeLimits(DescribeLimitsRequest request);



        /// <summary>
        /// Returns the current provisioned-capacity quotas for your AWS account in a Region,
        /// both for the Region as a whole and for any one DynamoDB table that you create there.
        /// 
        ///  
        /// <para>
        /// When you establish an AWS account, the account has initial quotas on the maximum read
        /// capacity units and write capacity units that you can provision across all of your
        /// DynamoDB tables in a given Region. Also, there are per-table quotas that apply when
        /// you create a table there. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service,
        /// Account, and Table Quotas</a> page in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        ///  
        /// <para>
        /// Although you can increase these quotas by filing a case at <a href="https://console.aws.amazon.com/support/home#/">AWS
        /// Support Center</a>, obtaining the increase is not instantaneous. The <code>DescribeLimits</code>
        /// action lets you write code to compare the capacity you are currently using to those
        /// quotas imposed by your account so that you have enough time to apply for an increase
        /// before you hit a quota.
        /// </para>
        ///  
        /// <para>
        /// For example, you could use one of the AWS SDKs to do the following:
        /// </para>
        ///  <ol> <li> 
        /// <para>
        /// Call <code>DescribeLimits</code> for a particular Region to obtain your current account
        /// quotas on provisioned capacity there.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a variable to hold the aggregate read capacity units provisioned for all your
        /// tables in that Region, and one to hold the aggregate write capacity units. Zero them
        /// both.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Call <code>ListTables</code> to obtain a list of all your DynamoDB tables.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// For each table name listed by <code>ListTables</code>, do the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Call <code>DescribeTable</code> with the table name.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Use the data returned by <code>DescribeTable</code> to add the read capacity units
        /// and write capacity units provisioned for the table itself to your variables.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// If the table has one or more global secondary indexes (GSIs), loop over these GSIs
        /// and add their provisioned capacity values to your variables as well.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Report the account quotas for that Region returned by <code>DescribeLimits</code>,
        /// along with the total current provisioned capacity levels you have calculated.
        /// </para>
        ///  </li> </ol> 
        /// <para>
        /// This will let you see whether you are getting close to your account-level quotas.
        /// </para>
        ///  
        /// <para>
        /// The per-table quotas apply only when you are creating a new table. They restrict the
        /// sum of the provisioned capacity of the new table itself and all its global secondary
        /// indexes.
        /// </para>
        ///  
        /// <para>
        /// For existing tables and their GSIs, DynamoDB doesn't let you increase provisioned
        /// capacity extremely rapidly, but the only quota that applies is that the aggregate
        /// provisioned capacity over all your tables and GSIs cannot exceed either of the per-account
        /// quotas.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>DescribeLimits</code> should only be called periodically. You can expect throttling
        /// errors if you call it more than once in a minute.
        /// </para>
        ///  </note> 
        /// <para>
        /// The <code>DescribeLimits</code> Request element has no content.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeLimits service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeLimits service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeLimits">REST API Reference for DescribeLimits Operation</seealso>
        Task<DescribeLimitsResponse> DescribeLimitsAsync(DescribeLimitsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeTable


        /// <summary>
        /// Returns information about the table, including the current status of the table, when
        /// it was created, the primary key schema, and any indexes on the table.
        /// 
        ///  <note> 
        /// <para>
        /// If you issue a <code>DescribeTable</code> request immediately after a <code>CreateTable</code>
        /// request, DynamoDB might return a <code>ResourceNotFoundException</code>. This is because
        /// <code>DescribeTable</code> uses an eventually consistent query, and the metadata for
        /// your table might not be available at that moment. Wait for a few seconds, and then
        /// try the <code>DescribeTable</code> request again.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="tableName">The name of the table to describe.</param>
        /// 
        /// <returns>The response from the DescribeTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTable">REST API Reference for DescribeTable Operation</seealso>
        DescribeTableResponse DescribeTable(string tableName);

        /// <summary>
        /// Returns information about the table, including the current status of the table, when
        /// it was created, the primary key schema, and any indexes on the table.
        /// 
        ///  <note> 
        /// <para>
        /// If you issue a <code>DescribeTable</code> request immediately after a <code>CreateTable</code>
        /// request, DynamoDB might return a <code>ResourceNotFoundException</code>. This is because
        /// <code>DescribeTable</code> uses an eventually consistent query, and the metadata for
        /// your table might not be available at that moment. Wait for a few seconds, and then
        /// try the <code>DescribeTable</code> request again.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTable service method.</param>
        /// 
        /// <returns>The response from the DescribeTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTable">REST API Reference for DescribeTable Operation</seealso>
        DescribeTableResponse DescribeTable(DescribeTableRequest request);


        /// <summary>
        /// Returns information about the table, including the current status of the table, when
        /// it was created, the primary key schema, and any indexes on the table.
        /// 
        ///  <note> 
        /// <para>
        /// If you issue a <code>DescribeTable</code> request immediately after a <code>CreateTable</code>
        /// request, DynamoDB might return a <code>ResourceNotFoundException</code>. This is because
        /// <code>DescribeTable</code> uses an eventually consistent query, and the metadata for
        /// your table might not be available at that moment. Wait for a few seconds, and then
        /// try the <code>DescribeTable</code> request again.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="tableName">The name of the table to describe.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTable">REST API Reference for DescribeTable Operation</seealso>
        Task<DescribeTableResponse> DescribeTableAsync(string tableName, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Returns information about the table, including the current status of the table, when
        /// it was created, the primary key schema, and any indexes on the table.
        /// 
        ///  <note> 
        /// <para>
        /// If you issue a <code>DescribeTable</code> request immediately after a <code>CreateTable</code>
        /// request, DynamoDB might return a <code>ResourceNotFoundException</code>. This is because
        /// <code>DescribeTable</code> uses an eventually consistent query, and the metadata for
        /// your table might not be available at that moment. Wait for a few seconds, and then
        /// try the <code>DescribeTable</code> request again.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTable">REST API Reference for DescribeTable Operation</seealso>
        Task<DescribeTableResponse> DescribeTableAsync(DescribeTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeTableReplicaAutoScaling


        /// <summary>
        /// Describes auto scaling settings across replicas of the global table at once.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTableReplicaAutoScaling service method.</param>
        /// 
        /// <returns>The response from the DescribeTableReplicaAutoScaling service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTableReplicaAutoScaling">REST API Reference for DescribeTableReplicaAutoScaling Operation</seealso>
        DescribeTableReplicaAutoScalingResponse DescribeTableReplicaAutoScaling(DescribeTableReplicaAutoScalingRequest request);



        /// <summary>
        /// Describes auto scaling settings across replicas of the global table at once.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTableReplicaAutoScaling service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeTableReplicaAutoScaling service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTableReplicaAutoScaling">REST API Reference for DescribeTableReplicaAutoScaling Operation</seealso>
        Task<DescribeTableReplicaAutoScalingResponse> DescribeTableReplicaAutoScalingAsync(DescribeTableReplicaAutoScalingRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  DescribeTimeToLive


        /// <summary>
        /// Gives a description of the Time to Live (TTL) status on the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to be described.</param>
        /// 
        /// <returns>The response from the DescribeTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTimeToLive">REST API Reference for DescribeTimeToLive Operation</seealso>
        DescribeTimeToLiveResponse DescribeTimeToLive(string tableName);

        /// <summary>
        /// Gives a description of the Time to Live (TTL) status on the specified table.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTimeToLive service method.</param>
        /// 
        /// <returns>The response from the DescribeTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTimeToLive">REST API Reference for DescribeTimeToLive Operation</seealso>
        DescribeTimeToLiveResponse DescribeTimeToLive(DescribeTimeToLiveRequest request);


        /// <summary>
        /// Gives a description of the Time to Live (TTL) status on the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to be described.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTimeToLive">REST API Reference for DescribeTimeToLive Operation</seealso>
        Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(string tableName, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Gives a description of the Time to Live (TTL) status on the specified table.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeTimeToLive service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/DescribeTimeToLive">REST API Reference for DescribeTimeToLive Operation</seealso>
        Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(DescribeTimeToLiveRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  GetItem


        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to retrieve. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        GetItemResponse GetItem(string tableName, Dictionary<string, AttributeValue> key);

        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to retrieve. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="consistentRead">Determines the read consistency model: If set to <code>true</code>, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads.</param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        GetItemResponse GetItem(string tableName, Dictionary<string, AttributeValue> key, bool consistentRead);

        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetItem service method.</param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        GetItemResponse GetItem(GetItemRequest request);


        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to retrieve. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the requested item.</param>
        /// <param name="key">A map of attribute names to <code>AttributeValue</code> objects, representing the primary key of the item to retrieve. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="consistentRead">Determines the read consistency model: If set to <code>true</code>, then the operation uses strongly consistent reads; otherwise, the operation uses eventually consistent reads.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, bool consistentRead, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// The <code>GetItem</code> operation returns a set of attributes for the item with the
        /// given primary key. If there is no matching item, <code>GetItem</code> does not return
        /// any data and there will be no <code>Item</code> element in the response.
        /// 
        ///  
        /// <para>
        ///  <code>GetItem</code> provides an eventually consistent read by default. If your application
        /// requires a strongly consistent read, set <code>ConsistentRead</code> to <code>true</code>.
        /// Although a strongly consistent read might take more time than an eventually consistent
        /// read, it always returns the last updated value.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/GetItem">REST API Reference for GetItem Operation</seealso>
        Task<GetItemResponse> GetItemAsync(GetItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  ListBackups


        /// <summary>
        /// List backups associated with an AWS account. To list backups for a given table, specify
        /// <code>TableName</code>. <code>ListBackups</code> returns a paginated list of results
        /// with at most 1 MB worth of items in a page. You can also specify a maximum number
        /// of entries to be returned in a page. 
        /// 
        ///  
        /// <para>
        /// In the request, start time is inclusive, but end time is exclusive. Note that these
        /// boundaries are for the time at which the original backup was requested.
        /// </para>
        ///  
        /// <para>
        /// You can call <code>ListBackups</code> a maximum of five times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListBackups service method.</param>
        /// 
        /// <returns>The response from the ListBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListBackups">REST API Reference for ListBackups Operation</seealso>
        ListBackupsResponse ListBackups(ListBackupsRequest request);



        /// <summary>
        /// List backups associated with an AWS account. To list backups for a given table, specify
        /// <code>TableName</code>. <code>ListBackups</code> returns a paginated list of results
        /// with at most 1 MB worth of items in a page. You can also specify a maximum number
        /// of entries to be returned in a page. 
        /// 
        ///  
        /// <para>
        /// In the request, start time is inclusive, but end time is exclusive. Note that these
        /// boundaries are for the time at which the original backup was requested.
        /// </para>
        ///  
        /// <para>
        /// You can call <code>ListBackups</code> a maximum of five times per second.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListBackups service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListBackups">REST API Reference for ListBackups Operation</seealso>
        Task<ListBackupsResponse> ListBackupsAsync(ListBackupsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  ListContributorInsights


        /// <summary>
        /// Returns a list of ContributorInsightsSummary for a table and all its global secondary
        /// indexes.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListContributorInsights service method.</param>
        /// 
        /// <returns>The response from the ListContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListContributorInsights">REST API Reference for ListContributorInsights Operation</seealso>
        ListContributorInsightsResponse ListContributorInsights(ListContributorInsightsRequest request);



        /// <summary>
        /// Returns a list of ContributorInsightsSummary for a table and all its global secondary
        /// indexes.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListContributorInsights service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListContributorInsights">REST API Reference for ListContributorInsights Operation</seealso>
        Task<ListContributorInsightsResponse> ListContributorInsightsAsync(ListContributorInsightsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  ListGlobalTables


        /// <summary>
        /// Lists all global tables that have a replica in the specified Region.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListGlobalTables service method.</param>
        /// 
        /// <returns>The response from the ListGlobalTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListGlobalTables">REST API Reference for ListGlobalTables Operation</seealso>
        ListGlobalTablesResponse ListGlobalTables(ListGlobalTablesRequest request);



        /// <summary>
        /// Lists all global tables that have a replica in the specified Region.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V1.html">Version
        /// 2017.11.29</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListGlobalTables service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListGlobalTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListGlobalTables">REST API Reference for ListGlobalTables Operation</seealso>
        Task<ListGlobalTablesResponse> ListGlobalTablesAsync(ListGlobalTablesRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  ListTables


        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        ListTablesResponse ListTables();


        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        ListTablesResponse ListTables(string exclusiveStartTableName);

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        /// <param name="limit">A maximum number of table names to return. If this parameter is not specified, the limit is 100.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        ListTablesResponse ListTables(string exclusiveStartTableName, int limit);

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="limit">A maximum number of table names to return. If this parameter is not specified, the limit is 100.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        ListTablesResponse ListTables(int limit);

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListTables service method.</param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        ListTablesResponse ListTables(ListTablesRequest request);


        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        Task<ListTablesResponse> ListTablesAsync(System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="exclusiveStartTableName">The first table name that this operation will evaluate. Use the value that was returned for <code>LastEvaluatedTableName</code> in a previous operation, so that you can obtain the next page of results.</param>
        /// <param name="limit">A maximum number of table names to return. If this parameter is not specified, the limit is 100.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, int limit, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="limit">A maximum number of table names to return. If this parameter is not specified, the limit is 100.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        Task<ListTablesResponse> ListTablesAsync(int limit, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Returns an array of table names associated with the current account and endpoint.
        /// The output from <code>ListTables</code> is paginated, with each page returning a maximum
        /// of 100 table names.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListTables service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTables service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTables">REST API Reference for ListTables Operation</seealso>
        Task<ListTablesResponse> ListTablesAsync(ListTablesRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  ListTagsOfResource


        /// <summary>
        /// List all tags on an Amazon DynamoDB resource. You can call ListTagsOfResource up to
        /// 10 times per second, per account.
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListTagsOfResource service method.</param>
        /// 
        /// <returns>The response from the ListTagsOfResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTagsOfResource">REST API Reference for ListTagsOfResource Operation</seealso>
        ListTagsOfResourceResponse ListTagsOfResource(ListTagsOfResourceRequest request);



        /// <summary>
        /// List all tags on an Amazon DynamoDB resource. You can call ListTagsOfResource up to
        /// 10 times per second, per account.
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListTagsOfResource service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListTagsOfResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/ListTagsOfResource">REST API Reference for ListTagsOfResource Operation</seealso>
        Task<ListTagsOfResourceResponse> ListTagsOfResourceAsync(ListTagsOfResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  PutItem


        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index. For more information about primary keys, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.CoreComponents.html#HowItWorks.CoreComponents.PrimaryKey">Primary Key</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each element in the <code>Item</code> map is an <code>AttributeValue</code> object.</param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        PutItemResponse PutItem(string tableName, Dictionary<string, AttributeValue> item);

        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index. For more information about primary keys, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.CoreComponents.html#HowItWorks.CoreComponents.PrimaryKey">Primary Key</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each element in the <code>Item</code> map is an <code>AttributeValue</code> object.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appeared before they were updated with the <code>PutItem</code> request. For <code>PutItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - If <code>PutItem</code> overwrote an attribute name-value pair, then the content of the old item is returned. </li> </ul> <note> The <code>ReturnValues</code> parameter is used by several DynamoDB operations; however, <code>PutItem</code> does not recognize any values other than <code>NONE</code> or <code>ALL_OLD</code>. </note></param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        PutItemResponse PutItem(string tableName, Dictionary<string, AttributeValue> item, ReturnValue returnValues);

        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the PutItem service method.</param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        PutItemResponse PutItem(PutItemRequest request);


        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index. For more information about primary keys, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.CoreComponents.html#HowItWorks.CoreComponents.PrimaryKey">Primary Key</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each element in the <code>Item</code> map is an <code>AttributeValue</code> object.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to contain the item.</param>
        /// <param name="item">A map of attribute name/value pairs, one for each attribute. Only the primary key attributes are required; you can optionally provide other attribute name-value pairs for the item. You must provide all of the attributes for the primary key. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide both values for both the partition key and the sort key. If you specify any attributes that are part of an index key, then the data types for those attributes must match those of the schema in the table's attribute definition. Empty String and Binary attribute values are allowed. Attribute values of type String and Binary must have a length greater than zero if the attribute is used as a key attribute for a table or index. For more information about primary keys, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.CoreComponents.html#HowItWorks.CoreComponents.PrimaryKey">Primary Key</a> in the <i>Amazon DynamoDB Developer Guide</i>. Each element in the <code>Item</code> map is an <code>AttributeValue</code> object.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appeared before they were updated with the <code>PutItem</code> request. For <code>PutItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - If <code>PutItem</code> overwrote an attribute name-value pair, then the content of the old item is returned. </li> </ul> <note> The <code>ReturnValues</code> parameter is used by several DynamoDB operations; however, <code>PutItem</code> does not recognize any values other than <code>NONE</code> or <code>ALL_OLD</code>. </note></param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, ReturnValue returnValues, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Creates a new item, or replaces an old item with a new item. If an item that has the
        /// same primary key as the new item already exists in the specified table, the new item
        /// completely replaces the existing item. You can perform a conditional put operation
        /// (add a new item if one with the specified primary key doesn't exist), or replace an
        /// existing item if it has certain attribute values. You can return the item's attribute
        /// values in the same operation, using the <code>ReturnValues</code> parameter.
        /// 
        ///  <important> 
        /// <para>
        /// This topic provides general information about the <code>PutItem</code> API.
        /// </para>
        ///  
        /// <para>
        /// For information on how to call the <code>PutItem</code> API using the AWS SDK in specific
        /// languages, see the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/aws-cli/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS Command Line Interface</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/DotNetSDKV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for .NET</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForCpp/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for C++</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForGoV1/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Go</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForJava/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Java</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/AWSJavaScriptSDK/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for JavaScript</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForPHPV3/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for PHP V3</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/boto3/dynamodb-2012-08-10/PutItem"> PutItem
        /// in the AWS SDK for Python</a> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <a href="http://docs.aws.amazon.com/goto/SdkForRubyV2/dynamodb-2012-08-10/PutItem">
        /// PutItem in the AWS SDK for Ruby V2</a> 
        /// </para>
        ///  </li> </ul> </important> 
        /// <para>
        /// When you add an item, the primary key attributes are the only required attributes.
        /// Attribute values cannot be null.
        /// </para>
        ///  
        /// <para>
        /// Empty String and Binary attribute values are allowed. Attribute values of type String
        /// and Binary must have a length greater than zero if the attribute is used as a key
        /// attribute for a table or index. Set type attributes cannot be empty. 
        /// </para>
        ///  
        /// <para>
        /// Invalid Requests with empty values will be rejected with a <code>ValidationException</code>
        /// exception.
        /// </para>
        ///  <note> 
        /// <para>
        /// To prevent a new item from replacing an existing item, use a conditional expression
        /// that contains the <code>attribute_not_exists</code> function with the name of the
        /// attribute being used as the partition key for the table. Since every record must contain
        /// that attribute, the <code>attribute_not_exists</code> function will only succeed if
        /// no matching item exists.
        /// </para>
        ///  </note> 
        /// <para>
        /// For more information about <code>PutItem</code>, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/WorkingWithItems.html">Working
        /// with Items</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the PutItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the PutItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/PutItem">REST API Reference for PutItem Operation</seealso>
        Task<PutItemResponse> PutItemAsync(PutItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  Query


        /// <summary>
        /// The <code>Query</code> operation finds items based on primary key values. You can
        /// query any table or secondary index that has a composite primary key (a partition key
        /// and a sort key). 
        /// 
        ///  
        /// <para>
        /// Use the <code>KeyConditionExpression</code> parameter to provide a specific value
        /// for the partition key. The <code>Query</code> operation will return all of the items
        /// from the table or index with that partition key value. You can optionally narrow the
        /// scope of the <code>Query</code> operation by specifying a sort key value and a comparison
        /// operator in <code>KeyConditionExpression</code>. To further refine the <code>Query</code>
        /// results, you can optionally provide a <code>FilterExpression</code>. A <code>FilterExpression</code>
        /// determines which items within the results should be returned to you. All of the other
        /// results are discarded. 
        /// </para>
        ///  
        /// <para>
        ///  A <code>Query</code> operation always returns a result set. If no matching items
        /// are found, the result set will be empty. Queries that do not return results consume
        /// the minimum number of read capacity units for that type of read operation. 
        /// </para>
        ///  <note> 
        /// <para>
        ///  DynamoDB calculates the number of read capacity units consumed based on item size,
        /// not on the amount of data that is returned to an application. The number of capacity
        /// units consumed will be the same whether you request all of the attributes (the default
        /// behavior) or just some of them (using a projection expression). The number will also
        /// be the same whether or not you use a <code>FilterExpression</code>. 
        /// </para>
        ///  </note> 
        /// <para>
        ///  <code>Query</code> results are always sorted by the sort key value. If the data type
        /// of the sort key is Number, the results are returned in numeric order; otherwise, the
        /// results are returned in order of UTF-8 bytes. By default, the sort order is ascending.
        /// To reverse the order, set the <code>ScanIndexForward</code> parameter to false. 
        /// </para>
        ///  
        /// <para>
        ///  A single <code>Query</code> operation will read up to the maximum number of items
        /// set (if using the <code>Limit</code> parameter) or a maximum of 1 MB of data and then
        /// apply any filtering to the results using <code>FilterExpression</code>. If <code>LastEvaluatedKey</code>
        /// is present in the response, you will need to paginate the result set. For more information,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html#Query.Pagination">Paginating
        /// the Results</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        ///  
        /// <para>
        ///  <code>FilterExpression</code> is applied after a <code>Query</code> finishes, but
        /// before the results are returned. A <code>FilterExpression</code> cannot contain partition
        /// key or sort key attributes. You need to specify those attributes in the <code>KeyConditionExpression</code>.
        /// 
        /// </para>
        ///  <note> 
        /// <para>
        ///  A <code>Query</code> operation can return an empty result set and a <code>LastEvaluatedKey</code>
        /// if all the items read for the page of results are filtered out. 
        /// </para>
        ///  </note> 
        /// <para>
        /// You can query a table, a local secondary index, or a global secondary index. For a
        /// query on a table or on a local secondary index, you can set the <code>ConsistentRead</code>
        /// parameter to <code>true</code> and obtain a strongly consistent result. Global secondary
        /// indexes support eventually consistent reads only, so do not specify <code>ConsistentRead</code>
        /// when querying a global secondary index.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the Query service method.</param>
        /// 
        /// <returns>The response from the Query service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Query">REST API Reference for Query Operation</seealso>
        QueryResponse Query(QueryRequest request);



        /// <summary>
        /// The <code>Query</code> operation finds items based on primary key values. You can
        /// query any table or secondary index that has a composite primary key (a partition key
        /// and a sort key). 
        /// 
        ///  
        /// <para>
        /// Use the <code>KeyConditionExpression</code> parameter to provide a specific value
        /// for the partition key. The <code>Query</code> operation will return all of the items
        /// from the table or index with that partition key value. You can optionally narrow the
        /// scope of the <code>Query</code> operation by specifying a sort key value and a comparison
        /// operator in <code>KeyConditionExpression</code>. To further refine the <code>Query</code>
        /// results, you can optionally provide a <code>FilterExpression</code>. A <code>FilterExpression</code>
        /// determines which items within the results should be returned to you. All of the other
        /// results are discarded. 
        /// </para>
        ///  
        /// <para>
        ///  A <code>Query</code> operation always returns a result set. If no matching items
        /// are found, the result set will be empty. Queries that do not return results consume
        /// the minimum number of read capacity units for that type of read operation. 
        /// </para>
        ///  <note> 
        /// <para>
        ///  DynamoDB calculates the number of read capacity units consumed based on item size,
        /// not on the amount of data that is returned to an application. The number of capacity
        /// units consumed will be the same whether you request all of the attributes (the default
        /// behavior) or just some of them (using a projection expression). The number will also
        /// be the same whether or not you use a <code>FilterExpression</code>. 
        /// </para>
        ///  </note> 
        /// <para>
        ///  <code>Query</code> results are always sorted by the sort key value. If the data type
        /// of the sort key is Number, the results are returned in numeric order; otherwise, the
        /// results are returned in order of UTF-8 bytes. By default, the sort order is ascending.
        /// To reverse the order, set the <code>ScanIndexForward</code> parameter to false. 
        /// </para>
        ///  
        /// <para>
        ///  A single <code>Query</code> operation will read up to the maximum number of items
        /// set (if using the <code>Limit</code> parameter) or a maximum of 1 MB of data and then
        /// apply any filtering to the results using <code>FilterExpression</code>. If <code>LastEvaluatedKey</code>
        /// is present in the response, you will need to paginate the result set. For more information,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html#Query.Pagination">Paginating
        /// the Results</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        ///  
        /// <para>
        ///  <code>FilterExpression</code> is applied after a <code>Query</code> finishes, but
        /// before the results are returned. A <code>FilterExpression</code> cannot contain partition
        /// key or sort key attributes. You need to specify those attributes in the <code>KeyConditionExpression</code>.
        /// 
        /// </para>
        ///  <note> 
        /// <para>
        ///  A <code>Query</code> operation can return an empty result set and a <code>LastEvaluatedKey</code>
        /// if all the items read for the page of results are filtered out. 
        /// </para>
        ///  </note> 
        /// <para>
        /// You can query a table, a local secondary index, or a global secondary index. For a
        /// query on a table or on a local secondary index, you can set the <code>ConsistentRead</code>
        /// parameter to <code>true</code> and obtain a strongly consistent result. Global secondary
        /// indexes support eventually consistent reads only, so do not specify <code>ConsistentRead</code>
        /// when querying a global secondary index.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the Query service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the Query service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Query">REST API Reference for Query Operation</seealso>
        Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  RestoreTableFromBackup


        /// <summary>
        /// Creates a new table from an existing backup. Any number of users can execute up to
        /// 4 concurrent restores (any type of restore) in a given account. 
        /// 
        ///  
        /// <para>
        /// You can call <code>RestoreTableFromBackup</code> at a maximum rate of 10 times per
        /// second.
        /// </para>
        ///  
        /// <para>
        /// You must manually set up the following on the restored table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Auto scaling policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// IAM policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Amazon CloudWatch metrics and alarms
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Tags
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Stream settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Time to Live (TTL) settings
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the RestoreTableFromBackup service method.</param>
        /// 
        /// <returns>The response from the RestoreTableFromBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableAlreadyExistsException">
        /// A target table with the specified name already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/RestoreTableFromBackup">REST API Reference for RestoreTableFromBackup Operation</seealso>
        RestoreTableFromBackupResponse RestoreTableFromBackup(RestoreTableFromBackupRequest request);



        /// <summary>
        /// Creates a new table from an existing backup. Any number of users can execute up to
        /// 4 concurrent restores (any type of restore) in a given account. 
        /// 
        ///  
        /// <para>
        /// You can call <code>RestoreTableFromBackup</code> at a maximum rate of 10 times per
        /// second.
        /// </para>
        ///  
        /// <para>
        /// You must manually set up the following on the restored table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Auto scaling policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// IAM policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Amazon CloudWatch metrics and alarms
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Tags
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Stream settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Time to Live (TTL) settings
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the RestoreTableFromBackup service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the RestoreTableFromBackup service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupInUseException">
        /// There is another ongoing conflicting backup control plane operation on the table.
        /// The backup is either being created, deleted or restored to a table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.BackupNotFoundException">
        /// Backup not found for the given BackupARN.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableAlreadyExistsException">
        /// A target table with the specified name already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/RestoreTableFromBackup">REST API Reference for RestoreTableFromBackup Operation</seealso>
        Task<RestoreTableFromBackupResponse> RestoreTableFromBackupAsync(RestoreTableFromBackupRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  RestoreTableToPointInTime


        /// <summary>
        /// Restores the specified table to the specified point in time within <code>EarliestRestorableDateTime</code>
        /// and <code>LatestRestorableDateTime</code>. You can restore your table to any point
        /// in time during the last 35 days. Any number of users can execute up to 4 concurrent
        /// restores (any type of restore) in a given account. 
        /// 
        ///  
        /// <para>
        ///  When you restore using point in time recovery, DynamoDB restores your table data
        /// to the state based on the selected date and time (day:hour:minute:second) to a new
        /// table. 
        /// </para>
        ///  
        /// <para>
        ///  Along with data, the following are also included on the new restored table using
        /// point in time recovery: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Global secondary indexes (GSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Local secondary indexes (LSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Provisioned read and write capacity
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Encryption settings
        /// </para>
        ///  <important> 
        /// <para>
        ///  All these settings come from the current settings of the source table at the time
        /// of restore. 
        /// </para>
        ///  </important> </li> </ul> 
        /// <para>
        /// You must manually set up the following on the restored table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Auto scaling policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// IAM policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Amazon CloudWatch metrics and alarms
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Tags
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Stream settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Time to Live (TTL) settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Point in time recovery settings
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the RestoreTableToPointInTime service method.</param>
        /// 
        /// <returns>The response from the RestoreTableToPointInTime service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InvalidRestoreTimeException">
        /// An invalid restore time was specified. RestoreDateTime must be between EarliestRestorableDateTime
        /// and LatestRestorableDateTime.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.PointInTimeRecoveryUnavailableException">
        /// Point in time recovery has not yet been enabled for this source table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableAlreadyExistsException">
        /// A target table with the specified name already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/RestoreTableToPointInTime">REST API Reference for RestoreTableToPointInTime Operation</seealso>
        RestoreTableToPointInTimeResponse RestoreTableToPointInTime(RestoreTableToPointInTimeRequest request);



        /// <summary>
        /// Restores the specified table to the specified point in time within <code>EarliestRestorableDateTime</code>
        /// and <code>LatestRestorableDateTime</code>. You can restore your table to any point
        /// in time during the last 35 days. Any number of users can execute up to 4 concurrent
        /// restores (any type of restore) in a given account. 
        /// 
        ///  
        /// <para>
        ///  When you restore using point in time recovery, DynamoDB restores your table data
        /// to the state based on the selected date and time (day:hour:minute:second) to a new
        /// table. 
        /// </para>
        ///  
        /// <para>
        ///  Along with data, the following are also included on the new restored table using
        /// point in time recovery: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Global secondary indexes (GSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Local secondary indexes (LSIs)
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Provisioned read and write capacity
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Encryption settings
        /// </para>
        ///  <important> 
        /// <para>
        ///  All these settings come from the current settings of the source table at the time
        /// of restore. 
        /// </para>
        ///  </important> </li> </ul> 
        /// <para>
        /// You must manually set up the following on the restored table:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Auto scaling policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// IAM policies
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Amazon CloudWatch metrics and alarms
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Tags
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Stream settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Time to Live (TTL) settings
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Point in time recovery settings
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the RestoreTableToPointInTime service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the RestoreTableToPointInTime service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InvalidRestoreTimeException">
        /// An invalid restore time was specified. RestoreDateTime must be between EarliestRestorableDateTime
        /// and LatestRestorableDateTime.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.PointInTimeRecoveryUnavailableException">
        /// Point in time recovery has not yet been enabled for this source table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableAlreadyExistsException">
        /// A target table with the specified name already exists.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableInUseException">
        /// A target table with the specified name is either being created or deleted.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/RestoreTableToPointInTime">REST API Reference for RestoreTableToPointInTime Operation</seealso>
        Task<RestoreTableToPointInTimeResponse> RestoreTableToPointInTimeAsync(RestoreTableToPointInTimeRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  Scan


        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="attributesToGet">This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        ScanResponse Scan(string tableName, List<string> attributesToGet);

        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="scanFilter">This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ScanFilter.html">ScanFilter</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        ScanResponse Scan(string tableName, Dictionary<string, Condition> scanFilter);

        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="attributesToGet">This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="scanFilter">This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ScanFilter.html">ScanFilter</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        ScanResponse Scan(string tableName, List<string> attributesToGet, Dictionary<string, Condition> scanFilter);

        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the Scan service method.</param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        ScanResponse Scan(ScanRequest request);


        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="attributesToGet">This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="scanFilter">This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ScanFilter.html">ScanFilter</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        Task<ScanResponse> ScanAsync(string tableName, Dictionary<string, Condition> scanFilter, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
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
        /// <param name="tableName">The name of the table containing the requested items; or, if you provide <code>IndexName</code>, the name of the table to which that index belongs.</param>
        /// <param name="attributesToGet">This is a legacy parameter. Use <code>ProjectionExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributesToGet.html">AttributesToGet</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="scanFilter">This is a legacy parameter. Use <code>FilterExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ScanFilter.html">ScanFilter</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, Dictionary<string, Condition> scanFilter, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
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
        /// <param name="request">Container for the necessary parameters to execute the Scan service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the Scan service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/Scan">REST API Reference for Scan Operation</seealso>
        Task<ScanResponse> ScanAsync(ScanRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  TagResource


        /// <summary>
        /// Associate a set of tags with an Amazon DynamoDB resource. You can then activate these
        /// user-defined tags so that they appear on the Billing and Cost Management console for
        /// cost allocation tracking. You can call TagResource up to five times per second, per
        /// account. 
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TagResource service method.</param>
        /// 
        /// <returns>The response from the TagResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TagResource">REST API Reference for TagResource Operation</seealso>
        TagResourceResponse TagResource(TagResourceRequest request);



        /// <summary>
        /// Associate a set of tags with an Amazon DynamoDB resource. You can then activate these
        /// user-defined tags so that they appear on the Billing and Cost Management console for
        /// cost allocation tracking. You can call TagResource up to five times per second, per
        /// account. 
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TagResource service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the TagResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TagResource">REST API Reference for TagResource Operation</seealso>
        Task<TagResourceResponse> TagResourceAsync(TagResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  TransactGetItems


        /// <summary>
        /// <code>TransactGetItems</code> is a synchronous operation that atomically retrieves
        /// multiple items from one or more tables (but not from indexes) in a single account
        /// and Region. A <code>TransactGetItems</code> call can contain up to 25 <code>TransactGetItem</code>
        /// objects, each of which contains a <code>Get</code> structure that specifies an item
        /// to retrieve from a table in the account and Region. A call to <code>TransactGetItems</code>
        /// cannot retrieve items from tables in more than one AWS account or Region. The aggregate
        /// size of the items in the transaction cannot exceed 4 MB.
        /// 
        ///  
        /// <para>
        /// DynamoDB rejects the entire <code>TransactGetItems</code> request if any of the following
        /// is true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A conflicting operation is in the process of updating an item to be read.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The aggregate size of the items in the transaction cannot exceed 4 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TransactGetItems service method.</param>
        /// 
        /// <returns>The response from the TransactGetItems service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionCanceledException">
        /// The entire transaction request was canceled.
        /// 
        ///  
        /// <para>
        /// DynamoDB cancels a <code>TransactWriteItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactWriteItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// More than one action in the <code>TransactWriteItems</code> operation targets the
        /// same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (larger than 400 KB), or a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB cancels a <code>TransactGetItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// There is an ongoing <code>TransactGetItems</code> operation that conflicts with a
        /// concurrent <code>PutItem</code>, <code>UpdateItem</code>, <code>DeleteItem</code>
        /// or <code>TransactWriteItems</code> request. In this case the <code>TransactGetItems</code>
        /// operation fails with a <code>TransactionCanceledException</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactGetItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// If using Java, DynamoDB lists the cancellation reasons on the <code>CancellationReasons</code>
        /// property. This property is not set for other languages. Transaction cancellation reasons
        /// are ordered in the order of requested items, if an item has no error it will have
        /// <code>NONE</code> code and <code>Null</code> message.
        /// </para>
        ///  </note> 
        /// <para>
        /// Cancellation reason codes and possible error messages:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// No Errors:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>NONE</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: <code>null</code> 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Conditional Check Failed:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ConditionalCheckFailed</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: The conditional request failed. 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Item Collection Size Limit Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ItemCollectionSizeLimitExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Collection size exceeded.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Transaction Conflict:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>TransactionConflict</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Transaction is ongoing for the item.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Provisioned Throughput Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ProvisionedThroughputExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The level of configured provisioned throughput for the table was exceeded. Consider
        /// increasing your provisioning level with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This Message is received when provisioned throughput is exceeded is on a provisioned
        /// DynamoDB table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// The level of configured provisioned throughput for one or more global secondary indexes
        /// of the table was exceeded. Consider increasing your provisioning level for the under-provisioned
        /// global secondary indexes with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when provisioned throughput is exceeded is on a provisioned
        /// GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Throttling Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ThrottlingError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Throughput exceeds the current capacity of your table or index. DynamoDB is automatically
        /// scaling your table or index so please try again shortly. If exceptions persist, check
        /// if you have a hot key: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-partition-key-design.html.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when writes get throttled on an On-Demand table as DynamoDB
        /// is automatically scaling the table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// Throughput exceeds the current capacity for one or more global secondary indexes.
        /// DynamoDB is automatically scaling your index so please try again shortly.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when when writes get throttled on an On-Demand GSI as DynamoDB
        /// is automatically scaling the GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Validation Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ValidationError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more parameter values were invalid.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key beyond allowed size
        /// limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key to unsupported type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An operand in the update expression has an incorrect data type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Item size to update has exceeded the maximum allowed size.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Number overflow. Attempting to store a number with magnitude larger than supported
        /// range.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Type mismatch for attribute to update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Nesting Levels have exceeded supported limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The document path provided in the update expression is invalid for update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The provided expression refers to an attribute that does not exist in the item.
        /// </para>
        ///  </li> </ul> </li> </ul> </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TransactGetItems">REST API Reference for TransactGetItems Operation</seealso>
        TransactGetItemsResponse TransactGetItems(TransactGetItemsRequest request);



        /// <summary>
        /// <code>TransactGetItems</code> is a synchronous operation that atomically retrieves
        /// multiple items from one or more tables (but not from indexes) in a single account
        /// and Region. A <code>TransactGetItems</code> call can contain up to 25 <code>TransactGetItem</code>
        /// objects, each of which contains a <code>Get</code> structure that specifies an item
        /// to retrieve from a table in the account and Region. A call to <code>TransactGetItems</code>
        /// cannot retrieve items from tables in more than one AWS account or Region. The aggregate
        /// size of the items in the transaction cannot exceed 4 MB.
        /// 
        ///  
        /// <para>
        /// DynamoDB rejects the entire <code>TransactGetItems</code> request if any of the following
        /// is true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A conflicting operation is in the process of updating an item to be read.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The aggregate size of the items in the transaction cannot exceed 4 MB.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TransactGetItems service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the TransactGetItems service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionCanceledException">
        /// The entire transaction request was canceled.
        /// 
        ///  
        /// <para>
        /// DynamoDB cancels a <code>TransactWriteItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactWriteItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// More than one action in the <code>TransactWriteItems</code> operation targets the
        /// same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (larger than 400 KB), or a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB cancels a <code>TransactGetItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// There is an ongoing <code>TransactGetItems</code> operation that conflicts with a
        /// concurrent <code>PutItem</code>, <code>UpdateItem</code>, <code>DeleteItem</code>
        /// or <code>TransactWriteItems</code> request. In this case the <code>TransactGetItems</code>
        /// operation fails with a <code>TransactionCanceledException</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactGetItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// If using Java, DynamoDB lists the cancellation reasons on the <code>CancellationReasons</code>
        /// property. This property is not set for other languages. Transaction cancellation reasons
        /// are ordered in the order of requested items, if an item has no error it will have
        /// <code>NONE</code> code and <code>Null</code> message.
        /// </para>
        ///  </note> 
        /// <para>
        /// Cancellation reason codes and possible error messages:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// No Errors:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>NONE</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: <code>null</code> 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Conditional Check Failed:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ConditionalCheckFailed</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: The conditional request failed. 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Item Collection Size Limit Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ItemCollectionSizeLimitExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Collection size exceeded.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Transaction Conflict:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>TransactionConflict</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Transaction is ongoing for the item.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Provisioned Throughput Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ProvisionedThroughputExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The level of configured provisioned throughput for the table was exceeded. Consider
        /// increasing your provisioning level with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This Message is received when provisioned throughput is exceeded is on a provisioned
        /// DynamoDB table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// The level of configured provisioned throughput for one or more global secondary indexes
        /// of the table was exceeded. Consider increasing your provisioning level for the under-provisioned
        /// global secondary indexes with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when provisioned throughput is exceeded is on a provisioned
        /// GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Throttling Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ThrottlingError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Throughput exceeds the current capacity of your table or index. DynamoDB is automatically
        /// scaling your table or index so please try again shortly. If exceptions persist, check
        /// if you have a hot key: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-partition-key-design.html.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when writes get throttled on an On-Demand table as DynamoDB
        /// is automatically scaling the table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// Throughput exceeds the current capacity for one or more global secondary indexes.
        /// DynamoDB is automatically scaling your index so please try again shortly.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when when writes get throttled on an On-Demand GSI as DynamoDB
        /// is automatically scaling the GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Validation Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ValidationError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more parameter values were invalid.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key beyond allowed size
        /// limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key to unsupported type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An operand in the update expression has an incorrect data type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Item size to update has exceeded the maximum allowed size.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Number overflow. Attempting to store a number with magnitude larger than supported
        /// range.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Type mismatch for attribute to update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Nesting Levels have exceeded supported limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The document path provided in the update expression is invalid for update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The provided expression refers to an attribute that does not exist in the item.
        /// </para>
        ///  </li> </ul> </li> </ul> </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TransactGetItems">REST API Reference for TransactGetItems Operation</seealso>
        Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  TransactWriteItems


        /// <summary>
        /// <code>TransactWriteItems</code> is a synchronous write operation that groups up to
        /// 25 action requests. These actions can target items in different tables, but not in
        /// different AWS accounts or Regions, and no two actions can target the same item. For
        /// example, you cannot both <code>ConditionCheck</code> and <code>Update</code> the same
        /// item. The aggregate size of the items in the transaction cannot exceed 4 MB.
        /// 
        ///  
        /// <para>
        /// The actions are completed atomically so that either all of them succeed, or all of
        /// them fail. They are defined by the following objects:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Put</code>  &#x97;   Initiates a <code>PutItem</code> operation to write a
        /// new item. This structure specifies the primary key of the item to be written, the
        /// name of the table to write it in, an optional condition expression that must be satisfied
        /// for the write to succeed, a list of the item's attributes, and a field indicating
        /// whether to retrieve the item's attributes if the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Update</code>  &#x97;   Initiates an <code>UpdateItem</code> operation to update
        /// an existing item. This structure specifies the primary key of the item to be updated,
        /// the name of the table where it resides, an optional condition expression that must
        /// be satisfied for the update to succeed, an expression that defines one or more attributes
        /// to be updated, and a field indicating whether to retrieve the item's attributes if
        /// the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Delete</code>  &#x97;   Initiates a <code>DeleteItem</code> operation to delete
        /// an existing item. This structure specifies the primary key of the item to be deleted,
        /// the name of the table where it resides, an optional condition expression that must
        /// be satisfied for the deletion to succeed, and a field indicating whether to retrieve
        /// the item's attributes if the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ConditionCheck</code>  &#x97;   Applies a condition to an item that is not
        /// being modified by the transaction. This structure specifies the primary key of the
        /// item to be checked, the name of the table where it resides, a condition expression
        /// that must be satisfied for the transaction to succeed, and a field indicating whether
        /// to retrieve the item's attributes if the condition is not met.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB rejects the entire <code>TransactWriteItems</code> request if any of the
        /// following is true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An ongoing operation is in the process of updating the same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (bigger than 400 KB), a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The aggregate size of the items in the transaction exceeds 4 MB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TransactWriteItems service method.</param>
        /// 
        /// <returns>The response from the TransactWriteItems service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.IdempotentParameterMismatchException">
        /// DynamoDB rejected the request because you retried a request with a different payload
        /// but with an idempotent token that was already used.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionCanceledException">
        /// The entire transaction request was canceled.
        /// 
        ///  
        /// <para>
        /// DynamoDB cancels a <code>TransactWriteItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactWriteItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// More than one action in the <code>TransactWriteItems</code> operation targets the
        /// same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (larger than 400 KB), or a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB cancels a <code>TransactGetItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// There is an ongoing <code>TransactGetItems</code> operation that conflicts with a
        /// concurrent <code>PutItem</code>, <code>UpdateItem</code>, <code>DeleteItem</code>
        /// or <code>TransactWriteItems</code> request. In this case the <code>TransactGetItems</code>
        /// operation fails with a <code>TransactionCanceledException</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactGetItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// If using Java, DynamoDB lists the cancellation reasons on the <code>CancellationReasons</code>
        /// property. This property is not set for other languages. Transaction cancellation reasons
        /// are ordered in the order of requested items, if an item has no error it will have
        /// <code>NONE</code> code and <code>Null</code> message.
        /// </para>
        ///  </note> 
        /// <para>
        /// Cancellation reason codes and possible error messages:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// No Errors:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>NONE</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: <code>null</code> 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Conditional Check Failed:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ConditionalCheckFailed</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: The conditional request failed. 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Item Collection Size Limit Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ItemCollectionSizeLimitExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Collection size exceeded.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Transaction Conflict:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>TransactionConflict</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Transaction is ongoing for the item.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Provisioned Throughput Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ProvisionedThroughputExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The level of configured provisioned throughput for the table was exceeded. Consider
        /// increasing your provisioning level with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This Message is received when provisioned throughput is exceeded is on a provisioned
        /// DynamoDB table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// The level of configured provisioned throughput for one or more global secondary indexes
        /// of the table was exceeded. Consider increasing your provisioning level for the under-provisioned
        /// global secondary indexes with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when provisioned throughput is exceeded is on a provisioned
        /// GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Throttling Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ThrottlingError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Throughput exceeds the current capacity of your table or index. DynamoDB is automatically
        /// scaling your table or index so please try again shortly. If exceptions persist, check
        /// if you have a hot key: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-partition-key-design.html.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when writes get throttled on an On-Demand table as DynamoDB
        /// is automatically scaling the table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// Throughput exceeds the current capacity for one or more global secondary indexes.
        /// DynamoDB is automatically scaling your index so please try again shortly.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when when writes get throttled on an On-Demand GSI as DynamoDB
        /// is automatically scaling the GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Validation Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ValidationError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more parameter values were invalid.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key beyond allowed size
        /// limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key to unsupported type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An operand in the update expression has an incorrect data type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Item size to update has exceeded the maximum allowed size.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Number overflow. Attempting to store a number with magnitude larger than supported
        /// range.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Type mismatch for attribute to update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Nesting Levels have exceeded supported limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The document path provided in the update expression is invalid for update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The provided expression refers to an attribute that does not exist in the item.
        /// </para>
        ///  </li> </ul> </li> </ul> </li> </ul>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionInProgressException">
        /// The transaction with the given request token is already in progress.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TransactWriteItems">REST API Reference for TransactWriteItems Operation</seealso>
        TransactWriteItemsResponse TransactWriteItems(TransactWriteItemsRequest request);



        /// <summary>
        /// <code>TransactWriteItems</code> is a synchronous write operation that groups up to
        /// 25 action requests. These actions can target items in different tables, but not in
        /// different AWS accounts or Regions, and no two actions can target the same item. For
        /// example, you cannot both <code>ConditionCheck</code> and <code>Update</code> the same
        /// item. The aggregate size of the items in the transaction cannot exceed 4 MB.
        /// 
        ///  
        /// <para>
        /// The actions are completed atomically so that either all of them succeed, or all of
        /// them fail. They are defined by the following objects:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>Put</code>  &#x97;   Initiates a <code>PutItem</code> operation to write a
        /// new item. This structure specifies the primary key of the item to be written, the
        /// name of the table to write it in, an optional condition expression that must be satisfied
        /// for the write to succeed, a list of the item's attributes, and a field indicating
        /// whether to retrieve the item's attributes if the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Update</code>  &#x97;   Initiates an <code>UpdateItem</code> operation to update
        /// an existing item. This structure specifies the primary key of the item to be updated,
        /// the name of the table where it resides, an optional condition expression that must
        /// be satisfied for the update to succeed, an expression that defines one or more attributes
        /// to be updated, and a field indicating whether to retrieve the item's attributes if
        /// the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>Delete</code>  &#x97;   Initiates a <code>DeleteItem</code> operation to delete
        /// an existing item. This structure specifies the primary key of the item to be deleted,
        /// the name of the table where it resides, an optional condition expression that must
        /// be satisfied for the deletion to succeed, and a field indicating whether to retrieve
        /// the item's attributes if the condition is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ConditionCheck</code>  &#x97;   Applies a condition to an item that is not
        /// being modified by the transaction. This structure specifies the primary key of the
        /// item to be checked, the name of the table where it resides, a condition expression
        /// that must be satisfied for the transaction to succeed, and a field indicating whether
        /// to retrieve the item's attributes if the condition is not met.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB rejects the entire <code>TransactWriteItems</code> request if any of the
        /// following is true:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An ongoing operation is in the process of updating the same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (bigger than 400 KB), a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The aggregate size of the items in the transaction exceeds 4 MB.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the TransactWriteItems service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the TransactWriteItems service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.IdempotentParameterMismatchException">
        /// DynamoDB rejected the request because you retried a request with a different payload
        /// but with an idempotent token that was already used.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionCanceledException">
        /// The entire transaction request was canceled.
        /// 
        ///  
        /// <para>
        /// DynamoDB cancels a <code>TransactWriteItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// A condition in one of the condition expressions is not met.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactWriteItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// More than one action in the <code>TransactWriteItems</code> operation targets the
        /// same item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An item size becomes too large (larger than 400 KB), or a local secondary index (LSI)
        /// becomes too large, or a similar validation error occurs because of changes made by
        /// the transaction.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// DynamoDB cancels a <code>TransactGetItems</code> request under the following circumstances:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// There is an ongoing <code>TransactGetItems</code> operation that conflicts with a
        /// concurrent <code>PutItem</code>, <code>UpdateItem</code>, <code>DeleteItem</code>
        /// or <code>TransactWriteItems</code> request. In this case the <code>TransactGetItems</code>
        /// operation fails with a <code>TransactionCanceledException</code>.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// A table in the <code>TransactGetItems</code> request is in a different account or
        /// region.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is insufficient provisioned capacity for the transaction to be completed.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// There is a user error, such as an invalid data format.
        /// </para>
        ///  </li> </ul> <note> 
        /// <para>
        /// If using Java, DynamoDB lists the cancellation reasons on the <code>CancellationReasons</code>
        /// property. This property is not set for other languages. Transaction cancellation reasons
        /// are ordered in the order of requested items, if an item has no error it will have
        /// <code>NONE</code> code and <code>Null</code> message.
        /// </para>
        ///  </note> 
        /// <para>
        /// Cancellation reason codes and possible error messages:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// No Errors:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>NONE</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: <code>null</code> 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Conditional Check Failed:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ConditionalCheckFailed</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: The conditional request failed. 
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Item Collection Size Limit Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ItemCollectionSizeLimitExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Collection size exceeded.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Transaction Conflict:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>TransactionConflict</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Message: Transaction is ongoing for the item.
        /// </para>
        ///  </li> </ul> </li> <li> 
        /// <para>
        /// Provisioned Throughput Exceeded:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ProvisionedThroughputExceeded</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// The level of configured provisioned throughput for the table was exceeded. Consider
        /// increasing your provisioning level with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This Message is received when provisioned throughput is exceeded is on a provisioned
        /// DynamoDB table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// The level of configured provisioned throughput for one or more global secondary indexes
        /// of the table was exceeded. Consider increasing your provisioning level for the under-provisioned
        /// global secondary indexes with the UpdateTable API.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when provisioned throughput is exceeded is on a provisioned
        /// GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Throttling Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ThrottlingError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Throughput exceeds the current capacity of your table or index. DynamoDB is automatically
        /// scaling your table or index so please try again shortly. If exceptions persist, check
        /// if you have a hot key: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-partition-key-design.html.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when writes get throttled on an On-Demand table as DynamoDB
        /// is automatically scaling the table.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// Throughput exceeds the current capacity for one or more global secondary indexes.
        /// DynamoDB is automatically scaling your index so please try again shortly.
        /// </para>
        ///  <note> 
        /// <para>
        /// This message is returned when when writes get throttled on an On-Demand GSI as DynamoDB
        /// is automatically scaling the GSI.
        /// </para>
        ///  </note> </li> </ul> </li> </ul> </li> <li> 
        /// <para>
        /// Validation Error:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Code: <code>ValidationError</code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Messages: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// One or more parameter values were invalid.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key beyond allowed size
        /// limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The update expression attempted to update the secondary index key to unsupported type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// An operand in the update expression has an incorrect data type.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Item size to update has exceeded the maximum allowed size.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Number overflow. Attempting to store a number with magnitude larger than supported
        /// range.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Type mismatch for attribute to update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Nesting Levels have exceeded supported limits.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The document path provided in the update expression is invalid for update.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// The provided expression refers to an attribute that does not exist in the item.
        /// </para>
        ///  </li> </ul> </li> </ul> </li> </ul>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionInProgressException">
        /// The transaction with the given request token is already in progress.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/TransactWriteItems">REST API Reference for TransactWriteItems Operation</seealso>
        Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UntagResource


        /// <summary>
        /// Removes the association of tags from an Amazon DynamoDB resource. You can call <code>UntagResource</code>
        /// up to five times per second, per account. 
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UntagResource service method.</param>
        /// 
        /// <returns>The response from the UntagResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UntagResource">REST API Reference for UntagResource Operation</seealso>
        UntagResourceResponse UntagResource(UntagResourceRequest request);



        /// <summary>
        /// Removes the association of tags from an Amazon DynamoDB resource. You can call <code>UntagResource</code>
        /// up to five times per second, per account. 
        /// 
        ///  
        /// <para>
        /// For an overview on tagging DynamoDB resources, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tagging.html">Tagging
        /// for DynamoDB</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UntagResource service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UntagResource service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UntagResource">REST API Reference for UntagResource Operation</seealso>
        Task<UntagResourceResponse> UntagResourceAsync(UntagResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateContinuousBackups


        /// <summary>
        /// <code>UpdateContinuousBackups</code> enables or disables point in time recovery for
        /// the specified table. A successful <code>UpdateContinuousBackups</code> call returns
        /// the current <code>ContinuousBackupsDescription</code>. Continuous backups are <code>ENABLED</code>
        /// on all tables at table creation. If point in time recovery is enabled, <code>PointInTimeRecoveryStatus</code>
        /// will be set to ENABLED.
        /// 
        ///  
        /// <para>
        ///  Once continuous backups and point in time recovery are enabled, you can restore to
        /// any point in time within <code>EarliestRestorableDateTime</code> and <code>LatestRestorableDateTime</code>.
        /// 
        /// </para>
        ///  
        /// <para>
        ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
        /// You can restore your table to any point in time during the last 35 days. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateContinuousBackups service method.</param>
        /// 
        /// <returns>The response from the UpdateContinuousBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ContinuousBackupsUnavailableException">
        /// Backups have not yet been enabled for this table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateContinuousBackups">REST API Reference for UpdateContinuousBackups Operation</seealso>
        UpdateContinuousBackupsResponse UpdateContinuousBackups(UpdateContinuousBackupsRequest request);



        /// <summary>
        /// <code>UpdateContinuousBackups</code> enables or disables point in time recovery for
        /// the specified table. A successful <code>UpdateContinuousBackups</code> call returns
        /// the current <code>ContinuousBackupsDescription</code>. Continuous backups are <code>ENABLED</code>
        /// on all tables at table creation. If point in time recovery is enabled, <code>PointInTimeRecoveryStatus</code>
        /// will be set to ENABLED.
        /// 
        ///  
        /// <para>
        ///  Once continuous backups and point in time recovery are enabled, you can restore to
        /// any point in time within <code>EarliestRestorableDateTime</code> and <code>LatestRestorableDateTime</code>.
        /// 
        /// </para>
        ///  
        /// <para>
        ///  <code>LatestRestorableDateTime</code> is typically 5 minutes before the current time.
        /// You can restore your table to any point in time during the last 35 days. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateContinuousBackups service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateContinuousBackups service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ContinuousBackupsUnavailableException">
        /// Backups have not yet been enabled for this table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateContinuousBackups">REST API Reference for UpdateContinuousBackups Operation</seealso>
        Task<UpdateContinuousBackupsResponse> UpdateContinuousBackupsAsync(UpdateContinuousBackupsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateContributorInsights


        /// <summary>
        /// Updates the status for contributor insights for a specific table or index.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateContributorInsights service method.</param>
        /// 
        /// <returns>The response from the UpdateContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateContributorInsights">REST API Reference for UpdateContributorInsights Operation</seealso>
        UpdateContributorInsightsResponse UpdateContributorInsights(UpdateContributorInsightsRequest request);



        /// <summary>
        /// Updates the status for contributor insights for a specific table or index.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateContributorInsights service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateContributorInsights service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateContributorInsights">REST API Reference for UpdateContributorInsights Operation</seealso>
        Task<UpdateContributorInsightsResponse> UpdateContributorInsightsAsync(UpdateContributorInsightsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateGlobalTable


        /// <summary>
        /// Adds or removes replicas in the specified global table. The global table must already
        /// exist to be able to use this operation. Any replica to be added must be empty, have
        /// the same name as the global table, have the same key schema, have DynamoDB Streams
        /// enabled, and have the same provisioned and maximum write capacity units.
        /// 
        ///  <note> 
        /// <para>
        /// Although you can use <code>UpdateGlobalTable</code> to add replicas and remove replicas
        /// in a single request, for simplicity we recommend that you issue separate requests
        /// for adding or removing replicas.
        /// </para>
        ///  </note> 
        /// <para>
        ///  If global secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The global secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same provisioned and maximum write capacity
        /// units. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateGlobalTable service method.</param>
        /// 
        /// <returns>The response from the UpdateGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaAlreadyExistsException">
        /// The specified replica is already part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaNotFoundException">
        /// The specified replica is no longer part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateGlobalTable">REST API Reference for UpdateGlobalTable Operation</seealso>
        UpdateGlobalTableResponse UpdateGlobalTable(UpdateGlobalTableRequest request);



        /// <summary>
        /// Adds or removes replicas in the specified global table. The global table must already
        /// exist to be able to use this operation. Any replica to be added must be empty, have
        /// the same name as the global table, have the same key schema, have DynamoDB Streams
        /// enabled, and have the same provisioned and maximum write capacity units.
        /// 
        ///  <note> 
        /// <para>
        /// Although you can use <code>UpdateGlobalTable</code> to add replicas and remove replicas
        /// in a single request, for simplicity we recommend that you issue separate requests
        /// for adding or removing replicas.
        /// </para>
        ///  </note> 
        /// <para>
        ///  If global secondary indexes are specified, then the following conditions must also
        /// be met: 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  The global secondary indexes must have the same name. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same hash key and sort key (if present).
        /// 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  The global secondary indexes must have the same provisioned and maximum write capacity
        /// units. 
        /// </para>
        ///  </li> </ul>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateGlobalTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateGlobalTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaAlreadyExistsException">
        /// The specified replica is already part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaNotFoundException">
        /// The specified replica is no longer part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TableNotFoundException">
        /// A source table with the name <code>TableName</code> does not currently exist within
        /// the subscriber's account.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateGlobalTable">REST API Reference for UpdateGlobalTable Operation</seealso>
        Task<UpdateGlobalTableResponse> UpdateGlobalTableAsync(UpdateGlobalTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateGlobalTableSettings


        /// <summary>
        /// Updates settings for a global table.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateGlobalTableSettings service method.</param>
        /// 
        /// <returns>The response from the UpdateGlobalTableSettings service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.IndexNotFoundException">
        /// The operation tried to access a nonexistent index.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaNotFoundException">
        /// The specified replica is no longer part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateGlobalTableSettings">REST API Reference for UpdateGlobalTableSettings Operation</seealso>
        UpdateGlobalTableSettingsResponse UpdateGlobalTableSettings(UpdateGlobalTableSettingsRequest request);



        /// <summary>
        /// Updates settings for a global table.
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateGlobalTableSettings service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateGlobalTableSettings service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.GlobalTableNotFoundException">
        /// The specified global table does not exist.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.IndexNotFoundException">
        /// The operation tried to access a nonexistent index.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ReplicaNotFoundException">
        /// The specified replica is no longer part of the global table.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateGlobalTableSettings">REST API Reference for UpdateGlobalTableSettings Operation</seealso>
        Task<UpdateGlobalTableSettingsResponse> UpdateGlobalTableSettingsAsync(UpdateGlobalTableSettingsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateItem


        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        UpdateItemResponse UpdateItem(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates);

        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appear before or after they are updated. For <code>UpdateItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - Returns all of the attributes of the item, as they appeared before the UpdateItem operation. </li> <li>  <code>UPDATED_OLD</code> - Returns only the updated attributes, as they appeared before the UpdateItem operation. </li> <li>  <code>ALL_NEW</code> - Returns all of the attributes of the item, as they appear after the UpdateItem operation. </li> <li>  <code>UPDATED_NEW</code> - Returns only the updated attributes, as they appear after the UpdateItem operation. </li> </ul> There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed. The values returned are strongly consistent.</param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        UpdateItemResponse UpdateItem(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues);

        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateItem service method.</param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        UpdateItemResponse UpdateItem(UpdateItemRequest request);


        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, System.Threading.CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appear before or after they are updated. For <code>UpdateItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - Returns all of the attributes of the item, as they appeared before the UpdateItem operation. </li> <li>  <code>UPDATED_OLD</code> - Returns only the updated attributes, as they appeared before the UpdateItem operation. </li> <li>  <code>ALL_NEW</code> - Returns all of the attributes of the item, as they appear after the UpdateItem operation. </li> <li>  <code>UPDATED_NEW</code> - Returns only the updated attributes, as they appear after the UpdateItem operation. </li> </ul> There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed. The values returned are strongly consistent.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Edits an existing item's attributes, or adds a new item to the table if it does not
        /// already exist. You can put, delete, or add attribute values. You can also perform
        /// a conditional update on an existing item (insert a new attribute name-value pair if
        /// it doesn't exist, or replace an existing name-value pair if it has certain expected
        /// attribute values).
        /// 
        ///  
        /// <para>
        /// You can also return the item's attribute values in the same <code>UpdateItem</code>
        /// operation using the <code>ReturnValues</code> parameter.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateItem service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateItem service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ConditionalCheckFailedException">
        /// A condition specified in the operation could not be evaluated.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ItemCollectionSizeLimitExceededException">
        /// An item collection is too large. This exception is only returned for tables that have
        /// one or more local secondary indexes.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ProvisionedThroughputExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Programming.Errors.html#Programming.Errors.RetryAndBackoff">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.RequestLimitExceededException">
        /// Throughput exceeds the current throughput quota for your account. Please contact AWS
        /// Support at <a href="https://aws.amazon.com/support">AWS Support</a> to request a quota
        /// increase.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TransactionConflictException">
        /// Operation was rejected because there is an ongoing transaction for the item.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateItem">REST API Reference for UpdateItem Operation</seealso>
        Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateTable


        /// <summary>
        /// Modifies the provisioned throughput settings, global secondary indexes, or DynamoDB
        /// Streams settings for a given table.
        /// 
        ///  
        /// <para>
        /// You can only perform one of the following operations at once:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Modify the provisioned throughput settings of the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Enable or disable DynamoDB Streams on the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Remove a global secondary index from the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a new global secondary index on the table. After the index begins backfilling,
        /// you can use <code>UpdateTable</code> to perform other operations.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  <code>UpdateTable</code> is an asynchronous operation; while it is executing, the
        /// table status changes from <code>ACTIVE</code> to <code>UPDATING</code>. While it is
        /// <code>UPDATING</code>, you cannot issue another <code>UpdateTable</code> request.
        /// When the table returns to the <code>ACTIVE</code> state, the <code>UpdateTable</code>
        /// operation is complete.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to be updated.</param>
        /// <param name="provisionedThroughput">The new provisioned throughput settings for the specified table or index.</param>
        /// 
        /// <returns>The response from the UpdateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTable">REST API Reference for UpdateTable Operation</seealso>
        UpdateTableResponse UpdateTable(string tableName, ProvisionedThroughput provisionedThroughput);

        /// <summary>
        /// Modifies the provisioned throughput settings, global secondary indexes, or DynamoDB
        /// Streams settings for a given table.
        /// 
        ///  
        /// <para>
        /// You can only perform one of the following operations at once:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Modify the provisioned throughput settings of the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Enable or disable DynamoDB Streams on the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Remove a global secondary index from the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a new global secondary index on the table. After the index begins backfilling,
        /// you can use <code>UpdateTable</code> to perform other operations.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  <code>UpdateTable</code> is an asynchronous operation; while it is executing, the
        /// table status changes from <code>ACTIVE</code> to <code>UPDATING</code>. While it is
        /// <code>UPDATING</code>, you cannot issue another <code>UpdateTable</code> request.
        /// When the table returns to the <code>ACTIVE</code> state, the <code>UpdateTable</code>
        /// operation is complete.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTable service method.</param>
        /// 
        /// <returns>The response from the UpdateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTable">REST API Reference for UpdateTable Operation</seealso>
        UpdateTableResponse UpdateTable(UpdateTableRequest request);


        /// <summary>
        /// Modifies the provisioned throughput settings, global secondary indexes, or DynamoDB
        /// Streams settings for a given table.
        /// 
        ///  
        /// <para>
        /// You can only perform one of the following operations at once:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Modify the provisioned throughput settings of the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Enable or disable DynamoDB Streams on the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Remove a global secondary index from the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a new global secondary index on the table. After the index begins backfilling,
        /// you can use <code>UpdateTable</code> to perform other operations.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  <code>UpdateTable</code> is an asynchronous operation; while it is executing, the
        /// table status changes from <code>ACTIVE</code> to <code>UPDATING</code>. While it is
        /// <code>UPDATING</code>, you cannot issue another <code>UpdateTable</code> request.
        /// When the table returns to the <code>ACTIVE</code> state, the <code>UpdateTable</code>
        /// operation is complete.
        /// </para>
        /// </summary>
        /// <param name="tableName">The name of the table to be updated.</param>
        /// <param name="provisionedThroughput">The new provisioned throughput settings for the specified table or index.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTable">REST API Reference for UpdateTable Operation</seealso>
        Task<UpdateTableResponse> UpdateTableAsync(string tableName, ProvisionedThroughput provisionedThroughput, System.Threading.CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Modifies the provisioned throughput settings, global secondary indexes, or DynamoDB
        /// Streams settings for a given table.
        /// 
        ///  
        /// <para>
        /// You can only perform one of the following operations at once:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Modify the provisioned throughput settings of the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Enable or disable DynamoDB Streams on the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Remove a global secondary index from the table.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Create a new global secondary index on the table. After the index begins backfilling,
        /// you can use <code>UpdateTable</code> to perform other operations.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  <code>UpdateTable</code> is an asynchronous operation; while it is executing, the
        /// table status changes from <code>ACTIVE</code> to <code>UPDATING</code>. While it is
        /// <code>UPDATING</code>, you cannot issue another <code>UpdateTable</code> request.
        /// When the table returns to the <code>ACTIVE</code> state, the <code>UpdateTable</code>
        /// operation is complete.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTable service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateTable service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTable">REST API Reference for UpdateTable Operation</seealso>
        Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateTableReplicaAutoScaling


        /// <summary>
        /// Updates auto scaling settings on your global tables at once.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTableReplicaAutoScaling service method.</param>
        /// 
        /// <returns>The response from the UpdateTableReplicaAutoScaling service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTableReplicaAutoScaling">REST API Reference for UpdateTableReplicaAutoScaling Operation</seealso>
        UpdateTableReplicaAutoScalingResponse UpdateTableReplicaAutoScaling(UpdateTableReplicaAutoScalingRequest request);



        /// <summary>
        /// Updates auto scaling settings on your global tables at once.
        /// 
        ///  <note> 
        /// <para>
        /// This operation only applies to <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/globaltables.V2.html">Version
        /// 2019.11.21</a> of global tables.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTableReplicaAutoScaling service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateTableReplicaAutoScaling service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTableReplicaAutoScaling">REST API Reference for UpdateTableReplicaAutoScaling Operation</seealso>
        Task<UpdateTableReplicaAutoScalingResponse> UpdateTableReplicaAutoScalingAsync(UpdateTableReplicaAutoScalingRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
        #region  UpdateTimeToLive


        /// <summary>
        /// The <code>UpdateTimeToLive</code> method enables or disables Time to Live (TTL) for
        /// the specified table. A successful <code>UpdateTimeToLive</code> call returns the current
        /// <code>TimeToLiveSpecification</code>. It can take up to one hour for the change to
        /// fully process. Any additional <code>UpdateTimeToLive</code> calls for the same table
        /// during this one hour duration result in a <code>ValidationException</code>. 
        /// 
        ///  
        /// <para>
        /// TTL compares the current time in epoch time format to the time stored in the TTL attribute
        /// of an item. If the epoch time value stored in the attribute is less than the current
        /// time, the item is marked as expired and subsequently deleted.
        /// </para>
        ///  <note> 
        /// <para>
        ///  The epoch time format is the number of seconds elapsed since 12:00:00 AM January
        /// 1, 1970 UTC. 
        /// </para>
        ///  </note> 
        /// <para>
        /// DynamoDB deletes expired items on a best-effort basis to ensure availability of throughput
        /// for other data operations. 
        /// </para>
        ///  <important> 
        /// <para>
        /// DynamoDB typically deletes expired items within two days of expiration. The exact
        /// duration within which an item gets deleted after expiration is specific to the nature
        /// of the workload. Items that have expired and not been deleted will still show up in
        /// reads, queries, and scans.
        /// </para>
        ///  </important> 
        /// <para>
        /// As items are deleted, they are removed from any local secondary index and global secondary
        /// index immediately in the same eventually consistent way as a standard delete operation.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html">Time
        /// To Live</a> in the Amazon DynamoDB Developer Guide. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTimeToLive service method.</param>
        /// 
        /// <returns>The response from the UpdateTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTimeToLive">REST API Reference for UpdateTimeToLive Operation</seealso>
        UpdateTimeToLiveResponse UpdateTimeToLive(UpdateTimeToLiveRequest request);



        /// <summary>
        /// The <code>UpdateTimeToLive</code> method enables or disables Time to Live (TTL) for
        /// the specified table. A successful <code>UpdateTimeToLive</code> call returns the current
        /// <code>TimeToLiveSpecification</code>. It can take up to one hour for the change to
        /// fully process. Any additional <code>UpdateTimeToLive</code> calls for the same table
        /// during this one hour duration result in a <code>ValidationException</code>. 
        /// 
        ///  
        /// <para>
        /// TTL compares the current time in epoch time format to the time stored in the TTL attribute
        /// of an item. If the epoch time value stored in the attribute is less than the current
        /// time, the item is marked as expired and subsequently deleted.
        /// </para>
        ///  <note> 
        /// <para>
        ///  The epoch time format is the number of seconds elapsed since 12:00:00 AM January
        /// 1, 1970 UTC. 
        /// </para>
        ///  </note> 
        /// <para>
        /// DynamoDB deletes expired items on a best-effort basis to ensure availability of throughput
        /// for other data operations. 
        /// </para>
        ///  <important> 
        /// <para>
        /// DynamoDB typically deletes expired items within two days of expiration. The exact
        /// duration within which an item gets deleted after expiration is specific to the nature
        /// of the workload. Items that have expired and not been deleted will still show up in
        /// reads, queries, and scans.
        /// </para>
        ///  </important> 
        /// <para>
        /// As items are deleted, they are removed from any local secondary index and global secondary
        /// index immediately in the same eventually consistent way as a standard delete operation.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html">Time
        /// To Live</a> in the Amazon DynamoDB Developer Guide. 
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the UpdateTimeToLive service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the UpdateTimeToLive service method, as returned by DynamoDB.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// There is no limit to the number of daily on-demand backups that can be taken. 
        /// 
        ///  
        /// <para>
        /// Up to 50 simultaneous table operations are allowed per account. These operations include
        /// <code>CreateTable</code>, <code>UpdateTable</code>, <code>DeleteTable</code>,<code>UpdateTimeToLive</code>,
        /// <code>RestoreTableFromBackup</code>, and <code>RestoreTableToPointInTime</code>. 
        /// </para>
        ///  
        /// <para>
        /// The only exception is when you are creating a table with one or more secondary indexes.
        /// You can have up to 25 such requests running at a time; however, if the table or index
        /// specifications are complex, DynamoDB might temporarily reduce the number of concurrent
        /// operations.
        /// </para>
        ///  
        /// <para>
        /// There is a soft account quota of 256 tables.
        /// </para>
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceInUseException">
        /// The operation conflicts with the resource's availability. For example, you attempted
        /// to recreate an existing table, or tried to delete a table currently in the <code>CREATING</code>
        /// state.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent table or index. The resource might not
        /// be specified correctly, or its status might not be <code>ACTIVE</code>.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/dynamodb-2012-08-10/UpdateTimeToLive">REST API Reference for UpdateTimeToLive Operation</seealso>
        Task<UpdateTimeToLiveResponse> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request, CancellationToken cancellationToken = default(CancellationToken));

        #endregion
        
    }
}