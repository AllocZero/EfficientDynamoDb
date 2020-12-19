using System.Collections.Generic;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Configs.Retries;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Converters.Collections;
using EfficientDynamoDb.Internal.Converters.Collections.Dictionaries;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContextConfig
    {
        internal static readonly DdbConverterFactory[] DefaultConverterFactories = {new ArrayDdbConverterFactory(), new ListDdbConverterFactory(), new DictionaryDdbConverterFactory()};
        
        internal readonly DynamoDbContextMetadata Metadata;
        
        public string? TableNamePrefix { get; set; }

        public RetryStrategies RetryStrategies { get; } = new RetryStrategies();

        public RegionEndpoint RegionEndpoint { get; }
        
        public AwsCredentials Credentials { get; }

        public IHttpClientFactory HttpClientFactory { get; set; } = DefaultHttpClientFactory.Instance;
        
        public IList<DdbConverter> Converters { get; }
        
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, AwsCredentials credentials)
        {
            RegionEndpoint = regionEndpoint;
            Credentials = credentials;
            
            var converters = new List<DdbConverter>(DefaultConverterFactories);
            Converters = converters;
            Metadata = new DynamoDbContextMetadata(converters);
        }
    }
}