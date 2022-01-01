using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.ExecuteStatement;
using EfficientDynamoDb.Internal.Signing;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDynamoDb
{
    public class DynamoDbPartiqlClient
    {
        private readonly SigningMetadata signingMetadata;
        private readonly HttpClient client;
        
        public static DynamoDbPartiqlClient CreateLocal(RegionEndpoint region, string url)
        {
            var endpoint = RegionEndpoint.Create(region.Region, url);
            var client = new HttpClient();
            var signingMetadata = new SigningMetadata(
                endpoint,
                new AwsCredentials("DUMMY", "DUMMY"),
                DateTime.UtcNow,
                client.DefaultRequestHeaders,
                null
            );

            var result = new DynamoDbPartiqlClient(client, signingMetadata);

            return result;
        }


        private DynamoDbPartiqlClient(HttpClient client, SigningMetadata signingMetadata)
        {
            this.client = client;
            this.signingMetadata = signingMetadata;
        }

        public async Task ExecuteStatementAsync(string statement)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8000")
            {
                Content = new ExecuteStatementRequestHttpContent(statement)
            };
            
            var stream = (RecyclableMemoryStream)await request.Content.ReadAsStreamAsync();
            AwsRequestSigner.Sign(request, stream, signingMetadata);

            var response = await client.SendAsync(request);

            var result = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            var body = await response.Content.ReadAsStringAsync();

            var ree = await response.RequestMessage.Content.ReadAsStringAsync();
        }
    }


}
