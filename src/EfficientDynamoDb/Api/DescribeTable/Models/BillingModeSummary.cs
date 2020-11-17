using System;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;
using EfficientDynamoDb.Internal.JsonConverters;

namespace EfficientDynamoDb.Api.DescribeTable.Models
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