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
using System.IO;
using System.Text;
using Amazon.Runtime.Internal.Auth;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class containing the data to use with endpoint discovery
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public abstract class EndpointDiscoveryDataBase
    {
        private bool _required;
        private SortedDictionary<string, string> _identifiers;
        
        protected EndpointDiscoveryDataBase(bool required)
        {
            _required = required;
            _identifiers = new SortedDictionary<string, string>();
        }

        /// <summary>
        /// Gets/sets and flag indicating if endpoint discovery is required for the request.
        /// </summary>
        public virtual bool Required
        {
            get
            {
                return _required;
            }
            protected set
            {
                _required = value;
            }
        }

        /// <summary>
        /// Sorted dictionary of the identifiers that must be sent with the endpoint discovery request. These
        /// identifiers are used to construct the cache key for the cache that stores discovered endpoints. A
        /// sorted dictionary is used instead of a dictionary to ensure the cache key comes out in the same order
        /// each time it is constructed.
        /// </summary>
        public virtual SortedDictionary<string, string> Identifiers
        {
            get
            {
                return _identifiers;
            }
            protected set
            {
                _identifiers = value;
            }
        }
    }

    /// <summary>
    /// Represents the data to be used with endpoint discovery operations
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public class EndpointDiscoveryData : EndpointDiscoveryDataBase
    {
        public EndpointDiscoveryData(bool required) : base(required) { }
    }
}
