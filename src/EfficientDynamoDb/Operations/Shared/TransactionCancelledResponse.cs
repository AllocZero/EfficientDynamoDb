using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared
{
    public class TransactionCancelledResponse
    {
        [DynamoDbProperty("Message")] 
        public string Message { get; set; } = null!;

        [DynamoDbProperty("CancellationReasons", typeof(JsonIReadOnlyListDdbConverter<TransactionCancellationReason>))]
        public IReadOnlyList<TransactionCancellationReason> CancellationReasons { get; set; } = null!;
    }
    
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<TransactionCancellationReason>))]
    public class TransactionCancellationReason
    {
        [DynamoDbProperty("Code")] 
        public string? Code { get; set; }
        
        [DynamoDbProperty("Message")] 
        public string? Message { get; set; }
        
        [DynamoDbProperty("Item")] 
        public Document? Item { get; set; }
    }
}