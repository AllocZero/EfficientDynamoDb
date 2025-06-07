using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.BatchGet;

[TestFixture]
public class BatchGetShould
{
    private const string KeyPrefix = "effddb_tests-batch_get";
    private DynamoDbContext _context = null!;
    private TestUser[] _testUsers = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        
        _testUsers = [
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-1",
                Name = "Test User 1",
                Age = 25,
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-2",
                Name = "Test User 2",
                Age = 30,
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-3",
                SortKey = $"{KeyPrefix}-sk-3",
                Name = "Test User 3",
                Age = 28,
            }
        ];

        await _context.BatchWrite()
            .WithItems(_testUsers.Select(Batch.PutItem))
            .ExecuteAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _context.BatchWrite()
            .WithItems(_testUsers.Select(user => Batch.DeleteItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ExecuteAsync();
    }

    [Test]
    public async Task ReturnEmptyListWhenNoItemsExist()
    {
        var result = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey($"{KeyPrefix}-non_existent-pk-1", $"{KeyPrefix}-non_existent-sk-1"),
                Batch.GetItem<TestUser>().WithPrimaryKey($"{KeyPrefix}-non_existent-pk-2", $"{KeyPrefix}-non_existent-sk-2")
            )
            .ToListAsync<TestUser>();

        result.ShouldNotBeNull().ShouldBeEmpty();
    }

    [Test]
    public async Task ReturnItemsWhenItemsExist()
    {
        var result = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
            )
            .ToListAsync<TestUser>();

        
        result.ShouldBe(_testUsers[..2], ignoreOrder: true);
    }

    [Test]
    public async Task ReturnMixedResultsWhenSomeItemsExist()
    {
        const string nonExistentPk = $"{KeyPrefix}-non_existent-pk";
        const string nonExistentSk = $"{KeyPrefix}-non_existent-sk";

        var result = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(nonExistentPk, nonExistentSk),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[2].PartitionKey, _testUsers[2].SortKey)
            )
            .ToListAsync<TestUser>();

        result.ShouldBe([_testUsers[0], _testUsers[2]], ignoreOrder: true);
    }

    [Test]
    public async Task ReturnAllItemsWhenAllExist()
    {
        var result = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[2].PartitionKey, _testUsers[2].SortKey)
            )
            .ToListAsync<TestUser>();

        result.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ReturnItemsUsingFromTablesWithConsistentRead()
    {
        var result = await _context.BatchGet()
            .FromTables(
                Batch.FromTable<TestUser>()
                    .WithConsistentRead(true)
                    .WithItems(
                        Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                        Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
                    )
            )
            .ToListAsync<TestUser>();

        result.ShouldBe(_testUsers[..2], ignoreOrder: true);
    }

    [Test]
    public async Task ReturnProjectedItemsUsingFromTables()
    {
        var result = await _context.BatchGet()
            .FromTables(
                Batch.FromTable<TestUser>()
                    .WithProjectedAttributes<TestUserProjection>()
                    .WithItems(
                        Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                        Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
                    )
            )
            .ToListAsync<TestUserProjection>();

        
        var expectedProjections = _testUsers[..2].Select(user => new TestUserProjection
        {
            PartitionKey = user.PartitionKey,
            SortKey = user.SortKey,
            Name = user.Name
        });
        result.ShouldBe(expectedProjections, ignoreOrder: true);
    }

    [Test]
    public async Task ReturnItemsAsDocuments()
    {
        var result = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
            )
            .AsDocuments()
            .ToListAsync();

        var entities = result.Select(x => x.ToObject<TestUser>(_context.Config.Metadata));
        entities.ShouldBe(_testUsers[..2], ignoreOrder: true);
    }

    [Test]
    public async Task ReturnResponseWithMetadata()
    {
        var response = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
            )
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync<TestUser>();

        response.ShouldNotBeNull();
        response.Items.ShouldBe(_testUsers[..2], ignoreOrder: true);
        
        response.ConsumedCapacity.ShouldNotBeNull().Count.ShouldBe(1);
        response.ConsumedCapacity[0].CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task ReturnDocumentResponseWithMetadata()
    {
        var response = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[0].PartitionKey, _testUsers[0].SortKey),
                Batch.GetItem<TestUser>().WithPrimaryKey(_testUsers[1].PartitionKey, _testUsers[1].SortKey)
            )
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .AsDocuments()
            .ToResponseAsync();

        response.ShouldNotBeNull();
        var entities = response.Items.Select(x => x.ToObject<TestUser>(_context.Config.Metadata));
        entities.ShouldBe(_testUsers[..2], ignoreOrder: true);
        
        response.ConsumedCapacity.ShouldNotBeNull().Count.ShouldBe(1);
        response.ConsumedCapacity[0].CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task SupportLargeBatchGetRequest()
    {
        var set = Enumerable.Range(0, 23).Select(_ => (Pk: Guid.NewGuid().ToString(), Sk: Guid.NewGuid().ToString()))
            .Concat(_testUsers.Select(x => (Pk: x.PartitionKey, Sk: x.SortKey)));
        
        var result = await _context.BatchGet()
            .WithItems(set.Select(x => Batch.GetItem<TestUser>().WithPrimaryKey(x.Pk, x.Sk)))
            .ToListAsync<TestUser>();
        
        result.ShouldBe(_testUsers, ignoreOrder: true);
    }
} 