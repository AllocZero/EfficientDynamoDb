using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal abstract class IterableHttpContent : DynamoDbHttpContent
    {
        protected IterableHttpContent(string amzTarget) : base(amzTarget)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void WriteExclusiveStartKey(Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> exclusiveStartKey)
        {
            writer.WritePropertyName("ExclusiveStartKey");
            writer.WriteAttributesDictionary(exclusiveStartKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void WriteSelect(Utf8JsonWriter writer, Select select)
        {
            var selectValue = select switch
            {
                Select.AllAttributes => "ALL_ATTRIBUTES",
                Select.AllProjectedAttributes => "ALL_PROJECTED_ATTRIBUTES",
                Select.Count => "COUNT",
                Select.SpecificAttributes => "SPECIFIC_ATTRIBUTES",
                _ => "ALL_ATTRIBUTES"
            };
            
            writer.WriteString("Select", selectValue);
        }
    }
}