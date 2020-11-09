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
    /// The available AWS access control policy actions for AWS CloudFormation.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CloudFormationActionIdentifiers
    {
        public static readonly ActionIdentifier AllCloudFormationActions = new ActionIdentifier("cloudformation:*");

        public static readonly ActionIdentifier CreateStack = new ActionIdentifier("cloudformation:CreateStack");
        public static readonly ActionIdentifier DeleteStack = new ActionIdentifier("cloudformation:DeleteStack");
        public static readonly ActionIdentifier DescribeStackEvents = new ActionIdentifier("cloudformation:DescribeStackEvents");
        public static readonly ActionIdentifier DescribeStackResource = new ActionIdentifier("cloudformation:DescribeStackResource");
        public static readonly ActionIdentifier DescribeStackResources = new ActionIdentifier("cloudformation:DescribeStackResources");
        public static readonly ActionIdentifier DescribeStacks = new ActionIdentifier("cloudformation:DescribeStacks");
        public static readonly ActionIdentifier EstimateTemplateCost = new ActionIdentifier("cloudformation:EstimateTemplateCost");
        public static readonly ActionIdentifier GetTemplate = new ActionIdentifier("cloudformation:GetTemplate");
        public static readonly ActionIdentifier ListStacks = new ActionIdentifier("cloudformation:ListStacks");
        public static readonly ActionIdentifier ListStackResources = new ActionIdentifier("cloudformation:ListStackResources");
        public static readonly ActionIdentifier UpdateStack = new ActionIdentifier("cloudformation:UpdateStack");
        public static readonly ActionIdentifier ValidateTemplate = new ActionIdentifier("cloudformation:ValidateTemplate");
    }
}

