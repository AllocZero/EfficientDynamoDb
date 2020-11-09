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
using System.Globalization;
using System.Text;

namespace Amazon.Auth.AccessControlPolicy
{
    /// <summary>
    /// A factory for creating resources to be used in the policy.
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class ResourceFactory
    {
        /// <summary>
        /// Constructs a new bucket resource that represents the the specified bucket
        /// but <b>not any of the contained objects</b>.
        /// </summary>
        /// <param name="bucketName">The name of the bucket represented by this AWS access control
        ///            policy resource.</param>
        public static Resource NewS3BucketResource(string bucketName)
        {
            if (bucketName == null)
            {
                throw new ArgumentNullException("bucketName");
            }
 
            return new Resource("arn:aws:s3:::" + bucketName);
        }

        /// <summary>
        /// Constructs a new object resource that represents the specified objects.
        /// The keyPattern argument may contain the '*' wildcard to match multiple
        /// objects. For example, an object resource created for bucket 'mybucket'
        /// and key pattern 'foo*' will match any object stored in 'mybucket' with a
        /// key that starts with 'foo'.
        /// </summary>
        /// <param name="bucketName">The name of the bucket containing the object or objects
        ///            represented by this resource.</param>
        /// <param name="keyPattern">The key or key pattern, which can optionally contain the '*'
        ///            wildcard to include multiple objects in the resource.</param>
        public static Resource NewS3ObjectResource(string bucketName, string keyPattern)
        {
            if (bucketName == null)
            {
                throw new ArgumentNullException("bucketName");
            }
            if (keyPattern == null)
            {
                throw new ArgumentNullException("keyPattern");
            }

            return new Resource(string.Format(CultureInfo.InvariantCulture, "arn:aws:s3:::{0}/{1}", bucketName, keyPattern));
        }

        /// <summary>
        /// Constructs a new SQS queue resource for an access control policy. A
        /// policy statement using this resource will allow or deny actions on the
        /// specified queue.
        /// </summary>
        /// <param name="accountId">The AWS account ID of the queue owner.</param>
        /// <param name="queueName">The name of the Amazon SQS queue.</param>
        public static Resource NewSQSQueueResource(string accountId, string queueName)
        {
            return new Resource("/" + FormatAccountId(accountId) + "/" + queueName);
        }

        private static string FormatAccountId(string accountId)
        {
            if (accountId == null)
            {
                throw new ArgumentNullException("accountId");
            }

            return accountId.Trim().Replace("-", "");
        }

    }
}

