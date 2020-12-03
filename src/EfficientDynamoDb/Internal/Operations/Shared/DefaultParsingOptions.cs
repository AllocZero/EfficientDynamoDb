using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    public class DefaultParsingOptions : IParsingOptions
    {
        public static readonly DefaultParsingOptions Instance = new DefaultParsingOptions();
        
        public JsonObjectMetadata? Metadata => null;
       
        public bool HasNumberCallback => false;
       
        public void StartParsing(ref DdbReadStack state)
        {
            
        }
        
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            
        }
    }
}