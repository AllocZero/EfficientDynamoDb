using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal class UpdateItemParsingOptions : IParsingOptions
    {
        public static readonly UpdateItemParsingOptions Instance = new UpdateItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Attributes", new JsonObjectMetadata(true, false)}
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