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
    /// The available AWS access control policy actions for Amazon Cognito Sync.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CognitoSyncActionIdentifiers
    {
        public static readonly ActionIdentifier AllCognitoSyncActions = new ActionIdentifier("cognito-sync:*");

        public static readonly ActionIdentifier DeleteDataset = new ActionIdentifier("cognito-sync:DeleteDataset");
        public static readonly ActionIdentifier DescribeDataset = new ActionIdentifier("cognito-sync:DescribeDataset");
        public static readonly ActionIdentifier DescribeIdentityUsage = new ActionIdentifier("cognito-sync:DescribeIdentityUsage");
        public static readonly ActionIdentifier DescribeIdentityPoolUsage = new ActionIdentifier("cognito-sync:DescribeIdentityPoolUsage");
        public static readonly ActionIdentifier ListDatasets = new ActionIdentifier("cognito-sync:ListDatasets");
        public static readonly ActionIdentifier ListIdentityPoolUsage = new ActionIdentifier("cognito-sync:ListIdentityPoolUsage");
        public static readonly ActionIdentifier ListRecords = new ActionIdentifier("cognito-sync:ListRecords");
        public static readonly ActionIdentifier UpdateRecords = new ActionIdentifier("cognito-sync:UpdateRecords");
    }
}

