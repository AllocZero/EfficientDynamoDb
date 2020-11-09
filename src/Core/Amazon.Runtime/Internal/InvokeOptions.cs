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
using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;

namespace Amazon.Runtime.Internal
{
    public delegate IEnumerable<DiscoveryEndpointBase> EndpointOperationDelegate(EndpointOperationContextBase context);

    /// <summary>
    /// Class containing the members used to invoke service calls
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public abstract class InvokeOptionsBase
    {
        private IMarshaller<IRequest, AmazonWebServiceRequest> _requestMarshaller;
        private ResponseUnmarshaller _responseUnmarshaller;
        private IMarshaller<EndpointDiscoveryDataBase, AmazonWebServiceRequest> _endpointDiscoveryMarshaller;
        private EndpointOperationDelegate _endpointOperation;
        
        protected InvokeOptionsBase()
        {
        }

        #region Standard Marshaller/Unmarshaller

        public virtual IMarshaller<IRequest, AmazonWebServiceRequest> RequestMarshaller
        {
            get
            {
                return _requestMarshaller;
            }
            set
            {
                _requestMarshaller = value;
            }
        }

        public virtual ResponseUnmarshaller ResponseUnmarshaller
        {
            get
            {
                return _responseUnmarshaller;
            }
            set
            {
                _responseUnmarshaller = value;
            }
        }

        #endregion

        #region EndpointDiscovery                

        public virtual IMarshaller<EndpointDiscoveryDataBase, AmazonWebServiceRequest> EndpointDiscoveryMarshaller
        {
            get
            {
                return _endpointDiscoveryMarshaller;
            }
            set
            {
                _endpointDiscoveryMarshaller = value;
            }
        }

        public virtual EndpointOperationDelegate EndpointOperation
        {
            get
            {
                return _endpointOperation;
            }
            set
            {
                _endpointOperation = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Class containing the members used to invoke service calls
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public class InvokeOptions : InvokeOptionsBase
    {
        public InvokeOptions() : base()
        {
        }
    }
}