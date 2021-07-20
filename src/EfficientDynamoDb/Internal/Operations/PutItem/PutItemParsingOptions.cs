using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemParsingOptions : IParsingOptions
    {
        public static readonly PutItemParsingOptions Instance = new PutItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Attributes", new JsonObjectMetadata(true, false)},
            {
                "ItemCollectionMetrics", new JsonObjectMetadata(new DictionaryFieldsMetadata
                {
                    {"ItemCollectionKey", new JsonObjectMetadata(true, false)}
                })
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