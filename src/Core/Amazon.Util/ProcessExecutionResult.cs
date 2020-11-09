/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */

namespace Amazon.Util
{
    /// <summary>
    /// Class that contains result of executing an external process through the SDKs
    /// AWSSDKUtils.RunProcess/AWSSDKUtils.RunProcessAsync
    /// </summary>
    public class ProcessExecutionResult
    {
        /// <summary>
        /// The exit code with which the process exited
        /// </summary>
        public int ExitCode { get; set; }
        /// <summary>
        /// The output of the process read till the end
        /// </summary>
        public string StandardOutput { get; set; }
        /// <summary>
        /// The error output of the process read till the end
        /// </summary>
        public string StandardError { get; set; }
    }
}
