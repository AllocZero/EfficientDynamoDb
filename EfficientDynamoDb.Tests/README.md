# EfficientDynamoDb Integration Tests

This project contains integration tests for the EfficientDynamoDb library that can run against both LocalDynamoDB (for development) and production DynamoDB (for CI/CD and comprehensive testing).

## Overview

The integration tests are designed to:
- Test the high-level API of EfficientDynamoDb
- Work with both LocalDynamoDB and production DynamoDB
- Keep credentials secure (no hardcoded secrets)
- Provide comprehensive coverage of GetItem operations
- Use proper test entity prefixes to avoid conflicts

## Prerequisites

### For LocalDynamoDB Testing
1. Docker and Docker Compose installed on your machine
2. .NET 8.0 SDK

### For Production DynamoDB Testing
1. AWS account with DynamoDB access
2. Valid AWS credentials
3. Appropriate IAM permissions for DynamoDB operations

## Quick Start with Docker Compose

### 1. Start LocalDynamoDB

The easiest way to run integration tests is using the provided Docker Compose setup:

```bash
# Start LocalDynamoDB
docker-compose up -d

# Create the test table
aws dynamodb create-table \
  --table-name ddb_test \
  --attribute-definitions \
    AttributeName=pk,AttributeType=S \
    AttributeName=sk,AttributeType=S \
  --key-schema \
    AttributeName=pk,KeyType=HASH \
    AttributeName=sk,KeyType=RANGE \
  --billing-mode PAY_PER_REQUEST \
  --endpoint-url http://localhost:8000

# Set environment variables and run tests
export EFFDDB_TEST_DYNAMODB_ENDPOINT="http://localhost:8000"
export EFFDDB_TEST_AWS_REGION="us-east-1"
cd EfficientDynamoDb.Tests
dotnet test
```

### 2. Using Environment Variables

You can customize the setup using environment variables:

```bash
# Copy the template and customize
cp env.template .env
# Edit .env with your preferred values

# Use the environment file
export $(cat .env | xargs)
docker-compose up -d
```

## Environment Variables

All integration test environment variables are prefixed with `EFFDDB_TEST_` to avoid collisions:

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `EFFDDB_TEST_AWS_ACCESS_KEY_ID` | AWS Access Key ID | No* | `"fake-access-key"` |
| `EFFDDB_TEST_AWS_SECRET_ACCESS_KEY` | AWS Secret Access Key | No* | `"fake-secret-key"` |
| `EFFDDB_TEST_AWS_SESSION_TOKEN` | AWS Session Token (for temporary credentials) | No | - |
| `EFFDDB_TEST_AWS_REGION` | AWS Region | No | `"us-east-1"` |
| `EFFDDB_TEST_DYNAMODB_ENDPOINT` | DynamoDB endpoint URL (for LocalDynamoDB) | No | - |
| `DYNAMODB_PORT` | Port for LocalDynamoDB (docker-compose only) | No | `8000` |

*Required for production DynamoDB, optional for LocalDynamoDB

## Manual Setup

### Running Tests Against LocalDynamoDB

1. **Start LocalDynamoDB using Docker Compose:**
   ```bash
   docker-compose up -d
   ```

2. **Create the test table:**
   ```bash
   aws dynamodb create-table \
     --table-name ddb_test \
     --attribute-definitions \
       AttributeName=pk,AttributeType=S \
       AttributeName=sk,AttributeType=S \
     --key-schema \
       AttributeName=pk,KeyType=HASH \
       AttributeName=sk,KeyType=RANGE \
     --billing-mode PAY_PER_REQUEST \
     --endpoint-url http://localhost:8000
   ```

3. **Set environment variables:**
   ```bash
   export EFFDDB_TEST_DYNAMODB_ENDPOINT="http://localhost:8000"
   export EFFDDB_TEST_AWS_REGION="us-east-1"
   
   # Optional - fake credentials will be used if not set
   export EFFDDB_TEST_AWS_ACCESS_KEY_ID="fake-access-key"
   export EFFDDB_TEST_AWS_SECRET_ACCESS_KEY="fake-secret-key"
   ```

4. **Run the tests:**
   ```bash
   cd EfficientDynamoDb.Tests
   dotnet test
   ```

### Running Tests Against Production DynamoDB

1. **Set up AWS credentials** (choose one method):

   **Option A: Environment Variables**
   ```bash
   export EFFDDB_TEST_AWS_ACCESS_KEY_ID="your-access-key-id"
   export EFFDDB_TEST_AWS_SECRET_ACCESS_KEY="your-secret-access-key"
   export EFFDDB_TEST_AWS_REGION="us-east-1"  # or your preferred region
   ```

   **Option B: AWS CLI Profile**
   ```bash
   aws configure
   # Follow prompts to set up credentials, then use default profile
   export EFFDDB_TEST_AWS_REGION="us-east-1"
   ```

   **Option C: IAM Role (for EC2/Lambda/etc.)**
   ```bash
   # No additional setup needed - credentials will be automatically resolved
   export EFFDDB_TEST_AWS_REGION="us-east-1"
   ```

2. **Ensure the test table exists:**
   ```bash
   aws dynamodb create-table \
     --table-name ddb_test \
     --attribute-definitions \
       AttributeName=pk,AttributeType=S \
       AttributeName=sk,AttributeType=S \
     --key-schema \
       AttributeName=pk,KeyType=HASH \
       AttributeName=sk,KeyType=RANGE \
     --billing-mode PAY_PER_REQUEST
   ```

3. **Run the tests:**
   ```bash
   cd EfficientDynamoDb.Tests
   dotnet test
   ```

## Test Configuration

The integration tests use a simple helper class that automatically detects whether to use LocalDynamoDB or production DynamoDB based on the `EFFDDB_TEST_DYNAMODB_ENDPOINT` environment variable.

### Helper Classes

- **`TestHelper`**: Simple static helper class with `CreateContext()` method that returns a configured `DynamoDbContext`
- **`TestEntity`**: Sample entity class for testing

### Using TestHelper

Each test method creates its own DynamoDB context using the helper for proper test isolation:

```csharp
[TestFixture]
public class MyTestClass
{
    [Test]
    public async Task SomeTest()
    {
        // Arrange
        var context = TestHelper.CreateContext();
        
        // Act & Assert
        var result = await context.GetItemAsync<TestEntity>("pk", "sk");
        Assert.That(result, Is.Null);
    }
}
```

### Test Entity Schema

The tests use a `TestEntity` class with the following schema:

```csharp
[DynamoDbTable("ddb_test")]
public class TestEntity
{
    [DynamoDbProperty("pk", typeof(StringDdbConverter), DynamoDbAttributeType.PartitionKey)]
    public string PartitionKey { get; set; }

    [DynamoDbProperty("sk", typeof(StringDdbConverter), DynamoDbAttributeType.SortKey)]
    public string SortKey { get; set; }

    [DynamoDbProperty("name")]
    public string Name { get; set; }

    [DynamoDbProperty("age")]
    public int Age { get; set; }

    [DynamoDbProperty("email")]
    public string? Email { get; set; }
}
```

### Test Entity Prefixes

All test entities use specific prefixes to avoid conflicts:
- GetItem tests: `effddb_tests-get_item-*`
- Future tests: `effddb_tests-{test_type}-*`

This ensures that tests can run in parallel and don't interfere with each other. The naming convention uses:
- `effddb_tests` as a single prefix (double-click selectable)
- Hyphens as main separators between logical sections
- Underscores within logical parts to keep them as single selectable units

## Test Examples

### GetItem Operations

The `GetItemShould` test class demonstrates various GetItem scenarios:

1. **Basic GetItem**: Retrieve an existing item
2. **Null handling**: Handle non-existent items
3. **Builder pattern**: Use the fluent API
4. **Projections**: Retrieve only specific attributes
5. **Consistent reads**: Use strongly consistent reads
6. **Response metadata**: Access ConsumedCapacity information

Example test:
```csharp
[TestFixture]
public class GetItemShould
{
    [Test]
    public async Task ReturnItemWhenItemExists()
    {
        // Arrange
        var context = TestHelper.CreateContext();
        var testEntity = CreateTestEntity("effddb_tests-get_item-existing-pk", "effddb_tests-get_item-existing-sk");
        await context.PutItemAsync(testEntity);

        // Act
        var result = await context.GetItemAsync<TestEntity>(
            testEntity.PartitionKey, 
            testEntity.SortKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo(testEntity.Name));
    }

    private static TestEntity CreateTestEntity(string pk, string sk)
    {
        return new TestEntity
        {
            PartitionKey = pk,
            SortKey = sk,
            Name = "Test User",
            Age = 25,
            Email = "test@example.com"
        };
    }
}
```

## Docker Compose

The `docker-compose.yml` provides a simple LocalDynamoDB service:

```yaml
version: '3.8'

services:
  dynamodb-local:
    image: amazon/dynamodb-local
    container_name: efficientdynamodb-local
    ports:
      - "${DYNAMODB_PORT:-8000}:8000"
    command: ["-jar", "DynamoDBLocal.jar", "-sharedDb", "-inMemory"]
```

### Customizing Docker Compose

You can customize the port by setting an environment variable:

```bash
# Custom port
export DYNAMODB_PORT=9000
docker-compose up -d
```

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Integration Tests

on: [push, pull_request]

jobs:
  test-localdynamodb:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Start LocalDynamoDB
      run: docker-compose up -d
    
    - name: Create test table
      run: |
        aws dynamodb create-table \
          --table-name ddb_test \
          --attribute-definitions \
            AttributeName=pk,AttributeType=S \
            AttributeName=sk,AttributeType=S \
          --key-schema \
            AttributeName=pk,KeyType=HASH \
            AttributeName=sk,KeyType=RANGE \
          --billing-mode PAY_PER_REQUEST \
          --endpoint-url http://localhost:8000
      env:
        AWS_ACCESS_KEY_ID: fake-access-key
        AWS_SECRET_ACCESS_KEY: fake-secret-key
        AWS_DEFAULT_REGION: us-east-1
    
    - name: Run Integration Tests
      run: dotnet test EfficientDynamoDb.Tests
      env:
        EFFDDB_TEST_DYNAMODB_ENDPOINT: http://localhost:8000
        EFFDDB_TEST_AWS_REGION: us-east-1

  test-production:
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'  # Only run on main branch
    
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Run Production Tests
      run: dotnet test EfficientDynamoDb.Tests
      env:
        EFFDDB_TEST_AWS_ACCESS_KEY_ID: ${{ secrets.EFFDDB_TEST_AWS_ACCESS_KEY_ID }}
        EFFDDB_TEST_AWS_SECRET_ACCESS_KEY: ${{ secrets.EFFDDB_TEST_AWS_SECRET_ACCESS_KEY }}
        EFFDDB_TEST_AWS_REGION: us-east-1
```

## Troubleshooting

### Common Issues

1. **LocalDynamoDB not accessible**
   - Ensure Docker is running
   - Check that the configured port is not in use by another service
   - Verify `EFFDDB_TEST_DYNAMODB_ENDPOINT` is set correctly

2. **AWS credentials not working**
   - Verify credentials are valid using `aws sts get-caller-identity`
   - Check IAM permissions for DynamoDB operations
   - Ensure region is supported

3. **Table not found errors**
   - Create the test table using the provided AWS CLI commands
   - Verify table name matches the entity configuration (`ddb_test`)

4. **Test failures in production**
   - Check for rate limiting (use exponential backoff)
   - Verify billing mode and capacity settings
   - Consider using different table names for parallel test runs

### Debugging

Enable detailed logging by setting:
```bash
export DOTNET_LOGGING__CONSOLE__LOGLEVEL__DEFAULT=Debug
```

## Contributing

When adding new integration tests:

1. Create a DynamoDB context using `TestHelper.CreateContext()` in each test method
2. Create your own test-specific entity creation methods
3. Use proper prefixes for partition keys: `effddb_tests-{test_type}-{unique_id}`
4. Test both success and failure scenarios
5. Clean up test data in teardown methods if needed

## Security Considerations

- Never commit AWS credentials to the repository
- Use IAM roles with minimal required permissions
- Consider using AWS Secrets Manager for production credentials
- Regularly rotate access keys
- Use separate AWS accounts for testing when possible
- All test environment variables are prefixed with `EFFDDB_TEST_` to avoid collisions with other applications 