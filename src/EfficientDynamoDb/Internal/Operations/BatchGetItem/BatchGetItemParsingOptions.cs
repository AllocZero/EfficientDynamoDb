using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    public class BatchGetItemParsingOptions : IParsingOptions
    { 
        public static readonly BatchGetItemParsingOptions Instance = new BatchGetItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } =  new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Responses", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, true)))},
            {"UnprocessedKeys", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, false)))}
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