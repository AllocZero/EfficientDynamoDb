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

using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class containing context information to use with endpoint discovery
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public abstract class EndpointOperationContextBase
    {
        private string _customerCredentials;
        private string _operationName;
        private EndpointDiscoveryDataBase _endpointDiscoveryData;
        private bool _evictCacheKey;
        private Uri _evictUri;

        protected EndpointOperationContextBase(string customerCredentials, string operationName, EndpointDiscoveryDataBase endpointDiscoveryData, bool evictCacheKey, Uri evictUri)
        {
            if (string.IsNullOrEmpty(customerCredentials))
            {
                throw new ArgumentNullException("customerCredentials");
            }

            _customerCredentials = customerCredentials;
            _operationName = operationName;
            _endpointDiscoveryData = endpointDiscoveryData;
            _evictCacheKey = evictCacheKey;
            _evictUri = evictUri;
        }

        /// <summary>
        /// Gets the customer credential information.
        /// </summary>
        public virtual string CustomerCredentials
        {
            get
            {
                return _customerCredentials;
            }
            protected set
            {
                _customerCredentials = value;
            }
        }

        /// <summary>
        /// Gets the operation name.
        /// </summary>
        public virtual string OperationName
        {
            get
            {
                return _operationName;
            }
            protected set
            {
                _operationName = value;
            }
        }

        /// <summary>
        /// Gets the current marshalled endpoint discovery data.
        /// </summary>
        public virtual EndpointDiscoveryDataBase EndpointDiscoveryData
        {
            get
            {
                return _endpointDiscoveryData;
            }
            protected set
            {
                _endpointDiscoveryData = value;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the specified key should be evicted from the cache.
        /// </summary>
        public virtual bool EvictCacheKey
        {
            get
            {
                return _evictCacheKey;
            }
            protected set
            {
                _evictCacheKey = value;
            }
        }

        /// <summary>
        /// Gets the Uri that should be evicted if EvictCacheKey is set to true.
        /// </summary>
        public virtual Uri EvictUri
        {
            get
            {
                return _evictUri;
            }
            protected set
            {
                _evictUri = value;
            }
        }
    }

    /// <summary>
    /// Class containing context information to use with endpoint discovery
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public class EndpointOperationContext : EndpointOperationContextBase
    {        
        public EndpointOperationContext(string customerCredentials, string operationName, EndpointDiscoveryDataBase endpointDiscoveryData, bool evictCacheKey, Uri evictUri) 
            : base(customerCredentials, operationName, endpointDiscoveryData, evictCacheKey, evictUri)
        {            
        }                
    }
}
