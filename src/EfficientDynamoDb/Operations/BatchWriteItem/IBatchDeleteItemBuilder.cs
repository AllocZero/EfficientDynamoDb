namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    public interface IBatchDeleteItemBuilder : IBatchWriteBuilder
    {
        IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk);
    }
}