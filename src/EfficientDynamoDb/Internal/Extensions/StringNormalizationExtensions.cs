using System;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class StringNormalizationExtensions
    {
        /// <summary>
        /// Normalize string by reducing multiple sequential whitespaces into a single space.
        /// </summary>
        public static string NormalizeWhiteSpace(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return string.Empty;
            }

            var currentIndex = 0;
            var skipped = false;
            var output = new char[self.Length];

            foreach (var currentChar in self.ToCharArray())
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    if (!skipped)
                    {
                        if (currentIndex > 0)
                        {
                            output[currentIndex++] = ' ';
                        }

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[currentIndex++] = currentChar;
                }
            }

            return new string(output, 0, currentIndex);
        }
        
        /// <summary>
        /// Converts the string to UPPER_SNAKE_CASE and appends it to the <paramref name="destination"/> span.
        /// </summary>
        public static int ToUpperSnakeCaseAscii(this string self, Span<char> destination)
        {
            var written = 0;

            for (var i = 0; i < self.Length; i++)
            {
                var c = self[i];
                if (i != 0 && char.IsAsciiLetterUpper(c))
                    destination[written++] = '_';

                destination[written++] = char.IsAsciiLetterLower(c) ? (char) (c - 32) : c;
            }

            return written;
        }
    }
}