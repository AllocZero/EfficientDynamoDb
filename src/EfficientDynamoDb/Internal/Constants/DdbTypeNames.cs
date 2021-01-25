using System;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Constants
{
    internal static class DdbTypeNames
    {
        public const string String = "S";
        public const string Number = "N";
        public const string NumberSet = "NS";
        public const string StringSet = "SS";
        public const string Bool = "BOOL";
        public const string List = "L";
        public const string Map = "M";
        public const string Null = "NULL";
        public const string Binary = "B";

        public static string ToDdbTypeName(this AttributeType type) => type switch
        {
            AttributeType.String => String,
            AttributeType.Number => Number,
            AttributeType.Bool => Bool,
            AttributeType.Map => Map,
            AttributeType.List => List,
            AttributeType.StringSet => StringSet,
            AttributeType.NumberSet => NumberSet,
            AttributeType.Null => Null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}