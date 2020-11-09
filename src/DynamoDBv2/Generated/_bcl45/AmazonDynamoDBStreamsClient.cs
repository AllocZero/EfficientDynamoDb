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

/*
 * Do not modify this file. This file is generated from the dynamodbstreams-2012-08-10.normal.json service model.
 */


using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.DynamoDBv2.Internal;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.DynamoDBv2
{
    /// <summary>
    /// Implementation for accessing DynamoDBStreams
    ///
    /// Amazon DynamoDB 
    /// <para>
    /// Amazon DynamoDB Streams provides API actions for accessing streams and processing
    /// stream records. To learn more about application development with Streams, see <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Streams.html">Capturing
    /// Table Activity with DynamoDB Streams</a> in the Amazon DynamoDB Developer Guide.
    /// </para>
    /// </summary>
    public partial class AmazonDynamoDBStreamsClient : AmazonServiceClient, IAmazonDynamoDBStreams
    {
        private static IServiceMetadata serviceMetadata = new AmazonDynamoDBStreamsMetadata();
        #region Constructors

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with the credentials loaded from the application's
        /// default configuration, and if unsuccessful from the Instance Profile service on an EC2 instance.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSProfileName" value="AWS Default"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        public AmazonDynamoDBStreamsClient()
            : base(FallbackCredentialsFactory.GetCredentials(), new AmazonDynamoDBStreamsConfig()) { }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with the credentials loaded from the application's
        /// default configuration, and if unsuccessful from the Instance Profile service on an EC2 instance.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSProfileName" value="AWS Default"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBStreamsClient(RegionEndpoint region)
            : base(FallbackCredentialsFactory.GetCredentials(), new AmazonDynamoDBStreamsConfig{RegionEndpoint = region}) { }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with the credentials loaded from the application's
        /// default configuration, and if unsuccessful from the Instance Profile service on an EC2 instance.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSProfileName" value="AWS Default"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        /// <param name="config">The AmazonDynamoDBStreamsClient Configuration Object</param>
        public AmazonDynamoDBStreamsClient(AmazonDynamoDBStreamsConfig config)
            : base(FallbackCredentialsFactory.GetCredentials(), config) { }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Credentials
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        public AmazonDynamoDBStreamsClient(AWSCredentials credentials)
            : this(credentials, new AmazonDynamoDBStreamsConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Credentials
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBStreamsClient(AWSCredentials credentials, RegionEndpoint region)
            : this(credentials, new AmazonDynamoDBStreamsConfig{RegionEndpoint = region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Credentials and an
        /// AmazonDynamoDBStreamsClient Configuration object.
        /// </summary>
        /// <param name="credentials">AWS Credentials</param>
        /// <param name="clientConfig">The AmazonDynamoDBStreamsClient Configuration Object</param>
        public AmazonDynamoDBStreamsClient(AWSCredentials credentials, AmazonDynamoDBStreamsConfig clientConfig)
            : base(credentials, clientConfig)
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey)
            : this(awsAccessKeyId, awsSecretAccessKey, new AmazonDynamoDBStreamsConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region)
            : this(awsAccessKeyId, awsSecretAccessKey, new AmazonDynamoDBStreamsConfig() {RegionEndpoint=region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID, AWS Secret Key and an
        /// AmazonDynamoDBStreamsClient Configuration object. 
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="clientConfig">The AmazonDynamoDBStreamsClient Configuration Object</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey, AmazonDynamoDBStreamsConfig clientConfig)
            : base(awsAccessKeyId, awsSecretAccessKey, clientConfig)
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken)
            : this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonDynamoDBStreamsConfig())
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        /// <param name="region">The region to connect.</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, RegionEndpoint region)
            : this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonDynamoDBStreamsConfig{RegionEndpoint = region})
        {
        }

        /// <summary>
        /// Constructs AmazonDynamoDBStreamsClient with AWS Access Key ID, AWS Secret Key and an
        /// AmazonDynamoDBStreamsClient Configuration object. 
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="awsSessionToken">AWS Session Token</param>
        /// <param name="clientConfig">The AmazonDynamoDBStreamsClient Configuration Object</param>
        public AmazonDynamoDBStreamsClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, AmazonDynamoDBStreamsConfig clientConfig)
            : base(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, clientConfig)
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates the signer for the service.
        /// </summary>
        protected override AbstractAWSSigner CreateSigner()
        {
            return new AWS4Signer();
        }    

        /// <summary>
        /// Customize the pipeline
        /// </summary>
        /// <param name="pipeline"></param>
        protected override void CustomizeRuntimePipeline(RuntimePipeline pipeline)
        {
            if(this.Config.RetryMode == RequestRetryMode.Legacy)
            {
                pipeline.ReplaceHandler<Amazon.Runtime.Internal.RetryHandler>(new Amazon.Runtime.Internal.RetryHandler(new Amazon.DynamoDBv2.Internal.DynamoDBRetryPolicy(this.Config)));
            }
        }    
        /// <summary>
        /// Capture metadata for the service.
        /// </summary>
        protected override IServiceMetadata ServiceMetadata
        {
            get
            {
                return serviceMetadata;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the service client.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion


        #region  DescribeStream


        /// <summary>
        /// Returns information about a stream, including the current status of the stream, its
        /// Amazon Resource Name (ARN), the composition of its shards, and its corresponding DynamoDB
        /// table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>DescribeStream</code> at a maximum rate of 10 times per second.
        /// </para>
        ///  </note> 
        /// <para>
        /// Each shard in the stream has a <code>SequenceNumberRange</code> associated with it.
        /// If the <code>SequenceNumberRange</code> has a <code>StartingSequenceNumber</code>
        /// but no <code>EndingSequenceNumber</code>, then the shard is still open (able to receive
        /// more stream records). If both <code>StartingSequenceNumber</code> and <code>EndingSequenceNumber</code>
        /// are present, then that shard is closed and can no longer receive more data.
        /// </para>
        /// </summary>
        /// <param name="streamArn">The Amazon Resource Name (ARN) for the stream.</param>
        /// 
        /// <returns>The response from the DescribeStream service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/DescribeStream">REST API Reference for DescribeStream Operation</seealso>
        public virtual DescribeStreamResponse DescribeStream(string streamArn)
        {
            var request = new DescribeStreamRequest();
            request.StreamArn = streamArn;
            return DescribeStream(request);
        }


        /// <summary>
        /// Returns information about a stream, including the current status of the stream, its
        /// Amazon Resource Name (ARN), the composition of its shards, and its corresponding DynamoDB
        /// table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>DescribeStream</code> at a maximum rate of 10 times per second.
        /// </para>
        ///  </note> 
        /// <para>
        /// Each shard in the stream has a <code>SequenceNumberRange</code> associated with it.
        /// If the <code>SequenceNumberRange</code> has a <code>StartingSequenceNumber</code>
        /// but no <code>EndingSequenceNumber</code>, then the shard is still open (able to receive
        /// more stream records). If both <code>StartingSequenceNumber</code> and <code>EndingSequenceNumber</code>
        /// are present, then that shard is closed and can no longer receive more data.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeStream service method.</param>
        /// 
        /// <returns>The response from the DescribeStream service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/DescribeStream">REST API Reference for DescribeStream Operation</seealso>
        public virtual DescribeStreamResponse DescribeStream(DescribeStreamRequest request)
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = DescribeStreamRequestMarshaller.Instance;
            options.ResponseUnmarshaller = DescribeStreamResponseUnmarshaller.Instance;

            return Invoke<DescribeStreamResponse>(request, options);
        }


        /// <summary>
        /// Returns information about a stream, including the current status of the stream, its
        /// Amazon Resource Name (ARN), the composition of its shards, and its corresponding DynamoDB
        /// table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>DescribeStream</code> at a maximum rate of 10 times per second.
        /// </para>
        ///  </note> 
        /// <para>
        /// Each shard in the stream has a <code>SequenceNumberRange</code> associated with it.
        /// If the <code>SequenceNumberRange</code> has a <code>StartingSequenceNumber</code>
        /// but no <code>EndingSequenceNumber</code>, then the shard is still open (able to receive
        /// more stream records). If both <code>StartingSequenceNumber</code> and <code>EndingSequenceNumber</code>
        /// are present, then that shard is closed and can no longer receive more data.
        /// </para>
        /// </summary>
        /// <param name="streamArn">The Amazon Resource Name (ARN) for the stream.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeStream service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/DescribeStream">REST API Reference for DescribeStream Operation</seealso>
        public virtual Task<DescribeStreamResponse> DescribeStreamAsync(string streamArn, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new DescribeStreamRequest();
            request.StreamArn = streamArn;
            return DescribeStreamAsync(request, cancellationToken);
        }


        /// <summary>
        /// Returns information about a stream, including the current status of the stream, its
        /// Amazon Resource Name (ARN), the composition of its shards, and its corresponding DynamoDB
        /// table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>DescribeStream</code> at a maximum rate of 10 times per second.
        /// </para>
        ///  </note> 
        /// <para>
        /// Each shard in the stream has a <code>SequenceNumberRange</code> associated with it.
        /// If the <code>SequenceNumberRange</code> has a <code>StartingSequenceNumber</code>
        /// but no <code>EndingSequenceNumber</code>, then the shard is still open (able to receive
        /// more stream records). If both <code>StartingSequenceNumber</code> and <code>EndingSequenceNumber</code>
        /// are present, then that shard is closed and can no longer receive more data.
        /// </para>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the DescribeStream service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the DescribeStream service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/DescribeStream">REST API Reference for DescribeStream Operation</seealso>
        public virtual Task<DescribeStreamResponse> DescribeStreamAsync(DescribeStreamRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = DescribeStreamRequestMarshaller.Instance;
            options.ResponseUnmarshaller = DescribeStreamResponseUnmarshaller.Instance;
            
            return InvokeAsync<DescribeStreamResponse>(request, options, cancellationToken);
        }

        #endregion
        
        #region  GetRecords


        /// <summary>
        /// Retrieves the stream records from a given shard.
        /// 
        ///  
        /// <para>
        /// Specify a shard iterator using the <code>ShardIterator</code> parameter. The shard
        /// iterator specifies the position in the shard from which you want to start reading
        /// stream records sequentially. If there are no stream records available in the portion
        /// of the shard that the iterator points to, <code>GetRecords</code> returns an empty
        /// list. Note that it might take multiple calls to get to a portion of the shard that
        /// contains stream records.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>GetRecords</code> can retrieve a maximum of 1 MB of data or 1000 stream records,
        /// whichever comes first.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="shardIterator">A shard iterator that was retrieved from a previous GetShardIterator operation. This iterator can be used to access the stream records in this shard.</param>
        /// 
        /// <returns>The response from the GetRecords service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ExpiredIteratorException">
        /// The shard iterator has expired and can no longer be used to retrieve stream records.
        /// A shard iterator expires 15 minutes after it is retrieved using the <code>GetShardIterator</code>
        /// action.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetRecords">REST API Reference for GetRecords Operation</seealso>
        public virtual GetRecordsResponse GetRecords(string shardIterator)
        {
            var request = new GetRecordsRequest();
            request.ShardIterator = shardIterator;
            return GetRecords(request);
        }


        /// <summary>
        /// Retrieves the stream records from a given shard.
        /// 
        ///  
        /// <para>
        /// Specify a shard iterator using the <code>ShardIterator</code> parameter. The shard
        /// iterator specifies the position in the shard from which you want to start reading
        /// stream records sequentially. If there are no stream records available in the portion
        /// of the shard that the iterator points to, <code>GetRecords</code> returns an empty
        /// list. Note that it might take multiple calls to get to a portion of the shard that
        /// contains stream records.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>GetRecords</code> can retrieve a maximum of 1 MB of data or 1000 stream records,
        /// whichever comes first.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetRecords service method.</param>
        /// 
        /// <returns>The response from the GetRecords service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ExpiredIteratorException">
        /// The shard iterator has expired and can no longer be used to retrieve stream records.
        /// A shard iterator expires 15 minutes after it is retrieved using the <code>GetShardIterator</code>
        /// action.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetRecords">REST API Reference for GetRecords Operation</seealso>
        public virtual GetRecordsResponse GetRecords(GetRecordsRequest request)
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = GetRecordsRequestMarshaller.Instance;
            options.ResponseUnmarshaller = GetRecordsResponseUnmarshaller.Instance;

            return Invoke<GetRecordsResponse>(request, options);
        }


        /// <summary>
        /// Retrieves the stream records from a given shard.
        /// 
        ///  
        /// <para>
        /// Specify a shard iterator using the <code>ShardIterator</code> parameter. The shard
        /// iterator specifies the position in the shard from which you want to start reading
        /// stream records sequentially. If there are no stream records available in the portion
        /// of the shard that the iterator points to, <code>GetRecords</code> returns an empty
        /// list. Note that it might take multiple calls to get to a portion of the shard that
        /// contains stream records.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>GetRecords</code> can retrieve a maximum of 1 MB of data or 1000 stream records,
        /// whichever comes first.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="shardIterator">A shard iterator that was retrieved from a previous GetShardIterator operation. This iterator can be used to access the stream records in this shard.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetRecords service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ExpiredIteratorException">
        /// The shard iterator has expired and can no longer be used to retrieve stream records.
        /// A shard iterator expires 15 minutes after it is retrieved using the <code>GetShardIterator</code>
        /// action.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetRecords">REST API Reference for GetRecords Operation</seealso>
        public virtual Task<GetRecordsResponse> GetRecordsAsync(string shardIterator, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new GetRecordsRequest();
            request.ShardIterator = shardIterator;
            return GetRecordsAsync(request, cancellationToken);
        }


        /// <summary>
        /// Retrieves the stream records from a given shard.
        /// 
        ///  
        /// <para>
        /// Specify a shard iterator using the <code>ShardIterator</code> parameter. The shard
        /// iterator specifies the position in the shard from which you want to start reading
        /// stream records sequentially. If there are no stream records available in the portion
        /// of the shard that the iterator points to, <code>GetRecords</code> returns an empty
        /// list. Note that it might take multiple calls to get to a portion of the shard that
        /// contains stream records.
        /// </para>
        ///  <note> 
        /// <para>
        ///  <code>GetRecords</code> can retrieve a maximum of 1 MB of data or 1000 stream records,
        /// whichever comes first.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetRecords service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetRecords service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.ExpiredIteratorException">
        /// The shard iterator has expired and can no longer be used to retrieve stream records.
        /// A shard iterator expires 15 minutes after it is retrieved using the <code>GetShardIterator</code>
        /// action.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.LimitExceededException">
        /// Your request rate is too high. The AWS SDKs for DynamoDB automatically retry requests
        /// that receive this exception. Your request is eventually successful, unless your retry
        /// queue is too large to finish. Reduce the frequency of requests and use exponential
        /// backoff. For more information, go to <a href="http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ErrorHandling.html#APIRetries">Error
        /// Retries and Exponential Backoff</a> in the <i>Amazon DynamoDB Developer Guide</i>.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetRecords">REST API Reference for GetRecords Operation</seealso>
        public virtual Task<GetRecordsResponse> GetRecordsAsync(GetRecordsRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = GetRecordsRequestMarshaller.Instance;
            options.ResponseUnmarshaller = GetRecordsResponseUnmarshaller.Instance;
            
            return InvokeAsync<GetRecordsResponse>(request, options, cancellationToken);
        }

        #endregion
        
        #region  GetShardIterator


        /// <summary>
        /// Returns a shard iterator. A shard iterator provides information about how to retrieve
        /// the stream records from within a shard. Use the shard iterator in a subsequent <code>GetRecords</code>
        /// request to read the stream records from the shard.
        /// 
        ///  <note> 
        /// <para>
        /// A shard iterator expires 15 minutes after it is returned to the requester.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetShardIterator service method.</param>
        /// 
        /// <returns>The response from the GetShardIterator service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetShardIterator">REST API Reference for GetShardIterator Operation</seealso>
        public virtual GetShardIteratorResponse GetShardIterator(GetShardIteratorRequest request)
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = GetShardIteratorRequestMarshaller.Instance;
            options.ResponseUnmarshaller = GetShardIteratorResponseUnmarshaller.Instance;

            return Invoke<GetShardIteratorResponse>(request, options);
        }


        /// <summary>
        /// Returns a shard iterator. A shard iterator provides information about how to retrieve
        /// the stream records from within a shard. Use the shard iterator in a subsequent <code>GetRecords</code>
        /// request to read the stream records from the shard.
        /// 
        ///  <note> 
        /// <para>
        /// A shard iterator expires 15 minutes after it is returned to the requester.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the GetShardIterator service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the GetShardIterator service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.TrimmedDataAccessException">
        /// The operation attempted to read past the oldest stream record in a shard.
        /// 
        ///  
        /// <para>
        /// In DynamoDB Streams, there is a 24 hour limit on data retention. Stream records whose
        /// age exceeds this limit are subject to removal (trimming) from the stream. You might
        /// receive a TrimmedDataAccessException if:
        /// </para>
        ///  <ul> <li>
        /// <para>
        /// You request a shard iterator with a sequence number older than the trim point (24
        /// hours).
        /// </para>
        ///  </li> <li>
        /// <para>
        /// You obtain a shard iterator, but before you use the iterator in a <code>GetRecords</code>
        /// request, a stream record in the shard exceeds the 24 hour period and is trimmed. This
        /// causes the iterator to access a record that no longer exists.
        /// </para>
        ///  </li> </ul>
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/GetShardIterator">REST API Reference for GetShardIterator Operation</seealso>
        public virtual Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = GetShardIteratorRequestMarshaller.Instance;
            options.ResponseUnmarshaller = GetShardIteratorResponseUnmarshaller.Instance;
            
            return InvokeAsync<GetShardIteratorResponse>(request, options, cancellationToken);
        }

        #endregion
        
        #region  ListStreams


        /// <summary>
        /// Returns an array of stream ARNs associated with the current account and endpoint.
        /// If the <code>TableName</code> parameter is present, then <code>ListStreams</code>
        /// will return only the streams ARNs for that table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>ListStreams</code> at a maximum rate of 5 times per second.
        /// </para>
        ///  </note>
        /// </summary>
        /// 
        /// <returns>The response from the ListStreams service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/ListStreams">REST API Reference for ListStreams Operation</seealso>
        public virtual ListStreamsResponse ListStreams()
        {
            return ListStreams(new ListStreamsRequest());
        }


        /// <summary>
        /// Returns an array of stream ARNs associated with the current account and endpoint.
        /// If the <code>TableName</code> parameter is present, then <code>ListStreams</code>
        /// will return only the streams ARNs for that table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>ListStreams</code> at a maximum rate of 5 times per second.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListStreams service method.</param>
        /// 
        /// <returns>The response from the ListStreams service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/ListStreams">REST API Reference for ListStreams Operation</seealso>
        public virtual ListStreamsResponse ListStreams(ListStreamsRequest request)
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = ListStreamsRequestMarshaller.Instance;
            options.ResponseUnmarshaller = ListStreamsResponseUnmarshaller.Instance;

            return Invoke<ListStreamsResponse>(request, options);
        }


        /// <summary>
        /// Returns an array of stream ARNs associated with the current account and endpoint.
        /// If the <code>TableName</code> parameter is present, then <code>ListStreams</code>
        /// will return only the streams ARNs for that table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>ListStreams</code> at a maximum rate of 5 times per second.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListStreams service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/ListStreams">REST API Reference for ListStreams Operation</seealso>
        public virtual Task<ListStreamsResponse> ListStreamsAsync(System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            return ListStreamsAsync(new ListStreamsRequest(), cancellationToken);
        }

        /// <summary>
        /// Returns an array of stream ARNs associated with the current account and endpoint.
        /// If the <code>TableName</code> parameter is present, then <code>ListStreams</code>
        /// will return only the streams ARNs for that table.
        /// 
        ///  <note> 
        /// <para>
        /// You can call <code>ListStreams</code> at a maximum rate of 5 times per second.
        /// </para>
        ///  </note>
        /// </summary>
        /// <param name="request">Container for the necessary parameters to execute the ListStreams service method.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// 
        /// <returns>The response from the ListStreams service method, as returned by DynamoDBStreams.</returns>
        /// <exception cref="Amazon.DynamoDBv2.Model.InternalServerErrorException">
        /// An error occurred on the server side.
        /// </exception>
        /// <exception cref="Amazon.DynamoDBv2.Model.ResourceNotFoundException">
        /// The operation tried to access a nonexistent stream.
        /// </exception>
        /// <seealso href="http://docs.aws.amazon.com/goto/WebAPI/streams-dynamodb-2012-08-10/ListStreams">REST API Reference for ListStreams Operation</seealso>
        public virtual Task<ListStreamsResponse> ListStreamsAsync(ListStreamsRequest request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = new InvokeOptions();
            options.RequestMarshaller = ListStreamsRequestMarshaller.Instance;
            options.ResponseUnmarshaller = ListStreamsResponseUnmarshaller.Instance;
            
            return InvokeAsync<ListStreamsResponse>(request, options, cancellationToken);
        }

        #endregion
        
    }
}