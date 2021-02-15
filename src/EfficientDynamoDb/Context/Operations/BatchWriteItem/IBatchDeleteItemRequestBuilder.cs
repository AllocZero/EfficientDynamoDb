namespace EfficientDynamoDb.Context.Operations.BatchWriteItem
{
    public interface IBatchDeleteItemRequestBuilder
    {
        IBatchWriteItemRequestBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IBatchWriteItemRequestBuilder WithPrimaryKey<TPk>(TPk pk);
    }
}