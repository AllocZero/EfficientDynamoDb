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
    /// The available AWS access control policy actions for Amazon Zocalo.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class ZocaloActionIdentifiers
    {
        public static readonly ActionIdentifier AllZocaloActions = new ActionIdentifier("zocalo:*");

        public static readonly ActionIdentifier ActivateUser = new ActionIdentifier("zocalo:ActivateUser");
        public static readonly ActionIdentifier AddUserToGroup = new ActionIdentifier("zocalo:AddUserToGroup");
        public static readonly ActionIdentifier CheckAlias = new ActionIdentifier("zocalo:CheckAlias");
        public static readonly ActionIdentifier CreateInstance = new ActionIdentifier("zocalo:CreateInstance");
        public static readonly ActionIdentifier DeactivateUser = new ActionIdentifier("zocalo:DeactivateUser");
        public static readonly ActionIdentifier DeleteInstance = new ActionIdentifier("zocalo:DeleteInstance");
        public static readonly ActionIdentifier DeregisterDirectory = new ActionIdentifier("zocalo:DeregisterDirectory");
        public static readonly ActionIdentifier DescribeAvailableDirectories = new ActionIdentifier("zocalo:DescribeAvailableDirectories");
        public static readonly ActionIdentifier DescribeInstances = new ActionIdentifier("zocalo:DescribeInstances");
        public static readonly ActionIdentifier RegisterDirectory = new ActionIdentifier("zocalo:RegisterDirectory");
        public static readonly ActionIdentifier RemoveUserFromGroup = new ActionIdentifier("zocalo:RemoveUserFromGroup");
        public static readonly ActionIdentifier UpdateInstanceAlias = new ActionIdentifier("zocalo:UpdateInstanceAlias");
    }
}

