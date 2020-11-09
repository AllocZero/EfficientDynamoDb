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

using System.Collections.Generic;
using Amazon.Runtime.Internal;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Default ServiceMetadata implementation.
    /// This implementation will be used if the service doesn't have a
    /// IServiceMetadata implementation.
    /// </summary>
    internal class ServiceMetadata : IServiceMetadata
    {
        /// <summary>
        /// Gets the value of the Service Id.
        /// </summary>
        public string ServiceId { get; }

        /// <summary>
        /// Gets the dictionary that gives mapping of renamed operations
        /// </summary>
        public IDictionary<string, string> OperationNameMapping { get; } = new Dictionary<string, string>();
    }
}