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
    /// The valid hashing algorithm supported by the sdk for request signing.
    /// </summary>
    public enum SigningAlgorithm 
    { 
        HmacSHA1, 
        HmacSHA256 
    };

    /// <summary>
    /// Which end of a request was responsible for a service error response.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// The sender was responsible for the error, i.e. the client
        /// request failed validation or was improperly formatted.
        /// </summary>
        Sender,
        /// <summary>
        /// The error occured within the service.
        /// </summary>
        Receiver,
        /// <summary>
        /// An unrecognized error type was returned.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Sts Regional Endpoints Value determines whether or not
    /// to send the sts request to the regional endpoint or to
    /// the global sts endpoint
    /// </summary>
    public enum StsRegionalEndpointsValue
    {
        /// <summary>
        /// Send the request to the global sts endpoint
        /// if the region is a legacy global region
        /// </summary>
        Legacy,
        /// <summary>
        /// Send the request to the regional endpoint
        /// </summary>
        Regional
    }

    /// <summary>
    /// S3 US East 1 endpoint value determines wheter or not
    /// to send the us-east-1 s3 requests to the regional endpoint or to
    /// the legacy global endpoint
    /// </summary>
    public enum S3UsEast1RegionalEndpointValue
    {
        /// <summary>
        /// Sends the requests to the legacy global s3 endpoint for us-east-1
        /// </summary>
        Legacy,
        /// <summary>
        /// Sends the request to the regional s3 endpoint for us-east-1
        /// </summary>
        Regional
    }

    /// <summary>
    /// RetryMode determines which request retry mode is used for requests that do 
    /// not complete successfully.
    /// </summary>
    public enum RequestRetryMode
    {
        /// <summary>
        /// Legacy request retry strategy.
        /// </summary>
        Legacy,
        /// <summary>
        /// Standardized request retry strategy that is consistent across all SDKs.
        /// </summary>
        Standard,
        /// <summary>
        /// An experimental request retry strategy that builds on the Standard strategy
        /// and introduces congestion control through client side rate limiting.
        /// </summary>
        Adaptive
    }
}
