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
    /// Represents a request to perform a check that an item exists or to check the condition
    /// of specific attributes of the item.
    /// </summary>
    public partial class ConditionCheck
    {
        private string _conditionExpression;
        private Dictionary<string, string> _expressionAttributeNames = new Dictionary<string, string>();
        private Dictionary<string, AttributeValue> _expressionAttributeValues = new Dictionary<string, AttributeValue>();
        private Dictionary<string, AttributeValue> _key = new Dictionary<string, AttributeValue>();
        private ReturnValuesOnConditionCheckFailure _returnValuesOnConditionCheckFailure;
        private string _tableName;

        /// <summary>
        /// Gets and sets the property ConditionExpression. 
        /// <para>
        /// A condition that must be satisfied in order for a conditional update to succeed.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
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
        /// Gets and sets the property ExpressionAttributeNames. 
        /// <para>
        /// One or more substitution tokens for attribute names in an expression.
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
        /// The primary key of the item to be checked. Each element consists of an attribute name
        /// and a value for that attribute.
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
        /// Gets and sets the property ReturnValuesOnConditionCheckFailure. 
        /// <para>
        /// Use <code>ReturnValuesOnConditionCheckFailure</code> to get the item attributes if
        /// the <code>ConditionCheck</code> condition fails. For <code>ReturnValuesOnConditionCheckFailure</code>,
        /// the valid values are: NONE and ALL_OLD.
        /// </para>
        /// </summary>
        public ReturnValuesOnConditionCheckFailure ReturnValuesOnConditionCheckFailure
        {
            get { return this._returnValuesOnConditionCheckFailure; }
            set { this._returnValuesOnConditionCheckFailure = value; }
        }

        // Check to see if ReturnValuesOnConditionCheckFailure property is set
        internal bool IsSetReturnValuesOnConditionCheckFailure()
        {
            return this._returnValuesOnConditionCheckFailure != null;
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// Name of the table for the check item request.
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

    }
}