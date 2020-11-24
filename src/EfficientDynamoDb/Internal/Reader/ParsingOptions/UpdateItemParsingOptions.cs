using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader.ParsingOptions
{
    public class UpdateItemParsingOptions : IParsingOptions
    {
        public static readonly UpdateItemParsingOptions Instance = new UpdateItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new Dictionary<string, JsonObjectMetadata>
        {
            {"Attributes", new JsonObjectMetadata(true, false)}
        });

        public bool HasNumberCallback => false;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}