using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Reader
{
    public static class AttributeTypesMap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AttributeType Get(short key) =>
            key switch
            {
                83 => AttributeType.String,
                78 => AttributeType.Number,
                20290 => AttributeType.Bool,
                77 => AttributeType.Map,
                76 => AttributeType.List,
                21331 => AttributeType.StringSet,
                21326 => AttributeType.NumberSet,
                21838 => AttributeType.Null,
                _ => throw new DdbException("Unexpected DDB type")
            };
    }
}