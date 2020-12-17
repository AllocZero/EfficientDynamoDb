using System;

namespace EfficientDynamoDb.Internal.TypeParsers
{
    public static class EnumParser
    {
        public static bool TryParseCaseInsensitive<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum =>
            Enum.TryParse(value, out result) || Enum.TryParse(value, true, out result);
    }
}