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
using System.Net;

using Amazon.Runtime.Internal.Auth;
using Amazon.Util;
#if NETSTANDARD
using System.Net.Http;
#endif

namespace Amazon.Runtime
{
    /// <summary>
    /// This interface is the read only access to the ClientConfig object used when setting up service clients. Once service clients
    /// are initiated the config object should not be changed to avoid issues with using a service client in a multi threaded environment.
    /// </summary>
    public partial interface IClientConfig
    {
        /// <summary>
        /// Gets the RegionEndpoint property. The region constant to use that 
        /// determines the endpoint to use.  If this is not set
        /// then the client will fallback to the value of ServiceURL.
        /// </summary>
        RegionEndpoint RegionEndpoint { get; }

        /// <summary>
        /// The constant used to lookup in the region hash the endpoint.
        /// </summary>
        string RegionEndpointServiceName { get; }

        /// <summary>
        /// Gets and sets of the ServiceURL property.
        /// This is an optional property if you need to set a specific service URL 
        /// instead setting the region with the RegionEndpoint property.
        /// </summary>
        string ServiceURL { get; }

        /// <summary>
        /// Gets the UseHttp property.
        /// If this property is set to true, the client attempts
        /// to use HTTP protocol.
        /// By default, this property is set to false.
        /// </summary>
        bool UseHttp { get; }


        /// <summary>
        /// Gets Service Version
        /// </summary>
        string ServiceVersion { get; }

        /// <summary>
        /// Gets the signatureMethod property.
        /// </summary>
        SigningAlgorithm SignatureMethod { get; }

        /// <summary>
        /// Gets the SignatureVersion property.
        ///
        /// Note: This property exists for backward compatibility but is no longer
        /// used by any service other than S3.
        /// </summary>
        string SignatureVersion { get; }

        /// <summary>
        /// Gets the AuthenticationRegion property.
        /// Used in AWS4 request signing, this is an optional property; 
        /// change it only if the region cannot be determined from the 
        /// service endpoint.
        /// </summary>
        string AuthenticationRegion { get; }

        /// <summary>
        /// Gets the AuthenticationServiceName property.
        /// Used in AWS4 request signing, this is the short-form
        /// name of the service being called.
        /// </summary>
        string AuthenticationServiceName { get; }


        /// <summary>
        /// Gets the UserAgent property.
        /// </summary>
        string UserAgent { get; }


        /// <summary>
        /// Gets the DisableLogging. If true logging for this client will be disabled.
        /// </summary>
        bool DisableLogging { get; }

        /// <summary>
        /// Flag on whether to log metrics for service calls.
        /// 
        /// This can be set in the application's configs, as below:
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSLogMetrics" value"true"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        /// </summary>
        bool LogMetrics { get; }

        /// <summary>
        /// Gets the LogResponse property.
        /// If this property is set to true, the service response
        /// is read in its entirety and logged.
        /// </summary>
        bool LogResponse { get; }

        /// <summary>
        /// Gets the ReadEntireResponse.
        /// If this property is set to true, the service response
        /// is read in its entirety before being processed.
        /// </summary>
        bool ReadEntireResponse { get; }


        /// <summary>
        /// This flag controls if .NET HTTP infrastructure should follow redirection
        ///  responses (e.g. HTTP 307 - temporary redirect).
        /// </summary>
        bool AllowAutoRedirect { get; }

        /// <summary>
        /// Gets the BufferSize property.
        /// The BufferSize controls the buffer used to read in from input streams and write 
        /// out to the request.
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        /// Returns the flag indicating how many retry HTTP requests an SDK should
        /// make for a single SDK operation invocation before giving up. This flag will 
        /// return 4 when the RetryMode is set to "Legacy" which is the default. For
        /// RetryMode values of "Standard" or "Adaptive" this flag will return 2. In 
        /// addition to the values returned that are dependant on the RetryMode, the
        /// value can be set to a specific value by using the AWS_MAX_ATTEMPTS environment
        /// variable, max_attempts in the shared configuration file, or by setting a
        /// value directly on this property. When using AWS_MAX_ATTEMPTS or max_attempts
        /// the value returned from this property will be one less than the value entered
        /// because this flag is the number of retry requests, not total requests. To 
        /// learn more about the RetryMode property that affects the values returned by 
        /// this flag, see <see cref="RetryMode"/>.
        /// </summary>
        int MaxErrorRetry { get; }


        /// <summary>
        /// Determines if MaxErrorRetry has been manually set.
        /// </summary>
        bool IsMaxErrorRetrySet { get; }

        /// <summary>
        /// Gets the interval at which progress update events are raised
        /// for upload operations. By default, the progress update events are 
        /// raised at every 100KB of data transferred. 
        /// </summary>
        long ProgressUpdateInterval { get; }

        /// <summary>
        /// Flag on whether to resign requests on retry or not.
        /// </summary>
        bool ResignRetries { get; }


        /// <summary>
        /// Credentials to use with a proxy.
        /// </summary>
        ICredentials ProxyCredentials { get; }

        /// <summary>
        /// Gets the default request timeout value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value is set, the value is assigned to the Timeout property of the HTTPWebRequest/HttpClient object used
        /// to send requests.
        /// </para>
        /// <para>
        /// Please specify a timeout value only if the operation will not complete within the default intervals
        /// specified for an HttpWebRequest/HttpClient.
        /// </para>
        /// </remarks>
        TimeSpan? Timeout { get; }

        /// <summary>
        /// Configures the endpoint calculation for a service to go to a dual stack (ipv6 enabled) endpoint
        /// for the configured region.
        /// </summary>
        /// <remarks>
        /// Note: AWS services are enabling dualstack endpoints over time. It is your responsibility to check 
        /// that the service actually supports a dualstack endpoint in the configured region before enabling 
        /// this option for a service.
        /// </remarks>
        bool UseDualstackEndpoint { get; }

        /// <summary>
        /// Configures a flag enabling to either opt in or opt out of the retry throttling service.
        /// Note: set value to true to enable retry throttling feature. The Default value for this flag is false.
        /// </summary>
        bool ThrottleRetries { get; }

        /// <summary>
        /// Using either the RegionEndpoint or the ServiceURL determine what the URL to the service is.
        /// </summary>
        /// <returns>The URL to the service.</returns>
        string DetermineServiceURL();

        /// <summary>
        /// Performs validation on this config object.
        /// Throws exception if any of the required values are missing/invalid.
        /// </summary>
        /// <exception cref="Amazon.Runtime.AmazonClientException">The timeout specified is null.</exception>
        void Validate();

        /// <summary>
        /// Returns the clock skew adjusted utc now.  This value is affected by AWSConfigs.ManualClockCorrection
        /// </summary>
        DateTime CorrectedUtcNow { get; }

        /// <summary>
        /// Returns the calculated clock skew value for this config's service endpoint. If AWSConfigs.CorrectForClockSkew is false,
        /// this value won't be used to construct service requests.
        /// </summary>
        TimeSpan ClockOffset { get; }

        /// <summary>
        /// Gets the DisableHostPrefixInjection flag. If true, host prefix injection will be disabled for this client, the default value of this flag is false. 
        /// Host prefix injection prefixes the service endpoint with request members from APIs which use this feature. 
        /// Example: for a hostPrefix of "foo-name." and a endpoint of "service.region.amazonaws.com" the default behavior is to
        /// prefix the endpoint with the hostPrefix resulting in a final endpoint of "foo-name.service.region.amazonaws.com". Setting 
        /// DisableHostPrefixInjection to true will disable hostPrefix injection resulting in a final endpoint of
        /// "service.region.amazonaws.com" regardless of the value of hostPrefix. E.g. You may want to disable host prefix injection for testing against a local mock endpoint.
        /// </summary>
        bool DisableHostPrefixInjection { get; }

        /// <summary>
        /// Returns the flag indicating if endpoint discovery should be enabled or disabled for operations that are not required to use endpoint discovery.
        /// </summary>
        bool EndpointDiscoveryEnabled { get; }

        /// <summary>
        /// Returns the maximum number of discovered endpoints that can be stored within the cache for the client. The default limit is 1000 cache entries.
        /// </summary>
        int EndpointDiscoveryCacheLimit { get; }

        /// <summary>
        /// Returns the flag indicating the current mode in use for request 
        /// retries and influences the value returned from <see cref="MaxErrorRetry"/>.
        /// The default value is RequestRetryMode.Legacy. This flag can be configured
        /// by using the AWS_RETRY_MODE environment variable, retry_mode in the
        /// shared configuration file, or by setting this value directly.
        /// </summary>
        RequestRetryMode RetryMode { get; }
                
        /// <summary>
        /// Under Adaptive retry mode, this flag determines if the client should wait for
        /// a send token to become available or don't block and fail the request immediately
        /// if a send token is not available.
        /// </summary>
        bool FastFailRequests { get;  }

#if BCL
        /// <summary>
        /// Gets the TCP keep-alive values to use for service requests. Enabling TCP keep-alive sends periodic TCP keep-alive probe packets, to prevent disconnection due to 
        /// network inactivity. This is useful when you make API calls that take a long time to respond. In this case, the connection could be dropped by an intermediate 
        /// node (e.g. proxy) as the connection is inactive for a long time. Timeout controls the duration of inactivity before a keep-alive probe packet is sent. 
        /// Interval controls the amount of time to wait before retrying a keep-alive probe packet in the event the server doesn't respond to a keep-alive probe.
        /// </summary>
        TcpKeepAlive TcpKeepAlive { get; }                
#endif

#if NETSTANDARD
        /// <summary>
        /// Get the value to use for <see cref="HttpClientHandler.MaxConnectionsPerServer"/> on requests.
        /// If this property is null, <see cref="HttpClientHandler.MaxConnectionsPerServer"/>
        /// will be left at its default value of <see cref="int.MaxValue"/>.
        /// </summary>
        int? MaxConnectionsPerServer { get; }
        
        /// <summary>
        /// <para>
        /// This is a switch used for performance testing and is not intended for production applications 
        /// to change. This switch may be removed in a future version of the SDK as the .NET Core platform matures.
        /// </para>
        /// <para>
        /// If true, the HttpClient is cached and reused for every request made by the service client 
        /// and shared with other service clients.
        /// </para>
        /// <para>
        /// For the .NET Core platform this is default to true because the HttpClient manages the connection
        /// pool.
        /// </para>
        /// </summary>
        bool CacheHttpClient { get; }

        /// <summary>
        /// If CacheHttpClient is set to true then HttpClientCacheSize controls the number of HttpClients cached.
        /// </summary>
        int HttpClientCacheSize { get; }
#endif
    }
}