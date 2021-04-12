using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemParsingOptions : IParsingOptions
    { 
        public static readonly BatchWriteItemParsingOptions Instance = new BatchWriteItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {
                "UnprocessedItems", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(new DictionaryFieldsMetadata
                {
                    {
                        "PutRequest", new JsonObjectMetadata(new DictionaryFieldsMetadata
                        {
                            {"Item", new JsonObjectMetadata(true, false)}
                        }, false, false)
                    },
                    {
                        "DeleteRequest", new JsonObjectMetadata(new DictionaryFieldsMetadata
                        {
                            {"Key", new JsonObjectMetadata(true, false)}
                        }, false, false)
                    }
                }, false, false, true)))
            }
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