using System;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.Tests.TestConfiguration;

public static class TestHelper
{
    public const string TestTableName = "ddb_test";
    
    public static DynamoDbContext CreateContext()
    {
        var accessKey = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_ACCESS_KEY_ID");
        var secretKey = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_SECRET_ACCESS_KEY");
        var sessionToken = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_SESSION_TOKEN");
        var region = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_REGION") ?? "us-east-1";
        var endpoint = Environment.GetEnvironmentVariable("EFFDDB_TEST_DYNAMODB_ENDPOINT");

        // If no credentials are provided, use fake credentials for LocalDynamoDB
        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        {
            accessKey = "fake-access-key";
            secretKey = "fake-secret-key";
        }

        var credentials = string.IsNullOrEmpty(sessionToken) 
            ? new AwsCredentials(accessKey, secretKey) 
            : new AwsCredentials(accessKey, secretKey, sessionToken);

        var regionEndpoint = !string.IsNullOrEmpty(endpoint) 
            ? RegionEndpoint.Create(region, endpoint)
            : RegionEndpoint.Create(region);
            
        var config = new DynamoDbContextConfig(regionEndpoint, credentials);
        return new DynamoDbContext(config);
    }
}