using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Benchmarks.Http;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using Document = EfficientDynamoDb.DocumentModel.Document;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    // public class EfficientQueryConverterBenchmark : QueryBenchmarkBase
    // {
    //     private readonly DynamoDbContext _context;
    //     
    //     public EfficientQueryConverterBenchmark()
    //     {
    //         _context = new DynamoDbContext(new DynamoDbContextConfig(RegionEndpoint.USEast1, new AwsCredentials("test", "test"))
    //         {
    //             HttpClientFactory = new DefaultHttpClientFactory(new HttpClient(new MockHttpClientHandler(CreateResponse)))
    //         });
    //     }
    //
    //     protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
    //     {
    //         var result = await _context.QueryAsync<Document>(new QueryRequest
    //         {
    //             KeyConditionExpression = "pk = :pk",
    //             ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    //             {
    //                 {":pk", "test"}
    //             }
    //         }).ConfigureAwait(false);
    //
    //         return result;
    //     }
    // }
}