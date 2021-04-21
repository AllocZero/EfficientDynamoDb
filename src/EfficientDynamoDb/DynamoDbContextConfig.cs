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

        /// <summary>
        /// Creates a context configuration instance with specified region and credentials provider.
        /// </summary>
        /// <param name="regionEndpoint">AWS service region.</param>
        /// <param name="credentialsProvider">Provider that is used to retrieve access credentials.</param>
        /// <remarks>
        /// AWS SDK credentials can be wrapped and used as <paramref name="credentialsProvider"/> via compatibility package.
        /// Check the docs section for more info: https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/sdk-compatibility#credentials
        /// </remarks>
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, IAwsCredentialsProvider credentialsProvider)
        {
            RegionEndpoint = regionEndpoint;
            CredentialsProvider = credentialsProvider;

            _converters = Array.Empty<DdbConverter>();
            Metadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
        }
        
        /// <summary>
        /// Creates a context config instance with specified region, credentials provider and a converters collection.
        /// </summary>
        /// <param name="regionEndpoint">AWS service region.</param>
        /// <param name="credentialsProvider">Provider that is used to retrieve access credentials.</param>
        /// <param name="converters">Collection of converter overrides.</param>
        /// <remarks>
        /// AWS SDK credentials can be wrapped and used as <paramref name="credentialsProvider"/> via compatibility package.
        /// Check the docs section for more info: https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/sdk-compatibility#credentials
        /// </remarks>
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, IAwsCredentialsProvider credentialsProvider, IReadOnlyCollection<DdbConverter> converters)
        {
            RegionEndpoint = regionEndpoint;
            CredentialsProvider = credentialsProvider;

            _converters = converters;
            Metadata = new DynamoDbContextMetadata(converters);
        }
    }
}