using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.UpdateItem;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.IntegrationTests.UpdateItem;

[TestFixture]
public class LowLevelUpdateItemShould
{
    private const string KeyPrefix = "effddb_tests-update_item-low_level";
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
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Original Name"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("original@example.com")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update the item
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET #n = :name, #a = :age, #e = :email",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                {"#n", "name"},
                {"#a", "age"},
                {"#e", "email"}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":name", new StringAttributeValue("Updated Name")},
                {":age", new NumberAttributeValue("30")},
                {":email", new StringAttributeValue("updated@example.com")}
            }
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.ConsumedCapacity.ShouldBeNull();
        
        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated Name");
        retrievedItem.Age.ShouldBe(30);
        retrievedItem.Email.ShouldBe("updated@example.com");
    }

    [Test]
    public async Task IncrementNumericAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-increment-pk";
        _testSortKey = $"{KeyPrefix}-increment-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Counter User"),
            ["counter"] = new NumberAttributeValue("5"),
            ["score"] = new NumberAttributeValue("100.5")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Increment counter and score
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "ADD #c :counterIncrement, score :scoreIncrement",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                {"#c", "counter"}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":counterIncrement", new NumberAttributeValue("3")},
                {":scoreIncrement", new NumberAttributeValue("25.5")}
            }
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        
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
    public async Task AppendToListAttributeSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-append-pk";
        _testSortKey = $"{KeyPrefix}-append-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("List User"),
            ["tags"] = new ListAttributeValue([new StringAttributeValue("tag1"), new StringAttributeValue("tag2")])
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Append to list
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET tags = list_append(tags, :newTags)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":newTags", new ListAttributeValue([new StringAttributeValue("tag3"), new StringAttributeValue("tag4")])}
            }
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        
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
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Remove User"),
            ["email"] = new StringAttributeValue("remove@example.com"),
            ["counter"] = new NumberAttributeValue("5")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Remove email attribute
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "REMOVE email"
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        
        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Remove User");
        retrievedItem.Email.ShouldBeEmpty(); // Should be default value
        retrievedItem.Counter.ShouldBe(5); // Should remain unchanged
    }

    [Test]
    public async Task ReturnConsumedCapacityWhenRequested()
    {
        _testPartitionKey = $"{KeyPrefix}-capacity-pk";
        _testSortKey = $"{KeyPrefix}-capacity-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Capacity User"),
            ["age"] = new NumberAttributeValue("25")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with consumed capacity request
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")}
            },
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.ConsumedCapacity.ShouldNotBeNull();
        result.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
        result.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
    }

    [Test]
    public async Task ReturnOldItemWhenReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_old-pk";
        _testSortKey = $"{KeyPrefix}-return_old-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Return Old User"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("old@example.com")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with return values all old
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age, email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":email", new StringAttributeValue("new@example.com")}
            },
            ReturnValues = ReturnValues.AllOld
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.Attributes.ShouldNotBeNull();
        result.Attributes["name"].AsStringAttribute().Value.ShouldBe("Return Old User");
        result.Attributes["age"].AsNumberAttribute().Value.ShouldBe("25");
        result.Attributes["email"].AsStringAttribute().Value.ShouldBe("old@example.com");
    }

    [Test]
    public async Task ReturnNewItemWhenReturnValuesAllNew()
    {
        _testPartitionKey = $"{KeyPrefix}-return_new-pk";
        _testSortKey = $"{KeyPrefix}-return_new-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Return New User"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("old@example.com")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with return values all new
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age, email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":email", new StringAttributeValue("new@example.com")}
            },
            ReturnValues = ReturnValues.AllNew
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.Attributes.ShouldNotBeNull();
        result.Attributes["name"].AsStringAttribute().Value.ShouldBe("Return New User");
        result.Attributes["age"].AsNumberAttribute().Value.ShouldBe("30");
        result.Attributes["email"].AsStringAttribute().Value.ShouldBe("new@example.com");
    }

    [Test]
    public async Task ReturnUpdatedOldWhenReturnValuesUpdatedOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_updated_old-pk";
        _testSortKey = $"{KeyPrefix}-return_updated_old-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Return Updated Old User"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("old@example.com"),
            ["counter"] = new NumberAttributeValue("5")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with return values updated old
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age, email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":email", new StringAttributeValue("new@example.com")}
            },
            ReturnValues = ReturnValues.UpdatedOld
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.Attributes.ShouldNotBeNull();
        // Only updated attributes should be returned with old values (no primary keys)
        result.Attributes["age"].AsNumberAttribute().Value.ShouldBe("25");
        result.Attributes["email"].AsStringAttribute().Value.ShouldBe("old@example.com");
        
        // Primary keys and non-updated attributes should not be present
        result.Attributes.ShouldNotContainKey("pk");
        result.Attributes.ShouldNotContainKey("sk");
        result.Attributes.ShouldNotContainKey("name");
        result.Attributes.ShouldNotContainKey("counter");
    }

    [Test]
    public async Task ReturnUpdatedNewWhenReturnValuesUpdatedNew()
    {
        _testPartitionKey = $"{KeyPrefix}-return_updated_new-pk";
        _testSortKey = $"{KeyPrefix}-return_updated_new-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Return Updated New User"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("old@example.com"),
            ["counter"] = new NumberAttributeValue("5")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with return values updated new
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age, email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":email", new StringAttributeValue("new@example.com")}
            },
            ReturnValues = ReturnValues.UpdatedNew
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        result.Attributes.ShouldNotBeNull();
        // Only updated attributes should be returned with new values (no primary keys)
        result.Attributes["age"].AsNumberAttribute().Value.ShouldBe("30");
        result.Attributes["email"].AsStringAttribute().Value.ShouldBe("new@example.com");
       
        // Primary keys and non-updated attributes should not be present
        result.Attributes.ShouldNotContainKey("pk");
        result.Attributes.ShouldNotContainKey("sk");
        result.Attributes.ShouldNotContainKey("name");
        result.Attributes.ShouldNotContainKey("counter");
    }

    [Test]
    public async Task SucceedWhenConditionIsMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_met-pk";
        _testSortKey = $"{KeyPrefix}-condition_met-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Condition User"),
            ["age"] = new NumberAttributeValue("25"),
            ["active"] = new BoolAttributeValue(true)
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with condition that should be met
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age",
            ConditionExpression = "active = :active AND age = :currentAge",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":active", new BoolAttributeValue(true)},
                {":currentAge", new NumberAttributeValue("25")}
            }
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        
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
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Condition User"),
            ["age"] = new NumberAttributeValue("25"),
            ["active"] = new BoolAttributeValue(false)
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with condition that should not be met
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET age = :age",
            ConditionExpression = "active = :active",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":age", new NumberAttributeValue("30")},
                {":active", new BoolAttributeValue(true)}
            }
        };

        var exception = await Should.ThrowAsync<ConditionalCheckFailedException>(() =>
            _context.LowLevel.UpdateItemAsync(updateRequest)
        );

        exception.ShouldNotBeNull();

        // Verify the item was not updated
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Age.ShouldBe(25); // Should remain unchanged
    }

    [Test]
    public async Task WorkWithComplexConditionsAndExpressionAttributes()
    {
        _testPartitionKey = $"{KeyPrefix}-complex_condition-pk";
        _testSortKey = $"{KeyPrefix}-complex_condition-sk";
        
        // Create initial item
        var initialItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Complex User"),
            ["age"] = new NumberAttributeValue("25"),
            ["status"] = new StringAttributeValue("active"),
            ["counter"] = new NumberAttributeValue("10")
        };

        await _context.LowLevel.PutItemAsync(new Operations.PutItem.PutItemRequest
        {
            Item = initialItem,
            TableName = TestHelper.TestTableName
        });

        // Update with complex condition and expression attribute names
        var updateRequest = new UpdateItemRequest
        {
            TableName = TestHelper.TestTableName,
            Key = new PrimaryKey("pk", _testPartitionKey, "sk", _testSortKey),
            UpdateExpression = "SET #n = :name, #a = :age ADD #c :increment",
            ConditionExpression = "#s = :status AND #c > :minCounter",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                {"#n", "name"},
                {"#a", "age"},
                {"#s", "status"},
                {"#c", "counter"}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":name", new StringAttributeValue("Updated Complex User")},
                {":age", new NumberAttributeValue("30")},
                {":status", new StringAttributeValue("active")},
                {":minCounter", new NumberAttributeValue("5")},
                {":increment", new NumberAttributeValue("3")}
            }
        };

        var result = await _context.LowLevel.UpdateItemAsync(updateRequest);

        result.ShouldNotBeNull();
        
        // Verify the update
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated Complex User");
        retrievedItem.Age.ShouldBe(30);
        retrievedItem.Counter.ShouldBe(13); // 10 + 3
    }
} 