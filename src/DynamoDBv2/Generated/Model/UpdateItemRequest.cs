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
    /// Container for the parameters to the UpdateItem operation.
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
    public partial class UpdateItemRequest : AmazonDynamoDBRequest
    {
        private Dictionary<string, AttributeValueUpdate> _attributeUpdates = new Dictionary<string, AttributeValueUpdate>();
        private ConditionalOperator _conditionalOperator;
        private string _conditionExpression;
        private Dictionary<string, ExpectedAttributeValue> _expected = new Dictionary<string, ExpectedAttributeValue>();
        private Dictionary<string, string> _expressionAttributeNames = new Dictionary<string, string>();
        private Dictionary<string, AttributeValue> _expressionAttributeValues = new Dictionary<string, AttributeValue>();
        private Dictionary<string, AttributeValue> _key = new Dictionary<string, AttributeValue>();
        private ReturnConsumedCapacity _returnConsumedCapacity;
        private ReturnItemCollectionMetrics _returnItemCollectionMetrics;
        private ReturnValue _returnValues;
        private string _tableName;
        private string _updateExpression;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public UpdateItemRequest() { }

        /// <summary>
        /// Instantiates UpdateItemRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        public UpdateItemRequest(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates)
        {
            _tableName = tableName;
            _key = key;
            _attributeUpdates = attributeUpdates;
        }

        /// <summary>
        /// Instantiates UpdateItemRequest with the parameterized properties
        /// </summary>
        /// <param name="tableName">The name of the table containing the item to update.</param>
        /// <param name="key">The primary key of the item to be updated. Each element consists of an attribute name and a value for that attribute. For the primary key, you must provide all of the attributes. For example, with a simple primary key, you only need to provide a value for the partition key. For a composite primary key, you must provide values for both the partition key and the sort key.</param>
        /// <param name="attributeUpdates">This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a> in the <i>Amazon DynamoDB Developer Guide</i>.</param>
        /// <param name="returnValues">Use <code>ReturnValues</code> if you want to get the item attributes as they appear before or after they are updated. For <code>UpdateItem</code>, the valid values are: <ul> <li>  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.) </li> <li>  <code>ALL_OLD</code> - Returns all of the attributes of the item, as they appeared before the UpdateItem operation. </li> <li>  <code>UPDATED_OLD</code> - Returns only the updated attributes, as they appeared before the UpdateItem operation. </li> <li>  <code>ALL_NEW</code> - Returns all of the attributes of the item, as they appear after the UpdateItem operation. </li> <li>  <code>UPDATED_NEW</code> - Returns only the updated attributes, as they appear after the UpdateItem operation. </li> </ul> There is no additional cost associated with requesting a return value aside from the small network and processing overhead of receiving a larger response. No read capacity units are consumed. The values returned are strongly consistent.</param>
        public UpdateItemRequest(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues)
        {
            _tableName = tableName;
            _key = key;
            _attributeUpdates = attributeUpdates;
            _returnValues = returnValues;
        }

        /// <summary>
        /// Gets and sets the property AttributeUpdates. 
        /// <para>
        /// This is a legacy parameter. Use <code>UpdateExpression</code> instead. For more information,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.AttributeUpdates.html">AttributeUpdates</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public Dictionary<string, AttributeValueUpdate> AttributeUpdates
        {
            get { return this._attributeUpdates; }
            set { this._attributeUpdates = value; }
        }

        // Check to see if AttributeUpdates property is set
        internal bool IsSetAttributeUpdates()
        {
            return this._attributeUpdates != null && this._attributeUpdates.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ConditionalOperator. 
        /// <para>
        /// This is a legacy parameter. Use <code>ConditionExpression</code> instead. For more
        /// information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.ConditionalOperator.html">ConditionalOperator</a>
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
        /// Gets and sets the property ConditionExpression. 
        /// <para>
        /// A condition that must be satisfied in order for a conditional update to succeed.
        /// </para>
        ///  
        /// <para>
        /// An expression can contain any of the following:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// Functions: <code>attribute_exists | attribute_not_exists | attribute_type | contains
        /// | begins_with | size</code> 
        /// </para>
        ///  
        /// <para>
        /// These function names are case-sensitive.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// Comparison operators: <code>= | &lt;&gt; | &lt; | &gt; | &lt;= | &gt;= | BETWEEN |
        /// IN </code> 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  Logical operators: <code>AND | OR | NOT</code> 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// For more information about condition expressions, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.SpecifyingConditions.html">Specifying
        /// Conditions</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public string ConditionExpression
        {
            get { return this._conditionExpression; }
            set { this._conditionExpression = value; }
        }

        // Check to see if ConditionExpression property is set
        internal bool IsSetConditionExpression()
        {
            return this._conditionExpression != null;
        }

        /// <summary>
        /// Gets and sets the property Expected. 
        /// <para>
        /// This is a legacy parameter. Use <code>ConditionExpression</code> instead. For more
        /// information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.Expected.html">Expected</a>
        /// in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public Dictionary<string, ExpectedAttributeValue> Expected
        {
            get { return this._expected; }
            set { this._expected = value; }
        }

        // Check to see if Expected property is set
        internal bool IsSetExpected()
        {
            return this._expected != null && this._expected.Count > 0; 
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
        /// Words</a> in the <i>Amazon DynamoDB Developer Guide</i>.) To work around this, you
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
        /// For more information about expression attribute names, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.AccessingItemAttributes.html">Specifying
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
        /// Gets and sets the property Key. 
        /// <para>
        /// The primary key of the item to be updated. Each element consists of an attribute name
        /// and a value for that attribute.
        /// </para>
        ///  
        /// <para>
        /// For the primary key, you must provide all of the attributes. For example, with a simple
        /// primary key, you only need to provide a value for the partition key. For a composite
        /// primary key, you must provide values for both the partition key and the sort key.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public Dictionary<string, AttributeValue> Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        // Check to see if Key property is set
        internal bool IsSetKey()
        {
            return this._key != null && this._key.Count > 0; 
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
        /// Gets and sets the property ReturnItemCollectionMetrics. 
        /// <para>
        /// Determines whether item collection metrics are returned. If set to <code>SIZE</code>,
        /// the response includes statistics about item collections, if any, that were modified
        /// during the operation are returned in the response. If set to <code>NONE</code> (the
        /// default), no statistics are returned.
        /// </para>
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics
        {
            get { return this._returnItemCollectionMetrics; }
            set { this._returnItemCollectionMetrics = value; }
        }

        // Check to see if ReturnItemCollectionMetrics property is set
        internal bool IsSetReturnItemCollectionMetrics()
        {
            return this._returnItemCollectionMetrics != null;
        }

        /// <summary>
        /// Gets and sets the property ReturnValues. 
        /// <para>
        /// Use <code>ReturnValues</code> if you want to get the item attributes as they appear
        /// before or after they are updated. For <code>UpdateItem</code>, the valid values are:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>NONE</code> - If <code>ReturnValues</code> is not specified, or if its value
        /// is <code>NONE</code>, then nothing is returned. (This setting is the default for <code>ReturnValues</code>.)
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL_OLD</code> - Returns all of the attributes of the item, as they appeared
        /// before the UpdateItem operation.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATED_OLD</code> - Returns only the updated attributes, as they appeared
        /// before the UpdateItem operation.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ALL_NEW</code> - Returns all of the attributes of the item, as they appear
        /// after the UpdateItem operation.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>UPDATED_NEW</code> - Returns only the updated attributes, as they appear after
        /// the UpdateItem operation.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// There is no additional cost associated with requesting a return value aside from the
        /// small network and processing overhead of receiving a larger response. No read capacity
        /// units are consumed.
        /// </para>
        ///  
        /// <para>
        /// The values returned are strongly consistent.
        /// </para>
        /// </summary>
        public ReturnValue ReturnValues
        {
            get { return this._returnValues; }
            set { this._returnValues = value; }
        }

        // Check to see if ReturnValues property is set
        internal bool IsSetReturnValues()
        {
            return this._returnValues != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table containing the item to update.
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
        /// Gets and sets the property UpdateExpression. 
        /// <para>
        /// An expression that defines one or more attributes to be updated, the action to be
        /// performed on them, and new values for them.
        /// </para>
        ///  
        /// <para>
        /// The following action values are available for <code>UpdateExpression</code>.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>SET</code> - Adds one or more attributes and values to an item. If any of these
        /// attributes already exist, they are replaced by the new values. You can also use <code>SET</code>
        /// to add or subtract from an attribute that is of type Number. For example: <code>SET
        /// myNum = myNum + :val</code> 
        /// </para>
        ///  
        /// <para>
        ///  <code>SET</code> supports the following functions:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>if_not_exists (path, operand)</code> - if the item does not contain an attribute
        /// at the specified path, then <code>if_not_exists</code> evaluates to operand; otherwise,
        /// it evaluates to path. You can use this function to avoid overwriting an attribute
        /// that may already be present in the item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>list_append (operand, operand)</code> - evaluates to a list with a new element
        /// added to it. You can append the new element to the start or the end of the list by
        /// reversing the order of the operands.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// These function names are case-sensitive.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>REMOVE</code> - Removes one or more attributes from an item.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ADD</code> - Adds the specified value to the item, if the attribute does not
        /// already exist. If the attribute does exist, then the behavior of <code>ADD</code>
        /// depends on the data type of the attribute:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// If the existing attribute is a number, and if <code>Value</code> is also a number,
        /// then <code>Value</code> is mathematically added to the existing attribute. If <code>Value</code>
        /// is a negative number, then it is subtracted from the existing attribute.
        /// </para>
        ///  <note> 
        /// <para>
        /// If you use <code>ADD</code> to increment or decrement a number value for an item that
        /// doesn't exist before the update, DynamoDB uses <code>0</code> as the initial value.
        /// </para>
        ///  
        /// <para>
        /// Similarly, if you use <code>ADD</code> for an existing item to increment or decrement
        /// an attribute value that doesn't exist before the update, DynamoDB uses <code>0</code>
        /// as the initial value. For example, suppose that the item you want to update doesn't
        /// have an attribute named <code>itemcount</code>, but you decide to <code>ADD</code>
        /// the number <code>3</code> to this attribute anyway. DynamoDB will create the <code>itemcount</code>
        /// attribute, set its initial value to <code>0</code>, and finally add <code>3</code>
        /// to it. The result will be a new <code>itemcount</code> attribute in the item, with
        /// a value of <code>3</code>.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// If the existing data type is a set and if <code>Value</code> is also a set, then <code>Value</code>
        /// is added to the existing set. For example, if the attribute value is the set <code>[1,2]</code>,
        /// and the <code>ADD</code> action specified <code>[3]</code>, then the final attribute
        /// value is <code>[1,2,3]</code>. An error occurs if an <code>ADD</code> action is specified
        /// for a set attribute and the attribute type specified does not match the existing set
        /// type. 
        /// </para>
        ///  
        /// <para>
        /// Both sets must have the same primitive data type. For example, if the existing data
        /// type is a set of strings, the <code>Value</code> must also be a set of strings.
        /// </para>
        ///  </li> </ul> <important> 
        /// <para>
        /// The <code>ADD</code> action only supports Number and set data types. In addition,
        /// <code>ADD</code> can only be used on top-level attributes, not nested attributes.
        /// </para>
        ///  </important> </li> <li> 
        /// <para>
        ///  <code>DELETE</code> - Deletes an element from a set.
        /// </para>
        ///  
        /// <para>
        /// If a set of values is specified, then those values are subtracted from the old set.
        /// For example, if the attribute value was the set <code>[a,b,c]</code> and the <code>DELETE</code>
        /// action specifies <code>[a,c]</code>, then the final attribute value is <code>[b]</code>.
        /// Specifying an empty set is an error.
        /// </para>
        ///  <important> 
        /// <para>
        /// The <code>DELETE</code> action only supports set data types. In addition, <code>DELETE</code>
        /// can only be used on top-level attributes, not nested attributes.
        /// </para>
        ///  </important> </li> </ul> 
        /// <para>
        /// You can have many actions in a single expression, such as the following: <code>SET
        /// a=:value1, b=:value2 DELETE :value3, :value4, :value5</code> 
        /// </para>
        ///  
        /// <para>
        /// For more information on update expressions, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.Modifying.html">Modifying
        /// Items and Attributes</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        public string UpdateExpression
        {
            get { return this._updateExpression; }
            set { this._updateExpression = value; }
        }

        // Check to see if UpdateExpression property is set
        internal bool IsSetUpdateExpression()
        {
            return this._updateExpression != null;
        }

    }
}