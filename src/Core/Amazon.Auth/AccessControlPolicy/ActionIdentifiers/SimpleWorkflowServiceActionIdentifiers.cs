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
    /// The available AWS access control policy actions for Amazon Simple Workflow Service.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class SimpleWorkflowServiceActionIdentifiers
    {
        public static readonly ActionIdentifier AllSimpleWorkflowServiceActions = new ActionIdentifier("swf:*");

        public static readonly ActionIdentifier CancelTimer = new ActionIdentifier("swf:CancelTimer");
        public static readonly ActionIdentifier CancelWorkflowExecution = new ActionIdentifier("swf:CancelWorkflowExecution");
        public static readonly ActionIdentifier CompleteWorkflowExecution = new ActionIdentifier("swf:CompleteWorkflowExecution");
        public static readonly ActionIdentifier ContinueAsNewWorkflowExecution = new ActionIdentifier("swf:ContinueAsNewWorkflowExecution");
        public static readonly ActionIdentifier CountClosedWorkflowExecutions = new ActionIdentifier("swf:CountClosedWorkflowExecutions");
        public static readonly ActionIdentifier CountOpenWorkflowExecutions = new ActionIdentifier("swf:CountOpenWorkflowExecutions");
        public static readonly ActionIdentifier CountPendingActivityTasks = new ActionIdentifier("swf:CountPendingActivityTasks");
        public static readonly ActionIdentifier CountPendingDecisionTasks = new ActionIdentifier("swf:CountPendingDecisionTasks");
        public static readonly ActionIdentifier DeprecateActivityType = new ActionIdentifier("swf:DeprecateActivityType");
        public static readonly ActionIdentifier DeprecateDomain = new ActionIdentifier("swf:DeprecateDomain");
        public static readonly ActionIdentifier DeprecateWorkflowType = new ActionIdentifier("swf:DeprecateWorkflowType");
        public static readonly ActionIdentifier DescribeActivityType = new ActionIdentifier("swf:DescribeActivityType");
        public static readonly ActionIdentifier DescribeDomain = new ActionIdentifier("swf:DescribeDomain");
        public static readonly ActionIdentifier DescribeWorkflowExecution = new ActionIdentifier("swf:DescribeWorkflowExecution");
        public static readonly ActionIdentifier DescribeWorkflowType = new ActionIdentifier("swf:DescribeWorkflowType");
        public static readonly ActionIdentifier FailWorkflowExecution = new ActionIdentifier("swf:FailWorkflowExecution");
        public static readonly ActionIdentifier GetWorkflowExecutionHistory = new ActionIdentifier("swf:GetWorkflowExecutionHistory");
        public static readonly ActionIdentifier ListActivityTypes = new ActionIdentifier("swf:ListActivityTypes");
        public static readonly ActionIdentifier ListClosedWorkflowExecutions = new ActionIdentifier("swf:ListClosedWorkflowExecutions");
        public static readonly ActionIdentifier ListDomains = new ActionIdentifier("swf:ListDomains");
        public static readonly ActionIdentifier ListOpenWorkflowExecutions = new ActionIdentifier("swf:ListOpenWorkflowExecutions");
        public static readonly ActionIdentifier ListWorkflowTypes = new ActionIdentifier("swf:ListWorkflowTypes");
        public static readonly ActionIdentifier PollForActivityTask = new ActionIdentifier("swf:PollForActivityTask");
        public static readonly ActionIdentifier PollForDecisionTask = new ActionIdentifier("swf:PollForDecisionTask");
        public static readonly ActionIdentifier RecordActivityTaskHeartbeat = new ActionIdentifier("swf:RecordActivityTaskHeartbeat");
        public static readonly ActionIdentifier RecordMarker = new ActionIdentifier("swf:RecordMarker");
        public static readonly ActionIdentifier RegisterActivityType = new ActionIdentifier("swf:RegisterActivityType");
        public static readonly ActionIdentifier RegisterDomain = new ActionIdentifier("swf:RegisterDomain");
        public static readonly ActionIdentifier RegisterWorkflowType = new ActionIdentifier("swf:RegisterWorkflowType");
        public static readonly ActionIdentifier RequestCancelActivityTask = new ActionIdentifier("swf:RequestCancelActivityTask");
        public static readonly ActionIdentifier RequestCancelExternalWorkflowExecution = new ActionIdentifier("swf:RequestCancelExternalWorkflowExecution");
        public static readonly ActionIdentifier RequestCancelWorkflowExecution = new ActionIdentifier("swf:RequestCancelWorkflowExecution");
        public static readonly ActionIdentifier RespondActivityTaskCanceled = new ActionIdentifier("swf:RespondActivityTaskCanceled");
        public static readonly ActionIdentifier RespondActivityTaskCompleted = new ActionIdentifier("swf:RespondActivityTaskCompleted");
        public static readonly ActionIdentifier RespondActivityTaskFailed = new ActionIdentifier("swf:RespondActivityTaskFailed");
        public static readonly ActionIdentifier RespondDecisionTaskCompleted = new ActionIdentifier("swf:RespondDecisionTaskCompleted");
        public static readonly ActionIdentifier ScheduleActivityTask = new ActionIdentifier("swf:ScheduleActivityTask");
        public static readonly ActionIdentifier SignalExternalWorkflowExecution = new ActionIdentifier("swf:SignalExternalWorkflowExecution");
        public static readonly ActionIdentifier SignalWorkflowExecution = new ActionIdentifier("swf:SignalWorkflowExecution");
        public static readonly ActionIdentifier StartChildWorkflowExecution = new ActionIdentifier("swf:StartChildWorkflowExecution");
        public static readonly ActionIdentifier StartTimer = new ActionIdentifier("swf:StartTimer");
        public static readonly ActionIdentifier StartWorkflowExecution = new ActionIdentifier("swf:StartWorkflowExecution");
        public static readonly ActionIdentifier TerminateWorkflowExecution = new ActionIdentifier("swf:TerminateWorkflowExecution");
    }
}

