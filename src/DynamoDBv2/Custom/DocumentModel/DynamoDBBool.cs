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

using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.DynamoDBv2.Model;
using System.IO;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// A DynamoDBEntry that represents a DynamoDB bool (BOOL) type.
    /// </summary>
    public class DynamoDBBool : DynamoDBEntry
    {
        /// <summary>
        /// Construct an instance of DynamnDBBool
        /// </summary>
        /// <param name="value"></param>
        public DynamoDBBool(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets and sets the Value property.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Constant DynamoDBBool for the true value
        /// </summary>
        public static readonly DynamoDBBool True = new DynamoDBBool(true);

        /// <summary>
        /// Constant DynamoDBBool for the false value
        /// </summary>
        public static readonly DynamoDBBool False = new DynamoDBBool(false);

        #region Overrides

        internal override AttributeValue ConvertToAttributeValue(AttributeConversionConfig conversionConfig)
        {
            return new AttributeValue
            {
                BOOL = Value
            };
        }

        /// <summary>
        /// Implement the Clone method
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DynamoDBBool(Value);
        }

        #endregion

        /// <summary>
        /// Explicitly convert DynamoDBBool to Boolean
        /// </summary>
        /// <returns>Boolean value of this object</returns>
        public override Boolean AsBoolean()
        {
            return this.Value;
        }
        /// <summary>
        /// Implicitly convert Boolean to DynamoDBBool
        /// </summary>
        /// <param name="data">Boolean data to convert</param>
        /// <returns>DynamoDBBool representing the data</returns>
        public static implicit operator DynamoDBBool(Boolean data)
        {
            return new DynamoDBBool(data);
        }
        /// <summary>
        /// Explicitly convert DynamoDBBool to Boolean
        /// </summary>
        /// <param name="p">DynamoDBBool to convert</param>
        /// <returns>Boolean value of DynamoDBBool</returns>
        public static explicit operator Boolean(DynamoDBBool p)
        {
            return p.AsBoolean();
        }

        #region Overrides

        /// <summary>
        /// Implement the Equals method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var otherBool = obj as DynamoDBBool;
            if (otherBool == null)
                return false;

            return (this.Value == otherBool.Value);
        }

        /// <summary>
        /// Implement the GetHashCode method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion
    }

}
