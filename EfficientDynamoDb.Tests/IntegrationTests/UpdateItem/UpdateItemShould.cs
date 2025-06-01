using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.IntegrationTests.UpdateItem;

[TestFixture]
public class UpdateItemShould
{
    private const string KeyPrefix = "effddb_tests-update_item";
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
    public async Task UpdateAttributesSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-update-pk";
        _testSortKey = $"{KeyPrefix}-update-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Original Name",
            Age = 25,
            Email = "original@example.com",
            Counter = 1,
            Score = 10.5m,
            Active = false,
            Tags = ["tag1"]
        };
        await _context.PutItemAsync(testUser);

        // Update the item
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Name).Assign("Updated Name")
            .On(x => x.Age).Assign(30)
            .On(x => x.Email).Assign("updated@example.com")
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated Name");
        retrievedItem.Age.ShouldBe(30);
        retrievedItem.Email.ShouldBe("updated@example.com");
        retrievedItem.Counter.ShouldBe(testUser.Counter); // Should remain unchanged
        retrievedItem.Score.ShouldBe(testUser.Score); // Should remain unchanged
        retrievedItem.Active.ShouldBe(testUser.Active); // Should remain unchanged
    }

    [Test]
    public async Task IncrementNumericAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-increment-pk";
        _testSortKey = $"{KeyPrefix}-increment-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Counter User",
            Counter = 5,
            Score = 100.5m
        };
        await _context.PutItemAsync(testUser);

        // Increment counter and score
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Counter).Increment(3)
            .On(x => x.Score).Increment(25.5m)
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Counter.ShouldBe(8);
        retrievedItem.Score.ShouldBe(126.0m);
    }

    [Test]
    public async Task DecrementNumericAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-decrement-pk";
        _testSortKey = $"{KeyPrefix}-decrement-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Decrement User",
            Counter = 10,
            Score = 50.0m
        };
        await _context.PutItemAsync(testUser);

        // Decrement counter and score
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Counter).Decrement(2)
            .On(x => x.Score).Decrement(10.5m)
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Counter.ShouldBe(8);
        retrievedItem.Score.ShouldBe(39.5m);
    }

    [Test]
    public async Task AppendToListAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-append-pk";
        _testSortKey = $"{KeyPrefix}-append-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "List User",
            Tags = ["tag1", "tag2"]
        };
        await _context.PutItemAsync(testUser);

        // Append to list
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Tags).Append(["tag3", "tag4"])
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Tags.ShouldBe(["tag1", "tag2", "tag3", "tag4"]);
    }

    [Test]
    public async Task PrependToListAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-prepend-pk";
        _testSortKey = $"{KeyPrefix}-prepend-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Prepend User",
            Tags = ["tag3", "tag4"]
        };
        await _context.PutItemAsync(testUser);

        // Prepend to list
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Tags).Prepend(["tag1", "tag2"])
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Tags.ShouldBe(["tag1", "tag2", "tag3", "tag4"]);
    }

    [Test]
    public async Task RemoveAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-remove-pk";
        _testSortKey = $"{KeyPrefix}-remove-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Remove User",
            Email = "remove@example.com",
            Counter = 5
        };
        await _context.PutItemAsync(testUser);

        // Remove email attribute
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Email).Remove()
            .ExecuteAsync();

        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe(testUser.Name);
        retrievedItem.Email.ShouldBeEmpty("email should be removed");
        retrievedItem.Counter.ShouldBe(testUser.Counter);
    }

    [Test]
    public async Task ReturnNullWhenReturnValuesNotSpecified()
    {
        _testPartitionKey = $"{KeyPrefix}-no_return-pk";
        _testSortKey = $"{KeyPrefix}-no_return-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "No Return User",
            Age = 25
        };
        await _context.PutItemAsync(testUser);

        // Update without requesting return values
        var result = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .ToItemAsync();

        result.ShouldBeNull();
    }

    [Test]
    public async Task ReturnOldItemWhenReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_old-pk";
        _testSortKey = $"{KeyPrefix}-return_old-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Return Old User",
            Age = 25,
            Email = "old@example.com"
        };
        await _context.PutItemAsync(testUser);

        // Update with return values all old
        var returnedItem = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .On(x => x.Email).Assign("new@example.com")
            .WithReturnValues(ReturnValues.AllOld)
            .ToItemAsync();

        // Verify old values are returned
        returnedItem.ShouldBeEquivalentTo(testUser);

        // Verify item was actually updated
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Age.ShouldBe(30);
        retrievedItem.Email.ShouldBe("new@example.com");
    }

    [Test]
    public async Task ReturnNewItemWhenReturnValuesAllNew()
    {
        _testPartitionKey = $"{KeyPrefix}-return_new-pk";
        _testSortKey = $"{KeyPrefix}-return_new-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Return New User",
            Age = 25,
            Email = "old@example.com"
        };
        await _context.PutItemAsync(testUser);

        // Update with return values all new
        var returnedItem = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .On(x => x.Email).Assign("new@example.com")
            .WithReturnValues(ReturnValues.AllNew)
            .ToItemAsync();

        // Verify new values are returned
        returnedItem.ShouldNotBeNull();
        returnedItem.Name.ShouldBe(testUser.Name);
        returnedItem.Age.ShouldBe(30);
        returnedItem.Email.ShouldBe("new@example.com");
        returnedItem.PartitionKey.ShouldBe(_testPartitionKey);
        returnedItem.SortKey.ShouldBe(_testSortKey);
    }

    [Test]
    public async Task ReturnUpdatedOldWhenReturnValuesUpdatedOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_updated_old-pk";
        _testSortKey = $"{KeyPrefix}-return_updated_old-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Return Updated Old User",
            Age = 25,
            Email = "old@example.com",
            Counter = 5
        };
        await _context.PutItemAsync(testUser);

        // Update with return values updated old
        var returnedItem = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .On(x => x.Email).Assign("new@example.com")
            .WithReturnValues(ReturnValues.UpdatedOld)
            .ToItemAsync();

        // Verify only updated old values are returned (no primary keys)
        returnedItem.ShouldNotBeNull();
        returnedItem.Age.ShouldBe(testUser.Age);
        returnedItem.Email.ShouldBe(testUser.Email);
        
        // Non-updated fields and primary keys should have default values
        returnedItem.PartitionKey.ShouldBeNull();
        returnedItem.SortKey.ShouldBeNull();
        returnedItem.Name.ShouldBeEmpty(); 
        returnedItem.Counter.ShouldBe(0);
        returnedItem.Score.ShouldBe(0);
        returnedItem.Active.ShouldBe(false);
    }

    [Test]
    public async Task ReturnUpdatedNewWhenReturnValuesUpdatedNew()
    {
        _testPartitionKey = $"{KeyPrefix}-return_updated_new-pk";
        _testSortKey = $"{KeyPrefix}-return_updated_new-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Return Updated New User",
            Age = 25,
            Email = "old@example.com",
            Counter = 5
        };
        await _context.PutItemAsync(testUser);

        // Update with return values updated new
        var returnedItem = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .On(x => x.Email).Assign("new@example.com")
            .WithReturnValues(ReturnValues.UpdatedNew)
            .ToItemAsync();

        // Verify only updated new values are returned (no primary keys)
        returnedItem.ShouldNotBeNull();
        returnedItem.Age.ShouldBe(30);
        returnedItem.Email.ShouldBe("new@example.com");
        
        // Non-updated fields and primary keys should have default values
        returnedItem.PartitionKey.ShouldBeNull();
        returnedItem.SortKey.ShouldBeNull();
        returnedItem.Name.ShouldBeEmpty();
        returnedItem.Counter.ShouldBe(0);
        returnedItem.Score.ShouldBe(0);
        returnedItem.Active.ShouldBe(false);
    }

    [Test]
    public async Task SucceedWhenConditionIsMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_met-pk";
        _testSortKey = $"{KeyPrefix}-condition_met-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Condition User",
            Age = 25,
            Active = true
        };
        await _context.PutItemAsync(testUser);

        // Update with condition that should be met
        await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .WithCondition(x => x.On(y => y.Active).EqualTo(testUser.Active) & x.On(y => y.Age).EqualTo(testUser.Age))
            .ExecuteAsync();

        // Verify the update was successful
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Age.ShouldBe(30);
    }

    [Test]
    public async Task FailWhenConditionIsNotMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_not_met-pk";
        _testSortKey = $"{KeyPrefix}-condition_not_met-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Condition User",
            Age = 25,
            Active = false
        };
        await _context.PutItemAsync(testUser);

        // Update with condition that should not be met
        var exception = await Should.ThrowAsync<ConditionalCheckFailedException>(() =>
            _context.UpdateItem<TestUser>()
                .WithPrimaryKey(_testPartitionKey, _testSortKey)
                .On(x => x.Age).Assign(30)
                .WithCondition(x => x.On(y => y.Active).EqualTo(true))
                .ExecuteAsync()
        );

        exception.ShouldNotBeNull();

        // Verify the item was not updated
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Age.ShouldBe(testUser.Age); // Should remain unchanged
    }

    [Test]
    public async Task ReturnResponseWithConsumedCapacity()
    {
        _testPartitionKey = $"{KeyPrefix}-capacity-pk";
        _testSortKey = $"{KeyPrefix}-capacity-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Capacity User",
            Age = 25
        };
        await _context.PutItemAsync(testUser);

        // Update with consumed capacity request
        var response = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.CapacityUnits.ShouldBe(1);
        response.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
    }

    [Test]
    public async Task ReturnResponseWithItemAndConsumedCapacity()
    {
        _testPartitionKey = $"{KeyPrefix}-item_capacity-pk";
        _testSortKey = $"{KeyPrefix}-item_capacity-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Item Capacity User",
            Age = 25,
            Email = "capacity@example.com"
        };
        await _context.PutItemAsync(testUser);

        // Update with both return values and consumed capacity
        var response = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .WithReturnValues(ReturnValues.AllOld)
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.Item.ShouldNotBeNull();
        response.Item.Age.ShouldBe(testUser.Age);
        response.Item.Email.ShouldBe(testUser.Email);
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.CapacityUnits.ShouldBe(1);
    }

    [Test]
    public async Task ReturnDocumentWhenRequested()
    {
        _testPartitionKey = $"{KeyPrefix}-document-pk";
        _testSortKey = $"{KeyPrefix}-document-sk";
        
        // Create initial item
        var testUser = new TestUser
        {
            PartitionKey = _testPartitionKey,
            SortKey = _testSortKey,
            Name = "Document User",
            Age = 25
        };
        await _context.PutItemAsync(testUser);

        // Update and return as document
        var document = await _context.UpdateItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .On(x => x.Age).Assign(30)
            .WithReturnValues(ReturnValues.AllNew)
            .AsDocument()
            .ToItemAsync();

        document.ShouldNotBeNull();
        var updatedUser = document.ToObject<TestUser>(_context.Config.Metadata);
        updatedUser.ShouldNotBeNull();
        updatedUser.PartitionKey.ShouldBe(testUser.PartitionKey);
        updatedUser.SortKey.ShouldBe(testUser.SortKey);
        updatedUser.Name.ShouldBe(testUser.Name);   
        updatedUser.Age.ShouldBe(30);
    }
} 