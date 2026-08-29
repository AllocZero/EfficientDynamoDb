using System;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class StringNormalizationExtensions
    {
        /// <summary>
        /// Converts the string to UPPER_SNAKE_CASE and appends it to the <paramref name="destination"/> span.
        /// </summary>
        public static int ToUpperSnakeCaseAscii(this string self, Span<char> destination)
        {
            var written = 0;

            for (var i = 0; i < self.Length; i++)
            {
                var c = self[i];
                if (i != 0 && char.IsAsciiLetterUpper(c) && RequiresSeparator(self, i))
                    destination[written++] = '_';

                destination[written++] = char.IsAsciiLetterLower(c) ? (char) (c - 32) : c;
            }

            return written;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool RequiresSeparator(string self, int i)
        {
            var previous = self[i - 1];
            if (char.IsAsciiLetterLower(previous) || char.IsAsciiDigit(previous))
                return true;

            if (!char.IsAsciiLetterUpper(previous)) 
                return false;

            var nextCharIsLower = i + 1 < self.Length && char.IsAsciiLetterLower(self[i + 1]);
            return nextCharIsLower;
        }
    }
}
