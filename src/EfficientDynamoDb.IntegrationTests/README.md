# EfficientDynamoDb Integration Tests

This project contains integration tests for the EfficientDynamoDb library that can run against both LocalDynamoDB and production DynamoDB.

## Overview

The integration tests are designed to:
- Test the high-level API of EfficientDynamoDb
- Work with both LocalDynamoDB and production DynamoDB
- Keep credentials secure (no hardcoded secrets)
- Provide comprehensive coverage of GetItem operations
- Use proper test entity prefixes to avoid conflicts

## Prerequisites

### For LocalDynamoDB Testing
1. Docker and Docker Compose installed.
2. .NET 8.0 SDK

### For Production DynamoDB Testing
1. AWS account with DynamoDB access
2. Valid AWS credentials
3. Appropriate IAM permissions for DynamoDB operations

## Quick Start with Docker Compose

### 1. Start LocalDynamoDB

The easiest way to run integration tests is using the provided Docker Compose setup:

1. Start local-dynamodb:
```bash
docker-compose up -d
```
2. Set test DynamoDB endpoint:

Bash:
```bash
export EFFDDB_TEST_DYNAMODB_ENDPOINT="http://localhost:8000"
```
or Powershell:
```powershell
$env:EFFDDB_TEST_DYNAMODB_ENDPOINT="http://localhost:8000"
```
3. Run tests:
```
dotnet test
```

## Environment Variables

All integration test environment variables are prefixed with `EFFDDB_TEST_` to avoid collisions.

### Local DynamoDB

Used when `EFFDDB_TEST_DYNAMODB_ENDPOINT` is set:

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `EFFDDB_TEST_DYNAMODB_ENDPOINT` | DynamoDB endpoint URL | Yes | - |
| `EFFDDB_TEST_DYNAMODB_PORT` | Port for LocalDynamoDB (docker-compose only) | No | `8000` |

### Production DynamoDB

Used when `EFFDDB_TEST_DYNAMODB_ENDPOINT` is not set:

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `EFFDDB_TEST_AWS_ACCESS_KEY_ID` | AWS Access Key ID | Yes | - |
| `EFFDDB_TEST_AWS_SECRET_ACCESS_KEY` | AWS Secret Access Key | Yes | - |
| `EFFDDB_TEST_AWS_SESSION_TOKEN` | AWS Session Token (for temporary credentials) | No | - |
| `EFFDDB_TEST_AWS_REGION` | AWS Region | No | `"us-east-1"` |

## Test Entity Prefixes

All test entities use specific prefixes to avoid conflicts:
- GetItem tests: `effddb_tests-get_item-*`
- Future tests: `effddb_tests-{test_type}-*`

This ensures that tests can run in parallel and don't interfere with each other. The naming convention uses:
- `effddb_tests` as a single prefix (double-click selectable)
- Hyphens as main separators between logical sections
- Underscores within logical parts to keep them as single selectable units
name: Integration Tests

## Common Issues

1. **LocalDynamoDB not accessible**
   - Ensure Docker is running and dynamodb-local container is up
   - Check that the configured port is not in use by another service
   - Verify `EFFDDB_TEST_DYNAMODB_ENDPOINT` is set correctly