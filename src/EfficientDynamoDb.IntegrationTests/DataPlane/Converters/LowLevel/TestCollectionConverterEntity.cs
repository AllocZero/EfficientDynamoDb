using EfficientDynamoDb.Attributes;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Converters.LowLevel;

[DynamoDbTable(TestHelper.TestTableName)]
public class TestCollectionConverterEntity
{
    [DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
    public required string PartitionKey { get; init; }

    [DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
    public required string SortKey { get; init; }
    
    [DynamoDbProperty("compositeKey", typeof(TestCompositeKeyDdbConverter))]
    public required TestCompositeKey CompositeKey { get; init; }
    
    [DynamoDbProperty("compositeKey2", typeof(TestCompositeKeyDdbConverter))]
    public required TestCompositeKey CompositeKey2 { get; init; }
}