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
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.Internal.Auth;
using Amazon.Util;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Default implementation of the IRequest interface.
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public class DefaultRequest : IRequest
    {
        readonly ParameterCollection parametersCollection;
        readonly IDictionary<string,string> parametersFacade;
        readonly IDictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        readonly IDictionary<string, string> subResources = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        readonly IDictionary<string, string> pathResources = new Dictionary<string, string>(StringComparer.Ordinal);

        Uri endpoint;
        string resourcePath;
        string serviceName;
        readonly AmazonWebServiceRequest originalRequest;
        byte[] content;
        Stream contentStream;
        string contentStreamHash;
        string httpMethod = "POST";
        bool useQueryString = false;
        string requestName;
        string canonicalResource;
        RegionEndpoint alternateRegion;        
        long originalStreamLength;
        int marshallerVersion = 1; //1 is the default version and must be used whenever a version is not specified in the marshaller.

        /// <summary>
        /// Constructs a new DefaultRequest with the specified service name and the
        /// original, user facing request object.
        /// </summary>
        /// <param name="request">The orignal request that is being wrapped</param>
        /// <param name="serviceName">The service name</param>
        public DefaultRequest(AmazonWebServiceRequest request, String serviceName)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentNullException("serviceName");

            this.serviceName = serviceName;
            this.originalRequest = request;
            this.requestName = this.originalRequest.GetType().Name;
            this.UseSigV4 = ((Amazon.Runtime.Internal.IAmazonWebServiceRequest)this.originalRequest).UseSigV4;
            this.HostPrefix = string.Empty;

            parametersCollection = new ParameterCollection();
            parametersFacade = new ParametersDictionaryFacade(parametersCollection);
        }


        /// <summary>
        /// The name of the request
        /// </summary>
        public string RequestName
        {
            get { return this.requestName; }
        }


        /// <summary>
        /// Gets and sets the type of http request to make, whether it should be POST,GET or DELETE
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return this.httpMethod;
            }
            set
            {
                this.httpMethod = value;
            }
        }

        /// <summary>
        /// Gets and sets a flag that indicates whether the request is sent as a query string instead of the request body.
        /// </summary>
        public bool UseQueryString
        {
            get
            {
                if (this.HttpMethod == "GET")
                    return true;
                return this.useQueryString;
            }
            set
            {
                this.useQueryString = value;
            }
        }

        /// <summary>
        /// Returns the original, user facing request object which this internal
        /// request object is representing.
        /// </summary>
        public AmazonWebServiceRequest OriginalRequest
        {
            get
            {
                return originalRequest;
            }
        }

        /// <summary>
        /// Returns a dictionary of the headers included in this request.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get
            {
                return this.headers;
            }
        }


        /// <summary>
        /// Returns a dictionary of the parameters included in this request.
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get
            {
                return this.parametersFacade;
            }
        }

        /// <summary>
        /// Collection of parameters included in this request.
        /// </summary>
        public ParameterCollection ParameterCollection
        {
            get
            {
                return this.parametersCollection;
            }
        }

        /// <summary>
        /// Returns the subresources that should be appended to the resource path.
        /// This is used primarily for Amazon S3, where object keys can contain '?'
        /// characters, making string-splitting of a resource path potentially 
        /// hazardous.
        /// </summary>
        public IDictionary<string, string> SubResources
        {
            get
            {
                return this.subResources;
            }
        }

        /// <summary>
        /// Adds a new null entry to the SubResources collection for the request
        /// </summary>
        /// <param name="subResource">The name of the subresource</param>
        public void AddSubResource(string subResource)
        {
            AddSubResource(subResource, null);
        }

        /// <summary>
        /// Adds a new entry to the SubResources collection for the request
        /// </summary>
        /// <param name="subResource">The name of the subresource</param>
        /// <param name="value">Value of the entry</param>
        public void AddSubResource(string subResource, string value)
        {
            SubResources.Add(subResource, value);
        }

        /// <summary>
        /// Gets and Sets the endpoint for this request.
        /// </summary>
        public Uri Endpoint
        {
            get
            {
                return this.endpoint;
            }
            set
            {
                this.endpoint = value;
            }
        }

        /// <summary>
        /// Gets and Sets the resource path added on to the endpoint.
        /// </summary>
        public string ResourcePath
        {
            get
            {
                return this.resourcePath;
            }
            set
            {
                this.resourcePath = value;
            }
        }

        /// <summary>
        /// Returns the path resources that should be used within the resource path.
        /// This is used for services where path keys can contain '/'
        /// characters, making string-splitting of a resource path potentially 
        /// hazardous.
        /// </summary>
        public IDictionary<string, string> PathResources
        {
            get
            {
                return this.pathResources;
            }
        }

        /// <summary>
        /// Adds a new entry to the PathResources collection for the request
        /// </summary>
        /// <param name="key">The name of the pathresource with potential greedy syntax: {key+}</param>
        /// <param name="value">Value of the entry</param>
        public void AddPathResource(string key, string value)
        {
            PathResources.Add(key, value);
        }

        /// <summary>
        /// Gets and Sets the version number for the marshaller used to create this request. The version number
        /// is used to support backward compatible changes that would otherwise be breaking changes when a 
        /// newer core is used with an older service assembly.
        /// Versions:
        ///     1 - Default version
        ///     2 - Support for path segments
        /// </summary>
        public int MarshallerVersion
        {
            get
            {
                return this.marshallerVersion;
            }            
            set
            {
                this.marshallerVersion = value;
            }
        }

        public string CanonicalResource
        {
            get
            {
                return this.canonicalResource;
            }
            set
            {
                this.canonicalResource = value;
            }
        }


        /// <summary>
        /// Gets and Sets the content for this request.
        /// </summary>
        public byte[] Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        /// <summary>
        /// Flag that signals that Content was and should be set
        /// from the Parameters collection.
        /// </summary>
        public bool SetContentFromParameters { get; set; }

        /// <summary>
        /// Gets and sets the content stream.
        /// </summary>
        public Stream ContentStream
        {
            get { return this.contentStream; }
            set
            {
                this.contentStream = value;
                OriginalStreamPosition = -1;
                if (this.contentStream != null)
                {
                    Stream baseStream = HashStream.GetNonWrapperBaseStream(this.contentStream);
                    if (baseStream.CanSeek)
                        OriginalStreamPosition = baseStream.Position;
                }
            }
        }

        /// <summary>
        /// Gets and sets the original stream position.
        /// If ContentStream is null or does not support seek, this propery
        /// should be equal to -1.
        /// </summary>
        public long OriginalStreamPosition
        {
            get { return this.originalStreamLength; }
            set { this.originalStreamLength = value; }
        }

        /// <summary>
        /// Computes the SHA 256 hash of the content stream. If the stream is not
        /// seekable, it searches the parent stream hierarchy to find a seekable
        /// stream prior to computation. Once computed, the hash is cached for future
        /// use. If a suitable stream cannot be found to use, null is returned.
        /// </summary>
        public string ComputeContentStreamHash()
        {
            if (this.contentStream == null)
                return null;

            if (this.contentStreamHash == null)
            {
                var seekableStream = WrapperStream.SearchWrappedStream(this.contentStream, s => s.CanSeek);
                if (seekableStream != null)
                {
                    var position = seekableStream.Position;
                    byte[] payloadHashBytes = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(seekableStream);
                    this.contentStreamHash = AWSSDKUtils.ToHex(payloadHashBytes, true);
                    seekableStream.Seek(position, SeekOrigin.Begin);
                }
            }

            return this.contentStreamHash;
        }

        /// <summary>
        /// The name of the service to which this request is being sent.
        /// </summary>
        public string ServiceName
        {
            get
            {
                return this.serviceName;
            }
        }

        /// <summary>
        /// Alternate endpoint to use for this request, if any.
        /// </summary>
        public RegionEndpoint AlternateEndpoint
        {
            get
            {
                return this.alternateRegion;
            }
            set
            {
                this.alternateRegion = value;
            }
        }

        /// <summary>
        /// Host prefix value to prepend to the endpoint for this request, if any.
        /// </summary>
        public string HostPrefix { get; set; }

        /// <summary>
        /// Gets and sets the Suppress404Exceptions property. If true then 404s return back from AWS will not cause an exception and 
        /// an empty response object will be returned.
        /// </summary>
        public bool Suppress404Exceptions
        {
            get;
            set;
        }

        /// <summary>
        /// If using AWS4 signing protocol, contains the resultant parts of the
        /// signature that we may need to make use of if we elect to do a chunked
        /// encoding upload.
        /// </summary>
        public AWS4SigningResult AWS4SignerResult { get; set; }

        /// <summary>      
        /// <para><b>WARNING: Setting DisablePayloadSigning to true disables the SigV4 payload signing 
        /// data integrity check on this request.</b></para>  
        /// <para>If using SigV4, the DisablePayloadSigning flag controls if the payload should be 
        /// signed on a request by request basis. By default this flag is null which will use the 
        /// default client behavior. The default client behavior is to sign the payload. When 
        /// DisablePayloadSigning is true, the request will be signed with an UNSIGNED-PAYLOAD value. 
        /// Setting DisablePayloadSigning to true requires that the request is sent over a HTTPS 
        /// connection.</para>        
        /// <para>Under certain circumstances, such as uploading to S3 while using MD5 hashing, it may 
        /// be desireable to use UNSIGNED-PAYLOAD to decrease signing CPU usage. This flag only applies 
        /// to Amazon S3 PutObject and UploadPart requests.</para>
        /// <para>MD5Stream, SigV4 payload signing, and HTTPS each provide some data integrity 
        /// verification. If DisableMD5Stream is true and DisablePayloadSigning is true, then the 
        /// possibility of data corruption is completely dependant on HTTPS being the only remaining 
        /// source of data integrity verification.</para>
        /// </summary>
        public bool? DisablePayloadSigning { get; set; }

        /// <summary>
        /// Determine whether to use a chunked encoding upload for the request
        /// (applies to Amazon S3 PutObject and UploadPart requests only).  If 
        /// DisablePayloadSigning is true, UseChunkEncoding will be automatically 
        /// set to false.
        /// </summary>
        public bool UseChunkEncoding { get; set; }

        /// <summary>
        /// Used for Amazon S3 requests where the bucket name is removed from
        /// the marshalled resource path into the host header. To comply with
        /// AWS2 signature calculation, we need to recover the bucket name
        /// and include it in the resource canonicalization, which we do using
        /// this field.
        /// </summary>
        public string CanonicalResourcePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// This flag specifies if SigV4 is required for the current request.
        /// </summary>
        public bool UseSigV4 { get; set; }

        /// <summary>
        /// The authentication region to use for the request.
        /// Set from Config.AuthenticationRegion.
        /// </summary>
        public string AuthenticationRegion { get; set; }

        /// <summary>
        /// The region in which the service request was signed.
        /// </summary>
        public string DeterminedSigningRegion { get ; set; }

        /// <summary>
        /// If the request needs to be signed with a different service name 
        /// than the client config AuthenticationServiceName, set it here to override
        /// the result of DetermineService in AWS4Signer
        /// </summary>
        public string OverrideSigningServiceName { get; set; }

        /// <summary>
        /// Checks if the request stream can be rewinded.
        /// </summary>
        /// <returns>Returns true if the request stream can be rewinded ,
        /// else false.</returns>
        public bool IsRequestStreamRewindable()
        {
            var stream = this.ContentStream;
            // Retries may not be possible with a stream
            if (stream != null)
            {
                // Pull out the underlying non-wrapper stream
                stream = WrapperStream.GetNonWrapperBaseStream(stream);

                // Retry is possible if stream is seekable
                return stream.CanSeek;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the request can contain a request body, else false.
        /// </summary>
        /// <returns>Returns true if the currect request can contain a request body, else false.</returns>
        public bool MayContainRequestBody()
        {
            return
                (this.HttpMethod == "POST" ||
                 this.HttpMethod == "PUT" ||
                 this.HttpMethod == "PATCH");
        }

        /// <summary>
        /// Returns true if the request has a body, else false.
        /// </summary>
        /// <returns>Returns true if the request has a body, else false.</returns>
        public bool HasRequestBody()
        {
            var isPutPost = (this.HttpMethod == "POST" || this.HttpMethod == "PUT" || this.HttpMethod == "PATCH");
            var hasContent = this.HasRequestData();
            return (isPutPost && hasContent);
        }

        public string GetHeaderValue(string headerName)
        {
            string headerValue;
            if (headers.TryGetValue(headerName, out headerValue))
                return headerValue;

            return string.Empty;
        }
    }
}
