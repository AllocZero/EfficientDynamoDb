using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.BatchWrite;

[TestFixture]
public class BatchWriteShould
{
    private const string KeyPrefix = "effddb_tests-batch_write";
    private DynamoDbContext _context = null!;
    private List<TestUser> _testUsersToCleanup = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestHelper.CreateContext();
        _testUsersToCleanup = [];
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_testUsersToCleanup.Count != 0)
        {
            await _context.BatchWrite()
                .WithItems(_testUsersToCleanup.Select(user => Batch.DeleteItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
                .ExecuteAsync();
        }
    }

    [Test]
    public async Task CreateNewItemsSuccessfullyWithPutOperations()
    {
        var testUsers = new[]
        {
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-put-pk-1",
                SortKey = $"{KeyPrefix}-put-sk-1",
                Name = "Batch User 1",
                Age = 25,
                Email = "batch1@example.com"
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-put-pk-2",
                SortKey = $"{KeyPrefix}-put-sk-2", 
                Name = "Batch User 2",
                Age = 30,
                Email = "batch2@example.com"
            }
        };

        _testUsersToCleanup.AddRange(testUsers);

        await _context.BatchWrite()
            .WithItems(testUsers.Select(Batch.PutItem))
            .ExecuteAsync();

        // Verify items were created using batch read
        var retrievedItems = await _context.BatchGet()
            .WithItems(testUsers.Select(user => Batch.GetItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ToListAsync<TestUser>();
        
        retrievedItems.ShouldBe(testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task DeleteExistingItemsSuccessfullyWithDeleteOperations()
    {
        var testUsers = new[]
        {
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-delete-pk-1",
                SortKey = $"{KeyPrefix}-delete-sk-1",
                Name = "Delete User 1",
                Age = 28,
                Email = "delete1@example.com"
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-delete-pk-2",
                SortKey = $"{KeyPrefix}-delete-sk-2",
                Name = "Delete User 2",
                Age = 32,
                Email = "delete2@example.com"
            }
        };

        // First create the items
        await _context.BatchWrite()
            .WithItems(testUsers.Select(Batch.PutItem))
            .ExecuteAsync();

        // Then delete them
        await _context.BatchWrite()
            .WithItems(testUsers.Select(user => Batch.DeleteItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ExecuteAsync();

        // Verify items were deleted using batch read
        var retrievedItems = await _context.BatchGet()
            .WithItems(testUsers.Select(user => Batch.GetItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ToListAsync<TestUser>();
        
        retrievedItems.ShouldBeEmpty();
    }

    [Test]
    public async Task ExecuteMixedPutAndDeleteOperationsSuccessfully()
    {
        var existingUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-mixed-existing-pk",
            SortKey = $"{KeyPrefix}-mixed-existing-sk",
            Name = "Existing User",
            Age = 35,
            Email = "existing@example.com"
        };

        var newUser1 = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-mixed-new-pk-1",
            SortKey = $"{KeyPrefix}-mixed-new-sk-1",
            Name = "New User 1",
            Age = 26,
            Email = "new1@example.com"
        };

        var newUser2 = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-mixed-new-pk-2",
            SortKey = $"{KeyPrefix}-mixed-new-sk-2",
            Name = "New User 2",
            Age = 29,
            Email = "new2@example.com"
        };

        // Create the existing user first
        await _context.PutItemAsync(existingUser);

        _testUsersToCleanup.AddRange(new[] { newUser1, newUser2 });

        // Execute mixed operations: delete existing user and put new users
        await _context.BatchWrite()
            .WithItems(
                Batch.DeleteItem<TestUser>().WithPrimaryKey(existingUser.PartitionKey, existingUser.SortKey),
                Batch.PutItem(newUser1),
                Batch.PutItem(newUser2)
            )
            .ExecuteAsync();

        // Verify results using batch read
        var allUsersToCheck = new[] { existingUser, newUser1, newUser2 };
        var retrievedItems = await _context.BatchGet()
            .WithItems(allUsersToCheck.Select(user => Batch.GetItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ToListAsync<TestUser>();

        // Should only contain the new users, not the deleted existing user
        retrievedItems.ShouldBe(new[] { newUser1, newUser2 }, ignoreOrder: true);
    }

    [Test]
    public async Task ReplaceExistingItemsSuccessfullyWithPutOperations()
    {
        var originalUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-replace-pk",
            SortKey = $"{KeyPrefix}-replace-sk",
            Name = "Original User",
            Age = 25,
            Email = "original@example.com"
        };

        var updatedUser = new TestUser
        {
            PartitionKey = originalUser.PartitionKey,
            SortKey = originalUser.SortKey,
            Name = "Updated User",
            Age = 35,
            Email = "updated@example.com"
        };

        _testUsersToCleanup.Add(updatedUser);

        // Create original user
        await _context.PutItemAsync(originalUser);

        // Replace with updated user using batch write
        await _context.BatchWrite()
            .WithItems(Batch.PutItem(updatedUser))
            .ExecuteAsync();

        // Verify item was replaced
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(updatedUser.PartitionKey, updatedUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(updatedUser);
    }

    [Test]
    public async Task ReturnResponseWithConsumedCapacity()
    {
        var testUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-capacity-pk",
            SortKey = $"{KeyPrefix}-capacity-sk",
            Name = "Capacity User",
            Age = 30,
            Email = "capacity@example.com"
        };

        _testUsersToCleanup.Add(testUser);

        var response = await _context.BatchWrite()
            .WithItems(Batch.PutItem(testUser))
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull().Count.ShouldBeGreaterThan(0);
        response.ConsumedCapacity[0].CapacityUnits.ShouldBeGreaterThan(0);

        // Verify item was created
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(testUser.PartitionKey, testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ReturnResponseWithBothConsumedCapacityAndItemCollectionMetrics()
    {
        var testUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-metrics-pk",
            SortKey = $"{KeyPrefix}-metrics-sk",
            Name = "Metrics User",
            Age = 30,
            Email = "metrics@example.com"
        };

        _testUsersToCleanup.Add(testUser);

        var response = await _context.BatchWrite()
            .WithItems(Batch.PutItem(testUser))
            .WithReturnConsumedCapacity(ReturnConsumedCapacity.Total)
            .WithReturnItemCollectionMetrics(ReturnItemCollectionMetrics.Size)
            .ToResponseAsync();

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull().Count.ShouldBeGreaterThan(0);
        response.ConsumedCapacity[0].CapacityUnits.ShouldBeGreaterThan(0);

        // ItemCollectionMetrics may be null for tables without local secondary indexes
        // but the request should succeed regardless

        // Verify item was created
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(testUser.PartitionKey, testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task HandleUnprocessedItemsGracefully()
    {
        // Note: It's difficult to reliably trigger unprocessed items in a test environment
        // as they typically occur due to throttling or internal DynamoDB errors.
        // This test verifies that the response structure supports UnprocessedItems,
        // but we can't easily force this condition.

        var testUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-unprocessed-pk",
            SortKey = $"{KeyPrefix}-unprocessed-sk",
            Name = "Unprocessed User",
            Age = 30,
            Email = "unprocessed@example.com"
        };

        _testUsersToCleanup.Add(testUser);

        var response = await _context.BatchWrite()
            .WithItems(Batch.PutItem(testUser))
            .ToResponseAsync();

        response.ShouldNotBeNull();
        // UnprocessedItems should typically be null for successful operations
        // in a test environment with sufficient capacity

        // Verify item was created successfully
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(testUser.PartitionKey, testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task HandelEmptyBatchWriteGracefully()
    {
        var exception = await Should.ThrowAsync<Exception>(() =>
            _context.BatchWrite()
                .ExecuteAsync());

        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("empty");
    }

    [Test]
    public async Task SupportLargeBatchWriteRequest()
    {
        // Create 20 users (well below the 25 limit)
        var testUsers = Enumerable.Range(1, 20)
            .Select(i => new TestUser
            {
                PartitionKey = $"{KeyPrefix}-large-pk-{i}",
                SortKey = $"{KeyPrefix}-large-sk-{i}",
                Name = $"Large Batch User {i}",
                Age = 20 + i,
                Email = $"large{i}@example.com"
            })
            .ToArray();

        _testUsersToCleanup.AddRange(testUsers);

        await _context.BatchWrite()
            .WithItems(testUsers.Select(Batch.PutItem))
            .ExecuteAsync();

        // Verify all items were created
        var retrievedItems = await _context.BatchGet()
            .WithItems(testUsers.Select(user => Batch.GetItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ToListAsync<TestUser>();

        retrievedItems.ShouldBe(testUsers, ignoreOrder: true);
    }

    [Test]
    public async Task HandleDeleteOperationsOnNonExistentItems()
    {
        // Attempt to delete items that don't exist - should succeed without error
        await Should.NotThrowAsync(_context.BatchWrite()
            .WithItems(
                Batch.DeleteItem<TestUser>().WithPrimaryKey($"{KeyPrefix}-nonexistent-pk-1", $"{KeyPrefix}-nonexistent-sk-1"),
                Batch.DeleteItem<TestUser>().WithPrimaryKey($"{KeyPrefix}-nonexistent-pk-2", $"{KeyPrefix}-nonexistent-sk-2")
            )
            .ExecuteAsync());
    }

    [Test]
    public async Task SupportBatchWriteWithSingleOperation()
    {
        var testUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-single-pk",
            SortKey = $"{KeyPrefix}-single-sk",
            Name = "Single User",
            Age = 27,
            Email = "single@example.com"
        };

        _testUsersToCleanup.Add(testUser);

        await _context.BatchWrite()
            .WithItems(Batch.PutItem(testUser))
            .ExecuteAsync();

        // Verify item was created
        var retrievedItem = await _context.GetItem<TestUser>()
            .WithPrimaryKey(testUser.PartitionKey, testUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedItem.ShouldBe(testUser);
    }

    [Test]
    public async Task ExecuteComplexMixedOperationsWithMultipleItemTypes()
    {
        var userToDelete = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-complex-user-pk",
            SortKey = $"{KeyPrefix}-complex-user-sk",
            Name = "User To Delete",
            Age = 40,
            Email = "delete@example.com"
        };

        var newUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-complex-new-pk",
            SortKey = $"{KeyPrefix}-complex-new-sk",
            Name = "New Complex User",
            Age = 31,
            Email = "newcomplex@example.com"
        };

        var replaceUser = new TestUser
        {
            PartitionKey = $"{KeyPrefix}-complex-replace-pk",
            SortKey = $"{KeyPrefix}-complex-replace-sk",
            Name = "Original Complex User",
            Age = 25,
            Email = "original@example.com"
        };

        var replacementUser = new TestUser
        {
            PartitionKey = replaceUser.PartitionKey,
            SortKey = replaceUser.SortKey,
            Name = "Replacement Complex User",
            Age = 35,
            Email = "replacement@example.com"
        };

        // Setup: Create initial users
        await _context.BatchWrite()
            .WithItems(
                Batch.PutItem(userToDelete),
                Batch.PutItem(replaceUser)
            )
            .ExecuteAsync();

        _testUsersToCleanup.AddRange(new[] { newUser, replacementUser });

        // Execute complex mixed operations
        await _context.BatchWrite()
            .WithItems(
                Batch.DeleteItem<TestUser>().WithPrimaryKey(userToDelete.PartitionKey, userToDelete.SortKey),
                Batch.PutItem(newUser),
                Batch.PutItem(replacementUser) // This replaces the existing item
            )
            .ExecuteAsync();

        // Verify delete operation
        var deletedUser = await _context.GetItem<TestUser>()
            .WithPrimaryKey(userToDelete.PartitionKey, userToDelete.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        deletedUser.ShouldBeNull();

        // Verify new user creation
        var retrievedNewUser = await _context.GetItem<TestUser>()
            .WithPrimaryKey(newUser.PartitionKey, newUser.SortKey)
            .WithConsistentRead(true)
            .ToItemAsync();
        retrievedNewUser.ShouldBe(newUser);

        // Verify user replacement
        var retrievedReplacementUser = await _context.GetItem<TestUser>()
            .WithPrimaryKey(replacementUser.PartitionKey, replacementUser.SortKey)
            .WithConsistentRead(true) 
            .ToItemAsync();
        retrievedReplacementUser.ShouldBe(replacementUser);
        retrievedReplacementUser!.Name.ShouldBe("Replacement Complex User");
        retrievedReplacementUser.Age.ShouldBe(35);
    }

    [Test]
    public void ThrowWhenInvalidBatchWriteParameters()
    {
        var invalidUsers = new[]
        {
            new TestUser { PartitionKey = $"{KeyPrefix}-pk-1", SortKey = "", Name = "test", Age = 25, Email = "test@example.com" },
            new TestUser { PartitionKey = $"{KeyPrefix}-pk-2", SortKey = "", Name = "test2", Age = 30, Email = "test2@example.com" }
        };
        
        Should.Throw<ValidationException>(async () =>
        {
            await _context.BatchWrite()
                .WithItems(invalidUsers.Select(Batch.PutItem))
                .ExecuteAsync();
        });
    }
    
    [Test]
    public async Task BatchWriteItemsWhenSuppressedThrowing()
    {
        var testUsers = new[]
        {
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-suppressed-pk-1",
                SortKey = $"{KeyPrefix}-suppressed-sk-1",
                Name = "Suppressed User 1",
                Age = 25,
                Email = "suppressed1@example.com"
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-suppressed-pk-2",
                SortKey = $"{KeyPrefix}-suppressed-sk-2",
                Name = "Suppressed User 2",
                Age = 30,
                Email = "suppressed2@example.com"
            }
        };

        _testUsersToCleanup.AddRange(testUsers);

        var result = await _context.BatchWrite()
            .WithItems(testUsers.Select(Batch.PutItem))
            .SuppressThrowing()
            .ExecuteAsync();
    
        result.IsSuccess.ShouldBeTrue();
        
        // Verify items were created using batch read
        var retrievedItems = await _context.BatchGet()
            .WithItems(testUsers.Select(user => Batch.GetItem<TestUser>().WithPrimaryKey(user.PartitionKey, user.SortKey)))
            .ToListAsync<TestUser>();
        
        retrievedItems.ShouldBe(testUsers, ignoreOrder: true);
    }
    
    [Test]
    public async Task ReturnErrorWhenInvalidRequestAndSuppressedThrowing()
    {
        var invalidUsers = new[]
        {
            new TestUser { PartitionKey = $"{KeyPrefix}-suppressed-error-pk-1", SortKey = "", Name = "test", Age = 25, Email = "test@example.com" },
            new TestUser { PartitionKey = $"{KeyPrefix}-suppressed-error-pk-2", SortKey = "", Name = "test2", Age = 30, Email = "test2@example.com" }
        };
        
        var result = await _context.BatchWrite()
            .WithItems(invalidUsers.Select(Batch.PutItem))
            .SuppressThrowing()
            .ExecuteAsync();
    
        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldNotBeNull();
    }
} 