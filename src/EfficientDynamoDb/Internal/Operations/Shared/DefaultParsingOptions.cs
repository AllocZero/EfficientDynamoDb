using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal sealed class DefaultParsingOptions : IParsingOptions
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