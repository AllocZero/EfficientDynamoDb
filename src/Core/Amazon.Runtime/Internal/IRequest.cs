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
    /// Represents a request being sent to an Amazon Web Service, including the
    /// parameters being sent as part of the request, the endpoint to which the
    /// request should be sent, etc.
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// The name of the request
        /// </summary>
        string RequestName
        {
            get;
        }

        /// <summary>
        /// Returns a dictionary of the headers included in this request.
        /// </summary>
        IDictionary<string, string> Headers
        {
            get;
        }

        /// <summary>
        /// Gets and sets a flag that indicates whether the request is sent as a query string instead of the request body.
        /// </summary>
        bool UseQueryString
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a dictionary of the parameters included in this request.
        /// </summary>
        IDictionary<String, String> Parameters
        {
            get;
        }

        /// <summary>
        /// Collection of parameters included in this request.
        /// </summary>
        ParameterCollection ParameterCollection
        {
            get;
        }

        /// <summary>
        /// Returns the subresources that should be appended to the resource path.
        /// This is used primarily for Amazon S3, where object keys can contain '?'
        /// characters, making string-splitting of a resource path potentially 
        /// hazardous.
        /// </summary>
        IDictionary<string, string> SubResources
        {
            get;
        }

        /// <summary>
        /// Adds a new null entry to the SubResources collection for the request
        /// </summary>
        /// <param name="subResource">The name of the subresource</param>
        void AddSubResource(string subResource);

        /// <summary>
        /// Adds a new entry to the SubResources collection for the request
        /// </summary>
        /// <param name="subResource">The name of the subresource</param>
        /// <param name="value">Value of the entry</param>
        void AddSubResource(string subResource, string value);

        /// <summary>
        /// Gets and sets the type of http request to make, whether it should be POST,GET or DELETE
        /// </summary>
        string HttpMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets the endpoint for this request.
        /// </summary>
        Uri Endpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets the resource path added on to the endpoint.
        /// </summary>
        string ResourcePath
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the path resources that should be used within the resource path.
        /// This is used for services where path keys can contain '/'
        /// characters, making string-splitting of a resource path potentially 
        /// hazardous.
        /// </summary>
        IDictionary<string, string> PathResources
        {
            get;
        }

        /// <summary>
        /// Adds a new entry to the PathResources collection for the request
        /// </summary>
        /// <param name="key">The name of the pathresource with potential greedy syntax: {key+}</param>
        /// <param name="value">Value of the entry</param>
        void AddPathResource(string key, string value);

        /// <summary>
        /// Gets and Sets the version number for the marshaller used to create this request. The version number
        /// is used to support backward compatible changes that would otherwise be breaking changes when a 
        /// newer core is used with an older service assembly.
        /// </summary>
        int MarshallerVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets the content for this request.
        /// </summary>
        byte[] Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the header value from the request.
        /// </summary>
        string GetHeaderValue(string headerName);

        /// <summary>
        /// Flag that signals that Content was and should be set
        /// from the Parameters collection.
        /// </summary>
        bool SetContentFromParameters { get; set; }

        /// <summary>
        /// Gets and sets the content stream.
        /// </summary>
        Stream ContentStream
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the original stream position.
        /// If ContentStream is null or does not support seek, this propery
        /// should be equal to -1.
        /// </summary>
        long OriginalStreamPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Computes the SHA 256 hash of the content stream. If the stream is not
        /// seekable, it searches the parent stream hierarchy to find a seekable
        /// stream prior to computation. Once computed, the hash is cached for future
        /// use.
        /// </summary>
        string ComputeContentStreamHash();

        /// <summary>
        /// If the request needs to be signed with a different service name 
        /// than the client config AuthenticationServiceName, set it here to override
        /// the result of DetermineService in AWS4Signer
        /// </summary>
        string OverrideSigningServiceName { get; set; }

        /// <summary>
        /// The name of the service to which this request is being sent.
        /// </summary>
        string ServiceName
        {
            get;
        }

        /// <summary>
        /// Returns the original, user facing request object which this internal
        /// request object is representing.
        /// </summary>
        AmazonWebServiceRequest OriginalRequest
        {
            get;
        }

        /// <summary>
        /// Alternate endpoint to use for this request, if any.
        /// </summary>
        RegionEndpoint AlternateEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Host prefix value to prepend to the endpoint for this request, if any. 
        /// </summary>
        string HostPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the Suppress404Exceptions property. If true then 404s return back from AWS will not cause an exception and 
        /// an empty response object will be returned.
        /// </summary>
        bool Suppress404Exceptions
        {
            get;
            set;
        }

        /// <summary>
        /// If using AWS4 signing protocol, contains the resultant parts of the
        /// signature that we may need to make use of if we elect to do a chunked
        /// encoding upload.
        /// </summary>
        AWS4SigningResult AWS4SignerResult
        {
            get; 
            set;
        }

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
        bool? DisablePayloadSigning
        {
            get;
            set;
        }

        /// <summary>
        /// Determine whether to use a chunked encoding upload for the request
        /// (applies to Amazon S3 PutObject and UploadPart requests only). If 
        /// DisablePayloadSigning is true, UseChunkEncoding will be automatically 
        /// set to false.
        /// </summary>
        bool UseChunkEncoding
        {
            get; 
            set;
        }

        /// <summary>
        /// Used for Amazon S3 requests where the bucket name is removed from
        /// the marshalled resource path into the host header. To comply with
        /// AWS2 signature calculation, we need to recover the bucket name
        /// and include it in the resource canonicalization, which we do using
        /// this field.
        /// </summary>
        string CanonicalResourcePrefix
        {
            get; 
            set;
        }

        /// <summary>
        /// This flag specifies if SigV4 is required for the current request.
        /// </summary>
        bool UseSigV4 { get; set; }

        /// <summary>
        /// The authentication region to use for the request.
        /// Set from Config.AuthenticationRegion.
        /// </summary>
        string AuthenticationRegion { get; set; }

        /// <summary>
        /// The region in which the service request was signed.
        /// </summary>
        string DeterminedSigningRegion { get; set; }

        /// <summary>
        /// Checks if the request stream can be rewinded.
        /// </summary>
        /// <returns>Returns true if the request stream can be rewinded ,
        /// else false.</returns>
        bool IsRequestStreamRewindable();

        /// <summary>
        /// Returns true if the request can contain a request body, else false.
        /// </summary>
        /// <returns>Returns true if the currect request can contain a request body, else false.</returns>
        bool MayContainRequestBody();

        /// <summary>
        /// Returns true if the request has a body, else false.
        /// </summary>
        /// <returns>Returns true if the request has a body, else false.</returns>
        bool HasRequestBody();
    }
}
