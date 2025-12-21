using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Converters.LowLevel;

[TestFixture]
public class LowLevelCollectionConverterShould
{
    private const string KeyPrefix = "effddb_tests-low_lvl_collection_converters";
    private DynamoDbContext _context = null!;
    private string? _testPartitionKey;
    private string? _testSortKey;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateContext();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_testPartitionKey != null && _testSortKey != null)
        {
            await _context.DeleteItemAsync<TestCollectionConverterEntity>(_testPartitionKey, _testSortKey);
        }
    }

    [Test]
    public async Task ApplyConvertersByDefaultForAllTimeTypes()
    {
        _testPartitionKey = $"{KeyPrefix}-pk";
        _testSortKey = $"{KeyPrefix}-sk";

        var item = new TestCollectionConverterEntity
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            CompositeKey = new()
            {
                Part1 = "part1_value",
                Part2 = "part2_value",
                Part3 = "part3_value"
            },
            CompositeKey2 = new()
            {
                Part1 = "part1_value_2",
                Part2 = "part2_value_2",
                Part3 = "part3_value_2"
            }
        };

        await _context.PutItemAsync(item);

        var retrieved = await _context.GetItem<TestCollectionConverterEntity>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();

        retrieved.ShouldBeEquivalentTo(item);
    }
}