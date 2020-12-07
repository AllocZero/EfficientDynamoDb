using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Configs.Retries;
using EfficientDynamoDb.Context.Config;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContextConfig
    {
        public string? TableNamePrefix { get; set; }

        public RetryStrategies RetryStrategies { get; } = new RetryStrategies();

        public RegionEndpoint RegionEndpoint { get; }
        
        public AwsCredentials Credentials { get; }

        public IHttpClientFactory HttpClientFactory { get; set; } = DefaultHttpClientFactory.Instance;
        
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, AwsCredentials credentials)
        {
            RegionEndpoint = regionEndpoint;
            Credentials = credentials;
        }
    }
}