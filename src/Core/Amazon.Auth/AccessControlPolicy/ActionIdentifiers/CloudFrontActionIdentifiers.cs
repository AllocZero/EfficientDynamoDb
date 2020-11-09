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
    /// The available AWS access control policy actions for Amazon CloudFront.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CloudFrontActionIdentifiers
    {
        public static readonly ActionIdentifier AllCloudFrontActions = new ActionIdentifier("cloudfront:*");

        public static readonly ActionIdentifier CreateCloudFrontOriginAccessIdentity = new ActionIdentifier("cloudfront:CreateCloudFrontOriginAccessIdentity");
        public static readonly ActionIdentifier CreateDistribution = new ActionIdentifier("cloudfront:CreateDistribution");
        public static readonly ActionIdentifier CreateInvalidation = new ActionIdentifier("cloudfront:CreateInvalidation");
        public static readonly ActionIdentifier CreateStreamingDistribution = new ActionIdentifier("cloudfront:CreateStreamingDistribution");
        public static readonly ActionIdentifier DeleteCloudFrontOriginAccessIdentity = new ActionIdentifier("cloudfront:DeleteCloudFrontOriginAccessIdentity");
        public static readonly ActionIdentifier DeleteDistribution = new ActionIdentifier("cloudfront:DeleteDistribution");
        public static readonly ActionIdentifier DeleteStreamingDistribution = new ActionIdentifier("cloudfront:DeleteStreamingDistribution");
        public static readonly ActionIdentifier GetCloudFrontOriginAccessIdentity = new ActionIdentifier("cloudfront:GetCloudFrontOriginAccessIdentity");
        public static readonly ActionIdentifier GetCloudFrontOriginAccessIdentityConfig = new ActionIdentifier("cloudfront:GetCloudFrontOriginAccessIdentityConfig");
        public static readonly ActionIdentifier GetDistribution = new ActionIdentifier("cloudfront:GetDistribution");
        public static readonly ActionIdentifier GetDistributionConfig = new ActionIdentifier("cloudfront:GetDistributionConfig");
        public static readonly ActionIdentifier GetInvalidation = new ActionIdentifier("cloudfront:GetInvalidation");
        public static readonly ActionIdentifier GetStreamingDistribution = new ActionIdentifier("cloudfront:GetStreamingDistribution");
        public static readonly ActionIdentifier GetStreamingDistributionConfig = new ActionIdentifier("cloudfront:GetStreamingDistributionConfig");
        public static readonly ActionIdentifier ListCloudFrontOriginAccessIdentities = new ActionIdentifier("cloudfront:ListCloudFrontOriginAccessIdentities");
        public static readonly ActionIdentifier ListDistributions = new ActionIdentifier("cloudfront:ListDistributions");
        public static readonly ActionIdentifier ListInvalidations = new ActionIdentifier("cloudfront:ListInvalidations");
        public static readonly ActionIdentifier ListStreamingDistributions = new ActionIdentifier("cloudfront:ListStreamingDistributions");
        public static readonly ActionIdentifier UpdateCloudFrontOriginAccessIdentity = new ActionIdentifier("cloudfront:UpdateCloudFrontOriginAccessIdentity");
        public static readonly ActionIdentifier UpdateDistribution = new ActionIdentifier("cloudfront:UpdateDistribution");
        public static readonly ActionIdentifier UpdateStreamingDistribution = new ActionIdentifier("cloudfront:UpdateStreamingDistribution");
    }
}

