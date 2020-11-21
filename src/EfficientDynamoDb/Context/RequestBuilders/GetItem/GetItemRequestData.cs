using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    internal class GetItemRequestData<TPk> 
        where TPk : IAttributeValue
    {
        public string TableName { get; }
        
        public string? PkName { get; }

        public TPk PkValue { get; }

        public GetItemRequestData(string tableName, string? pkName, TPk pkValue)
        {
            TableName = tableName;
            PkName = pkName;
            PkValue = pkValue;
        }
    }

    internal class GetItemRequestData<TPk, TSk> 
        where TPk : IAttributeValue
        where TSk : IAttributeValue
    {
        public string TableName { get; }
        
        public string? PkName { get; }

        public TPk PkValue { get; }

        public string? SkName { get; }

        public TSk SkValue { get; }
        
        public GetItemRequestData(string tableName, string? pkName, TPk pkValue, string? skName, TSk skValue)
        {
            TableName = tableName;
            PkName = pkName;
            PkValue = pkValue;
            SkName = skName;
            SkValue = skValue;
        }
    }
}