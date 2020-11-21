using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader.ParsingOptions
{
    public class GetParsingOptions : IParsingOptions
    {
        public static readonly GetParsingOptions Instance = new GetParsingOptions();
        
        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new Dictionary<string, JsonObjectMetadata>
        {
            {"Item", new JsonObjectMetadata(true, false)}
        });

        public bool HasNumberCallback => false;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}