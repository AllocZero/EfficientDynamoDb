using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    public class QueryParsingOptions : IParsingOptions
    {
        public static readonly QueryParsingOptions Instance = new QueryParsingOptions();
            
        public bool HasNumberCallback => true;

        public JsonObjectMetadata Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"Items", new JsonObjectMetadata(true, true)},
            {"LastEvaluatedKey", new JsonObjectMetadata(true, false)}
        });

        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            if (current.KeyName != "Count")
                return;

            var itemsCount = reader.GetInt32();
            current.BufferLengthHint = Math.Max(itemsCount, DdbReadStackFrame.DefaultAttributeBufferSize);
            
            // Only use keys cache when there are at least 2 items, otherwise it will only waste resources
            if(itemsCount > 1)
                state.KeysCache = new KeysCache(DdbReadStack.DefaultKeysCacheSize, DdbReadStack.MaxKeysCacheSize);
        }
    }
}