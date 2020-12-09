# EfficientDynamoDb
EfficientDynamoDb is a high performance DynamoDb library with a huge focus on efficient resources utilization. Due to DynamoDb extreme scaling capabilities it is very important to make sure that backend services don't waste valuable CPU time on unmarshalling responses. EfficientDynamoDb is capable of zero allocation deserialization and is 2-10X faster than official AWS SDK.
