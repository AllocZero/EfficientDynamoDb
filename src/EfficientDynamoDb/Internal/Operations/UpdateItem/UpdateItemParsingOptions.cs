using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    public class UpdateItemParsingOptions : IParsingOptions
    {
        public static readonly UpdateItemParsingOptions Instance = new UpdateItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Attributes", new JsonObjectMetadata(true, false)}
        });

        public bool HasNumberCallback => false;
        
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}