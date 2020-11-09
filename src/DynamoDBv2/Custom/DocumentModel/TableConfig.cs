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

using Amazon.DynamoDBv2.Model;

namespace Amazon.DynamoDBv2.DocumentModel
{
    /// <summary>
    /// Configuration for the Table.PutItem operation
    /// </summary>
    public class TableConfig
    {
        /// <summary>
        /// Name of the table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Conversion to use for converting .NET values to DynamoDB values.
        /// Default is AWSConfigs.DynamoDBConfig.ConversionSchema.
        /// </summary>
        public DynamoDBEntryConversion Conversion { get; set; }

        /// <summary>
        /// List of DateTime attributes that should be converted to epoch seconds
        /// before they are stored in DynamoDB.
        /// </summary>
        public List<string> AttributesToStoreAsEpoch { get; set; }


        internal Table.DynamoDBConsumer Consumer { get; set; }
        internal bool IsEmptyStringValueEnabled { get; set; }


        /// <summary>
        /// Constructs TableConfig for a given table.
        /// Uses conversions schema AWSConfigs.DynamoDBConfig.ConversionSchema.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public TableConfig(string tableName)
            : this(tableName, DynamoDBEntryConversion.CurrentConversion, Table.DynamoDBConsumer.DocumentModel, null,
                false)
        {
        }

        internal TableConfig(string tableName, DynamoDBEntryConversion conversion, Table.DynamoDBConsumer consumer,
            IEnumerable<string> storeAsEpoch, bool isEmptyStringValueEnabled)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException("tableName");

            TableName = tableName;
            Conversion = conversion;
            Consumer = consumer;
            IsEmptyStringValueEnabled = isEmptyStringValueEnabled;
            AttributesToStoreAsEpoch = new List<string>();
            if (storeAsEpoch != null)
            {
                AttributesToStoreAsEpoch.AddRange(storeAsEpoch);
            }
        }
    }
}
