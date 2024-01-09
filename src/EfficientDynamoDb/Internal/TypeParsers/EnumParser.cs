using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.TypeParsers
{
    public static class EnumParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseCaseInsensitive<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum =>
            Enum.TryParse(value, out result) || Enum.TryParse(value, true, out result);
        
        public static bool TryParseUpperSnakeCase<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            Span<char> buffer = stackalloc char[value.Length];
            var sb = new NoAllocStringBuilder(in buffer, true);
            
            var isNextUpper = false;
            foreach (var c in value)
            {
                if (c == '_')
                {
                    isNextUpper = true;
                    continue;
                }

                var nextChar = isNextUpper ? c : char.ToLowerInvariant(c);
                sb.Append(nextChar);
                isNextUpper = false;
            }
            
            return Enum.TryParse(sb.ToString(), true, out result);
        }
    }
}