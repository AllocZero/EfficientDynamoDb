using System;
using System.Net.Http;
using Amazon.Runtime;

namespace Benchmarks.Mocks.Http
{
    public class MockHttpClientFactory : HttpClientFactory
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

        public MockHttpClientFactory(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }
        
        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            return new HttpClient(new MockHttpClientHandler(_responseFactory));
        }
    } 
}