namespace EfficientDynamoDb.DocumentModel
{
    public enum AttributeType : byte
    {
        Unknown = 0,
        String = 1,
        Number = 2,
        Bool = 3,
        Map = 4,
        List = 5,
        StringSet = 6,
        NumberSet = 7
    }
}