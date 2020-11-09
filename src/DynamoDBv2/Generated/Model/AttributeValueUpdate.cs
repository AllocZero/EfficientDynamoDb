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
    /// For the <code>UpdateItem</code> operation, represents the attributes to be modified,
    /// the action to perform on each, and the new value for each.
    /// 
    ///  <note> 
    /// <para>
    /// You cannot use <code>UpdateItem</code> to update any primary key attributes. Instead,
    /// you will need to delete the item, and then use <code>PutItem</code> to create a new
    /// item with new attributes.
    /// </para>
    ///  </note> 
    /// <para>
    /// Attribute values cannot be null; string and binary type attributes must have lengths
    /// greater than zero; and set type attributes must not be empty. Requests with empty
    /// values will be rejected with a <code>ValidationException</code> exception.
    /// </para>
    /// </summary>
    public partial class AttributeValueUpdate
    {
        private AttributeAction _action;
        private AttributeValue _value;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public AttributeValueUpdate() { }

        /// <summary>
        /// Instantiates AttributeValueUpdate with the parameterized properties
        /// </summary>
        /// <param name="value">Represents the data for an attribute. Each attribute value is described as a name-value pair. The name is the data type, and the value is the data itself. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.NamingRulesDataTypes.html#HowItWorks.DataTypes">Data Types</a> in the <i>Amazon DynamoDB Developer Guide</i>. </param>
        /// <param name="action">Specifies how to perform the update. Valid values are <code>PUT</code> (default), <code>DELETE</code>, and <code>ADD</code>. The behavior depends on whether the specified primary key already exists in the table.  <b>If an item with the specified <i>Key</i> is found in the table:</b>  <ul> <li>  <code>PUT</code> - Adds the specified attribute to the item. If the attribute already exists, it is replaced by the new value.  </li> <li>  <code>DELETE</code> - If no value is specified, the attribute and its value are removed from the item. The data type of the specified value must match the existing value's data type. If a <i>set</i> of values is specified, then those values are subtracted from the old set. For example, if the attribute value was the set <code>[a,b,c]</code> and the <code>DELETE</code> action specified <code>[a,c]</code>, then the final attribute value would be <code>[b]</code>. Specifying an empty set is an error. </li> <li>  <code>ADD</code> - If the attribute does not already exist, then the attribute and its values are added to the item. If the attribute does exist, then the behavior of <code>ADD</code> depends on the data type of the attribute: <ul> <li> If the existing attribute is a number, and if <code>Value</code> is also a number, then the <code>Value</code> is mathematically added to the existing attribute. If <code>Value</code> is a negative number, then it is subtracted from the existing attribute. <note>  If you use <code>ADD</code> to increment or decrement a number value for an item that doesn't exist before the update, DynamoDB uses 0 as the initial value. In addition, if you use <code>ADD</code> to update an existing item, and intend to increment or decrement an attribute value which does not yet exist, DynamoDB uses <code>0</code> as the initial value. For example, suppose that the item you want to update does not yet have an attribute named <i>itemcount</i>, but you decide to <code>ADD</code> the number <code>3</code> to this attribute anyway, even though it currently does not exist. DynamoDB will create the <i>itemcount</i> attribute, set its initial value to <code>0</code>, and finally add <code>3</code> to it. The result will be a new <i>itemcount</i> attribute in the item, with a value of <code>3</code>. </note> </li> <li> If the existing data type is a set, and if the <code>Value</code> is also a set, then the <code>Value</code> is added to the existing set. (This is a <i>set</i> operation, not mathematical addition.) For example, if the attribute value was the set <code>[1,2]</code>, and the <code>ADD</code> action specified <code>[3]</code>, then the final attribute value would be <code>[1,2,3]</code>. An error occurs if an Add action is specified for a set attribute and the attribute type specified does not match the existing set type.  Both sets must have the same primitive data type. For example, if the existing data type is a set of strings, the <code>Value</code> must also be a set of strings. The same holds true for number sets and binary sets. </li> </ul> This action is only valid for an existing attribute whose data type is number or is a set. Do not use <code>ADD</code> for any other data types. </li> </ul>  <b>If no item with the specified <i>Key</i> is found:</b>  <ul> <li>  <code>PUT</code> - DynamoDB creates a new item with the specified primary key, and then adds the attribute.  </li> <li>  <code>DELETE</code> - Nothing happens; there is no attribute to delete. </li> <li>  <code>ADD</code> - DynamoDB creates an item with the supplied primary key and number (or set of numbers) for the attribute value. The only data types allowed are number and number set; no other data types can be specified. </li> </ul></param>
        public AttributeValueUpdate(AttributeValue value, AttributeAction action)
        {
            _value = value;
            _action = action;
        }

        /// <summary>
        /// Gets and sets the property Action. 
        /// <para>
        /// Specifies how to perform the update. Valid values are <code>PUT</code> (default),
        /// <code>DELETE</code>, and <code>ADD</code>. The behavior depends on whether the specified
        /// primary key already exists in the table.
        /// </para>
        ///  
        /// <para>
        ///  <b>If an item with the specified <i>Key</i> is found in the table:</b> 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PUT</code> - Adds the specified attribute to the item. If the attribute already
        /// exists, it is replaced by the new value. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETE</code> - If no value is specified, the attribute and its value are removed
        /// from the item. The data type of the specified value must match the existing value's
        /// data type.
        /// </para>
        ///  
        /// <para>
        /// If a <i>set</i> of values is specified, then those values are subtracted from the
        /// old set. For example, if the attribute value was the set <code>[a,b,c]</code> and
        /// the <code>DELETE</code> action specified <code>[a,c]</code>, then the final attribute
        /// value would be <code>[b]</code>. Specifying an empty set is an error.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ADD</code> - If the attribute does not already exist, then the attribute and
        /// its values are added to the item. If the attribute does exist, then the behavior of
        /// <code>ADD</code> depends on the data type of the attribute:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// If the existing attribute is a number, and if <code>Value</code> is also a number,
        /// then the <code>Value</code> is mathematically added to the existing attribute. If
        /// <code>Value</code> is a negative number, then it is subtracted from the existing attribute.
        /// </para>
        ///  <note> 
        /// <para>
        ///  If you use <code>ADD</code> to increment or decrement a number value for an item
        /// that doesn't exist before the update, DynamoDB uses 0 as the initial value.
        /// </para>
        ///  
        /// <para>
        /// In addition, if you use <code>ADD</code> to update an existing item, and intend to
        /// increment or decrement an attribute value which does not yet exist, DynamoDB uses
        /// <code>0</code> as the initial value. For example, suppose that the item you want to
        /// update does not yet have an attribute named <i>itemcount</i>, but you decide to <code>ADD</code>
        /// the number <code>3</code> to this attribute anyway, even though it currently does
        /// not exist. DynamoDB will create the <i>itemcount</i> attribute, set its initial value
        /// to <code>0</code>, and finally add <code>3</code> to it. The result will be a new
        /// <i>itemcount</i> attribute in the item, with a value of <code>3</code>.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        /// If the existing data type is a set, and if the <code>Value</code> is also a set, then
        /// the <code>Value</code> is added to the existing set. (This is a <i>set</i> operation,
        /// not mathematical addition.) For example, if the attribute value was the set <code>[1,2]</code>,
        /// and the <code>ADD</code> action specified <code>[3]</code>, then the final attribute
        /// value would be <code>[1,2,3]</code>. An error occurs if an Add action is specified
        /// for a set attribute and the attribute type specified does not match the existing set
        /// type. 
        /// </para>
        ///  
        /// <para>
        /// Both sets must have the same primitive data type. For example, if the existing data
        /// type is a set of strings, the <code>Value</code> must also be a set of strings. The
        /// same holds true for number sets and binary sets.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// This action is only valid for an existing attribute whose data type is number or is
        /// a set. Do not use <code>ADD</code> for any other data types.
        /// </para>
        ///  </li> </ul> 
        /// <para>
        ///  <b>If no item with the specified <i>Key</i> is found:</b> 
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>PUT</code> - DynamoDB creates a new item with the specified primary key, and
        /// then adds the attribute. 
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>DELETE</code> - Nothing happens; there is no attribute to delete.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>ADD</code> - DynamoDB creates an item with the supplied primary key and number
        /// (or set of numbers) for the attribute value. The only data types allowed are number
        /// and number set; no other data types can be specified.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public AttributeAction Action
        {
            get { return this._action; }
            set { this._action = value; }
        }

        // Check to see if Action property is set
        internal bool IsSetAction()
        {
            return this._action != null;
        }

        /// <summary>
        /// Gets and sets the property Value. 
        /// <para>
        /// Represents the data for an attribute.
        /// </para>
        ///  
        /// <para>
        /// Each attribute value is described as a name-value pair. The name is the data type,
        /// and the value is the data itself.
        /// </para>
        ///  
        /// <para>
        /// For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.NamingRulesDataTypes.html#HowItWorks.DataTypes">Data
        /// Types</a> in the <i>Amazon DynamoDB Developer Guide</i>. 
        /// </para>
        /// </summary>
        public AttributeValue Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        // Check to see if Value property is set
        internal bool IsSetValue()
        {
            return this._value != null;
        }

    }
}