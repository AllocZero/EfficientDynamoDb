using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    public class BatchGetItemParsingOptions : IParsingOptions
    { 
        public static readonly BatchGetItemParsingOptions Instance = new BatchGetItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } =  new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Responses", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, true)))},
            {"UnprocessedKeys", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, true)))}
        });

        public bool HasNumberCallback => false;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}