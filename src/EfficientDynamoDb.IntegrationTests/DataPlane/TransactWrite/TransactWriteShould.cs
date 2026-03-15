using EfficientDynamoDb.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.TransactWrite;

[TestFixture]
public class TransactWriteShould
{
    private const string KeyPrefix = "effddb_tests-transact_write";
    private static readonly Random Random = new(42);

    private DynamoDbContext _context = null!;
    private List<TestUser> _itemsToCleanup = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateContext();
        _itemsToCleanup = [];
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_itemsToCleanup.Count != 0)
        {
            await _context.BatchWrite()
                .WithItems(_itemsToCleanup.Select(user => Batch.DeleteItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
                .ExecuteAsync();
        }
    }

    [Test]
    public async Task PutItemsSuccessfully()
    {
        var item1 = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-put-pk-1",
            SortKey = $"{KeyPrefix}-put-sk-1",
            Name = "Transact User 1",
            Age = 25,
            Email = "transact1@example.com"
        };
        var item2 = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-put-pk-2",
            SortKey = $"{KeyPrefix}-put-sk-2",
            Name = "Transact User 2",
            Age = 30,
            Email = "transact2@example.com"
        };

        _itemsToCleanup.Add(item1);
        _itemsToCleanup.Add(item2);

        await _context.TransactWrite()
            .WithItems(
                Transact.PutItem(item1),
                Transact.PutItem(item2)
            )
            .ExecuteAsync();

        var retrieved1 = await _context.GetItem<TestUser>()
            .WithPrimaryKey(item1.PartitionKey, item1.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        var retrieved2 = await _context.GetItem<TestUser>()
            .WithPrimaryKey(item2.PartitionKey, item2.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();

        retrieved1.ShouldBe(item1);
        retrieved2.ShouldBe(item2);
    }

    [Test]
    public async Task ThrowTransactionCanceledException_WhenConditionCheckFails()
    {
        var existingItem = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-cond-pk",
            SortKey = $"{KeyPrefix}-cond-sk",
            Name = "Existing User",
            Age = 30,
            Email = "existing@example.com"
        };

        _itemsToCleanup.Add(existingItem);
        await _context.PutItemAsync(existingItem);

        var ex = await Should.ThrowAsync<TransactionCanceledException>(() =>
            _context.TransactWrite()
                .WithItems(
                    Transact.PutItem(existingItem).WithCondition(c => c.On(x => x.PartitionKey).NotExists())
                )
                .ExecuteAsync());

        ex.CancellationReasons.ShouldNotBeEmpty();
        ex.CancellationReasons[0].Code.ShouldBe("ConditionalCheckFailed");
    }

    [Test]
    public async Task ThrowTransactionCanceledException_WithMultipleItems_WhenConditionCheckFails()
    {
        var items = Enumerable.Range(1, 25)
            .Select(i => new TestUser
            {
                PartitionKey = $"{KeyPrefix}-multi-pk-{i:D3}",
                SortKey = $"{KeyPrefix}-multi-sk-{i:D3}",
                Name = $"Multi User {i}",
                Age = 20 + i,
                Email = $"multi{i}@example.com",
                LargeData = GenerateLargeString(1000)
            })
            .ToList();

        _itemsToCleanup.AddRange(items);

        // Pre-create first item so the condition check fails
        await _context.PutItemAsync(items[0]);

        var ex = await Should.ThrowAsync<TransactionCanceledException>(() =>
            _context.TransactWrite()
                .WithItems(items.Select(x => Transact.PutItem(x).WithCondition(c => c.On(p => p.PartitionKey).NotExists())))
                .ExecuteAsync());

        ex.CancellationReasons.ShouldNotBeEmpty();
        ex.CancellationReasons.Any(r => r.Code == "ConditionalCheckFailed").ShouldBeTrue();
    }

    private static string GenerateLargeString(int approximateLength)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var result = new char[approximateLength];
        for (var i = 0; i < approximateLength; i++)
            result[i] = chars[Random.Next(chars.Length)];
        return new string(result);
    }
}
