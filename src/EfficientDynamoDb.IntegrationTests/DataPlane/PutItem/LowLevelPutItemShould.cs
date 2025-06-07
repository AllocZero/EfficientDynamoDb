using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.PutItem;

[TestFixture]
public class LowLevelPutItemShould
{
    private const string KeyPrefix = "effddb_tests-put_item-low_level";
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
        
        var item = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Low Level User"),
            ["age"] = new NumberAttributeValue("30"),
            ["email"] = new StringAttributeValue("lowlevel@example.com")
        };

        var request = new PutItemRequest
        {
            Item = item,
            TableName = TestHelper.TestTableName
        };

        var result = await _context.LowLevel.PutItemAsync(request);

        result.ShouldNotBeNull();
        result.ConsumedCapacity.ShouldBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Low Level User");
        retrievedItem.Age.ShouldBe(30);
        retrievedItem.Email.ShouldBe("lowlevel@example.com");
    }

    [Test]
    public async Task ReplaceExistingItemSuccessfully()
    {
        _testPartitionKey = $"{KeyPrefix}-replace-pk";
        _testSortKey = $"{KeyPrefix}-replace-sk";
        
        var originalItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Original User"),
            ["age"] = new NumberAttributeValue("25")
        };

        await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = originalItem,
            TableName = TestHelper.TestTableName
        });

        var updatedItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Updated User"),
            ["age"] = new NumberAttributeValue("35"),
            ["email"] = new StringAttributeValue("updated@example.com")
        };

        var result = await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = updatedItem,
            TableName = TestHelper.TestTableName
        });

        result.ShouldNotBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated User");
        retrievedItem.Age.ShouldBe(35);
        retrievedItem.Email.ShouldBe("updated@example.com");
    }

    [Test]
    public async Task ReturnConsumedCapacityWhenRequested()
    {
        _testPartitionKey = $"{KeyPrefix}-capacity-pk";
        _testSortKey = $"{KeyPrefix}-capacity-sk";
        
        var item = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Capacity User"),
            ["age"] = new NumberAttributeValue("28")
        };

        var request = new PutItemRequest
        {
            Item = item,
            TableName = TestHelper.TestTableName,
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        var result = await _context.LowLevel.PutItemAsync(request);

        result.ShouldNotBeNull();
        result.ConsumedCapacity.ShouldNotBeNull();
        result.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task ReturnOldItemWhenReplacingWithReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-return_old-pk";
        _testSortKey = $"{KeyPrefix}-return_old-sk";
        
        var originalItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Original User"),
            ["age"] = new NumberAttributeValue("40")
        };

        await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = originalItem,
            TableName = TestHelper.TestTableName
        });

        var updatedItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Updated User"),
            ["age"] = new NumberAttributeValue("45")
        };

        var result = await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = updatedItem,
            TableName = TestHelper.TestTableName,
            ReturnValues = ReturnValues.AllOld
        });

        result.ShouldNotBeNull();
        result.Attributes.ShouldNotBeNull();
        result.Attributes["name"].AsStringAttribute().Value.ShouldBe("Original User");
        result.Attributes["age"].AsNumberAttribute().Value.ShouldBe("40");
    }

    [Test]
    public async Task ReturnNullWhenCreatingNewItemWithReturnValuesAllOld()
    {
        _testPartitionKey = $"{KeyPrefix}-new_return_old-pk";
        _testSortKey = $"{KeyPrefix}-new_return_old-sk";
        
        var item = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("New User"),
            ["age"] = new NumberAttributeValue("26")
        };

        var request = new PutItemRequest
        {
            Item = item,
            TableName = TestHelper.TestTableName,
            ReturnValues = ReturnValues.AllOld
        };

        var result = await _context.LowLevel.PutItemAsync(request);

        result.ShouldNotBeNull();
        result.Attributes.ShouldBeNull();
    }

    [Test]
    public async Task SucceedWhenConditionIsMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_met-pk";
        _testSortKey = $"{KeyPrefix}-condition_met-sk";
        
        var item = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Condition User"),
            ["age"] = new NumberAttributeValue("33")
        };

        var request = new PutItemRequest
        {
            Item = item,
            TableName = TestHelper.TestTableName,
            ConditionExpression = "attribute_not_exists(pk)"
        };

        var result = await _context.LowLevel.PutItemAsync(request);

        result.ShouldNotBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Condition User");
    }

    [Test]
    public async Task FailWhenConditionIsNotMet()
    {
        _testPartitionKey = $"{KeyPrefix}-condition_fail-pk";
        _testSortKey = $"{KeyPrefix}-condition_fail-sk";
        
        var originalItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Existing User"),
            ["age"] = new NumberAttributeValue("29")
        };

        await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = originalItem,
            TableName = TestHelper.TestTableName
        });

        var newItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("New User"),
            ["age"] = new NumberAttributeValue("35")
        };

        var exception = await Should.ThrowAsync<ConditionalCheckFailedException>(() =>
            _context.LowLevel.PutItemAsync(new PutItemRequest
            {
                Item = newItem,
                TableName = TestHelper.TestTableName,
                ConditionExpression = "attribute_not_exists(pk)"
            }));

        exception.ShouldNotBeNull();

        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Existing User");
        retrievedItem.Age.ShouldBe(29);
    }

    [Test]
    public async Task WorkWithComplexConditionsAndExpressionAttributes()
    {
        _testPartitionKey = $"{KeyPrefix}-complex-pk";
        _testSortKey = $"{KeyPrefix}-complex-sk";
        
        var originalItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Complex User"),
            ["age"] = new NumberAttributeValue("25")
        };

        await _context.LowLevel.PutItemAsync(new PutItemRequest
        {
            Item = originalItem,
            TableName = TestHelper.TestTableName
        });

        var updatedItem = new Document
        {
            ["pk"] = new StringAttributeValue(_testPartitionKey),
            ["sk"] = new StringAttributeValue(_testSortKey),
            ["name"] = new StringAttributeValue("Updated Complex User"),
            ["age"] = new NumberAttributeValue("30")
        };

        var request = new PutItemRequest
        {
            Item = updatedItem,
            TableName = TestHelper.TestTableName,
            ConditionExpression = "#age = :expectedAge",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                ["#age"] = "age"
            },
            ExpressionAttributeValues = new Document
            {
                [":expectedAge"] = new NumberAttributeValue("25")
            }
        };

        var result = await _context.LowLevel.PutItemAsync(request);

        result.ShouldNotBeNull();
        
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(_testPartitionKey, _testSortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated Complex User");
        retrievedItem.Age.ShouldBe(30);
    }
}