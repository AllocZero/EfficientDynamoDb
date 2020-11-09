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
    /// The available AWS access control policy actions for Amazon SQS.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class SQSActionIdentifiers
    {
        public static readonly ActionIdentifier AllSQSActions = new ActionIdentifier("sqs:*");

        public static readonly ActionIdentifier AddPermission = new ActionIdentifier("sqs:AddPermission");
        public static readonly ActionIdentifier ChangeMessageVisibility = new ActionIdentifier("sqs:ChangeMessageVisibility");
        public static readonly ActionIdentifier CreateQueue = new ActionIdentifier("sqs:CreateQueue");
        public static readonly ActionIdentifier DeleteMessage = new ActionIdentifier("sqs:DeleteMessage");
        public static readonly ActionIdentifier DeleteQueue = new ActionIdentifier("sqs:DeleteQueue");
        public static readonly ActionIdentifier GetQueueAttributes = new ActionIdentifier("sqs:GetQueueAttributes");
        public static readonly ActionIdentifier GetQueueUrl = new ActionIdentifier("sqs:GetQueueUrl");
        public static readonly ActionIdentifier ListQueues = new ActionIdentifier("sqs:ListQueues");
        public static readonly ActionIdentifier ReceiveMessage = new ActionIdentifier("sqs:ReceiveMessage");
        public static readonly ActionIdentifier RemovePermission = new ActionIdentifier("sqs:RemovePermission");
        public static readonly ActionIdentifier SendMessage = new ActionIdentifier("sqs:SendMessage");
        public static readonly ActionIdentifier SetQueueAttributes = new ActionIdentifier("sqs:SetQueueAttributes");
    }
}

