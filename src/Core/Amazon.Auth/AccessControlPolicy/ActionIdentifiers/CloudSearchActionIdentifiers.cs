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
    /// The available AWS access control policy actions for Amazon CloudSearch.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class CloudSearchActionIdentifiers
    {
        public static readonly ActionIdentifier AllCloudSearchActions = new ActionIdentifier("cloudsearch:*");

        public static readonly ActionIdentifier BuildSuggesters = new ActionIdentifier("cloudsearch:BuildSuggesters");
        public static readonly ActionIdentifier CreateDomain = new ActionIdentifier("cloudsearch:CreateDomain");
        public static readonly ActionIdentifier DefineAnalysisScheme = new ActionIdentifier("cloudsearch:DefineAnalysisScheme");
        public static readonly ActionIdentifier DefineExpression = new ActionIdentifier("cloudsearch:DefineExpression");
        public static readonly ActionIdentifier DefineIndexField = new ActionIdentifier("cloudsearch:DefineIndexField");
        public static readonly ActionIdentifier DefineSuggester = new ActionIdentifier("cloudsearch:DefineSuggester");
        public static readonly ActionIdentifier DeleteAnalysisScheme = new ActionIdentifier("cloudsearch:DeleteAnalysisScheme");
        public static readonly ActionIdentifier DeleteDomain = new ActionIdentifier("cloudsearch:DeleteDomain");
        public static readonly ActionIdentifier DeleteExpression = new ActionIdentifier("cloudsearch:DeleteExpression");
        public static readonly ActionIdentifier DeleteIndexField = new ActionIdentifier("cloudsearch:DeleteIndexField");
        public static readonly ActionIdentifier DeleteSuggester = new ActionIdentifier("cloudsearch:DeleteSuggester");
        public static readonly ActionIdentifier DescribeAnalysisSchemes = new ActionIdentifier("cloudsearch:DescribeAnalysisSchemes");
        public static readonly ActionIdentifier DescribeAvailabilityOptions = new ActionIdentifier("cloudsearch:DescribeAvailabilityOptions");
        public static readonly ActionIdentifier DescribeDomains = new ActionIdentifier("cloudsearch:DescribeDomains");
        public static readonly ActionIdentifier DescribeExpressions = new ActionIdentifier("cloudsearch:DescribeExpressions");
        public static readonly ActionIdentifier DescribeIndexFields = new ActionIdentifier("cloudsearch:DescribeIndexFields");
        public static readonly ActionIdentifier DescribeScalingParameters = new ActionIdentifier("cloudsearch:DescribeScalingParameters");
        public static readonly ActionIdentifier DescribeServiceAccessPolicies = new ActionIdentifier("cloudsearch:DescribeServiceAccessPolicies");
        public static readonly ActionIdentifier DescribeSuggesters = new ActionIdentifier("cloudsearch:DescribeSuggesters");
        public static readonly ActionIdentifier IndexDocuments = new ActionIdentifier("cloudsearch:IndexDocuments");
        public static readonly ActionIdentifier ListDomainNames = new ActionIdentifier("cloudsearch:ListDomainNames");
        public static readonly ActionIdentifier UpdateAvailabilityOptions = new ActionIdentifier("cloudsearch:UpdateAvailabilityOptions");
        public static readonly ActionIdentifier UpdateScalingParameters = new ActionIdentifier("cloudsearch:UpdateScalingParameters");
        public static readonly ActionIdentifier UpdateServiceAccessPolicies = new ActionIdentifier("cloudsearch:UpdateServiceAccessPolicies");
    }
}

