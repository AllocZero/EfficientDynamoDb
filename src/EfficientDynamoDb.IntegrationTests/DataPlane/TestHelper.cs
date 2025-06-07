using EfficientDynamoDb.Configs;

namespace EfficientDynamoDb.IntegrationTests.DataPlane;

public static class TestHelper
{
    public const string TestTableName = "ddb_test";

    public static DynamoDbContext CreateContext()
    {
        var region = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_REGION") ?? "us-east-1";
        var endpoint = Environment.GetEnvironmentVariable("EFFDDB_TEST_DYNAMODB_ENDPOINT");

        var regionEndpoint = !string.IsNullOrEmpty(endpoint)
            ? RegionEndpoint.Create(region, endpoint)
            : RegionEndpoint.Create(region);

        var credentials = GetCredentials(endpoint);

        var config = new DynamoDbContextConfig(regionEndpoint, credentials);
        return new DynamoDbContext(config);
    }

    private static AwsCredentials GetCredentials(string? endpoint)
    {
        // If endpoint is set, we're using LocalDynamoDB - always use dummy credentials
        if (!string.IsNullOrEmpty(endpoint))
            return new AwsCredentials("dummy", "dummy");

        var accessKey = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_ACCESS_KEY_ID");
        var secretKey = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_SECRET_ACCESS_KEY");
        var sessionToken = Environment.GetEnvironmentVariable("EFFDDB_TEST_AWS_SESSION_TOKEN");

        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("EFFDDB_TEST_AWS_ACCESS_KEY_ID and EFFDDB_TEST_AWS_SECRET_ACCESS_KEY must be set when using production DynamoDB.");
        }

        return new AwsCredentials(accessKey, secretKey, sessionToken);
    }
}