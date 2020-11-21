using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader.ParsingOptions
{
    public class DefaultParsingOptions : IParsingOptions
    {
        public static readonly DefaultParsingOptions Instance = new DefaultParsingOptions();
        
        public JsonObjectMetadata? Metadata => null;
       
        public bool HasNumberCallback => false;
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            
        }
    }
}