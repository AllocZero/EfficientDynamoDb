using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;

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
        // Arrange
        const string nonExistentPk = $"{KeyPrefix}-non_existent-pk";
        const string nonExistentSk = $"{KeyPrefix}-non_existent-sk";

        // Act
        var result = await _context.GetItemAsync<TestUser>(nonExistentPk, nonExistentSk);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ReturnItemWhenItemExists()
    {
        // Act
        var result = await _context.GetItemAsync<TestUser>(_testUser.PartitionKey, _testUser.SortKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(_testUser));
    }

    [Test]
    public async Task ReturnItemUsingBuilder()
    {
        // Act
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .ToItemAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(_testUser));
    }

    [Test]
    public async Task ReturnItemWithInplaceProjection()
    {
        // Act
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithProjectedAttributes(x => x.Name, x => x.PartitionKey, x => x.SortKey)
            .ToItemAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PartitionKey, Is.EqualTo(_testUser.PartitionKey));
        Assert.That(result.SortKey, Is.EqualTo(_testUser.SortKey));
        Assert.That(result.Name, Is.EqualTo(_testUser.Name));
        
        // Age and Email should be default values since they weren't projected
        Assert.That(result.Age, Is.EqualTo(0));
        Assert.That(result.Email, Is.Empty);
    }

    [Test]
    public async Task ReturnProjectedItem()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsProjection<TestUserProjection>()
            .ToItemAsync();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PartitionKey, Is.EqualTo(_testUser.PartitionKey));
        Assert.That(result.SortKey, Is.EqualTo(_testUser.SortKey));
        Assert.That(result.Name, Is.EqualTo(_testUser.Name));
    }
    
    [Test]
    public async Task ReturnProjectedItemWithInplaceProjection()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsProjection<TestUserProjection>(x => x.PartitionKey, x => x.SortKey)
            .ToItemAsync();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PartitionKey, Is.EqualTo(_testUser.PartitionKey));
        Assert.That(result.SortKey, Is.EqualTo(_testUser.SortKey));
        Assert.That(result.Name, Is.Empty);
    }

    [Test]
    public async Task ReturnItemWithConsistentRead()
    {
        // Act
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(_testUser));
    }

    [Test]
    public async Task ReturnItemAsDocument()
    {
        var result = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .AsDocument()
            .ToItemAsync();
        
        Assert.That(result, Is.Not.Null);
        
        var objectResult = result.ToObject<TestUser>(_context.Config.Metadata);
        Assert.That(objectResult , Is.Not.Null);
        Assert.That(objectResult, Is.EqualTo(_testUser));
    }

    [TestCase(true, TestName = "Return response with consistent read")]
    [TestCase(false, TestName = "Return response without consistent read")]
    public async Task ReturnResponseWithMetadata(bool useConsistentRead)
    {
        // Act
        var response = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testUser.PartitionKey, _testUser.SortKey)
            .WithConsistentRead(useConsistentRead)
            .ReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Item, Is.Not.Null);
        Assert.That(response.Item!.PartitionKey, Is.EqualTo(_testUser.PartitionKey));
     
        var expectedConsumedCapacity = useConsistentRead ? 1d : 0.5d;
        Assert.That(response.ConsumedCapacity, Is.Not.Null);
        Assert.That(response.ConsumedCapacity.CapacityUnits, Is.EqualTo(expectedConsumedCapacity));
    }
}