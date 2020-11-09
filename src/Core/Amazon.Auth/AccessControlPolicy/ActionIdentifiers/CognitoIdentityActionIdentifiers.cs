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
    /// The available AWS access control policy actions for Amazon Cognito Identity.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CognitoIdentityActionIdentifiers
    {
        public static readonly ActionIdentifier AllCognitoIdentityActions = new ActionIdentifier("cognito-identity:*");

        public static readonly ActionIdentifier CreateIdentityPool = new ActionIdentifier("cognito-identity:CreateIdentityPool");
        public static readonly ActionIdentifier DeleteIdentityPool = new ActionIdentifier("cognito-identity:DeleteIdentityPool");
        public static readonly ActionIdentifier DescribeIdentityPool = new ActionIdentifier("cognito-identity:DescribeIdentityPool");
        public static readonly ActionIdentifier ListIdentities = new ActionIdentifier("cognito-identity:ListIdentities");
        public static readonly ActionIdentifier ListIdentityPools = new ActionIdentifier("cognito-identity:ListIdentityPools");
        public static readonly ActionIdentifier UpdateIdentityPool = new ActionIdentifier("cognito-identity:UpdateIdentityPool");
    }
}

