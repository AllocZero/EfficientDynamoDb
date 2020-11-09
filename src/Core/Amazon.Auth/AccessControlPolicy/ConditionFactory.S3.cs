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

namespace Amazon.Auth.AccessControlPolicy
{
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static partial class ConditionFactory
    {
        /// <summary>
        /// Condition key for the canned ACL specified by a request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_CANNED_ACL_CONDITION_KEY = "s3:x-amz-acl";

        /// <summary>
        /// Condition key for the location constraint specified by a request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_LOCATION_CONSTRAINT_CONDITION_KEY = "s3:LocationConstraint";

        /// <summary>
        /// Condition key for the prefix specified by a request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_PREFIX_CONDITION_KEY = "s3:prefix";

        /// <summary>
        /// Condition key for the delimiter specified by a request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_DELIMITER_CONDITION_KEY = "s3:delimiter";

        /// <summary>
        /// Condition key for the max keys specified by a request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_MAX_KEYS_CONDITION_KEY = "s3:max-keys";

        /// <summary>
        /// Condition key for the source object specified by a request to copy an
        /// object.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_COPY_SOURCE_CONDITION_KEY = "s3:x-amz-copy-source";

        /// <summary>
        /// Condition key for the metadata directive specified by a request to copy
        /// an object.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_METADATA_DIRECTIVE_CONDITION_KEY = "s3:x-amz-metadata-directive";

        /// <summary>
        /// Condition key for the version ID of an object version specified by a
        /// request.
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string S3_VERSION_ID_CONDITION_KEY = "s3:VersionId";

        /// <summary>
        /// Constructs a new access policy condition that compares an Amazon S3
        /// canned ACL with the canned ACL specified by an incoming request.
        /// <para>
        /// You can use this condition to ensure that any objects uploaded to an
        /// Amazon S3 bucket have a specific canned ACL set.
        /// </para>
        /// </summary>
        /// <param name="cannedAcl">The Amazon S3 canned ACL to compare against.</param>
        /// <returns>A new access control policy condition that compares the Amazon S3
        ///         canned ACL specified in incoming requests against the value
        ///         specified.</returns>
        public static Condition NewCannedACLCondition(string cannedAcl)
        {
            return ConditionFactory.NewCondition(StringComparisonType.StringEquals, S3_CANNED_ACL_CONDITION_KEY, cannedAcl);
        }
    }
}

