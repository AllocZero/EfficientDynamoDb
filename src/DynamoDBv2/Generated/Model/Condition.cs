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
    /// Represents the selection criteria for a <code>Query</code> or <code>Scan</code> operation:
    /// 
    ///  <ul> <li> 
    /// <para>
    /// For a <code>Query</code> operation, <code>Condition</code> is used for specifying
    /// the <code>KeyConditions</code> to use when querying a table or an index. For <code>KeyConditions</code>,
    /// only the following comparison operators are supported:
    /// </para>
    ///  
    /// <para>
    ///  <code>EQ | LE | LT | GE | GT | BEGINS_WITH | BETWEEN</code> 
    /// </para>
    ///  
    /// <para>
    ///  <code>Condition</code> is also used in a <code>QueryFilter</code>, which evaluates
    /// the query results and returns only the desired values.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// For a <code>Scan</code> operation, <code>Condition</code> is used in a <code>ScanFilter</code>,
    /// which evaluates the scan results and returns only the desired values.
    /// </para>
    ///  </li> </ul>
    /// </summary>
    public partial class Condition
    {
        private List<AttributeValue> _attributeValueList = new List<AttributeValue>();
        private ComparisonOperator _comparisonOperator;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public Condition() { }

        /// <summary>
        /// Gets and sets the property AttributeValueList. 
        /// <para>
        /// One or more values to evaluate against the supplied attribute. The number of values
        /// in the list depends on the <code>ComparisonOperator</code> being used.
        /// </para>
        ///  
        /// <para>
        /// For type Number, value comparisons are numeric.
        /// </para>
        ///  
        /// <para>
        /// String value comparisons for greater than, equals, or less than are based on ASCII
        /// character code values. For example, <code>a</code> is greater than <code>A</code>,
        /// and <code>a</code> is greater than <code>B</code>. For a list of code values, see
        /// <a href="http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters">http://en.wikipedia.org/wiki/ASCII#ASCII_printable_characters</a>.
        /// </para>
        ///  
        /// <para>
        /// For Binary, DynamoDB treats each byte of the binary data as unsigned when it compares
        /// binary values.
        /// </para>
        /// </summary>
        public List<AttributeValue> AttributeValueList
        {
            get { return this._attributeValueList; }
            set { this._attributeValueList = value; }
        }

        // Check to see if AttributeValueList property is set
        internal bool IsSetAttributeValueList()
        {
            return this._attributeValueList != null && this._attributeValueList.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ComparisonOperator. 
        /// <para>
        /// A comparator for evaluating attributes. For example, equals, greater than, less than,
        /// etc.
        /// </para>
        ///  
        /// <para>
        /// The following comparison operators are available:
        /// </para>
        ///  
        /// <para>
        ///  <code>EQ | NE | LE | LT | GE | GT | NOT_NULL | NULL | CONTAINS | NOT_CONTAINS | BEGINS_WITH
        /// | IN | BETWEEN</code> 
        /// </para>
        ///  
        /// <para>
        /// The following are descriptions of each comparison operator.
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>EQ</code> : Equal. <code>EQ</code> is supported for all data types, including
        /// lists and maps.
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, Binary, String Set, Number Set, or Binary Set. If
        /// an item contains an <code>AttributeValue</code> element of a different type than the
        /// one provided in the request, the value does not match. For example, <code>{"S":"6"}</code>
        /// does not equal <code>{"N":"6"}</code>. Also, <code>{"N":"6"}</code> does not equal
        /// <code>{"NS":["6", "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>NE</code> : Not equal. <code>NE</code> is supported for all data types, including
        /// lists and maps.
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// of type String, Number, Binary, String Set, Number Set, or Binary Set. If an item
        /// contains an <code>AttributeValue</code> of a different type than the one provided
        /// in the request, the value does not match. For example, <code>{"S":"6"}</code> does
        /// not equal <code>{"N":"6"}</code>. Also, <code>{"N":"6"}</code> does not equal <code>{"NS":["6",
        /// "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>LE</code> : Less than or equal. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, or Binary (not a set type). If an item contains an
        /// <code>AttributeValue</code> element of a different type than the one provided in the
        /// request, the value does not match. For example, <code>{"S":"6"}</code> does not equal
        /// <code>{"N":"6"}</code>. Also, <code>{"N":"6"}</code> does not compare to <code>{"NS":["6",
        /// "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>LT</code> : Less than. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// of type String, Number, or Binary (not a set type). If an item contains an <code>AttributeValue</code>
        /// element of a different type than the one provided in the request, the value does not
        /// match. For example, <code>{"S":"6"}</code> does not equal <code>{"N":"6"}</code>.
        /// Also, <code>{"N":"6"}</code> does not compare to <code>{"NS":["6", "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>GE</code> : Greater than or equal. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, or Binary (not a set type). If an item contains an
        /// <code>AttributeValue</code> element of a different type than the one provided in the
        /// request, the value does not match. For example, <code>{"S":"6"}</code> does not equal
        /// <code>{"N":"6"}</code>. Also, <code>{"N":"6"}</code> does not compare to <code>{"NS":["6",
        /// "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>GT</code> : Greater than. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, or Binary (not a set type). If an item contains an
        /// <code>AttributeValue</code> element of a different type than the one provided in the
        /// request, the value does not match. For example, <code>{"S":"6"}</code> does not equal
        /// <code>{"N":"6"}</code>. Also, <code>{"N":"6"}</code> does not compare to <code>{"NS":["6",
        /// "2", "1"]}</code>.
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>NOT_NULL</code> : The attribute exists. <code>NOT_NULL</code> is supported
        /// for all data types, including lists and maps.
        /// </para>
        ///  <note> 
        /// <para>
        /// This operator tests for the existence of an attribute, not its data type. If the data
        /// type of attribute "<code>a</code>" is null, and you evaluate it using <code>NOT_NULL</code>,
        /// the result is a Boolean <code>true</code>. This result is because the attribute "<code>a</code>"
        /// exists; its data type is not relevant to the <code>NOT_NULL</code> comparison operator.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        ///  <code>NULL</code> : The attribute does not exist. <code>NULL</code> is supported
        /// for all data types, including lists and maps.
        /// </para>
        ///  <note> 
        /// <para>
        /// This operator tests for the nonexistence of an attribute, not its data type. If the
        /// data type of attribute "<code>a</code>" is null, and you evaluate it using <code>NULL</code>,
        /// the result is a Boolean <code>false</code>. This is because the attribute "<code>a</code>"
        /// exists; its data type is not relevant to the <code>NULL</code> comparison operator.
        /// </para>
        ///  </note> </li> <li> 
        /// <para>
        ///  <code>CONTAINS</code> : Checks for a subsequence, or value in a set.
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, or Binary (not a set type). If the target attribute
        /// of the comparison is of type String, then the operator checks for a substring match.
        /// If the target attribute of the comparison is of type Binary, then the operator looks
        /// for a subsequence of the target that matches the input. If the target attribute of
        /// the comparison is a set ("<code>SS</code>", "<code>NS</code>", or "<code>BS</code>"),
        /// then the operator evaluates to true if it finds an exact match with any member of
        /// the set.
        /// </para>
        ///  
        /// <para>
        /// CONTAINS is supported for lists: When evaluating "<code>a CONTAINS b</code>", "<code>a</code>"
        /// can be a list; however, "<code>b</code>" cannot be a set, a map, or a list.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>NOT_CONTAINS</code> : Checks for absence of a subsequence, or absence of a
        /// value in a set.
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// element of type String, Number, or Binary (not a set type). If the target attribute
        /// of the comparison is a String, then the operator checks for the absence of a substring
        /// match. If the target attribute of the comparison is Binary, then the operator checks
        /// for the absence of a subsequence of the target that matches the input. If the target
        /// attribute of the comparison is a set ("<code>SS</code>", "<code>NS</code>", or "<code>BS</code>"),
        /// then the operator evaluates to true if it <i>does not</i> find an exact match with
        /// any member of the set.
        /// </para>
        ///  
        /// <para>
        /// NOT_CONTAINS is supported for lists: When evaluating "<code>a NOT CONTAINS b</code>",
        /// "<code>a</code>" can be a list; however, "<code>b</code>" cannot be a set, a map,
        /// or a list.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>BEGINS_WITH</code> : Checks for a prefix. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain only one <code>AttributeValue</code>
        /// of type String or Binary (not a Number or a set type). The target attribute of the
        /// comparison must be of type String or Binary (not a Number or a set type).
        /// </para>
        ///   </li> <li> 
        /// <para>
        ///  <code>IN</code> : Checks for matching elements in a list.
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> can contain one or more <code>AttributeValue</code>
        /// elements of type String, Number, or Binary. These attributes are compared against
        /// an existing attribute of an item. If any elements of the input are equal to the item
        /// attribute, the expression evaluates to true.
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>BETWEEN</code> : Greater than or equal to the first value, and less than or
        /// equal to the second value. 
        /// </para>
        ///  
        /// <para>
        ///  <code>AttributeValueList</code> must contain two <code>AttributeValue</code> elements
        /// of the same type, either String, Number, or Binary (not a set type). A target attribute
        /// matches if the target value is greater than, or equal to, the first element and less
        /// than, or equal to, the second element. If an item contains an <code>AttributeValue</code>
        /// element of a different type than the one provided in the request, the value does not
        /// match. For example, <code>{"S":"6"}</code> does not compare to <code>{"N":"6"}</code>.
        /// Also, <code>{"N":"6"}</code> does not compare to <code>{"NS":["6", "2", "1"]}</code>
        /// 
        /// </para>
        ///  </li> </ul> 
        /// <para>
        /// For usage examples of <code>AttributeValueList</code> and <code>ComparisonOperator</code>,
        /// see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LegacyConditionalParameters.html">Legacy
        /// Conditional Parameters</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true)]
        public ComparisonOperator ComparisonOperator
        {
            get { return this._comparisonOperator; }
            set { this._comparisonOperator = value; }
        }

        // Check to see if ComparisonOperator property is set
        internal bool IsSetComparisonOperator()
        {
            return this._comparisonOperator != null;
        }

    }
}