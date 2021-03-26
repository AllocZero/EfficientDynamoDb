using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared
{
    public class TransactionCancelledResponse
    {
        [DynamoDBProperty("Message")] 
        public string Message { get; set; } = null!;

        [DynamoDBProperty("CancellationReasons", typeof(JsonIReadOnlyListDdbConverter<TransactionCancellationReason>))]
        public IReadOnlyList<TransactionCancellationReason> CancellationReasons { get; set; } = null!;
    }
    
    [DynamoDBConverter(typeof(JsonObjectDdbConverter<TransactionCancellationReason>))]
    public class TransactionCancellationReason
    {
        [DynamoDBProperty("Code")] 
        public string? Code { get; set; }
        
        [DynamoDBProperty("Message")] 
        public string? Message { get; set; }
        
        [DynamoDBProperty("Item")] 
        public Document? Item { get; set; }
    }
}