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
    /// The available AWS access control policy actions for Amazon Route 53.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class Route53ActionIdentifiers
    {
        public static readonly ActionIdentifier AllRoute53Actions = new ActionIdentifier("route53:*");

        public static readonly ActionIdentifier ChangeResourceRecordSets = new ActionIdentifier("route53:ChangeResourceRecordSets");
        public static readonly ActionIdentifier CreateHostedZone = new ActionIdentifier("route53:CreateHostedZone");
        public static readonly ActionIdentifier DeleteHostedZone = new ActionIdentifier("route53:DeleteHostedZone");
        public static readonly ActionIdentifier GetChange = new ActionIdentifier("route53:GetChange");
        public static readonly ActionIdentifier GetHostedZone = new ActionIdentifier("route53:GetHostedZone");
        public static readonly ActionIdentifier ListHostedZones = new ActionIdentifier("route53:ListHostedZones");
        public static readonly ActionIdentifier ListResourceRecordSets = new ActionIdentifier("route53:ListResourceRecordSets");
    }
}

