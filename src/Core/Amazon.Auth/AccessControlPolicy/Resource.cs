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
    /// <summary>
    /// Represents a resource involved in an AWS access control policy statement.
    /// Resources are the service specific AWS entities owned by your account. Amazon
    /// SQS queues, Amazon S3 buckets and objects, and Amazon SNS topics are all
    /// examples of AWS resources.
    /// <para>
    /// The standard way of specifying an AWS resource is with an Amazon Resource
    /// Name (ARN).
    /// </para>
    /// <para>
    /// The resource is C in the statement
    /// "A has permission to do B to C where D applies."
    /// </para>
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public class Resource
    {
        private string resource;

        /// <summary>
        /// Constructs a new AWS access control policy resource. Resources are
        /// typically specified as Amazon Resource Names (ARNs).
        /// <para>
        /// You specify the resource using the following Amazon Resource Name (ARN)
        /// format: arn:aws:&lt;vendor>:&lt;region>:&lt;namespace>:&lt;relative-id>
        /// <list type="bullet">
        ///     <item>
        ///         <description>>vendor identifies the AWS product (e.g., sns)</description>
        ///     </item>
        ///     <item>
        ///         <description>region is the AWS Region the resource resides in (e.g., us-east-1), if any</description>
        ///     </item>
        ///     <item>
        ///         <description>namespace is the AWS account ID with no hyphens (e.g., 123456789012)</description>
        ///     </item>
        ///     <item>
        ///         <description>relative-id is the service specific portion that identifies the specific resource</description>
        ///     </item>
        /// </list>
        /// </para>
        /// <para>
        /// For example, an Amazon SQS queue might be addressed with the following
        /// ARN: arn:aws:sqs:us-east-1:987654321000:MyQueue
        /// </para>
        /// <para>
        /// Some resources may not use every field in an ARN. For example, resources
        /// in Amazon S3 are global, so they omit the region field:
        /// arn:aws:s3:::bucket/*
        /// </para>
        /// </summary>
        /// <param name="resource">The Amazon Resource Name (ARN) uniquely identifying the desired AWS resource.</param>
        public Resource(string resource)
        {
            this.resource = resource;
        }

        /// <summary>
        /// Gets the resource ID, typically an Amazon Resource Name (ARN),
        /// identifying this resource.
        /// </summary>
        public string Id
        {
            get
            {
                return resource;
            }
        }
    }
}

