using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.GetItem;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;

namespace EfficientDynamoDb.Tests.IntegrationTests.GetItem;

[TestFixture]
public class LowLevelGetItemShould
{
    private const string KeyPrefix = "effddb_tests-get_item-low_level";
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

    [TestCase(true, TestName = "Key attribute names specified")]
    [TestCase(false, TestName = "Key attribute names not specified")]
    public async Task ReturnNullWhenItemDoesNotExist(bool specifyAttributeNames)
    {
        // Arrange
        const string nonExistentPk = $"{KeyPrefix}-non_existent-pk";
        const string nonExistentSk = $"{KeyPrefix}-non_existent-sk";

        // Act
        var request = new GetItemRequest()
        {
            Key = specifyAttributeNames
                ? new PrimaryKey("pk", nonExistentPk, "sk", nonExistentSk)
                : new PrimaryKey(new StringAttributeValue(nonExistentPk), new StringAttributeValue(nonExistentSk)),
            TableName = TestHelper.TestTableName
        };
        var result = await _context.LowLevel.GetItemAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ConsumedCapacity, Is.Null);
        Assert.That(result.Item, Is.Null);
    }

    [Test]
    public async Task ReturnItemWhenItemExists()
    {
        var request = new GetItemRequest
        {
            Key = new PrimaryKey("pk", _testUser.PartitionKey, "sk", _testUser.SortKey),
            TableName = TestHelper.TestTableName
        };
        var result = await _context.LowLevel.GetItemAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ConsumedCapacity, Is.Null);
        Assert.That(result.Item, Is.Not.Null);

        var entity = result.Item.ToObject<TestUser>(_context.Config.Metadata);
        Assert.That(entity, Is.EqualTo(_testUser));
    }

    [TestCase(true, TestName = "When item exists")]
    [TestCase(false, TestName = "When item does not exist")]
    public async Task ReturnConsumedCapacityWhenRequested(bool itemExists)
    {
        // Arrange
        var request = new GetItemRequest
        {
            Key = itemExists
                ? new PrimaryKey("pk", _testUser.PartitionKey, "sk", _testUser.SortKey)
                : new PrimaryKey("pk", $"{KeyPrefix}-non_existent-pk", "sk", $"{KeyPrefix}-non_existent-sk"),
            TableName = TestHelper.TestTableName,
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        // Act
        var result = await _context.LowLevel.GetItemAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ConsumedCapacity, Is.Not.Null);
        Assert.That(result.ConsumedCapacity.CapacityUnits, Is.EqualTo(0.5));
    }

    [Test]
    public async Task ReturnItemWhenConsistentReadRequested()
    {
        // Arrange
        var request = new GetItemRequest
        {
            Key = new PrimaryKey("pk", _testUser.PartitionKey, "sk", _testUser.SortKey),
            TableName = TestHelper.TestTableName,
            ConsistentRead = true,
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        // Act
        var result = await _context.LowLevel.GetItemAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Item, Is.Not.Null);

        var entity = result.Item.ToObject<TestUser>(_context.Config.Metadata);
        Assert.That(entity, Is.EqualTo(_testUser));
        
        Assert.That(result.ConsumedCapacity, Is.Not.Null);
        Assert.That(result.ConsumedCapacity.CapacityUnits, Is.EqualTo(1));
    }
}