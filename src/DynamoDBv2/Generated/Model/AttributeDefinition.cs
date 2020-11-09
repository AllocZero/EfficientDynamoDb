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
    /// Represents an attribute for describing the key schema for the table and indexes.
    /// </summary>
    public partial class AttributeDefinition
    {
        private string _attributeName;
        private ScalarAttributeType _attributeType;

        /// <summary>
        /// Empty constructor used to set  properties independently even when a simple constructor is available
        /// </summary>
        public AttributeDefinition() { }

        /// <summary>
        /// Instantiates AttributeDefinition with the parameterized properties
        /// </summary>
        /// <param name="attributeName">A name for the attribute.</param>
        /// <param name="attributeType">The data type for the attribute, where: <ul> <li>  <code>S</code> - the attribute is of type String </li> <li>  <code>N</code> - the attribute is of type Number </li> <li>  <code>B</code> - the attribute is of type Binary </li> </ul></param>
        public AttributeDefinition(string attributeName, ScalarAttributeType attributeType)
        {
            _attributeName = attributeName;
            _attributeType = attributeType;
        }

        /// <summary>
        /// Gets and sets the property AttributeName. 
        /// <para>
        /// A name for the attribute.
        /// </para>
        /// </summary>
        [AWSProperty(Required=true, Min=1, Max=255)]
        public string AttributeName
        {
            get { return this._attributeName; }
            set { this._attributeName = value; }
        }

        // Check to see if AttributeName property is set
        internal bool IsSetAttributeName()
        {
            return this._attributeName != null;
        }

        /// <summary>
        /// Gets and sets the property AttributeType. 
        /// <para>
        /// The data type for the attribute, where:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        ///  <code>S</code> - the attribute is of type String
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>N</code> - the attribute is of type Number
        /// </para>
        ///  </li> <li> 
        /// <para>
        ///  <code>B</code> - the attribute is of type Binary
        /// </para>
        ///  </li> </ul>
        /// </summary>
        [AWSProperty(Required=true)]
        public ScalarAttributeType AttributeType
        {
            get { return this._attributeType; }
            set { this._attributeType = value; }
        }

        // Check to see if AttributeType property is set
        internal bool IsSetAttributeType()
        {
            return this._attributeType != null;
        }

    }
}