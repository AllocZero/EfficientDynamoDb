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
    /// The available AWS access control policy actions for AWS OpsWorks.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class OpsWorksActionIdentifiers
    {
        public static readonly ActionIdentifier AllOpsWorksActions = new ActionIdentifier("opsworks:*");

        public static readonly ActionIdentifier AssignVolume = new ActionIdentifier("opsworks:AssignVolume");
        public static readonly ActionIdentifier AssociateElasticIp = new ActionIdentifier("opsworks:AssociateElasticIp");
        public static readonly ActionIdentifier AttachElasticLoadBalancer = new ActionIdentifier("opsworks:AttachElasticLoadBalancer");
        public static readonly ActionIdentifier CloneStack = new ActionIdentifier("opsworks:CloneStack");
        public static readonly ActionIdentifier CreateApp = new ActionIdentifier("opsworks:CreateApp");
        public static readonly ActionIdentifier CreateDeployment = new ActionIdentifier("opsworks:CreateDeployment");
        public static readonly ActionIdentifier CreateInstance = new ActionIdentifier("opsworks:CreateInstance");
        public static readonly ActionIdentifier CreateLayer = new ActionIdentifier("opsworks:CreateLayer");
        public static readonly ActionIdentifier CreateStack = new ActionIdentifier("opsworks:CreateStack");
        public static readonly ActionIdentifier CreateUserProfile = new ActionIdentifier("opsworks:CreateUserProfile");
        public static readonly ActionIdentifier DeleteApp = new ActionIdentifier("opsworks:DeleteApp");
        public static readonly ActionIdentifier DeleteInstance = new ActionIdentifier("opsworks:DeleteInstance");
        public static readonly ActionIdentifier DeleteLayer = new ActionIdentifier("opsworks:DeleteLayer");
        public static readonly ActionIdentifier DeleteStack = new ActionIdentifier("opsworks:DeleteStack");
        public static readonly ActionIdentifier DeleteUserProfile = new ActionIdentifier("opsworks:DeleteUserProfile");
        public static readonly ActionIdentifier DeregisterElasticIp = new ActionIdentifier("opsworks:DeregisterElasticIp");
        public static readonly ActionIdentifier DeregisterVolume = new ActionIdentifier("opsworks:DeregisterVolume");
        public static readonly ActionIdentifier DescribeApps = new ActionIdentifier("opsworks:DescribeApps");
        public static readonly ActionIdentifier DescribeCommands = new ActionIdentifier("opsworks:DescribeCommands");
        public static readonly ActionIdentifier DescribeDeployments = new ActionIdentifier("opsworks:DescribeDeployments");
        public static readonly ActionIdentifier DescribeElasticIps = new ActionIdentifier("opsworks:DescribeElasticIps");
        public static readonly ActionIdentifier DescribeElasticLoadBalancers = new ActionIdentifier("opsworks:DescribeElasticLoadBalancers");
        public static readonly ActionIdentifier DescribeInstances = new ActionIdentifier("opsworks:DescribeInstances");
        public static readonly ActionIdentifier DescribeLayers = new ActionIdentifier("opsworks:DescribeLayers");
        public static readonly ActionIdentifier DescribeLoadBasedAutoScaling = new ActionIdentifier("opsworks:DescribeLoadBasedAutoScaling");
        public static readonly ActionIdentifier DescribePermissions = new ActionIdentifier("opsworks:DescribePermissions");
        public static readonly ActionIdentifier DescribeRaidArrays = new ActionIdentifier("opsworks:DescribeRaidArrays");
        public static readonly ActionIdentifier DescribeServiceErrors = new ActionIdentifier("opsworks:DescribeServiceErrors");
        public static readonly ActionIdentifier DescribeStacks = new ActionIdentifier("opsworks:DescribeStacks");
        public static readonly ActionIdentifier DescribeTimeBasedAutoScaling = new ActionIdentifier("opsworks:DescribeTimeBasedAutoScaling");
        public static readonly ActionIdentifier DescribeUserProfiles = new ActionIdentifier("opsworks:DescribeUserProfiles");
        public static readonly ActionIdentifier DescribeVolumes = new ActionIdentifier("opsworks:DescribeVolumes");
        public static readonly ActionIdentifier DetachElasticLoadBalancer = new ActionIdentifier("opsworks:DetachElasticLoadBalancer");
        public static readonly ActionIdentifier DisassociateElasticIp = new ActionIdentifier("opsworks:DisassociateElasticIp");
        public static readonly ActionIdentifier GetHostnameSuggestion = new ActionIdentifier("opsworks:GetHostnameSuggestion");
        public static readonly ActionIdentifier RebootInstance = new ActionIdentifier("opsworks:RebootInstance");
        public static readonly ActionIdentifier RegisterElasticIp = new ActionIdentifier("opsworks:RegisterElasticIp");
        public static readonly ActionIdentifier RegisterVolume = new ActionIdentifier("opsworks:RegisterVolume");
        public static readonly ActionIdentifier SetLoadBasedAutoScaling = new ActionIdentifier("opsworks:SetLoadBasedAutoScaling");
        public static readonly ActionIdentifier SetPermission = new ActionIdentifier("opsworks:SetPermission");
        public static readonly ActionIdentifier SetTimeBasedAutoScaling = new ActionIdentifier("opsworks:SetTimeBasedAutoScaling");
        public static readonly ActionIdentifier StartInstance = new ActionIdentifier("opsworks:StartInstance");
        public static readonly ActionIdentifier StartStack = new ActionIdentifier("opsworks:StartStack");
        public static readonly ActionIdentifier StopInstance = new ActionIdentifier("opsworks:StopInstance");
        public static readonly ActionIdentifier StopStack = new ActionIdentifier("opsworks:StopStack");
        public static readonly ActionIdentifier UnassignVolume = new ActionIdentifier("opsworks:UnassignVolume");
        public static readonly ActionIdentifier UpdateApp = new ActionIdentifier("opsworks:UpdateApp");
        public static readonly ActionIdentifier UpdateElasticIp = new ActionIdentifier("opsworks:UpdateElasticIp");
        public static readonly ActionIdentifier UpdateInstance = new ActionIdentifier("opsworks:UpdateInstance");
        public static readonly ActionIdentifier UpdateLayer = new ActionIdentifier("opsworks:UpdateLayer");
        public static readonly ActionIdentifier UpdateStack = new ActionIdentifier("opsworks:UpdateStack");
        public static readonly ActionIdentifier UpdateUserProfile = new ActionIdentifier("opsworks:UpdateUserProfile");
        public static readonly ActionIdentifier UpdateVolume = new ActionIdentifier("opsworks:UpdateVolume");
    }
}

