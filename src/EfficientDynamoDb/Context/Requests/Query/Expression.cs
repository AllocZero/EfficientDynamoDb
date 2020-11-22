using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Requests.Query
{
    public class Expression
    {
        public string? ConditionExpression { get; set; }

        public IReadOnlyDictionary<string, string>? AttributeNames { get; set; }
        
        public IReadOnlyDictionary<string, AttributeValue>? AttributeValues { get; set; }
    }
}