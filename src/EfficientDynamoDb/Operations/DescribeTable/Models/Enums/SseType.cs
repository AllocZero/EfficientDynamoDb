using System;

namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum SseType
    {
        Undefined = 0,
        Kms = 10,
        [Obsolete("Not supported according to DDB docs")]
        Aes256 = 20
    }
}