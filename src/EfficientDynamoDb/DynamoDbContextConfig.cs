using System;
using System.Collections.Generic;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Configs.Retries;
using EfficientDynamoDb.Converters;

namespace EfficientDynamoDb
{
    public class DynamoDbContextConfig
    {
        private IReadOnlyCollection<DdbConverter> _converters;
        
        internal DynamoDbContextMetadata Metadata { get; private set; }

        public string? TableNamePrefix { get; set; }

        public RetryStrategies RetryStrategies { get; } = new RetryStrategies();

        public RegionEndpoint RegionEndpoint { get; }
        
        public IAwsCredentialsProvider CredentialsProvider { get; }

        public IHttpClientFactory HttpClientFactory { get; set; } = DefaultHttpClientFactory.Instance;

        public IReadOnlyCollection<DdbConverter> Converters
        {
            get => _converters;
            set => Metadata = new DynamoDbContextMetadata(_converters = value);
        }

        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, IAwsCredentialsProvider credentialsProvider)
        {
            RegionEndpoint = regionEndpoint;
            CredentialsProvider = credentialsProvider;

            _converters = Array.Empty<DdbConverter>();
            Metadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
        }
        
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, IAwsCredentialsProvider credentialsProvider, IReadOnlyCollection<DdbConverter> converters)
        {
            RegionEndpoint = regionEndpoint;
            CredentialsProvider = credentialsProvider;

            _converters = converters;
            Metadata = new DynamoDbContextMetadata(converters);
        }
    }
}