using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Signing;
using Microsoft.IO;

namespace Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class SignerBenchmark
    {
        private HttpRequestMessage _httpRequest;
        private RecyclableMemoryStream _contentStream;
        private readonly HttpClient _httpClient = new HttpClient();
        
        [GlobalSetup]
        public async Task SetupAsync()
        {
            var request = new GetItemRequest
            {
                Key = new PrimaryKey("pk", "test_pk", "sk", "test_sk"),
                ConsistentRead = true,
                ProjectionExpression = new[] {"#f1"},
                TableName = "test_table",
                ExpressionAttributeNames = new Dictionary<string, string> {{"#f1", "absolute"}},
                ReturnConsumedCapacity = ReturnConsumedCapacity.Total
            };

            var httpContent = new GetItemHttpContent(request, request.TableName, request.Key.PartitionKeyName!, request.Key.SortKeyName);
            _httpRequest = new HttpRequestMessage(HttpMethod.Post, RegionEndpoint.USEast1.RequestUri)
            {
                Content = httpContent
            };
            _contentStream = (RecyclableMemoryStream) await _httpRequest.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
        
        
        [Benchmark]
        [BenchmarkCategory("Signing")]
        public HttpRequestMessage SigningNative()
        {
            _contentStream.Position = 0;
            CleanupHeaders(_httpRequest);
            
            var meta = new SigningMetadata(RegionEndpoint.USEast1, new AwsCredentials("accessKey", "secretKey"), DateTime.UtcNow, 
                _httpClient.DefaultRequestHeaders, null);
            AwsRequestSigner.Sign(_httpRequest, _contentStream, meta);

            return _httpRequest;
        }

        private void CleanupHeaders(HttpRequestMessage request)
        {
            request.Headers.Remove(HeaderKeys.XAmzDateHeader);
            request.Headers.Remove(HeaderKeys.XAmzSecurityTokenHeader);
            request.Headers.Remove(HeaderKeys.HostHeader);
            request.Headers.Remove(HeaderKeys.AuthorizationHeader);
        }
    }
}