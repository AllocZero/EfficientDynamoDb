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
    /// The available AWS access control policy actions for Amazon CloudWatch Logs.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CloudWatchLogsActionIdentifiers
    {
        public static readonly ActionIdentifier AllCloudWatchLogsActions = new ActionIdentifier("logs:*");

        public static readonly ActionIdentifier CreateLogGroup = new ActionIdentifier("logs:CreateLogGroup");
        public static readonly ActionIdentifier CreateLogStream = new ActionIdentifier("logs:CreateLogStream");
        public static readonly ActionIdentifier DeleteLogGroup = new ActionIdentifier("logs:DeleteLogGroup");
        public static readonly ActionIdentifier DeleteLogStream = new ActionIdentifier("logs:DeleteLogStream");
        public static readonly ActionIdentifier DeleteMetricFilter = new ActionIdentifier("logs:DeleteMetricFilter");
        public static readonly ActionIdentifier DeleteRetentionPolicy = new ActionIdentifier("logs:DeleteRetentionPolicy");
        public static readonly ActionIdentifier DescribeLogGroups = new ActionIdentifier("logs:DescribeLogGroups");
        public static readonly ActionIdentifier DescribeLogStreams = new ActionIdentifier("logs:DescribeLogStreams");
        public static readonly ActionIdentifier DescribeMetricFilters = new ActionIdentifier("logs:DescribeMetricFilters");
        public static readonly ActionIdentifier GetLogEvents = new ActionIdentifier("logs:GetLogEvents");
        public static readonly ActionIdentifier PutLogEvents = new ActionIdentifier("logs:PutLogEvents");
        public static readonly ActionIdentifier PutMetricFilter = new ActionIdentifier("logs:PutMetricFilter");
        public static readonly ActionIdentifier PutRetentionPolicy = new ActionIdentifier("logs:PutRetentionPolicy");
        public static readonly ActionIdentifier TestMetricFilter = new ActionIdentifier("logs:TestMetricFilter");
    }
}

