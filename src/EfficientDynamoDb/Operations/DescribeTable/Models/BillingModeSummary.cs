using System;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations.DescribeTable.Models
{
    public class BillingModeSummary
    {
        public BillingMode BillingMode { get; }
        
        [JsonConverter(typeof(UnixDateTimeJsonConverter))]
        public DateTime LastUpdateToPayPerRequestDateTime { get; }

        public BillingModeSummary(BillingMode billingMode, DateTime lastUpdateToPayPerRequestDateTime)
        {
            BillingMode = billingMode;
            LastUpdateToPayPerRequestDateTime = lastUpdateToPayPerRequestDateTime;
        }
    }
}