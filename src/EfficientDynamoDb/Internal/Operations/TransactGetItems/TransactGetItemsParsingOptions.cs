using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    public class TransactGetItemsParsingOptions: IParsingOptions
    {
        public static readonly TransactGetItemsParsingOptions Instance = new TransactGetItemsParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new Dictionary<string, JsonObjectMetadata>
        {
            {
                "Responses", new JsonObjectMetadata(new Dictionary<string, JsonObjectMetadata>
                {
                    {"Item", new JsonObjectMetadata(true, false)}
                })
            }
        });

        public bool HasNumberCallback => false;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}