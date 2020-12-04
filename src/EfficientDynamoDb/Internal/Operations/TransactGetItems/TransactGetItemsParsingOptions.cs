using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsParsingOptions : IParsingOptions
    {
        public static readonly TransactGetItemsParsingOptions Instance = new TransactGetItemsParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {
                "Responses", new JsonObjectMetadata(new DictionaryFieldsMetadata
                {
                    {"Item", new JsonObjectMetadata(true, false)}
                })
            }
        });

        public bool HasNumberCallback => false;
        
        public void StartParsing(ref DdbReadStack state)
        {
            if(GlobalDynamoDbConfig.InternAttributeNames)
                state.KeysCache = new KeysCache(DdbReadStack.DefaultKeysCacheSize, DdbReadStack.MaxKeysCacheSize);
        }
        
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}