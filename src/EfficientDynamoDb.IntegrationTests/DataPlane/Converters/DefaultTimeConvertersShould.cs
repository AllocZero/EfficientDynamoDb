using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Converters;

[TestFixture]
public class DefaultTimeConvertersShould
{
	private const string KeyPrefix = "effddb_tests-time_converters";
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
			await _context.DeleteItemAsync<TestTimeEntity>(_testPartitionKey, _testSortKey);
		}
	}

	[Test]
	public async Task ApplyConvertersByDefaultForAllTimeTypes()
	{
		_testPartitionKey = $"{KeyPrefix}-pk";
		_testSortKey = $"{KeyPrefix}-sk";

		var item = new TestTimeEntity
		{
			PartitionKey = _testPartitionKey,
			SortKey = _testSortKey,
			DateTimeUtc = new DateTime(2025, 01, 01, 13, 0, 0, DateTimeKind.Utc),
			DateTimeLocal = new DateTime(2025, 01, 01, 13, 0, 0, DateTimeKind.Local),
			DateTimeOffsetUtc = new DateTimeOffset(2025, 01, 01, 13, 0, 0, TimeSpan.Zero),
			DateTimeOffsetLocal = new DateTimeOffset(2025, 01, 01, 13, 0, 0, TimeSpan.FromHours(3)),
			DateOnly = new DateOnly(2025, 01, 01),
			TimeOnly = new TimeOnly(13, 0, 0)
		};

		await _context.PutItemAsync(item);

		var retrieved = await _context.GetItem<TestTimeEntity>()
			.WithPrimaryKey(_testPartitionKey, _testSortKey)
			.WithConsistentRead(true)
			.ToItemAsync();

		retrieved.ShouldBe(item);
	}
}


