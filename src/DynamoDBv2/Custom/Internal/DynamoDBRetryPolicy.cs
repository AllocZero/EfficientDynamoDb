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
using System.Linq;
using System.Text;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.Internal
{
    /// <summary>
    /// Custom DynamoDB retry policy.
    /// This policy has a slightly different exponential backoff logic compared
    /// to the default retry policy.
    /// </summary>
    public class DynamoDBRetryPolicy : DefaultRetryPolicy
    {
        /// <summary>
        /// Constructor for DynamoDBRRetryPolicy.
        /// </summary>
        /// <param name="config">The IClientConfig object</param>
        public DynamoDBRetryPolicy(IClientConfig config) :
            base(config)
        {
            ThrottlingErrorCodes.Add("TransactionInProgressException");
               
            //When derived from DefaultRetryPolicy, we are in legacy retry 
            //mode. When in legacy retry mode MaxErrorRetry used to be set
            //to 10 in the DynamoDB and DynamoDBStreams configs. This
            //can no longer be set in the configs because the retry mode
            //may not be known at that point where standard and adaptive 
            //retry modes are not to have this default.
            if(!config.IsMaxErrorRetrySet)
            {
                this.MaxRetries = 10;
            }
        }

        /// <summary>
        /// Overriden to cause a pause between retries.
        /// </summary>
        /// <param name="executionContext"></param>
        public override void WaitBeforeRetry(IExecutionContext executionContext)
        {
            pauseExponentially(executionContext.RequestContext.Retries);
        }

        /// <summary>
        /// Override the pausing function so retries would happen more frequent then the default operation.
        /// </summary>
        /// <param name="retries">Current number of retries.</param>
        private void pauseExponentially(int retries)
        {
            int delay;
               
            if (retries <= 0)       delay = 0;
            else if (retries < 20)  delay = Convert.ToInt32(Math.Pow(2, retries - 1) * 50.0);
            else                    delay = Int32.MaxValue;

            if (retries > 0 && (delay > MaxBackoffInMilliseconds || delay <= 0))
                delay = MaxBackoffInMilliseconds;
            Amazon.Util.AWSSDKUtils.Sleep(delay);
        }
    }
}
