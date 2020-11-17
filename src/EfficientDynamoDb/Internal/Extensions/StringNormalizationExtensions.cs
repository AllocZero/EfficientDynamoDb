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
    }
}