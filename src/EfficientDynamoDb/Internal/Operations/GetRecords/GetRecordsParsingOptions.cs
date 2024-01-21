using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Operations.GetRecords
{
    internal sealed class GetRecordsParsingOptions : IParsingOptions
    {
        public static readonly GetRecordsParsingOptions Instance = new GetRecordsParsingOptions();
        
        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Records", new JsonObjectMetadata(new DictionaryFieldsMetadata
                {
                    {"dynamodb", new JsonObjectMetadata(new DictionaryFieldsMetadata
                        {
                            { "Keys", new JsonObjectMetadata(true, false) },
                            { "NewImage", new JsonObjectMetadata(true, false) },
                            { "OldImage", new JsonObjectMetadata(true, false) }
                        }, false, false)
                    }
                }, false, false, true)
            }
        }, false, false);

        public bool HasNumberCallback => false;

        public void StartParsing(ref DdbReadStack state)
        {
            if(GlobalDynamoDbConfig.InternAttributeNames)
                state.KeysCache = new KeysCache(DdbReadStack.DefaultKeysCacheSize, DdbReadStack.MaxKeysCacheSize);
        }

        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}