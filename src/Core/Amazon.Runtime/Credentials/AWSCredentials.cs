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

namespace Amazon.Runtime
{
    /// <summary>
    /// Abstract class that represents a credentials object for AWS services.
    /// </summary>
    public abstract class AWSCredentials
    {
        /// <summary>
        /// Returns a copy of ImmutableCredentials
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableCredentials GetCredentials();

        /// <summary>
        /// Called by AmazonServiceClient to validate the credential state
        /// on client construction.
        /// </summary>
        protected virtual void Validate() { }

#if AWS_ASYNC_API
        public virtual System.Threading.Tasks.Task<ImmutableCredentials> GetCredentialsAsync()
        {
            return System.Threading.Tasks.Task.FromResult<ImmutableCredentials>(this.GetCredentials());
        }
#endif
    }
}
