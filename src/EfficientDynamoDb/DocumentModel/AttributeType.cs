namespace EfficientDynamoDb.DocumentModel
{
    public enum AttributeType : byte
    {
        None = 0,
        String = 1,
        Number = 2,
        Bool = 3,
        Map = 4,
        List = 5,
        StringSet = 6,
        NumberSet = 7,
        Null = 8,
    }
}