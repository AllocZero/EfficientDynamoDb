using System;
using System.Net.Http.Headers;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Context.Config;

namespace EfficientDynamoDb.Internal.Signing
{
    internal readonly struct SigningMetadata
    {
        public RegionEndpoint RegionEndpoint { get; }

        public AwsCredentials Credentials { get; }

        public DateTime Timestamp { get; }

        public HttpRequestHeaders DefaultRequestHeaders { get; }

        public Uri? BaseAddress { get; }

        public SigningMetadata(RegionEndpoint regionEndpoint, AwsCredentials credentials, in DateTime timestamp,
            HttpRequestHeaders defaultRequestHeaders, Uri? baseAddress)
        {
            RegionEndpoint = regionEndpoint;
            Credentials = credentials;
            Timestamp = timestamp;
            DefaultRequestHeaders = defaultRequestHeaders;
            BaseAddress = baseAddress;
        }
    }
}