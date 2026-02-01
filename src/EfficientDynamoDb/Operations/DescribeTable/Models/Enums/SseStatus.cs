using System;

namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum SseStatus
    {
        Undefined = 0,
        Enabled = 10,
        [Obsolete("Not supported according to DDB docs")]
        Enabling = 20,
        [Obsolete("Not supported according to DDB docs")]
        Disabled = 30,
        [Obsolete("Not supported according to DDB docs")]
        Disabling = 40,
        Updating = 50
    }
}