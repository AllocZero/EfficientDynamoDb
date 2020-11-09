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
    /// The available AWS access control policy actions for Amazon CloudWatch.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CloudWatchActionIdentifiers
    {
        public static readonly ActionIdentifier AllCloudWatchActions = new ActionIdentifier("cloudwatch:*");

        public static readonly ActionIdentifier DeleteAlarms = new ActionIdentifier("cloudwatch:DeleteAlarms");
        public static readonly ActionIdentifier DescribeAlarmHistory = new ActionIdentifier("cloudwatch:DescribeAlarmHistory");
        public static readonly ActionIdentifier DescribeAlarms = new ActionIdentifier("cloudwatch:DescribeAlarms");
        public static readonly ActionIdentifier DescribeAlarmsForMetric = new ActionIdentifier("cloudwatch:DescribeAlarmsForMetric");
        public static readonly ActionIdentifier DisableAlarmActions = new ActionIdentifier("cloudwatch:DisableAlarmActions");
        public static readonly ActionIdentifier EnableAlarmActions = new ActionIdentifier("cloudwatch:EnableAlarmActions");
        public static readonly ActionIdentifier GetMetricStatistics = new ActionIdentifier("cloudwatch:GetMetricStatistics");
        public static readonly ActionIdentifier ListMetrics = new ActionIdentifier("cloudwatch:ListMetrics");
        public static readonly ActionIdentifier PutMetricAlarm = new ActionIdentifier("cloudwatch:PutMetricAlarm");
        public static readonly ActionIdentifier PutMetricData = new ActionIdentifier("cloudwatch:PutMetricData");
        public static readonly ActionIdentifier SetAlarmState = new ActionIdentifier("cloudwatch:SetAlarmState");
    }
}

