using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Context.Config;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContextConfig
    {
        internal static string ServiceName => "dynamodb";
        
        public string? TableNamePrefix { get; set; }
        
        public RegionEndpoint RegionEndpoint { get; }
        
        public AwsCredentials Credentials { get; }
        
        public DynamoDbContextConfig(RegionEndpoint regionEndpoint, AwsCredentials credentials)
        {
            RegionEndpoint = regionEndpoint;
            Credentials = credentials;
        }
    }
}