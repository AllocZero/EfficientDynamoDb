using System;

namespace EfficientDynamoDb.Internal.Signing.Utils
{
    internal static class EnvironmentProbe
    {
        private static readonly Lazy<bool> _isMono = new Lazy<bool>(() => Type.GetType("Mono.Runtime") != null);

        public static bool IsMono => _isMono.Value;
    }
}