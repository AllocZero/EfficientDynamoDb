using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    public static class AttributeTypesMap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AttributeType Get(short key) =>
            key switch
            {
                // String
                83 => AttributeType.String,
                // Number
                78 => AttributeType.Number,
            };
    }
}