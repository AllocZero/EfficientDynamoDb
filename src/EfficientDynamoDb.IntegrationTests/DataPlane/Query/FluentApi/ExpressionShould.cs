using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.IntegrationTests.DataPlane;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Query.FluentApi;

[DynamoDbTable(TestHelper.TestTableName)]
public record TestUserWithNullable
{
    [DynamoDbProperty("pk", DynamoDbAttributeType.PartitionKey)]
    public required string PartitionKey { get; init; }

    [DynamoDbProperty("sk", DynamoDbAttributeType.SortKey)]
    public required string SortKey { get; init; }

    [DynamoDbProperty("name")]
    public string Name { get; init; } = "";

    [DynamoDbProperty("nullableAge")]
    public int? NullableAge { get; init; }
}

[TestFixture]
public class ExpressionShould
{
    private const string KeyPrefix = "effddb_tests-expressions";
    private DynamoDbContext _context = null!;
    private List<TestUserWithNullable> _testUsers = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        
        _testUsers =
        [
            new() { PartitionKey = KeyPrefix, SortKey = "sk-1", Name = "User with age", NullableAge = 25 },
            new() { PartitionKey = KeyPrefix, SortKey = "sk-2", Name = "User without age", NullableAge = null }
        ];

        await _context.BatchWrite()
            .WithItems(_testUsers.Select(Batch.PutItem))
            .ExecuteAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _context.BatchWrite()
            .WithItems(_testUsers.Select(item => Batch.DeleteItem<TestUserWithNullable>().WithPrimaryKey(item.PartitionKey, item.SortKey)))
            .ExecuteAsync();
    }
    
    [Test(Description = "Should throw an exception when trying to filter on a nullable value type with a non-nullable value")]
    public async Task ThrowExceptionOnNullableValueTypeMismatch()
    {
        await Should.ThrowAsync<InvalidCastException>(async () => await _context.Query<TestUserWithNullable>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(KeyPrefix))
            .WithFilterExpression(x => x.On(y => y.NullableAge).EqualTo(25))
            .ToAsyncEnumerable()
            .ToListAsync()
        );
    }

    [Test(Description = "Casting int to nullable int should work in expressions")]
    public async Task SupportNullableValueTypePropertiesInExpressions()
    {
        var results = await _context.Query<TestUserWithNullable>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(KeyPrefix))
            .WithFilterExpression(x => x.On(y => y.NullableAge).EqualTo((int?)25))
            .ToAsyncEnumerable()
            .ToListAsync();

        results.ShouldBe([_testUsers[0]]);
    }
}