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
    /// The available AWS access control policy actions for Amazon RDS.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class RDSActionIdentifiers
    {
        public static readonly ActionIdentifier AllRDSActions = new ActionIdentifier("rds:*");

        public static readonly ActionIdentifier AuthorizeDBSecurityGroupIngress = new ActionIdentifier("rds:AuthorizeDBSecurityGroupIngress");
        public static readonly ActionIdentifier AddTagsToResource = new ActionIdentifier("rds:AddTagsToResource");
        public static readonly ActionIdentifier AddSourceIdentifierToSubscription = new ActionIdentifier("rds:AddSourceIdentifierToSubscription");
        public static readonly ActionIdentifier CopyDBSnapshot = new ActionIdentifier("rds:CopyDBSnapshot");
        public static readonly ActionIdentifier CreateDBInstance = new ActionIdentifier("rds:CreateDBInstance");
        public static readonly ActionIdentifier CreateDBInstanceReadReplica = new ActionIdentifier("rds:CreateDBInstanceReadReplica");
        public static readonly ActionIdentifier CreateDBParameterGroup = new ActionIdentifier("rds:CreateDBParameterGroup");
        public static readonly ActionIdentifier CreateDBSecurityGroup = new ActionIdentifier("rds:CreateDBSecurityGroup");
        public static readonly ActionIdentifier CreateDBSnapshot = new ActionIdentifier("rds:CreateDBSnapshot");
        public static readonly ActionIdentifier CreateDBSubnetGroup = new ActionIdentifier("rds:CreateDBSubnetGroup");
        public static readonly ActionIdentifier CreateEventSubscription = new ActionIdentifier("rds:CreateEventSubscription");
        public static readonly ActionIdentifier CreateOptionGroup = new ActionIdentifier("rds:CreateOptionGroup");
        public static readonly ActionIdentifier DeleteDBInstance = new ActionIdentifier("rds:DeleteDBInstance");
        public static readonly ActionIdentifier DeleteDBParameterGroup = new ActionIdentifier("rds:DeleteDBParameterGroup");
        public static readonly ActionIdentifier DeleteDBSecurityGroup = new ActionIdentifier("rds:DeleteDBSecurityGroup");
        public static readonly ActionIdentifier DeleteDBSnapshot = new ActionIdentifier("rds:DeleteDBSnapshot");
        public static readonly ActionIdentifier DeleteDBSubnetGroup = new ActionIdentifier("rds:DeleteDBSubnetGroup");
        public static readonly ActionIdentifier DeleteEventSubscription = new ActionIdentifier("rds:DeleteEventSubscription");
        public static readonly ActionIdentifier DeleteOptionGroup = new ActionIdentifier("rds:DeleteOptionGroup");
        public static readonly ActionIdentifier DescribeEngineDefaultParameters = new ActionIdentifier("rds:DescribeEngineDefaultParameters");
        public static readonly ActionIdentifier DescribeDBInstances = new ActionIdentifier("rds:DescribeDBInstances");
        public static readonly ActionIdentifier DescribeDBLogFiles = new ActionIdentifier("rds:DescribeDBLogFiles");
        public static readonly ActionIdentifier DescribeDBParameterGroups = new ActionIdentifier("rds:DescribeDBParameterGroups");
        public static readonly ActionIdentifier DescribeDBParameters = new ActionIdentifier("rds:DescribeDBParameters");
        public static readonly ActionIdentifier DescribeDBSecurityGroups = new ActionIdentifier("rds:DescribeDBSecurityGroups");
        public static readonly ActionIdentifier DescribeDBSnapshots = new ActionIdentifier("rds:DescribeDBSnapshots");
        public static readonly ActionIdentifier DescribeDBEngineVersions = new ActionIdentifier("rds:DescribeDBEngineVersions");
        public static readonly ActionIdentifier DescribeDBSubnetGroups = new ActionIdentifier("rds:DescribeDBSubnetGroups");
        public static readonly ActionIdentifier DescribeEventCategories = new ActionIdentifier("rds:DescribeEventCategories");
        public static readonly ActionIdentifier DescribeEvents = new ActionIdentifier("rds:DescribeEvents");
        public static readonly ActionIdentifier DescribeEventSubscriptions = new ActionIdentifier("rds:DescribeEventSubscriptions");
        public static readonly ActionIdentifier DescribeOptionGroups = new ActionIdentifier("rds:DescribeOptionGroups");
        public static readonly ActionIdentifier DescribeOptionGroupOptions = new ActionIdentifier("rds:DescribeOptionGroupOptions");
        public static readonly ActionIdentifier DescribeOrderableDBInstanceOptions = new ActionIdentifier("rds:DescribeOrderableDBInstanceOptions");
        public static readonly ActionIdentifier DescribeReservedDBInstances = new ActionIdentifier("rds:DescribeReservedDBInstances");
        public static readonly ActionIdentifier DescribeReservedDBInstancesOfferings = new ActionIdentifier("rds:DescribeReservedDBInstancesOfferings");
        public static readonly ActionIdentifier DownloadDBLogFilePortion = new ActionIdentifier("rds:DownloadDBLogFilePortion");
        public static readonly ActionIdentifier ListTagsForResource = new ActionIdentifier("rds:ListTagsForResource");
        public static readonly ActionIdentifier ModifyDBInstance = new ActionIdentifier("rds:ModifyDBInstance");
        public static readonly ActionIdentifier ModifyDBParameterGroup = new ActionIdentifier("rds:ModifyDBParameterGroup");
        public static readonly ActionIdentifier ModifyDBSubnetGroup = new ActionIdentifier("rds:ModifyDBSubnetGroup");
        public static readonly ActionIdentifier ModifyEventSubscription = new ActionIdentifier("rds:ModifyEventSubscription");
        public static readonly ActionIdentifier ModifyOptionGroup = new ActionIdentifier("rds:ModifyOptionGroup");
        public static readonly ActionIdentifier PromoteReadReplica = new ActionIdentifier("rds:PromoteReadReplica");
        public static readonly ActionIdentifier PurchaseReservedDBInstancesOffering = new ActionIdentifier("rds:PurchaseReservedDBInstancesOffering");
        public static readonly ActionIdentifier RebootDBInstance = new ActionIdentifier("rds:RebootDBInstance");
        public static readonly ActionIdentifier RemoveSourceIdentifierFromSubscription = new ActionIdentifier("rds:RemoveSourceIdentifierFromSubscription");
        public static readonly ActionIdentifier RemoveTagsFromResource = new ActionIdentifier("rds:RemoveTagsFromResource");
        public static readonly ActionIdentifier RestoreDBInstanceFromDBSnapshot = new ActionIdentifier("rds:RestoreDBInstanceFromDBSnapshot");
        public static readonly ActionIdentifier RestoreDBInstanceToPointInTime = new ActionIdentifier("rds:RestoreDBInstanceToPointInTime");
        public static readonly ActionIdentifier ResetDBParameterGroup = new ActionIdentifier("rds:ResetDBParameterGroup");
        public static readonly ActionIdentifier RevokeDBSecurityGroupIngress = new ActionIdentifier("rds:RevokeDBSecurityGroupIngress");
    }
}

