using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Tests.TestConfiguration;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.IntegrationTests.Query;

[TestFixture]
public class LowLevelQueryShould
{
    private const string KeyPrefix = "effddb_tests-low_level_query";
    private DynamoDbContext _context = null!;
    private List<TestUser> _testUsers = null!;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _context = TestHelper.CreateContext();
        
        // Create test data for querying
        _testUsers = [
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-001",
                Name = "Low Level Query User 1",
                Age = 25,
                Email = "user1@example.com",
                Counter = 10,
                Score = 85.5m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-002",
                Name = "Low Level Query User 2",
                Age = 30,
                Email = "user2@example.com",
                Counter = 20,
                Score = 90.0m,
                Active = false
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-1",
                SortKey = $"{KeyPrefix}-sk-003",
                Name = "Low Level Query User 3",
                Age = 35,
                Email = "user3@example.com",
                Counter = 30,
                Score = 95.5m,
                Active = true
            },
            new TestUser
            {
                PartitionKey = $"{KeyPrefix}-pk-2",
                SortKey = $"{KeyPrefix}-sk-001",
                Name = "Low Level Query User 4",
                Age = 28,
                Email = "user4@example.com",
                Counter = 15,
                Score = 88.0m,
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
    public async Task QueryWithBasicRequestSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();
        response.Count.ShouldBeGreaterThan(0);
        response.ScannedCount.ShouldBeGreaterThanOrEqualTo(response.Count);

        // Verify all returned items have the correct partition key
        foreach (var item in response.Items)
        {
            var pk = item["pk"].AsString();
            pk.ShouldBe(partitionKey);
        }
    }

    [Test]
    public async Task QueryWithSortKeyConditionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyPrefix = $"{KeyPrefix}-sk-00";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk AND begins_with(sk, :sk_prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":sk_prefix", sortKeyPrefix }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify all returned items match the conditions
        foreach (var item in response.Items)
        {
            var pk = item["pk"].AsString();
            var sk = item["sk"].AsString();
            pk.ShouldBe(partitionKey);
            sk.ShouldStartWith(sortKeyPrefix);
        }
    }

    [Test]
    public async Task QueryWithFilterExpressionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "active = :active",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":active", true }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items match the filter
        foreach (var item in response.Items)
        {
            var active = item["active"].AsBool();
            active.ShouldBeTrue();
        }
    }

    [Test]
    public async Task QueryWithLimitSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            Limit = 2
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.Count.ShouldBeLessThanOrEqualTo(2);
        response.LastEvaluatedKey.ShouldNotBeNull();
    }

    [Test]
    public async Task QueryWithPaginationSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        // First query with limit
        var firstRequest = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            Limit = 1
        };

        var firstResponse = await _context.LowLevel.QueryAsync(firstRequest);
        firstResponse.ShouldNotBeNull();
        firstResponse.Items.Count.ShouldBeLessThanOrEqualTo(1);
        firstResponse.LastEvaluatedKey.ShouldNotBeNull();

        // Second query with pagination
        var secondRequest = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            ExclusiveStartKey = firstResponse.LastEvaluatedKey,
            Limit = 1
        };

        var secondResponse = await _context.LowLevel.QueryAsync(secondRequest);
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
    public async Task QueryWithConsistentReadSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            ConsistentRead = true
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();
    }

    [Test]
    public async Task QueryWithProjectionExpressionSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            ProjectionExpression = "pk, sk, #n",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#n", "name" }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

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
    public async Task QueryWithSortKeyRangeSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyStart = $"{KeyPrefix}-sk-001";
        var sortKeyEnd = $"{KeyPrefix}-sk-002";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk AND sk BETWEEN :sk_start AND :sk_end",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":sk_start", sortKeyStart },
                { ":sk_end", sortKeyEnd }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items fall within the range
        foreach (var item in response.Items)
        {
            var sk = item["sk"].AsString();
            sk.ShouldBeGreaterThanOrEqualTo(sortKeyStart);
            sk.ShouldBeLessThanOrEqualTo(sortKeyEnd);
        }
    }

    [Test]
    public async Task QueryWithNumericFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "age > :min_age",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":min_age", "25" }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items match the numeric filter
        foreach (var item in response.Items)
        {
            var age = item["age"].AsNumberAttribute().ToInt();
            age.ShouldBeGreaterThan(25);
        }
    }

    [Test]
    public async Task QueryWithComplexFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "age BETWEEN :min_age AND :max_age AND active = :active_value",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":min_age", "25" },
                { ":max_age", "35" },
                { ":active_value", true }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

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
    public async Task QueryWithReturnConsumedCapacitySuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            ReturnConsumedCapacity = ReturnConsumedCapacity.Total
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.ConsumedCapacity.ShouldNotBeNull();
        response.ConsumedCapacity.TableName.ShouldBe(TestHelper.TestTableName);
        response.ConsumedCapacity.CapacityUnits.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task QueryWithSelectCountSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            Select = Select.Count
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Count.ShouldBeGreaterThan(0);
        response.Items.ShouldBeEmpty(); // Should be empty when Select is Count
    }

    [Test]
    public async Task QueryWithSelectAllAttributesSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            Select = Select.AllAttributes
        };

        var response = await _context.LowLevel.QueryAsync(request);

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
    public async Task QueryWithDescendingOrderSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            },
            ScanIndexForward = false
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeEmpty();

        // Verify items are returned in descending order
        if (response.Items.Count > 1)
        {
            for (int i = 0; i < response.Items.Count - 1; i++)
            {
                var currentSk = response.Items[i]["sk"].AsString();
                var nextSk = response.Items[i + 1]["sk"].AsString();
                string.CompareOrdinal(currentSk, nextSk).ShouldBeGreaterThanOrEqualTo(0);
            }
        }
    }

    [Test]
    public async Task QueryEmptyResultSuccessfully()
    {
        var nonExistentPartitionKey = $"{KeyPrefix}-non-existent-pk";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", nonExistentPartitionKey }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.ShouldBeEmpty();
        response.Count.ShouldBe(0);
    }

    [Test]
    public async Task QueryWithAttributeExistsFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "attribute_exists(email)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

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
    public async Task QueryWithStringContainsFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "contains(#n, :search_term)",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#n", "name" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":search_term", "User 1" }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items contain the search term in name
        foreach (var item in response.Items)
        {
            item["name"].AsString().ShouldContain("User 1");
        }
    }

    [Test]
    public async Task QueryWithInOperatorFilterSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk",
            FilterExpression = "age IN (:age1, :age2)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":age1", "25" },
                { ":age2", "30" }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items have age in the specified list
        foreach (var item in response.Items)
        {
            var age = item["age"].AsNumberAttribute().ToInt();
            age.ShouldBeOneOf(25, 30);
        }
    }

    [Test]
    public async Task QueryWithSortKeyEqualToSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKey = $"{KeyPrefix}-sk-001";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk AND sk = :sk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":sk", sortKey }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();
        response.Items.Count.ShouldBeLessThanOrEqualTo(1);

        if (response.Items.Any())
        {
            var item = response.Items[0];
            item["pk"].AsString().ShouldBe(partitionKey);
            item["sk"].AsString().ShouldBe(sortKey);
        }
    }

    [Test]
    public async Task QueryWithSortKeyGreaterThanSuccessfully()
    {
        var partitionKey = $"{KeyPrefix}-pk-1";
        var sortKeyThreshold = $"{KeyPrefix}-sk-001";
        
        var request = new QueryRequest
        {
            TableName = TestHelper.TestTableName,
            KeyConditionExpression = "pk = :pk AND sk > :sk_threshold",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", partitionKey },
                { ":sk_threshold", sortKeyThreshold }
            }
        };

        var response = await _context.LowLevel.QueryAsync(request);

        response.ShouldNotBeNull();

        // Verify all returned items have sort key greater than threshold
        foreach (var item in response.Items)
        {
            var sk = item["sk"].AsString();
            string.CompareOrdinal(sk, sortKeyThreshold).ShouldBeGreaterThan(0);
        }
    }
} 