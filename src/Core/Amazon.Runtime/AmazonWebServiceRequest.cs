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

namespace Amazon.Runtime
{
    /// <summary>
    /// Base class for request used by some of the services.
    /// </summary>
    public abstract partial class AmazonWebServiceRequest : Amazon.Runtime.Internal.IAmazonWebServiceRequest
    {
        internal RequestEventHandler mBeforeRequestEvent;
        internal string UserAgentAddition { get; set; } = null;

        internal event RequestEventHandler BeforeRequestEvent
        {
            add
            {
                lock (this)
                {
                    mBeforeRequestEvent += value;
                }
            }
            remove
            {
                lock (this)
                {
                    mBeforeRequestEvent -= value;
                }
            }
        }

        EventHandler<StreamTransferProgressArgs> Amazon.Runtime.Internal.IAmazonWebServiceRequest.StreamUploadProgressCallback { get; set; }

        private Dictionary<string, object> requestState = null;
        Dictionary<string, object> Amazon.Runtime.Internal.IAmazonWebServiceRequest.RequestState
        {
            get
            {
                if (requestState == null)
                {
                    requestState = new Dictionary<string, object>();
                }
                return requestState;
            }
        }

        protected AmazonWebServiceRequest()
        {
        }

        void Amazon.Runtime.Internal.IAmazonWebServiceRequest.AddBeforeRequestHandler(RequestEventHandler handler)
        {
            BeforeRequestEvent += handler;
        }

        void Amazon.Runtime.Internal.IAmazonWebServiceRequest.RemoveBeforeRequestHandler(RequestEventHandler handler)
        {
            BeforeRequestEvent -= handler;
        }

        internal void FireBeforeRequestEvent(object sender, RequestEventArgs args)
        {
            if (mBeforeRequestEvent != null)
                mBeforeRequestEvent(sender, args);
        }
    }
}
