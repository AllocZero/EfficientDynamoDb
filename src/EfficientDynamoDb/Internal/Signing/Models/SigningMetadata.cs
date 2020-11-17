using System;
using System.Net.Http.Headers;
using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.Internal.Signing
{
    public readonly struct SigningMetadata
    {
        public string RegionName { get; }

        public string ServiceName { get; }

        public AwsCredentials Credentials { get; }

        public DateTime Timestamp { get; }

        public HttpRequestHeaders DefaultRequestHeaders { get; }

        public Uri? BaseAddress { get; }

        public SigningMetadata(string regionName, string serviceName, in AwsCredentials credentials, in DateTime timestamp,
            HttpRequestHeaders defaultRequestHeaders, Uri? baseAddress)
        {
            RegionName = regionName;
            ServiceName = serviceName;
            Credentials = credentials;
            Timestamp = timestamp;
            DefaultRequestHeaders = defaultRequestHeaders;
            BaseAddress = baseAddress;
        }
    }
}