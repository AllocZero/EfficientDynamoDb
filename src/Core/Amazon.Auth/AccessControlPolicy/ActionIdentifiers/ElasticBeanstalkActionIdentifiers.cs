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
    /// The available AWS access control policy actions for AWS Elastic Beanstalk.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class ElasticBeanstalkActionIdentifiers
    {
        public static readonly ActionIdentifier AllElasticBeanstalkActions = new ActionIdentifier("elasticbeanstalk:*");

        public static readonly ActionIdentifier CheckDNSAvailability = new ActionIdentifier("elasticbeanstalk:CheckDNSAvailability");
        public static readonly ActionIdentifier CreateApplication = new ActionIdentifier("elasticbeanstalk:CreateApplication");
        public static readonly ActionIdentifier CreateApplicationVersion = new ActionIdentifier("elasticbeanstalk:CreateApplicationVersion");
        public static readonly ActionIdentifier CreateConfigurationTemplate = new ActionIdentifier("elasticbeanstalk:CreateConfigurationTemplate");
        public static readonly ActionIdentifier CreateEnvironment = new ActionIdentifier("elasticbeanstalk:CreateEnvironment");
        public static readonly ActionIdentifier CreateStorageLocation = new ActionIdentifier("elasticbeanstalk:CreateStorageLocation");
        public static readonly ActionIdentifier DeleteApplication = new ActionIdentifier("elasticbeanstalk:DeleteApplication");
        public static readonly ActionIdentifier DeleteApplicationVersion = new ActionIdentifier("elasticbeanstalk:DeleteApplicationVersion");
        public static readonly ActionIdentifier DeleteConfigurationTemplate = new ActionIdentifier("elasticbeanstalk:DeleteConfigurationTemplate");
        public static readonly ActionIdentifier DeleteEnvironmentConfiguration = new ActionIdentifier("elasticbeanstalk:DeleteEnvironmentConfiguration");
        public static readonly ActionIdentifier DescribeApplicationVersions = new ActionIdentifier("elasticbeanstalk:DescribeApplicationVersions");
        public static readonly ActionIdentifier DescribeApplications = new ActionIdentifier("elasticbeanstalk:DescribeApplications");
        public static readonly ActionIdentifier DescribeConfigurationOptions = new ActionIdentifier("elasticbeanstalk:DescribeConfigurationOptions");
        public static readonly ActionIdentifier DescribeConfigurationSettings = new ActionIdentifier("elasticbeanstalk:DescribeConfigurationSettings");
        public static readonly ActionIdentifier DescribeEnvironmentResources = new ActionIdentifier("elasticbeanstalk:DescribeEnvironmentResources");
        public static readonly ActionIdentifier DescribeEnvironments = new ActionIdentifier("elasticbeanstalk:DescribeEnvironments");
        public static readonly ActionIdentifier DescribeEvents = new ActionIdentifier("elasticbeanstalk:DescribeEvents");
        public static readonly ActionIdentifier ListAvailableSolutionStacks = new ActionIdentifier("elasticbeanstalk:ListAvailableSolutionStacks");
        public static readonly ActionIdentifier RebuildEnvironment = new ActionIdentifier("elasticbeanstalk:RebuildEnvironment");
        public static readonly ActionIdentifier RequestEnvironmentInfo = new ActionIdentifier("elasticbeanstalk:RequestEnvironmentInfo");
        public static readonly ActionIdentifier RestartAppServer = new ActionIdentifier("elasticbeanstalk:RestartAppServer");
        public static readonly ActionIdentifier RetrieveEnvironmentInfo = new ActionIdentifier("elasticbeanstalk:RetrieveEnvironmentInfo");
        public static readonly ActionIdentifier SwapEnvironmentCNAMEs = new ActionIdentifier("elasticbeanstalk:SwapEnvironmentCNAMEs");
        public static readonly ActionIdentifier TerminateEnvironment = new ActionIdentifier("elasticbeanstalk:TerminateEnvironment");
        public static readonly ActionIdentifier UpdateApplication = new ActionIdentifier("elasticbeanstalk:UpdateApplication");
        public static readonly ActionIdentifier UpdateApplicationVersion = new ActionIdentifier("elasticbeanstalk:UpdateApplicationVersion");
        public static readonly ActionIdentifier UpdateConfigurationTemplate = new ActionIdentifier("elasticbeanstalk:UpdateConfigurationTemplate");
        public static readonly ActionIdentifier UpdateEnvironment = new ActionIdentifier("elasticbeanstalk:UpdateEnvironment");
        public static readonly ActionIdentifier ValidateConfigurationSettings = new ActionIdentifier("elasticbeanstalk:ValidateConfigurationSettings");
    }
}

