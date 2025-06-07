using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Scan;
using EfficientDynamoDb.Operations.Shared;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Scan;

[TestFixture]
public class LowLevelScanShould
{
    private const string KeyPrefix = "effddb_tests-low_level_scan";
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
                Name = "Low Level Test User 1",
                Age = 25,
                Email = "user1@example.com",
                Counter = 10,
                Score = 85.5m,
                Active = true
            },

            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-2",
                Name = "Low Level Test User 2",
                Age = 30,
                Email = "user2@example.com",
                Counter = 20,
                Score = 90.0m,
                Active = false
            },

            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-3",
                SortKey = $"{KeyPrefix}-sk-3",
                Name = "Low Level Test User 3",
                Age = 35,
                Email = "user3@example.com",
                Counter = 30,
                Score = 95.5m,
                Active = true
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
    public async Task ScanWithBasicRequestSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();
        response.Count.ShouldBeGreaterThan(0);
        response.ScannedCount.ShouldBeGreaterThanOrEqualTo(response.Count);

        // Verify all returned items match our filter
        foreach (var item in response.Items)
        {
            var pk = item["pk"].AsString();
            pk.ShouldStartWith($"{KeyPrefix}-pk-");
        }
    }

    [Test]
    public async Task ScanWithLimitSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            Limit = 2
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.Count.ShouldBeLessThanOrEqualTo(2);
        response.LastEvaluatedKey.ShouldNotBeNull();
    }

    [Test]
    public async Task ScanWithPaginationSuccessfully()
    {
        // First scan with limit
        var firstRequest = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            Limit = 1
        };

        var firstResponse = await _context.LowLevel.ScanAsync(firstRequest);
        firstResponse.ShouldNotBeNull();
        firstResponse.Items.Count.ShouldBeLessThanOrEqualTo(1);
        firstResponse.LastEvaluatedKey.ShouldNotBeNull();

        // Second scan with pagination
        var secondRequest = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            ExclusiveStartKey = firstResponse.LastEvaluatedKey,
            Limit = 1
        };

        var secondResponse = await _context.LowLevel.ScanAsync(secondRequest);
        secondResponse.ShouldNotBeNull();

        // Ensure we get different items
        if (firstResponse.Items.Any() && secondResponse.Items.Any())
        {
            var firstItemKey = firstResponse.Items[0]["pk"].AsString() + firstResponse.Items[0]["sk"].AsString();
            var secondItemKey = secondResponse.Items[0]["pk"].AsString() + secondResponse.Items[0]["sk"].AsString();
            firstItemKey.ShouldNotBe(secondItemKey);
        }
    }

    [Test]
    public async Task ScanWithConsistentReadSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            ConsistentRead = true
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();
    }

    [Test]
    public async Task ScanWithProjectionExpressionSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            ProjectionExpression = "pk, sk, #n",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#n", "name" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify only projected attributes are returned
        foreach (var item in response.Items)
        {
            item.ShouldContainKey("pk");
            item.ShouldContainKey("sk");
            item.ShouldContainKey("name");
            item.ShouldNotContainKey("age");
            item.ShouldNotContainKey("email");
        }
    }

    [Test]
    public async Task ScanWithNumericFilterSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix) AND attribute_exists(age)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify all returned items have age attribute
        foreach (var item in response.Items)
        {
            item.ShouldContainKey("age");
            var age = item["age"].AsNumberAttribute().ToInt();
            age.ShouldBeGreaterThan(0);
        }
    }

    [Test]
    public async Task ScanWithComplexFilterSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix) AND age BETWEEN :minAge AND :maxAge AND active = :activeValue",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" },
                { ":minAge", "25" },
                { ":maxAge", "35" },
                { ":activeValue", true }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items match the complex filter
        foreach (var item in response.Items)
        {
            var age = item["age"].AsNumberAttribute().ToInt();
            var active = item["active"].AsBool();
            
            age.ShouldBeGreaterThanOrEqualTo(25);
            age.ShouldBeLessThanOrEqualTo(35);
            active.ShouldBeTrue();
        }
    }

    [Test]
    public async Task ScanWithReturnConsumedCapacitySuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task ScanWithSelectCountSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            Select = Select.Count
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Count.ShouldBeGreaterThan(0);
        response.Items.ShouldBeEmpty(); // Should be empty when Select is Count
    }

    [Test]
    public async Task ScanWithSelectAllAttributesSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            },
            Select = Select.AllAttributes
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify all attributes are returned
        foreach (var item in response.Items)
        {
            item.ShouldContainKey("pk");
            item.ShouldContainKey("sk");
            item.ShouldContainKey("name");
            item.ShouldContainKey("age");
            item.ShouldContainKey("email");
        }
    }

    [Test]
    public async Task ScanWithParallelSegmentsSuccessfully()
    {
        const int totalSegments = 2;
        var allItems = new List<Document>();

        // Scan all segments in parallel
        var tasks = new List<Task<ScanResponse>>();
        for (int segment = 0; segment < totalSegments; segment++)
        {
            var request = new ScanRequest
            {
                TableName = TestHelper.TestTableName,
                FilterExpression = "begins_with(pk, :prefix)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":prefix", $"{KeyPrefix}-pk-" }
                },
                Segment = segment,
                TotalSegments = totalSegments
            };

            tasks.Add(_context.LowLevel.ScanAsync(request));
        }

        var responses = await Task.WhenAll(tasks);

        // Collect all items from all segments
        foreach (var response in responses)
        {
            response.ShouldNotBeNull();
            allItems.AddRange(response.Items);
        }

        allItems.Count.ShouldBeGreaterThanOrEqualTo(_testUsers.Count);

        // Verify no duplicates across segments
        var itemKeys = allItems.Select(x => x["pk"].AsString() + x["sk"].AsString()).ToList();
        var uniqueKeys = itemKeys.Distinct().ToList();
        uniqueKeys.Count.ShouldBe(itemKeys.Count);
    }

    [Test]
    public async Task ScanEmptyResultSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", "non_existent_prefix" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldBeEmpty();
        response.Count.ShouldBe(0);
    }

    [Test]
    public async Task ScanWithAttributeExistsFilterSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix) AND attribute_exists(email)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify all returned items have email attribute
        foreach (var item in response.Items)
        {
            item.ShouldContainKey("email");
            item["email"].AsString().ShouldNotBeEmpty();
        }
    }

    [Test]
    public async Task ScanWithStringContainsFilterSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix) AND contains(#n, :searchTerm)",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#n", "name" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" },
                { ":searchTerm", "User 1" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items contain the search term in name
        foreach (var item in response.Items)
        {
            item["name"].AsString().ShouldContain("User 1");
        }
    }

    [Test]
    public async Task ScanWithInOperatorFilterSuccessfully()
    {
        var request = new ScanRequest
        {
            TableName = TestHelper.TestTableName,
            FilterExpression = "begins_with(pk, :prefix) AND age IN (:age1, :age2)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", $"{KeyPrefix}-pk-" },
                { ":age1", "25" },
                { ":age2", "30" }
            }
        };

        var response = await _context.LowLevel.ScanAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items have age in the specified list
        foreach (var item in response.Items)
        {
            var age = item["age"].AsNumberAttribute().ToInt();
            age.ShouldBeOneOf(25, 30);
        }
    }
} 