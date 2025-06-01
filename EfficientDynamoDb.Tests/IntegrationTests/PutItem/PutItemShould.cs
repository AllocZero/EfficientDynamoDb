using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.IntegrationTests.PutItem;

[TestFixture]
public class PutItemShould
{
    private const string KeyPrefix = "effddb_tests-put_item";
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
            await _context.DeleteItemAsync<TestUser>(_testPartitionKey, _testSortKey);
        }
    }
    
    [Test]
    public async Task CreateNewItemSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-create-pk";
        _testSortKey = $"{KeyPrefix}-create-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "John Doe",
            Age = 30,
            Email = "john.doe@example.com"
        };

        await _context.PutItemAsync(testUser);

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReplaceExistingItemSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-replace-pk";
        _testSortKey = $"{KeyPrefix}-replace-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Original Name",
            Age = 25,
            Email = "original@example.com"
        };

        await _context.PutItemAsync(testUser);

        var updatedUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Updated Name",
            Age = 35,
            Email = "updated@example.com"
        };

        await _context.PutItemAsync(updatedUser);

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(updatedUser);
        retrievedItem!.Name.ShouldBe("Updated Name");
        retrievedItem.Age.ShouldBe(35);
        retrievedItem.Email.ShouldBe("updated@example.com");
        
        _testPartitionKey = updatedUser.PartitionKey;
        _testSortKey = updatedUser.SortKey;
    }

    [Test]
    public async Task CreateItemUsingBuilder()
    {
        _testPartitionKey = $"{KeyPrefix}-builder-pk";
        _testSortKey = $"{KeyPrefix}-builder-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Builder User",
            Age = 28,
            Email = "builder@example.com"
        };

        await _context.PutItem()
            .WithItem(testUser)
            .ExecuteAsync();

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReturnNullWhenReturnValuesNotSpecified()
    {
        _testPartitionKey = $"{KeyPrefix}-no_return-pk";
        _testSortKey = $"{KeyPrefix}-no_return-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "No Return User",
            Age = 32,
            Email = "noreturn@example.com"
        };

        var result = await _context.PutItem()
            .WithItem(testUser)
            .ToItemAsync();

        result.ShouldBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReturnOldItemWhenReplacingWithReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_old-pk";
        _testSortKey = $"{KeyPrefix}-return_old-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Original User",
            Age = 40,
            Email = "original@example.com"
        };

        await _context.PutItemAsync(testUser);

        var updatedUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Updated User",
            Age = 45,
            Email = "updated@example.com"
        };

        var returnedItem = await _context.PutItem()
            .WithItem(updatedUser)
            .WithReturnValues(ReturnValues.AllOld)
            .ToItemAsync();

        returnedItem.ShouldBe(testUser);
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(updatedUser);
        
        _testPartitionKey = updatedUser.PartitionKey;
        _testSortKey = updatedUser.SortKey;
    }

    [Test]
    public async Task ReturnNullWhenCreatingNewItemWithReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-new_return_old-pk";
        _testSortKey = $"{KeyPrefix}-new_return_old-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "New User",
            Age = 26,
            Email = "newuser@example.com"
        };

        var returnedItem = await _context.PutItem()
            .WithItem(testUser)
            .WithReturnValues(ReturnValues.AllOld)
            .ToItemAsync();

        returnedItem.ShouldBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }
    
    [Test]
    public async Task SucceedWhenConditionIsMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_met-pk";
        _testSortKey = $"{KeyPrefix}-condition_met-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Condition User",
            Age = 33,
            Email = "condition@example.com"
        };

        await _context.PutItem()
            .WithItem(testUser)
            .WithCondition(cond => cond.On(x => x.PartitionKey).NotExists())
            .ExecuteAsync();

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task FailWhenConditionIsNotMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_fail-pk";
        _testSortKey = $"{KeyPrefix}-condition_fail-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Existing User",
            Age = 29,
            Email = "existing@example.com"
        };

        await _context.PutItemAsync(testUser);

        var newUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "New User",
            Age = 35,
            Email = "new@example.com"
        };

        var exception = await Should.ThrowAsync<ConditionalCheckFailedException>(() =>
            _context.PutItem()
                .WithItem(newUser)
                .WithCondition(cond => cond.On(x => x.PartitionKey).NotExists())
                .ExecuteAsync());

        exception.ShouldNotBeNull();

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReturnResponseWithConsumedCapacity()
    {
        _testPartitionKey = $"{KeyPrefix}-consumed_capacity-pk";
        _testSortKey = $"{KeyPrefix}-consumed_capacity-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Capacity User",
            Age = 31,
            Email = "capacity@example.com"
        };

        var response = await _context.PutItem()
            .WithItem(testUser)
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReturnResponseWithItemAndConsumedCapacity()
    {
        _testPartitionKey = $"{KeyPrefix}-response_full-pk";
        _testSortKey = $"{KeyPrefix}-response_full-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Response User",
            Age = 27,
            Email = "response@example.com"
        };

        await _context.PutItemAsync(testUser);

        var updatedUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Updated Response User",
            Age = 32,
            Email = "updated-response@example.com"
        };

        var response = await _context.PutItem()
            .WithItem(updatedUser)
            .WithReturnValues(ReturnValues.AllOld)
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.Item.ShouldBe(testUser);
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(updatedUser);
        
        _testPartitionKey = updatedUser.PartitionKey;
        _testSortKey = updatedUser.SortKey;
    }

    [Test]
    public async Task ReturnDocumentWhenRequested()
    {
        _testPartitionKey = $"{KeyPrefix}-document-pk";
        _testSortKey = $"{KeyPrefix}-document-sk";
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Document User",
            Age = 34,
            Email = "document@example.com"
        };

        await _context.PutItem()
            .WithItem(testUser)
            .AsDocument()
            .ExecuteAsync();

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }
}