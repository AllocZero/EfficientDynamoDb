using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.IntegrationTests.GetItem;

[TestFixture]
public class GetItemShould
{
    private const string KeyPrefix = "effddb_tests-get_item";
    private DynamoDbContext _context = null!;
    private TestUser _testUser = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        _testUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-pk",
            SortKey = $"{KeyPrefix}-sk",
            Name = "Test User",
            Age = 25,
            Email = "test@example.com"
        };
        
        await _context.PutItemAsync(_testUser);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _context.DeleteItemAsync<TestUser>(_testUser.PartitionKey, _testUser.SortKey);
    }

    [Test]
    public async Task ReturnNullWhenItemDoesNotExist()
    {
        const string nonExistentPk = $"{KeyPrefix}-non_existent-pk";
        const string nonExistentSk = $"{KeyPrefix}-non_existent-sk";

        var result = await _context.GetItemAsync<TestUser>(nonExistentPk, nonExistentSk);
        result.ShouldBeNull();
    }

    [Test]
    public async Task ReturnItemWhenItemExists()
    {
        var result = await _context.GetItemAsync<TestUser>(_testUser.PartitionKey, _testUser.SortKey);
        result.ShouldBe(_testUser);
    }

    [Test]
    public async Task ReturnItemUsingBuilder()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .ToItemAsync();

        result.ShouldBe(_testUser);
    }

    [Test]
    public async Task ReturnItemWithInplaceProjection()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithProjectedAttributes(x => x.Name, x => x.PartitionKey, x => x.SortKey)
            .ToItemAsync();

        result.ShouldNotBeNull();
        result.PartitionKey.ShouldBe(_testUser.PartitionKey);
        result.SortKey.ShouldBe(_testUser.SortKey);
        result.Name.ShouldBe(_testUser.Name);
        
        // Age and Email should be default values since they weren't projected
        result.Age.ShouldBe(0);
        result.Email.ShouldBeEmpty();
    }

    [Test]
    public async Task ReturnProjectedItem()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsProjection<TestUserProjection>()
            .ToItemAsync();
        
        result.ShouldNotBeNull();
        result.PartitionKey.ShouldBe(_testUser.PartitionKey);
        result.SortKey.ShouldBe(_testUser.SortKey);
        result.Name.ShouldBe(_testUser.Name);
    }
    
    [Test]
    public async Task ReturnProjectedItemWithInplaceProjection()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsProjection<TestUserProjection>(x => x.PartitionKey, x => x.SortKey)
            .ToItemAsync();
        
        result.ShouldNotBeNull();
        result.PartitionKey.ShouldBe(_testUser.PartitionKey);
        result.SortKey.ShouldBe(_testUser.SortKey);
        result.Name.ShouldBeEmpty();
    }

    [Test]
    public async Task ReturnItemWithConsistentRead()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();

        result.ShouldBe(_testUser);
    }

    [Test]
    public async Task ReturnItemAsDocument()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsDocument()
            .ToItemAsync();
        
        result.ShouldNotBeNull();
        
        var objectResult = result.ToObject<TestUser>(_context.Config.Metadata);
        objectResult.ShouldBe(_testUser);
    }

    [TestCase(true, TestName = "Return response with consistent read")]
    [TestCase(false, TestName = "Return response without consistent read")]
    public async Task ReturnResponseWithMetadata(bool useConsistentRead)
    {
        var response = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithConsistentRead(useConsistentRead)
            .ReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.Item.ShouldNotBeNull();
        response.Item.ShouldBe(_testUser);
     
        var expectedConsumedCapacity = useConsistentRead ? 1.0f : 0.5f;
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.CapacityUnits.ShouldBe(expectedConsumedCapacity);
    }
}