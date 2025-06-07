using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Query;

[TestFixture]
public class QueryShould
{
    private const string KeyPrefix = "effddb_tests-query";
    private DynamoDbContext _context = null!;
    private List<TestUser> _testUsers = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        
        // Create test data for querying - items with same partition key for testing sort key conditions
        _testUsers = [
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-001",
                Name = "Alice Johnson",
                Age = 25,
                Email = "alice@example.com",
                Counter = 10,
                Score = 95.5m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-002",
                Name = "Bob Smith",
                Age = 30,
                Email = "bob@example.com",
                Counter = 15,
                Score = 88.0m,
                Active = false
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-003",
                Name = "Charlie Brown",
                Age = 35,
                Email = "charlie@example.com",
                Counter = 20,
                Score = 92.3m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-001",
                Name = "Diana Prince",
                Age = 28,
                Email = "diana@example.com",
                Counter = 12,
                Score = 97.8m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-002",
                Name = "Eve Wilson",
                Age = 32,
                Email = "eve@example.com",
                Counter = 18,
                Score = 89.5m,
                Active = false
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
    public async Task QueryItemsByPartitionKeySuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithSortKeyConditionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyPrefix = $"{KeyPrefix}-sk-00";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => 
                x.On(y => y.PartitionKey).EqualTo(partitionKey) &
                x.On(y => y.SortKey).BeginsWith(sortKeyPrefix))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey && x.SortKey.StartsWith(sortKeyPrefix));
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithFilterExpressionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithFilterExpression(x => x.On(y => y.Active).EqualTo(true))
            .ToAsyncEnumerable()
            .ToListAsync();
        
        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey && x.Active);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithNumericFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithFilterExpression(x => x.On(y => y.Age).GreaterThan(25))
            .ToAsyncEnumerable()
            .ToListAsync();
        
        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey && x.Age > 25);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithSortKeyRangeSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyStart = $"{KeyPrefix}-sk-001";
        var sortKeyEnd = $"{KeyPrefix}-sk-002";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => 
                x.On(y => y.PartitionKey).EqualTo(partitionKey) &
                x.On(y => y.SortKey).Between(sortKeyStart, sortKeyEnd))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => 
            x.PartitionKey == partitionKey && 
            string.CompareOrdinal(x.SortKey, sortKeyStart) >= 0 && 
            string.CompareOrdinal(x.SortKey, sortKeyEnd) <= 0);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithLimitSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var result = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithLimit(2)
            .ToPageAsync();

        result.Items.Count.ShouldBeLessThanOrEqualTo(2);
        result.PaginationToken.ShouldNotBeNull();
    }

    [Test]
    public async Task QueryItemsWithPaginationSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        // First page
        var firstPage = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithLimit(1)
            .ToPageAsync();

        firstPage.Items.Count.ShouldBeLessThanOrEqualTo(1);
        firstPage.PaginationToken.ShouldNotBeNull();

        // Second page
        var secondPage = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithLimit(1)
            .WithPaginationToken(firstPage.PaginationToken)
            .ToPageAsync();

        // Ensure we get different items
        var firstPageKeys = firstPage.Items.Select(x => x.PartitionKey + x.SortKey).ToHashSet();
        var secondPageKeys = secondPage.Items.Select(x => x.PartitionKey + x.SortKey).ToHashSet();
        
        firstPageKeys.Intersect(secondPageKeys).ShouldBeEmpty();
    }

    [Test]
    public async Task QueryItemsWithProjectionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .AsProjections<TestUserProjection>()
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedProjections = _testUsers
            .Where(x => x.PartitionKey == partitionKey)
            .Select(x => new TestUserProjection { PartitionKey = x.PartitionKey, SortKey = x.SortKey, Name = x.Name });
        queriedItems.ShouldBe(expectedProjections, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithInPlaceProjectionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithProjectedAttributes(x => x.PartitionKey, x => x.SortKey, x => x.Name)
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedProjections = _testUsers
            .Where(x => x.PartitionKey == partitionKey)
            .Select(x => new TestUser { PartitionKey = x.PartitionKey, SortKey = x.SortKey, Name = x.Name });
        queriedItems.ShouldBe(expectedProjections, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithConsistentReadSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithConsistentRead(true)
            .ToAsyncEnumerable()
            .ToListAsync();
        
        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsAsDocumentsSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedDocuments = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .AsDocuments()
            .ToAsyncEnumerable()
            .ToListAsync();

        var queriedItems = queriedDocuments.Select(document => _context.ToObject<TestUser>(document));
        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithResponseMetadataSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var response = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .ReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .WithLimit(2)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.Items.Count.ShouldBeLessThanOrEqualTo(2);
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
        response.Count.ShouldBeGreaterThan(0);
        response.ScannedCount.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task QueryItemsWithComplexFilterExpressionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithFilterExpression(x => 
                (x.On(y => y.Age).GreaterThan(25) & x.On(y => y.Age).LessThan(35)) &
                x.On(y => y.Score).GreaterThanOrEqualTo(90.0m))
            .ToAsyncEnumerable()
            .ToListAsync();

        queriedItems.ShouldAllBe(x => x.Age > 25 && x.Age < 35 && x.Score >= 90);
    }

    [Test]
    public async Task QueryItemsPagedAsyncEnumerableSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var pageCount = 0;
        var queriedItems = new List<TestUser>();

        await foreach (var page in _context.Query<TestUser>()
                           .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
                           .WithLimit(1)
                           .ToPagedAsyncEnumerable())
        {
            pageCount++;
            queriedItems.AddRange(page);
            
            // Each page should have at most 1 item due to limit
            page.Count.ShouldBeLessThanOrEqualTo(1);
        }

        pageCount.ShouldBeGreaterThan(1); // Should have multiple pages due to limit
        
        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithDescendingOrderSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .BackwardSearch(true)
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers
            .Where(x => x.PartitionKey == partitionKey)
            .OrderByDescending(x => x.SortKey);
        
        queriedItems.ShouldBe(expectedItems);
    }

    [Test]
    public async Task QueryEmptyResultSuccessfully()
    {
        var nonExistentPartitionKey = $"{KeyPrefix}-non-existent-pk";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(nonExistentPartitionKey))
            .ToAsyncEnumerable()
            .ToListAsync();

        queriedItems.ShouldBeEmpty();
    }

    [Test]
    public async Task QueryItemsWithStringContainsFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo(partitionKey))
            .WithFilterExpression(x => x.On(y => y.Name).Contains("Alice"))
            .ToAsyncEnumerable()
            .ToListAsync();

        queriedItems.ShouldAllBe(x => x.Name.Contains("Alice"));
    }

    [Test]
    public async Task QueryItemsWithSortKeyEqualToSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKey = $"{KeyPrefix}-sk-001";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => 
                x.On(y => y.PartitionKey).EqualTo(partitionKey) &
                x.On(y => y.SortKey).EqualTo(sortKey))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => x.PartitionKey == partitionKey && x.SortKey == sortKey);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithSortKeyGreaterThanSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyThreshold = $"{KeyPrefix}-sk-001";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => 
                x.On(y => y.PartitionKey).EqualTo(partitionKey) &
                x.On(y => y.SortKey).GreaterThan(sortKeyThreshold))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => 
            x.PartitionKey == partitionKey && 
            string.CompareOrdinal(x.SortKey, sortKeyThreshold) > 0);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public async Task QueryItemsWithSortKeyLessThanOrEqualToSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyThreshold = $"{KeyPrefix}-sk-002";
        
        var queriedItems = await _context.Query<TestUser>()
            .WithKeyExpression(x => 
                x.On(y => y.PartitionKey).EqualTo(partitionKey) &
                x.On(y => y.SortKey).LessThanOrEqualTo(sortKeyThreshold))
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedItems = _testUsers.Where(x => 
            x.PartitionKey == partitionKey && 
            string.CompareOrdinal(x.SortKey, sortKeyThreshold) <= 0);
        queriedItems.ShouldBe(expectedItems, ignoreOrder: true);
    }

    [Test]
    public void ThrowWhenInvalidQueryParameters()
    {
        Should.Throw<ResourceNotFoundException>(async () =>
        {
            await _context.Query<TestUser>()
                .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo($"{KeyPrefix}-pk-1"))
                .WithTableName("non_existent_table")
                .ToAsyncEnumerable()
                .ToListAsync();
        });
    }
    
    [Test]
    public async Task QueryItemsWhenSuppressedThrowing()
    {
        var result = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo($"{KeyPrefix}-pk-1"))
            .SuppressThrowing()
            .ToListAsync();
    
        result.Exception.ShouldBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_testUsers.Where(x => x.PartitionKey == $"{KeyPrefix}-pk-1"), ignoreOrder: true);
    }
    
    [Test]
    public async Task ReturnErrorWhenInvalidRequestAndSuppressedThrowing()
    {
        var result = await _context.Query<TestUser>()
            .WithKeyExpression(x => x.On(y => y.PartitionKey).EqualTo($"{KeyPrefix}-pk-1"))
            .WithTableName("non_existent_table")
            .SuppressThrowing()
            .ToPageAsync();
    
        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldNotBeNull();
    }
} 