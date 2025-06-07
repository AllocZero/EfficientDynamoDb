using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Scan;

[TestFixture]
public class ScanShould
{
    private const string KeyPrefix = "effddb_tests-scan";
    private DynamoDbContext _context = null!;
    private List<TestUser> _testUsers = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        
        // Create test data for scanning
        _testUsers = [
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-1",
                Name = "Alice Johnson",
                Age = 25,
                Email = "alice@example.com",
                Counter = 10,
                Score = 95.5m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-2",
                Name = "Bob Smith",
                Age = 30,
                Email = "bob@example.com",
                Counter = 15,
                Score = 88.0m,
                Active = false
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-3",
                SortKey = $"{KeyPrefix}-sk-3",
                Name = "Charlie Brown",
                Age = 35,
                Email = "charlie@example.com",
                Counter = 20,
                Score = 92.3m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-4",
                SortKey = $"{KeyPrefix}-sk-4",
                Name = "Diana Prince",
                Age = 28,
                Email = "diana@example.com",
                Counter = 12,
                Score = 97.8m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-5",
                SortKey = $"{KeyPrefix}-sk-5",
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
    public async Task ScanAllItemsSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .ToAsyncEnumerable()
            .ToListAsync();

        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithFilterExpressionSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => 
                x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-") &
                x.On(y => y.Active).EqualTo(true))
            .ToAsyncEnumerable()
            .ToListAsync();
        
        scannedItems.ShouldBe(_testUsers.Where(x => x.Active), ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithNumericFilterSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => 
                x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-") &
                x.On(y => y.Age).GreaterThan(30))
            .ToAsyncEnumerable()
            .ToListAsync();
        
        scannedItems.ShouldBe(_testUsers.Where(x => x.Age > 30), ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithLimitSuccessfully()
    {
        var result = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .WithLimit(2)
            .ToPageAsync();

        result.Items.Count.ShouldBeLessThanOrEqualTo(2);
        result.PaginationToken.ShouldNotBeNull();
    }

    [Test]
    public async Task ScanWithPaginationSuccessfully()
    {
        // First page
        var firstPage = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .WithLimit(2)
            .ToPageAsync();

        firstPage.Items.Count.ShouldBeLessThanOrEqualTo(2);
        firstPage.PaginationToken.ShouldNotBeNull();

        // Second page
        var secondPage = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .WithLimit(2)
            .WithPaginationToken(firstPage.PaginationToken)
            .ToPageAsync();

        // Ensure we get different items
        var firstPageKeys = firstPage.Items.Select(x => x.PartitionKey + x.SortKey).ToHashSet();
        var secondPageKeys = secondPage.Items.Select(x => x.PartitionKey + x.SortKey).ToHashSet();
        
        firstPageKeys.Intersect(secondPageKeys).ShouldBeEmpty();
    }

    [Test]
    public async Task ScanWithProjectionSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .AsProjections<TestUserProjection>()
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedProjection = _testUsers.Select(x => new TestUserProjection { PartitionKey = x.PartitionKey, SortKey = x.SortKey, Name = x.Name });
        scannedItems.ShouldBe(expectedProjection, ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithInPlaceProjectionSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .WithProjectedAttributes(x => x.PartitionKey, x => x.SortKey, x => x.Name)
            .ToAsyncEnumerable()
            .ToListAsync();

        var expectedProjection = _testUsers.Select(x => new TestUser { PartitionKey = x.PartitionKey, SortKey = x.SortKey, Name = x.Name });
        scannedItems.ShouldBe(expectedProjection, ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithConsistentReadSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .WithConsistentRead(true)
            .ToAsyncEnumerable()
            .ToListAsync();
        
        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanAsDocumentsSuccessfully()
    {
        var scannedDocuments = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .AsDocuments()
            .ToAsyncEnumerable()
            .ToListAsync();

        var scannedItems = scannedDocuments.Select(document => _context.ToObject<TestUser>(document));
        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanWithResponseMetadataSuccessfully()
    {
        var response = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .ReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .WithLimit(3)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.Items.Count.ShouldBeLessThanOrEqualTo(3);
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
        response.Count.ShouldBeGreaterThan(0);
        response.ScannedCount.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task ScanWithComplexFilterExpressionSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => 
                x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-") &
                (x.On(y => y.Age).GreaterThan(25) & x.On(y => y.Age).LessThan(35)) &
                x.On(y => y.Score).GreaterThanOrEqualTo(90.0m))
            .ToAsyncEnumerable()
            .ToListAsync();

        scannedItems.ShouldAllBe(x => x.Age > 25 && x.Age < 35 && x.Score >= 90);
    }

    [Test]
    public async Task ScanPagedAsyncEnumerableSuccessfully()
    {
        var pageCount = 0;
        var scannedItems = new List<TestUser>();

        await foreach (var page in _context.Scan<TestUser>()
                           .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
                           .WithLimit(2)
                           .ToPagedAsyncEnumerable())
        {
            pageCount++;
            scannedItems.AddRange(page);
            
            // Each page should have at most 2 items due to limit
            page.Count.ShouldBeLessThanOrEqualTo(2);
        }

        pageCount.ShouldBeGreaterThan(1); // Should have multiple pages due to limit
        
        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanParallelAsyncEnumerableSuccessfully()
    {
        const int totalSegments = 2;
        
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
            .ToParallelAsyncEnumerable(totalSegments)
            .ToListAsync();
        
        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanParallelPagedAsyncEnumerableSuccessfully()
    {
        var pageCount = 0;
        const int totalSegments = 2;
        var scannedItems = new List<TestUser>();

        await foreach (var page in _context.Scan<TestUser>()
                          .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-"))
                          .ToParallelPagedAsyncEnumerable(totalSegments))
        {
            pageCount++;
            scannedItems.AddRange(page);
        }

        pageCount.ShouldBeGreaterThan(0);
        scannedItems.ShouldBe(_testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task ScanEmptyTableReturnEmptyResult()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => x.On(y => y.PartitionKey).BeginsWith("non_existent_prefix"))
            .ToAsyncEnumerable()
            .ToListAsync();

        scannedItems.ShouldBeEmpty();
    }

    [Test]
    public async Task ScanWithStringContainsFilterSuccessfully()
    {
        var scannedItems = await _context.Scan<TestUser>()
            .WithFilterExpression(x => 
                x.On(y => y.PartitionKey).BeginsWith($"{KeyPrefix}-pk-") &
                x.On(y => y.Name).Contains("Alice"))
            .ToAsyncEnumerable()
            .ToListAsync();

        scannedItems.ShouldAllBe(x => x.Name.Contains("Alice"));
    }
} 