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
using System.Text;

namespace Amazon.Auth.AccessControlPolicy.ActionIdentifiers
{
    /// <summary>
    /// The available AWS access control policy actions for Amazon DynamoDB.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class DynamoDBActionIdentifiers
    {
        public static readonly ActionIdentifier AllDynamoDBActions = new ActionIdentifier("dynamodb:*");

        public static readonly ActionIdentifier BatchGetItem = new ActionIdentifier("dynamodb:BatchGetItem");
        public static readonly ActionIdentifier BatchWriteItem = new ActionIdentifier("dynamodb:BatchWriteItem");
        public static readonly ActionIdentifier CreateTable = new ActionIdentifier("dynamodb:CreateTable");
        public static readonly ActionIdentifier DeleteItem = new ActionIdentifier("dynamodb:DeleteItem");
        public static readonly ActionIdentifier DeleteTable = new ActionIdentifier("dynamodb:DeleteTable");
        public static readonly ActionIdentifier DescribeTable = new ActionIdentifier("dynamodb:DescribeTable");
        public static readonly ActionIdentifier GetItem = new ActionIdentifier("dynamodb:GetItem");
        public static readonly ActionIdentifier ListTables = new ActionIdentifier("dynamodb:ListTables");
        public static readonly ActionIdentifier PutItem = new ActionIdentifier("dynamodb:PutItem");
        public static readonly ActionIdentifier Query = new ActionIdentifier("dynamodb:Query");
        public static readonly ActionIdentifier Scan = new ActionIdentifier("dynamodb:Scan");
        public static readonly ActionIdentifier UpdateItem = new ActionIdentifier("dynamodb:UpdateItem");
        public static readonly ActionIdentifier UpdateTable = new ActionIdentifier("dynamodb:UpdateTable");
    }
}

