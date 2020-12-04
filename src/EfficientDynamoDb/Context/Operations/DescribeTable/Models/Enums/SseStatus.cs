using System;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums
{
    public enum SseStatus
    {
        _UNKNOWN = 0,
        ENABLED = 10,
        [Obsolete("Not supported according to DDB docs")]
        ENABLING = 20,
        [Obsolete("Not supported according to DDB docs")]
        DISABLED = 30,
        [Obsolete("Not supported according to DDB docs")]
        DISABLING = 40,
        UPDATING = 50
    }
}