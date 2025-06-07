using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;
using ValidationException = EfficientDynamoDb.Exceptions.ValidationException;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.BatchWrite;

[TestFixture]
public class LowLevelBatchWriteShould
{
    private const string KeyPrefix = "effddb_tests-batch_write-low_level";
    private DynamoDbContext _context = null!;
    private readonly List<(string Pk, string Sk)> _itemsToCleanup = [];

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateContext();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_itemsToCleanup.Count != 0)
        {
            await _context.BatchWrite()
                .WithItems(_itemsToCleanup.Select(item => Batch.DeleteItem<TestUser>().WithPrimaryKey(item.Pk, item.Sk)))
                .ExecuteAsync();
        }
        _itemsToCleanup.Clear();
    }

    [Test]
    public async Task CreateItemsSuccessfullyWithPutOperations()
    {
        var pk1 = $"{KeyPrefix}-put-pk-1";
        var sk1 = $"{KeyPrefix}-put-sk-1";
        var pk2 = $"{KeyPrefix}-put-pk-2";
        var sk2 = $"{KeyPrefix}-put-sk-2";

        _itemsToCleanup.AddRange([(pk1, sk1), (pk2, sk2)]);

        var item1 = new Document
        {
            ["pk"] = new StringAttributeValue(pk1),
            ["sk"] = new StringAttributeValue(sk1),
            ["name"] = new StringAttributeValue("Low Level User 1"),
            ["age"] = new NumberAttributeValue("25"),
            ["email"] = new StringAttributeValue("lowlevel1@example.com")
        };

        var item2 = new Document
        {
            ["pk"] = new StringAttributeValue(pk2),
            ["sk"] = new StringAttributeValue(sk2),
            ["name"] = new StringAttributeValue("Low Level User 2"),
            ["age"] = new NumberAttributeValue("30"),
            ["email"] = new StringAttributeValue("lowlevel2@example.com")
        };

        var request = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(item1)), new(new BatchWritePutRequest(item2))]
                }
            }
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(request);

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldBeNull();

        // Verify items were created using batch read
        var retrievedItems = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(pk1, sk1),
                Batch.GetItem<TestUser>().WithPrimaryKey(pk2, sk2)
            )
            .ToListAsync<TestUser>();

        retrievedItems.Count.ShouldBe(2);
        var user1 = retrievedItems.First(u => u.PartitionKey == pk1);
        var user2 = retrievedItems.First(u => u.PartitionKey == pk2);
        
        user1.Name.ShouldBe("Low Level User 1");
        user1.Age.ShouldBe(25);
        user2.Name.ShouldBe("Low Level User 2");
        user2.Age.ShouldBe(30);
    }

    [Test]
    public async Task DeleteItemsSuccessfullyWithDeleteOperations()
    {
        var pk1 = $"{KeyPrefix}-delete-pk-1";
        var sk1 = $"{KeyPrefix}-delete-sk-1";
        var pk2 = $"{KeyPrefix}-delete-pk-2";
        var sk2 = $"{KeyPrefix}-delete-sk-2";

        // First create the items to delete
        var createRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [
                        new(new BatchWritePutRequest(new()
                        {
                            ["pk"] = new StringAttributeValue(pk1),
                            ["sk"] = new StringAttributeValue(sk1),
                            ["name"] = new StringAttributeValue("To Delete 1"),
                            ["age"] = new NumberAttributeValue("35")
                        })),
                        new(new BatchWritePutRequest(new()
                        {
                            ["pk"] = new StringAttributeValue(pk2),
                            ["sk"] = new StringAttributeValue(sk2),
                            ["name"] = new StringAttributeValue("To Delete 2"),
                            ["age"] = new NumberAttributeValue("40")
                        }))
                    ]
                }
            }
        };

        await _context.LowLevel.BatchWriteItemAsync(createRequest);

        // Now delete them
        var deleteRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [
                        new(new BatchWriteDeleteRequest(new Dictionary<string, AttributeValue>
                        {
                            ["pk"] = new StringAttributeValue(pk1),
                            ["sk"] = new StringAttributeValue(sk1)
                        })),
                        new(new BatchWriteDeleteRequest(new Dictionary<string, AttributeValue>
                        {
                            ["pk"] = new StringAttributeValue(pk2),
                            ["sk"] = new StringAttributeValue(sk2)
                        }))
                    ]
                }
            }
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(deleteRequest);

        response.ShouldNotBeNull();

        // Verify items were deleted using batch read
        var retrievedItems = await _context.BatchGet()
            .WithItems(
                Batch.GetItem<TestUser>().WithPrimaryKey(pk1, sk1),
                Batch.GetItem<TestUser>().WithPrimaryKey(pk2, sk2)
            )
            .ToListAsync<TestUser>();

        retrievedItems.ShouldBeEmpty();
    }

    [Test]
    public async Task ExecuteMixedPutAndDeleteOperationsSuccessfully()
    {
        var existingPk = $"{KeyPrefix}-mixed-existing-pk";
        var existingSk = $"{KeyPrefix}-mixed-existing-sk";
        var newPk = $"{KeyPrefix}-mixed-new-pk";
        var newSk = $"{KeyPrefix}-mixed-new-sk";

        _itemsToCleanup.Add((newPk, newSk));

        // Create an existing item first
        var existingItem = new Document
        {
            ["pk"] = new StringAttributeValue(existingPk),
            ["sk"] = new StringAttributeValue(existingSk),
            ["name"] = new StringAttributeValue("Existing Item"),
            ["age"] = new NumberAttributeValue("45")
        };

        var createRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(existingItem))]
                }
            }
        };

        await _context.LowLevel.BatchWriteItemAsync(createRequest);

        // Now execute mixed operations: delete existing item and create new item
        var newItem = new Document
        {
            ["pk"] = new StringAttributeValue(newPk),
            ["sk"] = new StringAttributeValue(newSk),
            ["name"] = new StringAttributeValue("New Mixed Item"),
            ["age"] = new NumberAttributeValue("28")
        };

        var mixedRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [
                        new(new BatchWriteDeleteRequest(new Dictionary<string, AttributeValue>
                        {
                            ["pk"] = new StringAttributeValue(existingPk),
                            ["sk"] = new StringAttributeValue(existingSk)
                        })),
                        new(new BatchWritePutRequest(newItem))
                    ]
                }
            }
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(mixedRequest);

        response.ShouldNotBeNull();

        // Verify existing item was deleted
        var deletedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(existingPk, existingSk)
            .WithConsistentRead(true)
            .ToItemAsync();
        deletedItem.ShouldBeNull();

        // Verify new item was created
        var retrievedNewItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(newPk, newSk)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedNewItem.ShouldNotBeNull();
        retrievedNewItem.Name.ShouldBe("New Mixed Item");
        retrievedNewItem.Age.ShouldBe(28);
    }

    [Test]
    public async Task ReturnConsumedCapacityWhenRequested()
    {
        var pk = $"{KeyPrefix}-capacity-pk";
        var sk = $"{KeyPrefix}-capacity-sk";

        _itemsToCleanup.Add((pk, sk));

        var item = new Document
        {
            ["pk"] = new StringAttributeValue(pk),
            ["sk"] = new StringAttributeValue(sk),
            ["name"] = new StringAttributeValue("Capacity User"),
            ["age"] = new NumberAttributeValue("30")
        };

        var request = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(item))]
                }
            },
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(request);

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull().Count.ShouldBeGreaterThan(0);
        response.ConsumedCapacity[0].CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task HandleDeleteOperationsOnNonExistentItems()
    {
        var nonExistentPk1 = $"{KeyPrefix}-nonexistent-pk-1";
        var nonExistentSk1 = $"{KeyPrefix}-nonexistent-sk-1";
        var nonExistentPk2 = $"{KeyPrefix}-nonexistent-pk-2";
        var nonExistentSk2 = $"{KeyPrefix}-nonexistent-sk-2";

        var request = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [
                        new(new BatchWriteDeleteRequest(new Dictionary<string, AttributeValue>
                        {
                            ["pk"] = new StringAttributeValue(nonExistentPk1),
                            ["sk"] = new StringAttributeValue(nonExistentSk1)
                        })),
                        new(new BatchWriteDeleteRequest(new Dictionary<string, AttributeValue>
                        {
                            ["pk"] = new StringAttributeValue(nonExistentPk2),
                            ["sk"] = new StringAttributeValue(nonExistentSk2)
                        }))
                    ]
                }
            }
        };

        // Should not throw an exception - DynamoDB allows deleting non-existent items
        var response = await _context.LowLevel.BatchWriteItemAsync(request);

        response.ShouldNotBeNull();
    }

    [Test]
    public async Task ReplaceExistingItemSuccessfully()
    {
        var pk = $"{KeyPrefix}-replace-pk";
        var sk = $"{KeyPrefix}-replace-sk";

        _itemsToCleanup.Add((pk, sk));

        // Create original item
        var originalItem = new Document
        {
            ["pk"] = new StringAttributeValue(pk),
            ["sk"] = new StringAttributeValue(sk),
            ["name"] = new StringAttributeValue("Original Name"),
            ["age"] = new NumberAttributeValue("25")
        };

        var createRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(originalItem))]
                }
            }
        };

        await _context.LowLevel.BatchWriteItemAsync(createRequest);

        // Replace with updated item
        var updatedItem = new Document
        {
            ["pk"] = new StringAttributeValue(pk),
            ["sk"] = new StringAttributeValue(sk),
            ["name"] = new StringAttributeValue("Updated Name"),
            ["age"] = new NumberAttributeValue("35"),
            ["email"] = new StringAttributeValue("updated@example.com")
        };

        var updateRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(updatedItem))]
                }
            }
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(updateRequest);

        response.ShouldNotBeNull();

        // Verify item was replaced
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(pk, sk)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Updated Name");
        retrievedItem.Age.ShouldBe(35);
        retrievedItem.Email.ShouldBe("updated@example.com");
    }

    [Test]
    public async Task ThrowValidationExceptionWhenNoItemsRequested()
    {
        var request = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>()
        };

        await Should.ThrowAsync<ValidationException>(_context.LowLevel.BatchWriteItemAsync(request));
    }

    [Test]
    public async Task SupportSingleOperationBatch()
    {
        var pk = $"{KeyPrefix}-single-pk";
        var sk = $"{KeyPrefix}-single-sk";

        _itemsToCleanup.Add((pk, sk));

        var item = new Document
        {
            ["pk"] = new StringAttributeValue(pk),
            ["sk"] = new StringAttributeValue(sk),
            ["name"] = new StringAttributeValue("Single Operation User"),
            ["age"] = new NumberAttributeValue("29")
        };

        var request = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, IReadOnlyList<BatchWriteOperation>>
            {
                {
                    TestHelper.TestTableName,
                    [new(new BatchWritePutRequest(item))]
                }
            }
        };

        var response = await _context.LowLevel.BatchWriteItemAsync(request);

        response.ShouldNotBeNull();

        // Verify item was created
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(pk, sk)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldNotBeNull();
        retrievedItem.Name.ShouldBe("Single Operation User");
        retrievedItem.Age.ShouldBe(29);
    }
} 