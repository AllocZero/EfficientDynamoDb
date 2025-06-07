using EfficientDynamoDb.Attributes;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.GetItem;

[DynamoDbTable(TestHelper.TestTableName)]
public record TestUser
{
    [DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
    public required string PartitionKey { get; init; }

    [DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
    public required string SortKey { get; init; }

    [DynamoDbProperty("name")]
    public string Name { get; init; } = "";

    [DynamoDbProperty("age")]
    public int Age { get; init; }

    [DynamoDbProperty("email")]
    public string Email { get; init; } = "";
}
    
[DynamoDbTable(TestHelper.TestTableName)]
public record TestUserProjection
{
    [DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
    public required string PartitionKey { get; init; }

    [DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
    public required string SortKey { get; init; }

    [DynamoDbProperty("name")]
    public string Name { get; init; } = "";
}