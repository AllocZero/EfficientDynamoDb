using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class ParsingExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetOptionalInt(this Document document, string key) => document.TryGetValue(key, out var number) ? number.ToInt() : 0;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetOptionalFloat(this Document document, string key) => document.TryGetValue(key, out var number) ? number.ToFloat() : 0;
    }
}