using EfficientDynamoDb.Attributes;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Converters;

[DynamoDbTable(TestHelper.TestTableName)]
public record TestTimeEntity
{
	[DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
	public required string PartitionKey { get; init; }

	[DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
	public required string SortKey { get; init; }

	[DynamoDbProperty("dateTimeUtc")]
	public DateTime DateTimeUtc { get; init; }

	[DynamoDbProperty("dateTimeLocal")]
	public DateTime DateTimeLocal { get; init; }

	[DynamoDbProperty("dateTimeOffsetUtc")]
	public DateTimeOffset DateTimeOffsetUtc { get; init; }

	[DynamoDbProperty("dateTimeOffsetLocal")]
	public DateTimeOffset DateTimeOffsetLocal { get; init; }

	[DynamoDbProperty("dateOnly")]
	public DateOnly DateOnly { get; init; }

	[DynamoDbProperty("timeOnly")]
	public TimeOnly TimeOnly { get; init; }
}


