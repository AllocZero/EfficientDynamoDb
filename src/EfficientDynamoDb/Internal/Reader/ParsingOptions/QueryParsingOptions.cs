using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader.ParsingOptions
{
    public class QueryParsingOptions : IParsingOptions
    {
        public static readonly QueryParsingOptions Instance = new QueryParsingOptions();
            
        public bool HasNumberCallback => true;

        public JsonObjectMetadata Metadata { get; } = new JsonObjectMetadata(new Dictionary<string, JsonObjectMetadata>
        {
            {"Items", new JsonObjectMetadata(true, true)}
        });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            if (current.KeyName != "Count")
                return;

            current.BufferLengthHint = reader.GetInt32();
        }
    }
}