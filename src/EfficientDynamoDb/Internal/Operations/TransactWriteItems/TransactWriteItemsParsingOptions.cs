using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    public sealed class TransactWriteItemsParsingOptions : IParsingOptions
    {
        public static readonly TransactWriteItemsParsingOptions Instance = new TransactWriteItemsParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {
                "ItemCollectionMetrics", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, false)))
            }
        });

        public bool HasNumberCallback => false;
        
        public void StartParsing(ref DdbReadStack state)
        {
            
        }
        
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}