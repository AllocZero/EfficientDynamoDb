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
            
            current.BufferLengthHint = Math.Max(reader.GetInt32(), DdbReadStackFrame.DefaultAttributeBufferSize);
        }
    }
}