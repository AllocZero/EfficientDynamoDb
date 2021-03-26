using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Signing
{
    [StructLayout(LayoutKind.Auto)]
    internal readonly struct SigningMetadata
    {
        public RegionEndpoint RegionEndpoint { get; }

        public AwsCredentials Credentials { get; }

        public string TimestampIso8601BasicDateTimeString { get; }

        public ReadOnlySpan<char> TimestampIso8601BasicDateString => TimestampIso8601BasicDateTimeString.AsSpan().Slice(0, 8);

        public HttpRequestHeaders DefaultRequestHeaders { get; }
        
        public bool HasDefaultRequestHeaders { get; }

        public Uri? BaseAddress { get; }

        public SigningMetadata(RegionEndpoint regionEndpoint, AwsCredentials credentials, in DateTime timestamp,
            HttpRequestHeaders defaultRequestHeaders, Uri? baseAddress)
        {
            RegionEndpoint = regionEndpoint;
            Credentials = credentials;
            TimestampIso8601BasicDateTimeString = timestamp.ToIso8601BasicDateTime();
            DefaultRequestHeaders = defaultRequestHeaders;
            HasDefaultRequestHeaders = defaultRequestHeaders.Any();
            BaseAddress = baseAddress;
        }
    }
}