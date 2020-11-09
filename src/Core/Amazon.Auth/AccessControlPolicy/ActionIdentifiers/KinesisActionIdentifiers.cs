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
    /// The available AWS access control policy actions for Amazon Kinesis.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class KinesisActionIdentifiers
    {
        public static readonly ActionIdentifier AllKinesisActions = new ActionIdentifier("kinesis:*");

        public static readonly ActionIdentifier CreateStream = new ActionIdentifier("kinesis:CreateStream");
        public static readonly ActionIdentifier DeleteStream = new ActionIdentifier("kinesis:DeleteStream");
        public static readonly ActionIdentifier DescribeStream = new ActionIdentifier("kinesis:DescribeStream");
        public static readonly ActionIdentifier ListStreams = new ActionIdentifier("kinesis:ListStreams");
        public static readonly ActionIdentifier PutRecord = new ActionIdentifier("kinesis:PutRecord");
        public static readonly ActionIdentifier GetShardIterator = new ActionIdentifier("kinesis:GetShardIterator");
        public static readonly ActionIdentifier GetRecords = new ActionIdentifier("kinesis:GetRecords");
        public static readonly ActionIdentifier MergeShards = new ActionIdentifier("kinesis:MergeShards");
        public static readonly ActionIdentifier SplitShard = new ActionIdentifier("kinesis:SplitShard");
    }
}

