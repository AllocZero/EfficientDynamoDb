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
using System.Globalization;
using System.Text.RegularExpressions;
using Amazon.Util;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal.Auth
{
    /// <summary>
    /// AWS4 protocol signer for service calls that transmit authorization in the header field "Authorization".
    /// </summary>
    public class AWS4Signer : AbstractAWSSigner
    {
        
        public const string Scheme = "AWS4";
        public const string Algorithm = "HMAC-SHA256";

        public const string AWS4AlgorithmTag = Scheme + "-" + Algorithm;

        public const string Terminator = "aws4_request";
        public static readonly byte[] TerminatorBytes = Encoding.UTF8.GetBytes(Terminator);

        public const string Credential = "Credential";
        public const string SignedHeaders = "SignedHeaders";
        public const string Signature = "Signature";

        public const string EmptyBodySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        public const string StreamingBodySha256 = "STREAMING-AWS4-HMAC-SHA256-PAYLOAD";
        public const string AWSChunkedEncoding = "aws-chunked";

        public const string UnsignedPayload = "UNSIGNED-PAYLOAD";

        const SigningAlgorithm SignerAlgorithm = SigningAlgorithm.HmacSHA256;

        private static IEnumerable<string> _headersToIgnoreWhenSigning = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            HeaderKeys.XAmznTraceIdHeader,
            HeaderKeys.TransferEncodingHeader
        };

        public AWS4Signer()
            : this(true)
        {
        }

        public AWS4Signer(bool signPayload)
        {
            SignPayload = signPayload;
        }

        public bool SignPayload
        {
            get;
            private set;
        }

        public override ClientProtocol Protocol
        {
            get { return ClientProtocol.RestProtocol; }
        }

        /// <summary>
        /// Calculates and signs the specified request using the AWS4 signing protocol by using the
        /// AWS account credentials given in the method parameters. The resulting signature is added
        /// to the request headers as 'Authorization'. Parameters supplied in the request, either in
        /// the resource path as a query string or in the Parameters collection must not have been
        /// uri encoded. If they have, use the SignRequest method to obtain a signature.
        /// </summary>
        /// <param name="request">
        /// The request to compute the signature for. Additional headers mandated by the AWS4 protocol 
        /// ('host' and 'x-amz-date') will be added to the request before signing.
        /// </param>
        /// <param name="clientConfig">
        /// Client configuration data encompassing the service call (notably authentication
        /// region, endpoint and service name).
        /// </param>
        /// <param name="metrics">
        /// Metrics for the request
        /// </param>
        /// <param name="awsAccessKeyId">
        /// The AWS public key for the account making the service call.
        /// </param>
        /// <param name="awsSecretAccessKey">
        /// The AWS secret key for the account making the call, in clear text.
        /// </param>
        /// <exception cref="Amazon.Runtime.SignatureException">
        /// If any problems are encountered while signing the request.
        /// </exception>
        public override void Sign(IRequest request, 
                                  IClientConfig clientConfig, 
                                  RequestMetrics metrics, 
                                  string awsAccessKeyId, 
                                  string awsSecretAccessKey)
        {
            var signingResult = SignRequest(request, clientConfig, metrics, awsAccessKeyId, awsSecretAccessKey);
            request.Headers[HeaderKeys.AuthorizationHeader] = signingResult.ForAuthorizationHeader;
        }

        /// <summary>
        /// Calculates and signs the specified request using the AWS4 signing protocol by using the
        /// AWS account credentials given in the method parameters.
        /// </summary>
        /// <param name="request">
        /// The request to compute the signature for. Additional headers mandated by the AWS4 protocol 
        /// ('host' and 'x-amz-date') will be added to the request before signing.
        /// </param>
        /// <param name="clientConfig">
        /// Client configuration data encompassing the service call (notably authentication
        /// region, endpoint and service name).
        /// </param>
        /// <param name="metrics">
        /// Metrics for the request.
        /// </param>
        /// <param name="awsAccessKeyId">
        /// The AWS public key for the account making the service call.
        /// </param>
        /// <param name="awsSecretAccessKey">
        /// The AWS secret key for the account making the call, in clear text.
        /// </param>
        /// <exception cref="Amazon.Runtime.SignatureException">
        /// If any problems are encountered while signing the request.
        /// </exception>
        /// <remarks>
        /// Parameters passed as part of the resource path should be uri-encoded prior to
        /// entry to the signer. Parameters passed in the request.Parameters collection should
        /// be not be encoded; encoding will be done for these parameters as part of the 
        /// construction of the canonical request.
        /// </remarks>
        public AWS4SigningResult SignRequest(IRequest request,
                                             IClientConfig clientConfig,
                                             RequestMetrics metrics,
                                             string awsAccessKeyId,
                                             string awsSecretAccessKey)
        {
            ValidateRequest(request);
            var signedAt = InitializeHeaders(request.Headers, request.Endpoint);
            var service = !string.IsNullOrEmpty(request.OverrideSigningServiceName) ? request.OverrideSigningServiceName : DetermineService(clientConfig);
            request.DeterminedSigningRegion = DetermineSigningRegion(clientConfig, service, request.AlternateEndpoint, request);

            var parametersToCanonicalize = GetParametersToCanonicalize(request);
            var canonicalParameters = CanonicalizeQueryParameters(parametersToCanonicalize);
            var bodyHash = SetRequestBodyHash(request, SignPayload);
            var sortedHeaders = SortAndPruneHeaders(request.Headers);

            var canonicalRequest = CanonicalizeRequest(request.Endpoint,
                                                       request.ResourcePath,
                                                       request.HttpMethod,
                                                       sortedHeaders,
                                                       canonicalParameters,
                                                       bodyHash,
                                                       request.PathResources,
                                                       request.MarshallerVersion,
                                                       service);
            if (metrics != null)
                metrics.AddProperty(Metric.CanonicalRequest, canonicalRequest);

            return ComputeSignature(awsAccessKeyId,
                                    awsSecretAccessKey,
                                    request.DeterminedSigningRegion,
                                    signedAt,
                                    service,
                                    CanonicalizeHeaderNames(sortedHeaders),
                                    canonicalRequest,
                                    metrics);
        }

        #region Public Signing Helpers

        /// <summary>
        /// Sets the AWS4 mandated 'host' and 'x-amz-date' headers, returning the date/time that will
        /// be used throughout the signing process in various elements and formats.
        /// </summary>
        /// <param name="headers">The current set of headers</param>
        /// <param name="requestEndpoint"></param>
        /// <returns>Date and time used for x-amz-date, in UTC</returns>
        public static DateTime InitializeHeaders(IDictionary<string, string> headers, Uri requestEndpoint)
        {
            return InitializeHeaders(headers, requestEndpoint, CorrectClockSkew.GetCorrectedUtcNowForEndpoint(requestEndpoint.ToString()));
        }

        /// <summary>
        /// Sets the AWS4 mandated 'host' and 'x-amz-date' headers, accepting and returning the date/time that will
        /// be used throughout the signing process in various elements and formats.
        /// </summary>
        /// <param name="headers">The current set of headers</param>
        /// <param name="requestEndpoint"></param>
        /// <param name="requestDateTime"></param>
        /// <returns>Date and time used for x-amz-date, in UTC</returns>
        public static DateTime InitializeHeaders(IDictionary<string, string> headers, Uri requestEndpoint, DateTime requestDateTime)
        {
            // clean up any prior signature in the headers if resigning
            CleanHeaders(headers);

            if (!headers.ContainsKey(HeaderKeys.HostHeader))
            {
                var hostHeader = requestEndpoint.Host;
                if (!requestEndpoint.IsDefaultPort)
                    hostHeader += ":" + requestEndpoint.Port;
                headers.Add(HeaderKeys.HostHeader, hostHeader);
            }

            var dt = requestDateTime;
            headers[HeaderKeys.XAmzDateHeader] = dt.ToUniversalTime().ToString(AWSSDKUtils.ISO8601BasicDateTimeFormat, CultureInfo.InvariantCulture);

            return dt;
        }

        private static void CleanHeaders(IDictionary<string, string> headers)
        {
            headers.Remove(HeaderKeys.AuthorizationHeader);
            headers.Remove(HeaderKeys.XAmzContentSha256Header);

            if (headers.ContainsKey(HeaderKeys.XAmzDecodedContentLengthHeader))
            {
                headers[HeaderKeys.ContentLengthHeader] =
                    headers[HeaderKeys.XAmzDecodedContentLengthHeader];
                headers.Remove(HeaderKeys.XAmzDecodedContentLengthHeader);
            }
        }

        private static void ValidateRequest(IRequest request)
        {
            Uri url = request.Endpoint;

            // Before we sign the request, we need to validate if the request is being 
            // sent over https when DisablePayloadSigning is true.
            if((request.DisablePayloadSigning ?? false) && url.Scheme != "https")
            {
                throw new AmazonClientException("When DisablePayloadSigning is true, the request must be sent over HTTPS.");
            }
        }

        /// <summary>
        /// Computes and returns an AWS4 signature for the specified canonicalized request
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="region"></param>
        /// <param name="signedAt"></param>
        /// <param name="service"></param>
        /// <param name="signedHeaders"></param>
        /// <param name="canonicalRequest"></param>
        /// <returns></returns>
        public static AWS4SigningResult ComputeSignature(ImmutableCredentials credentials,
                                                         string region,
                                                         DateTime signedAt,
                                                         string service,
                                                         string signedHeaders,
                                                         string canonicalRequest)
        {
            return ComputeSignature(credentials.AccessKey,
                                    credentials.SecretKey,
                                    region,
                                    signedAt,
                                    service,
                                    signedHeaders,
                                    canonicalRequest);
        }

        /// <summary>
        /// Computes and returns an AWS4 signature for the specified canonicalized request
        /// </summary>
        /// <param name="awsAccessKey"></param>
        /// <param name="awsSecretAccessKey"></param>
        /// <param name="region"></param>
        /// <param name="signedAt"></param>
        /// <param name="service"></param>
        /// <param name="signedHeaders"></param>
        /// <param name="canonicalRequest"></param>
        /// <returns></returns>
        public static AWS4SigningResult ComputeSignature(string awsAccessKey,
                                                         string awsSecretAccessKey,
                                                         string region,
                                                         DateTime signedAt,
                                                         string service,
                                                         string signedHeaders,
                                                         string canonicalRequest)
        {
            return ComputeSignature(awsAccessKey, awsSecretAccessKey, region, signedAt, service, signedHeaders, canonicalRequest, null);
        }

        /// <summary>
        /// Computes and returns an AWS4 signature for the specified canonicalized request
        /// </summary>
        /// <param name="awsAccessKey"></param>
        /// <param name="awsSecretAccessKey"></param>
        /// <param name="region"></param>
        /// <param name="signedAt"></param>
        /// <param name="service"></param>
        /// <param name="signedHeaders"></param>
        /// <param name="canonicalRequest"></param>
        /// <param name="metrics"></param>
        /// <returns></returns>
        public static AWS4SigningResult ComputeSignature(string awsAccessKey,
                                                         string awsSecretAccessKey,
                                                         string region,
                                                         DateTime signedAt,
                                                         string service,
                                                         string signedHeaders,
                                                         string canonicalRequest,
                                                         RequestMetrics metrics)
        {
            var dateStamp = FormatDateTime(signedAt, AWSSDKUtils.ISO8601BasicDateFormat);
            var scope = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}", dateStamp, region, service, Terminator);

            var stringToSignBuilder = new StringBuilder();
            stringToSignBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}-{1}\n{2}\n{3}\n",
                                      Scheme,
                                      Algorithm,
                                      FormatDateTime(signedAt, AWSSDKUtils.ISO8601BasicDateTimeFormat),
                                      scope);

            var canonicalRequestHashBytes = ComputeHash(canonicalRequest);
            stringToSignBuilder.Append(AWSSDKUtils.ToHex(canonicalRequestHashBytes, true));

            if (metrics != null)
                metrics.AddProperty(Metric.StringToSign, stringToSignBuilder);

            var key = ComposeSigningKey(awsSecretAccessKey,
                                        region,
                                        dateStamp,
                                        service);

            var stringToSign = stringToSignBuilder.ToString();
            var signature = ComputeKeyedHash(SignerAlgorithm, key, stringToSign);
            return new AWS4SigningResult(awsAccessKey, signedAt, signedHeaders, scope, key, signature);
        }

        /// <summary>
        /// Formats the supplied date and time for use in AWS4 signing, where various formats are used.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="formatString">The required format</param>
        /// <returns>The UTC date/time in the requested format</returns>
        public static string FormatDateTime(DateTime dt, string formatString)
        {
            return dt.ToUniversalTime().ToString(formatString, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Compute and return the multi-stage signing key for the request.
        /// </summary>
        /// <param name="awsSecretAccessKey">The clear-text AWS secret key, if not held in secureKey</param>
        /// <param name="region">The region in which the service request will be processed</param>
        /// <param name="date">Date of the request, in yyyyMMdd format</param>
        /// <param name="service">The name of the service being called by the request</param>
        /// <returns>Computed signing key</returns>
        public static byte[] ComposeSigningKey(string awsSecretAccessKey, string region, string date, string service)
        {
            char[] ksecret = null;

            try
            {
                ksecret = (Scheme + awsSecretAccessKey).ToCharArray();

                var hashDate = ComputeKeyedHash(SignerAlgorithm, Encoding.UTF8.GetBytes(ksecret), Encoding.UTF8.GetBytes(date));
                var hashRegion = ComputeKeyedHash(SignerAlgorithm, hashDate, Encoding.UTF8.GetBytes(region));
                var hashService = ComputeKeyedHash(SignerAlgorithm, hashRegion, Encoding.UTF8.GetBytes(service));
                return ComputeKeyedHash(SignerAlgorithm, hashService, TerminatorBytes);
            }
            finally
            {
                // clean up all secrets, regardless of how initially seeded (for simplicity)
                if (ksecret != null)
                    Array.Clear(ksecret, 0, ksecret.Length);
            }
        }

        /// <summary>
        /// If the caller has already set the x-amz-content-sha256 header with a pre-computed
        /// content hash, or it is present as ContentStreamHash on the request instance, return
        /// the value to be used in request canonicalization. 
        /// If not set as a header or in the request, attempt to compute a hash based on
        /// inspection of the style of the request content.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// The computed hash, whether already set in headers or computed here. Null
        /// if we were not able to compute a hash.
        /// </returns>
        public static string SetRequestBodyHash(IRequest request)
        {
            return SetRequestBodyHash(request, true);
        }

        /// <summary>
        /// If signPayload is false set the x-amz-content-sha256 header to
        /// the UNSIGNED-PAYLOAD magic string and return it.
        /// Otherwise, if the caller has already set the x-amz-content-sha256 header with a pre-computed
        /// content hash, or it is present as ContentStreamHash on the request instance, return
        /// the value to be used in request canonicalization.
        /// If not set as a header or in the request, attempt to compute a hash based on
        /// inspection of the style of the request content.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="signPayload"></param>
        /// <returns>
        /// The computed hash, whether already set in headers or computed here. Null
        /// if we were not able to compute a hash.
        /// </returns>
        public static string SetRequestBodyHash(IRequest request, bool signPayload)
        {
            // if unsigned payload, set the magic string in the header and return it
            if (request.DisablePayloadSigning != null ? request.DisablePayloadSigning.Value : !signPayload)
                return SetPayloadSignatureHeader(request, UnsignedPayload);

            // if the body hash has been precomputed and already placed in the header, just extract and return it
            string computedContentHash = null;
            var shaHeaderPresent = request.Headers.TryGetValue(HeaderKeys.XAmzContentSha256Header, out computedContentHash);
            if (shaHeaderPresent && !request.UseChunkEncoding)
                return computedContentHash;

            // otherwise continue to calculate the hash and set it in the headers before returning
            if (request.UseChunkEncoding)
            {
                computedContentHash = StreamingBodySha256;
                if (request.Headers.ContainsKey(HeaderKeys.ContentLengthHeader))
                {
                    // substitute the originally declared content length with the true size of
                    // the data we'll upload, which is inflated with chunk metadata
                    request.Headers[HeaderKeys.XAmzDecodedContentLengthHeader] = request.Headers[HeaderKeys.ContentLengthHeader];
                    var originalContentLength = long.Parse(request.Headers[HeaderKeys.ContentLengthHeader], CultureInfo.InvariantCulture);
                    request.Headers[HeaderKeys.ContentLengthHeader]
                        = ChunkedUploadWrapperStream.ComputeChunkedContentLength(originalContentLength).ToString(CultureInfo.InvariantCulture);
                }

                if (request.Headers.ContainsKey(HeaderKeys.ContentEncodingHeader))
                {
                    var originalEncoding = request.Headers[HeaderKeys.ContentEncodingHeader];
                    if (!originalEncoding.Contains(AWSChunkedEncoding))
                    {
                        request.Headers[HeaderKeys.ContentEncodingHeader]
                        = string.Concat(originalEncoding, ", ", AWSChunkedEncoding);
                    }
                }
            }
            else
            {
                if (request.ContentStream != null)
                    computedContentHash = request.ComputeContentStreamHash();
                else
                {
                    byte[] payloadBytes = GetRequestPayloadBytes(request);
                    byte[] payloadHashBytes = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(payloadBytes);
                    computedContentHash = AWSSDKUtils.ToHex(payloadHashBytes, true);
                }
            }


            // set the header if needed and return it
            return SetPayloadSignatureHeader(request, computedContentHash ?? UnsignedPayload);
        }

        /// <summary>
        /// Returns the HMAC256 for an arbitrary blob using the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SignBlob(byte[] key, string data)
        {
            return SignBlob(key, Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Returns the HMAC256 for an arbitrary blob using the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SignBlob(byte[] key, byte[] data)
        {
            return CryptoUtilFactory.CryptoInstance.HMACSignBinary(data, key, SignerAlgorithm);
        }

        /// <summary>
        /// Compute and return the hash of a data blob using the specified key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        public static byte[] ComputeKeyedHash(SigningAlgorithm algorithm, byte[] key, string data)
        {
            return ComputeKeyedHash(algorithm, key, Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Compute and return the hash of a data blob using the specified key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        public static byte[] ComputeKeyedHash(SigningAlgorithm algorithm, byte[] key, byte[] data)
        {
            return CryptoUtilFactory.CryptoInstance.HMACSignBinary(data, key, algorithm);
        }

        /// <summary>
        /// Computes the non-keyed hash of the supplied data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string data)
        {
            return ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Computes the non-keyed hash of the supplied data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] data)
        {
            return CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(data);
        }

        #endregion

        #region Private Signing Helpers
        static string SetPayloadSignatureHeader(IRequest request, string payloadHash)
        {
            if (request.Headers.ContainsKey(HeaderKeys.XAmzContentSha256Header))
                request.Headers[HeaderKeys.XAmzContentSha256Header] = payloadHash;
            else
                request.Headers.Add(HeaderKeys.XAmzContentSha256Header, payloadHash);

            return payloadHash;
        }

        public static string DetermineSigningRegion(IClientConfig clientConfig, 
                                                    string serviceName, 
                                                    RegionEndpoint alternateEndpoint,
                                                    IRequest request)
        {
            // Alternate endpoint (IRequest.AlternateEndopoint) takes precedence over
            // client config properties.
            if (alternateEndpoint != null)
            {
                var serviceEndpoint = alternateEndpoint.GetEndpointForService(serviceName, clientConfig.UseDualstackEndpoint);
                if (serviceEndpoint.AuthRegion != null)
                    return serviceEndpoint.AuthRegion;

                return alternateEndpoint.SystemName;
            }

            string authenticationRegion = clientConfig.AuthenticationRegion;
            if (request != null && request.AuthenticationRegion != null)
                authenticationRegion = request.AuthenticationRegion;

            if (!string.IsNullOrEmpty(authenticationRegion))
                return authenticationRegion.ToLowerInvariant();

            if (!string.IsNullOrEmpty(clientConfig.ServiceURL))
            {
                var parsedRegion = AWSSDKUtils.DetermineRegion(clientConfig.ServiceURL);
                if (!string.IsNullOrEmpty(parsedRegion))
                    return parsedRegion.ToLowerInvariant();
            }

            var endpoint = clientConfig.RegionEndpoint;
            if (endpoint != null)
            {
                var serviceEndpoint = endpoint.GetEndpointForService(serviceName, clientConfig.UseDualstackEndpoint);
                if (!string.IsNullOrEmpty(serviceEndpoint.AuthRegion))
                    return serviceEndpoint.AuthRegion;

                // Check if the region is overridden in the endpoints.json file
                var overrideRegion = RegionEndpoint.GetRegionEndpointOverride(endpoint);
                if (overrideRegion != null)
                    return overrideRegion.SystemName;

                return endpoint.SystemName; 
            }

            return string.Empty;
        }

        internal static string DetermineService(IClientConfig clientConfig)
        {
            return (!string.IsNullOrEmpty(clientConfig.AuthenticationServiceName)) 
                ? clientConfig.AuthenticationServiceName 
                : AWSSDKUtils.DetermineService(clientConfig.DetermineServiceURL());
        }

        /// <summary>
        /// Computes and returns the canonical request
        /// </summary>
        /// <param name="endpoint">The endpoint URL</param>
        /// <param name="resourcePath">the path of the resource being operated on</param>
        /// <param name="httpMethod">The http method used for the request</param>
        /// <param name="sortedHeaders">The full request headers, sorted into canonical order</param>
        /// <param name="canonicalQueryString">The query parameters for the request</param>
        /// <param name="precomputedBodyHash">
        /// The hash of the binary request body if present. If not supplied, the routine
        /// will look for the hash as a header on the request.
        /// </param>
        /// <returns>Canonicalised request as a string</returns>
        protected static string CanonicalizeRequest(Uri endpoint,
                                                    string resourcePath,
                                                    string httpMethod,
                                                    IDictionary<string, string> sortedHeaders,
                                                    string canonicalQueryString,
                                                    string precomputedBodyHash)
        {
            return CanonicalizeRequest(endpoint, resourcePath, httpMethod, sortedHeaders, canonicalQueryString, precomputedBodyHash, null, 1);
        }

        /// <summary>
        /// Computes and returns the canonical request
        /// </summary>
        /// <param name="endpoint">The endpoint URL</param>
        /// <param name="resourcePath">the path of the resource being operated on</param>
        /// <param name="httpMethod">The http method used for the request</param>
        /// <param name="sortedHeaders">The full request headers, sorted into canonical order</param>
        /// <param name="canonicalQueryString">The query parameters for the request</param>
        /// <param name="precomputedBodyHash">
        /// <param name="pathResources">The path resource values lookup to use to replace the keys within resourcePath</param>
        /// The hash of the binary request body if present. If not supplied, the routine
        /// will look for the hash as a header on the request.
        /// </param>
        /// <param name="marshallerVersion">The version of the marshaller that constructed the request object.</param>
        /// <returns>Canonicalised request as a string</returns>
        protected static string CanonicalizeRequest(Uri endpoint,
                                                    string resourcePath,
                                                    string httpMethod,
                                                    IDictionary<string, string> sortedHeaders,
                                                    string canonicalQueryString,
                                                    string precomputedBodyHash,
                                                    IDictionary<string, string> pathResources,
                                                    int marshallerVersion)
        {
            return CanonicalizeRequestHelper(endpoint,
                resourcePath,
                httpMethod,
                sortedHeaders,
                canonicalQueryString,
                precomputedBodyHash,
                pathResources,
                marshallerVersion,
                true);
        }

        /// <summary>
        /// Computes and returns the canonical request
        /// </summary>
        /// <param name="endpoint">The endpoint URL</param>
        /// <param name="resourcePath">the path of the resource being operated on</param>
        /// <param name="httpMethod">The http method used for the request</param>
        /// <param name="sortedHeaders">The full request headers, sorted into canonical order</param>
        /// <param name="canonicalQueryString">The query parameters for the request</param>
        /// <param name="precomputedBodyHash">
        /// <param name="pathResources">The path resource values lookup to use to replace the keys within resourcePath</param>
        /// The hash of the binary request body if present. If not supplied, the routine
        /// will look for the hash as a header on the request.
        /// </param>
        /// <param name="marshallerVersion">The version of the marshaller that constructed the request object.</param>
        /// <param name="service">The service being called for the request</param>
        /// <returns>Canonicalised request as a string</returns>
        protected static string CanonicalizeRequest(Uri endpoint,
                                                    string resourcePath,
                                                    string httpMethod,
                                                    IDictionary<string, string> sortedHeaders,
                                                    string canonicalQueryString,
                                                    string precomputedBodyHash,
                                                    IDictionary<string, string> pathResources,
                                                    int marshallerVersion,
                                                    string service)
        {
            return CanonicalizeRequestHelper(endpoint,
                resourcePath,
                httpMethod,
                sortedHeaders,
                canonicalQueryString,
                precomputedBodyHash,
                pathResources,
                marshallerVersion,
                !(service == "s3"));
        }

        private static string CanonicalizeRequestHelper(Uri endpoint,
                                                    string resourcePath,
                                                    string httpMethod,
                                                    IDictionary<string, string> sortedHeaders,
                                                    string canonicalQueryString,
                                                    string precomputedBodyHash,
                                                    IDictionary<string, string> pathResources,
                                                    int marshallerVersion,
                                                    bool detectPreEncode)
        {
            var canonicalRequest = new StringBuilder();
            canonicalRequest.AppendFormat("{0}\n", httpMethod);
            canonicalRequest.AppendFormat("{0}\n", AWSSDKUtils.CanonicalizeResourcePath(endpoint, resourcePath, detectPreEncode, pathResources, marshallerVersion));
            canonicalRequest.AppendFormat("{0}\n", canonicalQueryString);

            canonicalRequest.AppendFormat("{0}\n", CanonicalizeHeaders(sortedHeaders));
            canonicalRequest.AppendFormat("{0}\n", CanonicalizeHeaderNames(sortedHeaders));

            if (precomputedBodyHash != null)
            {
                canonicalRequest.Append(precomputedBodyHash);
            }
            else
            {
                string contentHash;
                if (sortedHeaders.TryGetValue(HeaderKeys.XAmzContentSha256Header, out contentHash))
                    canonicalRequest.Append(contentHash);
            }

            return canonicalRequest.ToString();
        }

        /// <summary>
        /// Reorders the headers for the request for canonicalization.
        /// </summary>
        /// <param name="requestHeaders">The set of proposed headers for the request</param>
        /// <returns>List of headers that must be included in the signature</returns>
        /// <remarks>For AWS4 signing, all headers are considered viable for inclusion</remarks>
        protected static IDictionary<string, string> SortAndPruneHeaders(IEnumerable<KeyValuePair<string, string>> requestHeaders)
        {
            var sortedHeaders = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var header in requestHeaders)
            {
                if (_headersToIgnoreWhenSigning.Contains(header.Key))
                {
                    continue;
                }
                sortedHeaders.Add(header.Key, header.Value);
            }
            
            return sortedHeaders;
        }

        /// <summary>
        /// Computes the canonical headers with values for the request. Only headers included in the signature
        /// are included in the canonicalization process.
        /// </summary>
        /// <param name="sortedHeaders">All request headers, sorted into canonical order</param>
        /// <returns>Canonicalized string of headers, with the header names in lower case.</returns>
        protected static string CanonicalizeHeaders(IEnumerable<KeyValuePair<string, string>> sortedHeaders)
        {
            if (sortedHeaders == null || sortedHeaders.Count() == 0)
                return string.Empty;

            var builder = new StringBuilder();
            
            foreach (var entry in sortedHeaders)
            {
                builder.Append(entry.Key.ToLowerInvariant());
                builder.Append(":");
                builder.Append(AWSSDKUtils.CompressSpaces(entry.Value));
                builder.Append("\n");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns the set of headers included in the signature as a flattened, ;-delimited string
        /// </summary>
        /// <param name="sortedHeaders">The headers included in the signature</param>
        /// <returns>Formatted string of header names</returns>
        protected static string CanonicalizeHeaderNames(IEnumerable<KeyValuePair<string, string>> sortedHeaders)
        {
            var builder = new StringBuilder();
            
            foreach (var header in sortedHeaders)
            {
                if (builder.Length > 0)
                    builder.Append(";");
                builder.Append(header.Key.ToLowerInvariant());
            }
            
            return builder.ToString();
        }

        /// <summary>
        /// Collects the subresource and query string parameters into one collection
        /// ready for canonicalization
        /// </summary>
        /// <param name="request">The in-flight request being signed</param>
        /// <returns>The fused set of parameters</returns>
        protected static List<KeyValuePair<string, string>> GetParametersToCanonicalize(IRequest request)
        {
            var parametersToCanonicalize = new List<KeyValuePair<string, string>>();

            if (request.SubResources != null && request.SubResources.Count > 0)
            {
                foreach (var subResource in request.SubResources)
                {
                    parametersToCanonicalize.Add(new KeyValuePair<string,string>(subResource.Key, subResource.Value));
                }
            }

            if (request.UseQueryString && request.Parameters != null && request.Parameters.Count > 0)
            {
                var requestParameters = request.ParameterCollection.GetSortedParametersList();
                foreach (var queryParameter in requestParameters.Where(queryParameter => queryParameter.Value != null))
                {
                    parametersToCanonicalize.Add(new KeyValuePair<string,string>(queryParameter.Key, queryParameter.Value));
                }
            }

            return parametersToCanonicalize;
        }

        protected static string CanonicalizeQueryParameters(string queryString)
        {
            return CanonicalizeQueryParameters(queryString, true);
        }

        /// <summary>
        /// Computes and returns the canonicalized query string, if query parameters have been supplied.
        /// Parameters with no value will be canonicalized as 'param='. The expectation is that parameters
        /// have not already been url encoded prior to canonicalization.
        /// </summary>
        /// <param name="queryString">The set of parameters being passed on the uri</param>
        /// <param name="uriEncodeParameters">
        /// Parameters must be uri encoded into the canonical request and by default the signer expects
        /// that the supplied collection contains non-encoded data. Set this to false if the encoding was
        /// done prior to signer entry.
        /// </param>
        /// <returns>The uri encoded query string parameters in canonical ordering</returns>
        protected static string CanonicalizeQueryParameters(string queryString, bool uriEncodeParameters)
        {
            if (string.IsNullOrEmpty(queryString))
                return string.Empty;

            var queryParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var queryParamsStart = queryString.IndexOf('?');
            var qs = queryString.Substring(++queryParamsStart); 
            var subStringPos = 0;
            var index = qs.IndexOfAny(new char[] { '&', ';' }, 0);
            if (index == -1 && subStringPos < qs.Length)
                index = qs.Length;
            while (index != -1)
            {
                var token = qs.Substring(subStringPos, index - subStringPos);

                // If the next character is a space then this isn't the end of query string value
                // Content Disposition is an example of this.
                if (!(index + 1 < qs.Length && qs[index + 1] == ' '))
                {
                    var equalPos = token.IndexOf('=');
                    if (equalPos == -1)
                        queryParams.Add(token, null);
                    else
                        queryParams.Add(token.Substring(0, equalPos), token.Substring(equalPos + 1));

                    subStringPos = index + 1;
                }

                if (qs.Length <= index + 1)
                    break;

                index = qs.IndexOfAny(new char[] { '&', ';' }, index + 1);
                if (index == -1 && subStringPos < qs.Length)
                    index = qs.Length;
            }

            return CanonicalizeQueryParameters(queryParams, uriEncodeParameters: uriEncodeParameters);
        }

        protected static string CanonicalizeQueryParameters(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return CanonicalizeQueryParameters(parameters, true);
        }
        /// <summary>
        /// Computes and returns the canonicalized query string, if query parameters have been supplied.
        /// Parameters with no value will be canonicalized as 'param='. The expectation is that parameters
        /// have not already been url encoded prior to canonicalization.
        /// </summary>
        /// <param name="parameters">The set of parameters to be encoded in the query string</param>
        /// <param name="uriEncodeParameters">
        /// Parameters must be uri encoded into the canonical request and by default the signer expects
        /// that the supplied collection contains non-encoded data. Set this to false if the encoding was
        /// done prior to signer entry.
        /// </param>
        /// <returns>The uri encoded query string parameters in canonical ordering</returns>
        protected static string CanonicalizeQueryParameters(
            IEnumerable<KeyValuePair<string, string>> parameters,
            bool uriEncodeParameters)
        {
            if (parameters == null)
                return string.Empty;

            var sortedParameters = parameters.OrderBy(kvp => kvp.Key, StringComparer.Ordinal).ToList();
            var canonicalQueryString = new StringBuilder();
            foreach (var param in sortedParameters)
            {
                var key = param.Key;
                var value = param.Value;

                if (canonicalQueryString.Length > 0)
                    canonicalQueryString.Append("&");
                if (uriEncodeParameters)
                {
                    if (string.IsNullOrEmpty(value))
                        canonicalQueryString.AppendFormat("{0}=", AWSSDKUtils.UrlEncode(key, false));
                    else
                        canonicalQueryString.AppendFormat("{0}={1}", AWSSDKUtils.UrlEncode(key, false), AWSSDKUtils.UrlEncode(value, false));
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                        canonicalQueryString.AppendFormat("{0}=", key);
                    else
                        canonicalQueryString.AppendFormat("{0}={1}", key, value);
                }
            }

            return canonicalQueryString.ToString();
        }

        /// <summary>
        /// Returns the request parameters in the form of a query string.
        /// </summary>
        /// <param name="request">The request instance</param>
        /// <returns>Request parameters in query string format</returns>
        static byte[] GetRequestPayloadBytes(IRequest request)
        {
            if (request.Content != null)
                return request.Content;

            var content = request.UseQueryString ? string.Empty : AWSSDKUtils.GetParametersAsString(request);
            return Encoding.UTF8.GetBytes(content);
        }
        #endregion
    }

    /// <summary>
    /// AWS4 protocol signer for Amazon S3 presigned urls.
    /// </summary>
    public class AWS4PreSignedUrlSigner : AWS4Signer
    {
        // 7 days is the maximum period for presigned url expiry with AWS4
        public const Int64 MaxAWS4PreSignedUrlExpiry = 7 * 24 * 60 * 60;

        internal const string XAmzSignature = "X-Amz-Signature";
        internal const string XAmzAlgorithm = "X-Amz-Algorithm";
        internal const string XAmzCredential = "X-Amz-Credential";
        internal const string XAmzExpires = "X-Amz-Expires";

        /// <summary>
        /// Calculates and signs the specified request using the AWS4 signing protocol by using the
        /// AWS account credentials given in the method parameters. The resulting signature is added
        /// to the request headers as 'Authorization'.
        /// </summary>
        /// <param name="request">
        /// The request to compute the signature for. Additional headers mandated by the AWS4 protocol 
        /// ('host' and 'x-amz-date') will be added to the request before signing.
        /// </param>
        /// <param name="clientConfig">
        /// Adding supporting data for the service call required by the signer (notably authentication
        /// region, endpoint and service name).
        /// </param>
        /// <param name="metrics">
        /// Metrics for the request
        /// </param>
        /// <param name="awsAccessKeyId">
        /// The AWS public key for the account making the service call.
        /// </param>
        /// <param name="awsSecretAccessKey">
        /// The AWS secret key for the account making the call, in clear text
        /// </param>
        /// <exception cref="Amazon.Runtime.SignatureException">
        /// If any problems are encountered while signing the request.
        /// </exception>
        public override void Sign(IRequest request,
                                  IClientConfig clientConfig,
                                  RequestMetrics metrics,
                                  string awsAccessKeyId,
                                  string awsSecretAccessKey)
        {
            throw new InvalidOperationException("PreSignedUrl signature computation is not supported by this method; use SignRequest instead.");
        }

        /// <summary>
        /// Calculates the AWS4 signature for a presigned url.
        /// </summary>
        /// <param name="request">
        /// The request to compute the signature for. Additional headers mandated by the AWS4 protocol 
        /// ('host' and 'x-amz-date') will be added to the request before signing. If the Expires parameter
        /// is present, it is renamed to 'X-Amz-Expires' before signing.
        /// </param>
        /// <param name="clientConfig">
        /// Adding supporting data for the service call required by the signer (notably authentication
        /// region, endpoint and service name).
        /// </param>
        /// <param name="metrics">
        /// Metrics for the request
        /// </param>
        /// <param name="awsAccessKeyId">
        /// The AWS public key for the account making the service call.
        /// </param>
        /// <param name="awsSecretAccessKey">
        /// The AWS secret key for the account making the call, in clear text
        /// </param>
        /// <exception cref="Amazon.Runtime.SignatureException">
        /// If any problems are encountered while signing the request.
        /// </exception>
        /// <remarks>
        /// Parameters passed as part of the resource path should be uri-encoded prior to
        /// entry to the signer. Parameters passed in the request.Parameters collection should
        /// be not be encoded; encoding will be done for these parameters as part of the 
        /// construction of the canonical request.
        /// </remarks>
        public new AWS4SigningResult SignRequest(IRequest request,
                                                 IClientConfig clientConfig,
                                                 RequestMetrics metrics,
                                                 string awsAccessKeyId,
                                                 string awsSecretAccessKey)
        {
            return SignRequest(request, clientConfig, metrics, awsAccessKeyId, awsSecretAccessKey, "s3", null);
        }

        /// <summary>
        /// Calculates the AWS4 signature for a presigned url.
        /// </summary>
        /// <param name="request">
        /// The request to compute the signature for. Additional headers mandated by the AWS4 protocol 
        /// ('host' and 'x-amz-date') will be added to the request before signing. If the Expires parameter
        /// is present, it is renamed to 'X-Amz-Expires' before signing.
        /// </param>
        /// <param name="clientConfig">
        /// Adding supporting data for the service call required by the signer (notably authentication
        /// region, endpoint and service name).
        /// </param>
        /// <param name="metrics">
        /// Metrics for the request
        /// </param>
        /// <param name="awsAccessKeyId">
        /// The AWS public key for the account making the service call.
        /// </param>
        /// <param name="awsSecretAccessKey">
        /// The AWS secret key for the account making the call, in clear text
        /// </param>
        /// <param name="service">
        /// The service to sign for
        /// </param>
        /// <param name="overrideSigningRegion">
        /// The region to sign to, if null then the region the client is configured for will be used.
        /// </param>
        /// <exception cref="Amazon.Runtime.SignatureException">
        /// If any problems are encountered while signing the request.
        /// </exception>
        /// <remarks>
        /// Parameters passed as part of the resource path should be uri-encoded prior to
        /// entry to the signer. Parameters passed in the request.Parameters collection should
        /// be not be encoded; encoding will be done for these parameters as part of the 
        /// construction of the canonical request.
        ///
        /// The X-Amz-Content-SHA256 is cleared out of the request.
        /// If the request is for S3 then the UNSIGNED_PAYLOAD value is used to generate the canonical request.
        /// If the request isn't for S3 then the empty body SHA is used to generate the canonical request.
        /// </remarks>
        public static AWS4SigningResult SignRequest(IRequest request,
                                                 IClientConfig clientConfig,
                                                 RequestMetrics metrics,
                                                 string awsAccessKeyId,
                                                 string awsSecretAccessKey,
                                                 string service,
                                                 string overrideSigningRegion)
        {
            // clean up any prior signature in the headers if resigning
            request.Headers.Remove(HeaderKeys.AuthorizationHeader);
            if (!request.Headers.ContainsKey(HeaderKeys.HostHeader))
            {
                var hostHeader = request.Endpoint.Host;
                if (!request.Endpoint.IsDefaultPort)
                    hostHeader += ":" + request.Endpoint.Port;
                request.Headers.Add(HeaderKeys.HostHeader, hostHeader);
            }

            var signedAt = CorrectClockSkew.GetCorrectedUtcNowForEndpoint(request.Endpoint.ToString());
            var region = overrideSigningRegion ?? DetermineSigningRegion(clientConfig, service, request.AlternateEndpoint, request);

            // AWS4 presigned urls got S3 are expected to contain a 'UNSIGNED-PAYLOAD' magic string
            // during signing (other services use the empty-body sha)
            if (request.Headers.ContainsKey(HeaderKeys.XAmzContentSha256Header))
                request.Headers.Remove(HeaderKeys.XAmzContentSha256Header);

            var sortedHeaders = SortAndPruneHeaders(request.Headers);
            var canonicalizedHeaderNames = CanonicalizeHeaderNames(sortedHeaders);

            var parametersToCanonicalize = GetParametersToCanonicalize(request);
            parametersToCanonicalize.Add(new KeyValuePair<string,string>(XAmzAlgorithm, AWS4AlgorithmTag));
            var xAmzCredentialValue = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}/{4}",
                                                       awsAccessKeyId,
                                                       FormatDateTime(signedAt, AWSSDKUtils.ISO8601BasicDateFormat),
                                                       region,
                                                       service,
                                                       Terminator);
            parametersToCanonicalize.Add(new KeyValuePair<string,string>(XAmzCredential, xAmzCredentialValue));

            parametersToCanonicalize.Add(new KeyValuePair<string,string>(HeaderKeys.XAmzDateHeader, FormatDateTime(signedAt, AWSSDKUtils.ISO8601BasicDateTimeFormat)));
            parametersToCanonicalize.Add(new KeyValuePair<string,string>(HeaderKeys.XAmzSignedHeadersHeader, canonicalizedHeaderNames));

            var canonicalQueryParams = CanonicalizeQueryParameters(parametersToCanonicalize);

            var canonicalRequest = CanonicalizeRequest(request.Endpoint,
                                                       request.ResourcePath,
                                                       request.HttpMethod,
                                                       sortedHeaders,
                                                       canonicalQueryParams,
                                                       service == "s3" ? UnsignedPayload : EmptyBodySha256,
                                                       request.PathResources,
                                                       request.MarshallerVersion,
                                                       service);
            if (metrics != null)
                metrics.AddProperty(Metric.CanonicalRequest, canonicalRequest);

            return ComputeSignature(awsAccessKeyId,
                                    awsSecretAccessKey,
                                    region,
                                    signedAt,
                                    service,
                                    canonicalizedHeaderNames,
                                    canonicalRequest,
                                    metrics);
        }
    }

    /// <summary>
    /// Encapsulates the various fields and eventual signing value that makes up 
    /// an AWS4 signature. This can be used to retrieve the required authorization string
    /// or authorization query parameters for the final request as well as hold ongoing
    /// signature computations for subsequent calls related to the initial signing.
    /// </summary>
    public class AWS4SigningResult
    {
        private readonly string _awsAccessKeyId;
        private readonly DateTime _originalDateTime;
        private readonly string _signedHeaders; 
        private readonly string _scope;
        private readonly byte[] _signingKey;
        private readonly byte[] _signature;

        /// <summary>
        /// Constructs a new signing result instance for a computed signature
        /// </summary>
        /// <param name="awsAccessKeyId">The access key that was included in the signature</param>
        /// <param name="signedAt">Date/time (UTC) that the signature was computed</param>
        /// <param name="signedHeaders">The collection of headers names that were included in the signature</param>
        /// <param name="scope">Formatted 'scope' value for signing (YYYYMMDD/region/service/aws4_request)</param>
        /// <param name="signingKey">Returns the key that was used to compute the signature</param>
        /// <param name="signature">Computed signature</param>
        public AWS4SigningResult(string awsAccessKeyId, 
                                 DateTime signedAt, 
                                 string signedHeaders, 
                                 string scope, 
                                 byte[] signingKey, 
                                 byte[] signature)
        {
            this._awsAccessKeyId = awsAccessKeyId;
            this._originalDateTime = signedAt;
            this._signedHeaders = signedHeaders;
            this._scope = scope;
            this._signingKey = signingKey;
            this._signature = signature;
        }

        /// <summary>
        /// The access key that was used in signature computation.
        /// </summary>
        public string AccessKeyId
        {
            get { return _awsAccessKeyId; }
        }

        /// <summary>
        /// ISO8601 formatted date/time that the signature was computed
        /// </summary>
        public string ISO8601DateTime
        {
            get { return AWS4Signer.FormatDateTime(_originalDateTime, AWSSDKUtils.ISO8601BasicDateTimeFormat); }
        }

        /// <summary>
        /// ISO8601 formatted date that the signature was computed
        /// </summary>
        public string ISO8601Date
        {
            get { return AWS4Signer.FormatDateTime(_originalDateTime, AWSSDKUtils.ISO8601BasicDateFormat); }
        }

        /// <summary>
        /// The ;-delimited collection of header names that were included in the signature computation
        /// </summary>
        public string SignedHeaders
        {
            get
            {
                return _signedHeaders;                
            }
        }

        /// <summary>
        /// Formatted 'scope' value for signing (YYYYMMDD/region/service/aws4_request)
        /// </summary>
        public string Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// Returns a copy of the key that was used to compute the signature
        /// </summary>
        public byte[] SigningKey
        {
            get
            {
                var kSigningCopy = new byte[_signingKey.Length];
                _signingKey.CopyTo(kSigningCopy, 0);
                return kSigningCopy;
            }
        }

        /// <summary>
        /// Returns the hex string representing the signature
        /// </summary>
        public string Signature
        {
            get
            {
                return AWSSDKUtils.ToHex(_signature, true);
            }
        }

        /// <summary>
        /// Returns a copy of the byte array containing the signature
        /// </summary>
        public byte[] SignatureBytes
        {
            get
            {
                var signatureCopy = new byte[_signature.Length];
                _signature.CopyTo(signatureCopy, 0);
                return signatureCopy;
            }
        }

        /// <summary>
        /// Returns the signature in a form usable as an 'Authorization' header value.
        /// </summary>
        public string ForAuthorizationHeader
        {
            get
            {
                var authorizationHeader = new StringBuilder();
                authorizationHeader.Append(AWS4Signer.AWS4AlgorithmTag);
                authorizationHeader.AppendFormat(" {0}={1}/{2},", AWS4Signer.Credential, AccessKeyId, Scope);
                authorizationHeader.AppendFormat(" {0}={1},", AWS4Signer.SignedHeaders, SignedHeaders);
                authorizationHeader.AppendFormat(" {0}={1}", AWS4Signer.Signature, Signature);

                return authorizationHeader.ToString();
            }
        }

        /// <summary>
        /// Returns the signature in a form usable as a set of query string parameters.
        /// </summary>
        public string ForQueryParameters
        {
            get
            {
                var authParams = new StringBuilder();

                authParams.AppendFormat("{0}={1}",
                                        AWS4PreSignedUrlSigner.XAmzAlgorithm,
                                        AWS4Signer.AWS4AlgorithmTag);
                authParams.AppendFormat("&{0}={1}",
                                        AWS4PreSignedUrlSigner.XAmzCredential,
                                        string.Format(CultureInfo.InvariantCulture, "{0}/{1}", AccessKeyId, Scope));
                authParams.AppendFormat("&{0}={1}", HeaderKeys.XAmzDateHeader, ISO8601DateTime);
                authParams.AppendFormat("&{0}={1}", HeaderKeys.XAmzSignedHeadersHeader, SignedHeaders);
                authParams.AppendFormat("&{0}={1}", AWS4PreSignedUrlSigner.XAmzSignature, Signature);

                return authParams.ToString();
            }
        }
    }
}
